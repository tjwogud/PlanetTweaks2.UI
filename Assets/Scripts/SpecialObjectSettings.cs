using UnityEngine;
using UnityEngine.UI;

namespace PlanetTweaks2.UI
{
    public class SpecialObjectSettings : PTBase
    {
        [SerializeField] private Button disable;
        [SerializeField] private Button samurai;
        [SerializeField] private Button emoji;

        private void Awake()
        {
            disable.onClick.AddListener(() =>
            {
                UI.SetValue(Keys.Samurai, false);
                UI.preview.SetSamurai(false);
                UI.SetValue(Keys.Emoji, false);
                UI.preview.SetEmoji(false);
            });
            samurai.onClick.AddListener(() =>
            {
                UI.SetValue(Keys.Emoji, false);
                UI.preview.SetEmoji(false);
                UI.SetValue(Keys.Samurai, true);
                UI.preview.SetSamurai(true);
            });
            emoji.onClick.AddListener(() =>
            {
                UI.SetValue(Keys.Samurai, false);
                UI.preview.SetSamurai(false);
                UI.SetValue(Keys.Emoji, true);
                UI.preview.SetEmoji(true);
            });
        }
    }
}
