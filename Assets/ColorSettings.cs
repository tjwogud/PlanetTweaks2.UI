using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PlanetTweaks2.UI
{
    public class ColorSettings : PTBase
    {
        [SerializeField] private Button toggle;
         
        [SerializeField] private CanvasGroup buttons;
        [SerializeField] private Button red;
        [SerializeField] private Button blue;
        [SerializeField] private Button green;
        [SerializeField] private Button custom;

        private static Color redColor = new(1, 0, 0, 0);
        private static Color blueColor = new(0, 0, 1, 0);
        private static Color disableColor = new(0, 0, 0, 0);
        public UnityAction<Color> SetColor { get; private set; }

        private bool isEnabled;

        public void Init(UnityAction<Color> setColor)
        {
            SetColor = setColor;
        }

        private void Awake()
        {
            if (toggle)
                toggle.onClick.AddListener(() => Toggle(!isEnabled));

            red.onClick.AddListener(() => SetColor(redColor));
            blue.onClick.AddListener(() => SetColor(blueColor));
            green.onClick.AddListener(() => SetColor(new(.3f, .7f, 1)));
            custom.onClick.AddListener(() => UI.colorPicker.Show(color => SetColor(color)));
        }

        public void Toggle(bool enable)
        {
            isEnabled = enable;
            if (toggle)
                toggle.GetComponentInChildren<TMP_Text>().text = enable ? "On" : "Off";
            buttons.interactable = enable;

            if (!enable)
                SetColor(disableColor);
        }
    }
}