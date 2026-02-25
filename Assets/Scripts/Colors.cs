using UnityEngine;

namespace PlanetTweaks2.UI
{
    public static class Colors
    {
        public readonly static Color greenColor = new(.3f, .7f, 0); // this one is NOT alpha 0, because it doesn't need additional work to apply
        public readonly static Color disableColor = new(0, 0, 0, 0);

        public static readonly Color goldColor = new(1, 1, 0, 0);
        public static readonly Color rainbowColor = new(1, 1, 1, 0);
        public static readonly Color overseerColor = new(0, 1, 1, 0);

        public static readonly Color realGoldColor = new(1, .9023f, .0039f);
        public static readonly Color realOverseerColor = new(.1059f, .6471f, .7843f);

        public static bool IsSpecial(Color color)
        {
            return color == goldColor
                || color == rainbowColor
                || color == overseerColor;
        }

        public static Color WithAlpha(this Color color, float alpha)
        {
            return new(color.r, color.g, color.b, alpha);
        }

        public static string ToHex(Color color)
        {
            return
                ((byte)(color.r * 255)).ToString("X2") +
                ((byte)(color.g * 255)).ToString("X2") +
                ((byte)(color.b * 255)).ToString("X2");
        }
    }
}
