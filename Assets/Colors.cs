using UnityEngine;

namespace PlanetTweaks2.UI
{
    public static class Colors
    {
        public readonly static Color redColor = new(1, 0, 0, 0);
        public readonly static Color blueColor = new(0, 0, 1, 0);
        public readonly static Color greenColor = new(.3f, .7f, 1); // this one is NOT alpha 0, because it doesn't need additional work to apply
        public readonly static Color disableColor = new(0, 0, 0, 0);

        public static readonly Color goldColor = new(1, 1, 0, 0);
        public static readonly Color rainbowColor = new(1, 1, 1, 0);
        public static readonly Color overseerColor = new(0, 1, 1, 0);

        public static bool IsSpecial(Color color)
        {
            return color == goldColor
                || color == rainbowColor
                || color == overseerColor;
        }
    }
}
