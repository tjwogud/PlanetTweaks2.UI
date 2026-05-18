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
                SetColor(new(new(.8f, .8f, .8f)));
            });
            gold.onClick.AddListener(() => SetColor(SimplePlanetColor.Gold));
            rainbow.onClick.AddListener(() => SetColor(SimplePlanetColor.Rainbow));
            overseer.onClick.AddListener(() => SetColor(SimplePlanetColor.Overseer));
        }

        public void SetColor(SimplePlanetColor color)
        {
            UI.SetValue(Keys.PlanetColor, color);
            UI.preview.SetColor(Keys.PlanetColor, color);
            UI.SetCurrent(UI.current);
        }
    }
}
