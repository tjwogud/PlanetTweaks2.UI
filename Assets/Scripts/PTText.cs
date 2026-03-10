using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace PlanetTweaks2.UI
{
    public class PTText : PTBase
    {
        [SerializeField] private bool translate;
        [SerializeField] private string key;
        private TMP_Text text;
        private TMP_InputField input;
        [SerializeField] private TMP_Dropdown dropdown;

        private static readonly List<PTText> texts = new();

        private static TMP_FontAsset font;
        private static Material material;

        private string dump;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
            input = GetComponent<TMP_InputField>();
            if (!text && !input)
            {
                Destroy(this);
                Debug.Log(this);
                return;
            }
            texts.Add(this);

            if (font != null)
                SetFont();
        }

        private void Start()
        {
            if (UI.localization.Loaded && !UI.localization.Failed)
            {
                Translate();
            }
        }

        public void Translate()
        {
            if (!translate) return;

            if (dropdown)
            {
                text.text = UI.localization.Get(dropdown.options[dropdown.value].text, null, UI.language);
                return;
            }

            var value = UI.localization.Get(string.IsNullOrEmpty(key) ? text.text : key, null, UI.language);
            if (value != null) text.text = value;
        }

        public static void TranslateAll()
        {
            foreach (var text in texts)
                text.Translate();
        }

        public static void SetFont(TMP_FontAsset font)
        {
            PTText.font = font;
            var material = new Material(font.material);
            material.DisableKeyword("UNDERLAY_ON");
            PTText.material = material;
            foreach (var text in texts)
            {
                if (!text) continue;
                text.SetFont();
            }
        }

        private void SetFont()
        {
            if (text)
            {
                text.font = font;
                text.fontMaterial = material;
            }
            else if (input)
            {
                input.fontAsset = font;
                input.textComponent.fontMaterial = material;
            }
        }
    }
}
