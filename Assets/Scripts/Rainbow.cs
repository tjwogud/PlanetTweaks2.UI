using UnityEngine;
using UnityEngine.Events;

namespace PlanetTweaks2.UI
{
    public class Rainbow : MonoBehaviour
    {
        public Color Current => Color.HSVToRGB(Time.unscaledTime / 5 % 1, 1, 1);
        public UnityEvent<Color> onUpdate;

        private void LateUpdate()
        {
            onUpdate?.Invoke(Current);
        }
    }
}