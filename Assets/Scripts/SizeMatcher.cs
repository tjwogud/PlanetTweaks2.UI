using UnityEngine;

namespace PlanetTweaks2.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SizeMatcher : PTBase
    {
        [SerializeField] private RectTransform targetRect;
        private RectTransform rect;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
        }

        private void LateUpdate()
        {
            if (targetRect.sizeDelta.x != rect.sizeDelta.x)
            {
                rect.sizeDelta = new(targetRect.sizeDelta.x, rect.sizeDelta.y);
            }
        }
    }
}
