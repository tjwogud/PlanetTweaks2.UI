using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlanetTweaks2.UI
{
    public class PlanetTweaksUI : PTBase
    {
        public static PlanetTweaksUI Instance { get; private set; }

        [Header("Preview")]
        

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


        }

        public delegate object GetValueDelegate(Keys key);
        public delegate void SetValueDelegate(Keys key, object value);
    }
}