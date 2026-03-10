using Localizations2;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlanetTweaks2.UI
{
    public class PlanetTweaks2UI : PTBase
    {
        public static PlanetTweaks2UI Instance { get; private set; }

        [Header("Preview")]
        public Preview preview;

        [Header("Selector")]
        [SerializeField] private TMP_Dropdown planetSelect;
        [SerializeField] private TMP_Dropdown languageSelect;
        [SerializeField] private Button exitButton;

        [Header("Settings")]
        [SerializeField] private CanvasGroup colorAlphaSettings;
        [SerializeField] private GameObject disableColorAlphaSettings;
        [SerializeField] private ColorSettings planetColor;
        [SerializeField] private AlphaSettings planetAlpha;
        [SerializeField] private ColorSettings tailColor;
        [SerializeField] private AlphaSettings tailAlpha;
        [SerializeField] private ColorSettings ringColor;
        [SerializeField] private AlphaSettings ringAlpha;
        [SerializeField] private ImageSettings imageSettings;

        [SerializeField] private SpecialPlanetSettings specialPlanet;
        [SerializeField] private SpecialObjectSettings specialObject;

        [SerializeField] private GameObject goldLock;
        [SerializeField] private GameObject overseerLock;
        [SerializeField] private GameObject rainbowLock;
        [SerializeField] private GameObject samuraiLock;
        [SerializeField] private GameObject emojiLock;
        [SerializeField] private GameObject specialNote;

        public ColorPicker colorPicker;

        public GetValueDelegate GetValue { get; private set; }
        public SetValueDelegate SetValue { get; private set; }

        public int current;
        public Localization localization;
        public SystemLanguage language = SystemLanguage.Korean;

        public static readonly SystemLanguage[] availableLanguages = new SystemLanguage[]
        { 
            SystemLanguage.Korean,
            SystemLanguage.English
        };

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            planetSelect.onValueChanged.AddListener(SetCurrent);
            languageSelect.onValueChanged.AddListener(i =>
            {
                language = availableLanguages[i];
                PTText.TranslateAll();
                SetValue(Keys.Language, language);

                LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            });

            if (!Application.isEditor) return;
            SetValue = (key, value) => Debug.Log($"{key} : {value}");
            localization = new("1QcrRL6LAs8WxJj_hFsEJa3CLM5g3e8Ya0KQlRKXwdlU", 646714919, s => Debug.Log(s), "Assets/PlanetTweaks2", null, () => Translate());
        }

        private bool translate;

        public void Translate()
        {
            translate = true;
        }

        private void LateUpdate()
        {
            if (translate)
            {
                PTText.TranslateAll();
                translate = false;
            }
        }

        public void Init(GetValueDelegate getValue, SetValueDelegate setValue, Localization localization)
        {
            GetValue = getValue;
            SetValue = setValue;

            SetCurrent(0);

            this.localization = localization;

            language = (SystemLanguage)GetValue(Keys.Language);
        }

        public void SetCurrent(int index)
        {
            current = index;

            if (Application.isEditor) return;

            var planetColor = (Color)GetValue(Keys.PlanetColor);
            var isSpecialColor = Colors.IsSpecial(planetColor);

            colorAlphaSettings.interactable = !isSpecialColor;
            colorAlphaSettings.alpha = isSpecialColor ? .3f : 1;
            disableColorAlphaSettings.SetActive(isSpecialColor);

            var tailColor = (Color)GetValue(Keys.TailColor);
            this.tailColor.Toggle(tailColor != Colors.disableColor);
            var ringColor = (Color)GetValue(Keys.RingColor);
            this.ringColor.Toggle(ringColor != Colors.disableColor);

            var planetAlpha = (float)GetValue(Keys.PlanetAlpha);
            this.planetAlpha.Init(planetAlpha);

            var tailAlpha = (float)GetValue(Keys.TailAlpha);
            this.tailAlpha.Init(tailAlpha);

            var ringAlpha = (float)GetValue(Keys.RingAlpha);
            this.ringAlpha.Init(ringAlpha);

            preview.SetColor(Keys.PlanetColor, planetColor);
            preview.SetAlpha(Keys.PlanetAlpha, planetAlpha);
            preview.SetColor(Keys.TailColor, (Color)GetValue(Keys.TailColor));
            preview.SetAlpha(Keys.TailAlpha, tailAlpha);
            preview.SetColor(Keys.RingColor, (Color)GetValue(Keys.RingColor));
            preview.SetAlpha(Keys.RingAlpha, ringAlpha);

            imageSettings.UpdateValue();
            preview.SetImage((PlanetImage)GetValue(Keys.Image));
            preview.SetImagePosition((Vector2)GetValue(Keys.ImagePosition));
            preview.SetImageSize((Vector2)GetValue(Keys.ImageSize));
        }

        public void UpdateProgress(bool gold, bool overseer, bool emoji)
        {
            if (gold) goldLock.SetActive(false);
            if (overseer) overseerLock.SetActive(false);
            if (emoji) emojiLock.SetActive(false);

            CheckAllUnlocked();
        }

        public void UpdateCheatCode(bool rainbow, bool samurai)
        {
            if (rainbow) rainbowLock.SetActive(false);
            if (samurai) samuraiLock.SetActive(false);

            CheckAllUnlocked();
        }

        public void CheckAllUnlocked()
        {
            if (goldLock.activeSelf || overseerLock.activeSelf || emojiLock.activeSelf || rainbowLock.activeSelf || samuraiLock.activeSelf)
                return;
            specialNote.SetActive(false);
        }

        public void Toggle(bool active)
        {
            if (!active && colorPicker.gameObject.activeSelf)
                colorPicker.Apply();
            gameObject.SetActive(active);
        }

        public delegate object GetValueDelegate(Keys key);
        public delegate void SetValueDelegate(Keys key, object value);
    }
}