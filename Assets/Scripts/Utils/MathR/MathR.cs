using Unity.XR.CoreUtils;
using UnityEngine;

namespace Utils.MathR
{
    public static class MathR
    {
        public static int Mod(int number, int max)
        {
            return (number % max + max) % max;
        }

        public static Vector3 Mirror(this Vector3 position, Vector3 center)
        {
            return - (position - center) + center;
        }
    }
}