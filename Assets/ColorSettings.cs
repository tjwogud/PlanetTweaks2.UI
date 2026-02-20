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

        [SerializeField] private Keys key;

        private bool isEnabled;

        private void Awake()
        {
            if (toggle)
                toggle.onClick.AddListener(() => Toggle(!isEnabled));

            red.onClick.AddListener(() => SetColor(Colors.redColor));
            blue.onClick.AddListener(() => SetColor(Colors.blueColor));
            green.onClick.AddListener(() => SetColor(Colors.greenColor));
            custom.onClick.AddListener(() => UI.colorPicker.Show(color => SetColor(color)));
        }

        public void Toggle(bool enable)
        {
            isEnabled = enable;
            if (toggle)
                toggle.GetComponentInChildren<TMP_Text>().text = enable ? "On" : "Off";
            buttons.interactable = enable;

            if (!enable)
                SetColor(Colors.disableColor);
        }

        public void SetColor(Color color)
        {
            UI.SetValue(key, color);
        }
    }
}