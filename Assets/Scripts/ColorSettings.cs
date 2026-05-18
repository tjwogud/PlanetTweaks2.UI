using TMPro;
using UnityEngine;
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

            red.onClick.AddListener(() => SetColor(SimplePlanetColor.DefaultRed));
            blue.onClick.AddListener(() => SetColor(SimplePlanetColor.DefaultBlue));
            green.onClick.AddListener(() => SetColor(SimplePlanetColor.Green));
            custom.onClick.AddListener(() => UI.colorPicker.Show(color => SetColor(new(color))));
        }

        public void Toggle(bool enable)
        {
            isEnabled = enable;
            if (toggle)
                toggle.GetComponentInChildren<TMP_Text>().text = enable ? "On" : "Off";
            buttons.interactable = enable;

            if (!enable)
                SetColor(SimplePlanetColor.Disable);
        }

        public void SetColor(SimplePlanetColor color)
        {
            UI.SetValue(key, color);
            UI.preview.SetColor(key, color);
        }
    }
}