using System;

namespace Utils.FloatExtension
{
    public static class FloatExtension
    {
        public static bool Approx(this float a, float b, float threshold = float.Epsilon) =>
            Math.Abs(a - b) > threshold;
    }
}