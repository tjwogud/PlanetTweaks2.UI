using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

namespace PlanetTweaks2.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class PTText : PTBase
    {
        [SerializeField] private bool translate;
        [SerializeField] private string key;
        private TMP_Text text;

        private static readonly List<PTText> texts = new();

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
            texts.Add(this);
        }

        public static void SetLanguage(SystemLanguage language)
        {
            foreach (var text in texts)
                if (text.translate)
                    text.text.text = text.UI.localization.Get(text.key, null, language);
        }

        public static void SetFont(TMP_FontAsset font)
        {
            foreach (var text in texts)
                text.text.font = font;
        }
    }
}
