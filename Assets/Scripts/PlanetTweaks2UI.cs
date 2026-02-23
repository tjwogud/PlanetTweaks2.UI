using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlanetTweaks2.UI
{
    public class PlanetTweaks2UI : PTBase
    {
        public static PlanetTweaks2UI Instance { get; private set; }

        [Header("Preview")]
        [SerializeField] public Preview preview;

        [Header("Selector")]
        [SerializeField] private TMP_Dropdown planetSelect;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button loadButton;
        [SerializeField] private Button exitButton;

        [Header("Settings")]
        [SerializeField] private GameObject colorAlphaSettings;
        [SerializeField] private GameObject disableColorAlphaSettings;
        [SerializeField] private ColorSettings planetColor;
        [SerializeField] private AlphaSettings planetAlpha;
        [SerializeField] private ColorSettings tailColor;
        [SerializeField] private AlphaSettings tailAlpha;
        [SerializeField] private ColorSettings ringColor;
        [SerializeField] private AlphaSettings ringAlpha;
        public ColorPicker colorPicker;

        public GetValueDelegate GetValue { get; private set; }
        public SetValueDelegate SetValue { get; private set; }

        public int current;

        private void Awake()
        {
            SetValue = (key, value) => Debug.Log($"{key} : {value}");

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Init(GetValueDelegate getValue, SetValueDelegate setValue)
        {
            GetValue = getValue;
            SetValue = setValue;

            SetCurrent(0);
        }

        public void SetCurrent(int index)
        {
            current = index;
            
            var planetColor = (Color)GetValue(Keys.PlanetColor);
            var isSpecialColor = Colors.IsSpecial(planetColor);

            colorAlphaSettings.SetActive(!isSpecialColor);
            disableColorAlphaSettings.SetActive(isSpecialColor);

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