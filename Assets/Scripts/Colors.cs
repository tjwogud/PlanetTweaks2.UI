using UnityEngine;

namespace PlanetTweaks2.UI
{
    public static class Colors
    {
        public readonly static Color greenColor = new(.3f, .7f, 0);

        public static readonly Color realGoldColor = new(1, .9023f, .0039f);
        public static readonly Color realOverseerColor = new(.1059f, .6471f, .7843f);

        public static Color WithAlpha(this Color color, float alpha)
        {
            return new(color.r, color.g, color.b, alpha);
        }
    }
}
