using UnityEngine;
using UnityEngine.UI;

namespace PlanetTweaks2.UI
{
    public class SpecialPlanetSettings : PTBase
    {
        [SerializeField] private Button disable;
        [SerializeField] private Button gold;
        [SerializeField] private Button rainbow;
        [SerializeField] private Button overseer;

        private void Awake()
        {
            disable.onClick.AddListener(() =>
            {
                if (Colors.IsSpecial((Color)UI.GetValue(Keys.PlanetColor)))
                    SetColor(new(.8f, .8f, .8f));
            });
            gold.onClick.AddListener(() => SetColor(Colors.goldColor));
            rainbow.onClick.AddListener(() => SetColor(Colors.rainbowColor));
            overseer.onClick.AddListener(() => SetColor(Colors.overseerColor));
        }

        public void SetColor(Color color)
        {
            UI.SetValue(Keys.PlanetColor, color);
            UI.preview.SetColor(Keys.PlanetColor, color);
            UI.SetCurrent(UI.current);
        }
    }
}
