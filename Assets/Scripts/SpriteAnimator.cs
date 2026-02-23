using UnityEngine;
using UnityEngine.UI;

namespace PlanetTweaks2.UI
{
    public class SpriteAnimator : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private float fps;
        [SerializeField] private Sprite[] sprites;

        private void LateUpdate()
        {
            image.sprite = sprites[(int)(Time.unscaledTime * fps % sprites.Length)];
        }
    }
}