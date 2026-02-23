using UnityEngine;

namespace PlanetTweaks2.UI
{
    public class Rainbow : PTBase
    {
        private void LateUpdate()
        {
            var color = Color.HSVToRGB(Time.unscaledTime / 5 % 1, 1, 1);
            UI.preview.SetColor(Keys.PlanetColor, color);
        }
    }
}