using UnityEngine;

namespace PlanetTweaks2.UI
{
    public readonly struct SimplePlanetColor
    {
        public static SimplePlanetColor Disable => new(-1, Color.black);
        public static SimplePlanetColor DefaultRed => new(1, Color.red);
        public static SimplePlanetColor DefaultBlue => new(2, Color.blue);
        public static SimplePlanetColor Green => new(Colors.greenColor); // this is not a special color;
        public static SimplePlanetColor NoAlpha => new(3, new(0, 0, 0, 0));
        public static SimplePlanetColor Gold => new(3, Colors.realGoldColor);
        public static SimplePlanetColor Rainbow => new(4, Color.white);
        public static SimplePlanetColor Overseer => new(5, Colors.realOverseerColor);

        public readonly int code;
        public readonly Color color;
        public readonly bool isSpecial;

        public SimplePlanetColor(Color _color)
        {
            code = 0;
            color = _color;
            isSpecial = false;
        }

        private SimplePlanetColor(int _code, Color realColor)
        {
            code = _code;
            color = realColor;
            isSpecial = true;
        }

        public static bool operator ==(SimplePlanetColor a, SimplePlanetColor b)
        {
            if (a.isSpecial || b.isSpecial)
                return a.code == b.code;
            return a.color == b.color;
        }

        public static bool operator !=(SimplePlanetColor a, SimplePlanetColor b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return obj is SimplePlanetColor color && this == color;
        }

        public override int GetHashCode()
        {
            return isSpecial ? code : color.GetHashCode();
        }
    }
}
