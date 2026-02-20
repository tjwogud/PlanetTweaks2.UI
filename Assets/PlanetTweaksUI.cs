using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PlanetTweaks2.UI
{
    public partial class PlanetTweaksUI : PTBase
    {
        public static PlanetTweaksUI Instance { get; private set; }

        [Header("Preview")]
        

        [Header("Selector")]
        [SerializeField] private TMP_Dropdown planetSelect;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button loadButton;
        [SerializeField] private Button exitButton;

        [Header("Settings")]
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

            planetColor.Init(color => SetValue(Key.PlanetColor, color));
            planetAlpha.Init(alpha => SetValue(Key.PlanetAlpha, alpha));
            tailColor.Init(color => SetValue(Key.TailColor, color));
            tailAlpha.Init(alpha => SetValue(Key.TailAlpha, alpha));
            ringColor.Init(color => SetValue(Key.RingColor, color));
            ringAlpha.Init(alpha => SetValue(Key.RingAlpha, alpha));
        }

        public void Init(GetValueDelegate getValue, SetValueDelegate setValue)
        {
            GetValue = getValue;
            SetValue = setValue;
        }

        public delegate object GetValueDelegate(Key key);
        public delegate void SetValueDelegate(Key key, object value);
    }
}