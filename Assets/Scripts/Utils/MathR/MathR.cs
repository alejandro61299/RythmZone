using System;
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

        public static float VectorScalar(this Vector3 vector1, Vector3 vector2)
        {
            return (vector1.x * vector2.x + vector1.y * vector2.y + vector1.z * vector2.z);
        }
        
        public static Vector3 PerpendicularTo(this Vector3 vector)
        {
            float[] vectorComponents = { vector.x, vector.y, vector.z };
            float[] tempVector = new float[3];
            int i, j, k;
            float a, b;
            if (Math.Abs(vectorComponents[1]) > Math.Abs(vectorComponents[0]))
            {
                if (Math.Abs(vectorComponents[2]) > Math.Abs(vectorComponents[1]))
                {
                    i = 2;
                    j = 1;
                    k = 0;
                    a = vectorComponents[2];
                    b = -vectorComponents[1];
                }
                else if (Math.Abs(vectorComponents[2]) > Math.Abs(vectorComponents[0]))
                {
                    i = 1;
                    j = 2;
                    k = 0;
                    a = vectorComponents[1];
                    b = -vectorComponents[2];
                }
                else
                {
                    i = 1;
                    j = 0;
                    k = 2;
                    a = vectorComponents[1];
                    b = -vectorComponents[0];
                }
            }
            else if (Math.Abs(vectorComponents[2]) > Math.Abs(vectorComponents[0]))
            {
                i = 2;
                j = 0;
                k = 1;
                a = vectorComponents[2];
                b = -vectorComponents[0];
            }
            else if (Math.Abs(vectorComponents[2]) > Math.Abs(vectorComponents[1]))
            {
                i = 0;
                j = 2;
                k = 1;
                a = vectorComponents[0];
                b = -vectorComponents[2];
            }
            else
            {
                i = 0;
                j = 1;
                k = 2;
                a = vectorComponents[0];
                b = -vectorComponents[1];
            }

            tempVector[i] = b;
            tempVector[j] = a;
            tempVector[k] = 0.0f;

            return new Vector3(tempVector[0], tempVector[1], tempVector[2]);
        }
    }
}