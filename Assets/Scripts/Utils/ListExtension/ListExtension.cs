using System.Collections.Generic;
using Utils.FloatExtension;

namespace Utils.ListExtension
{
    public static class ListExtension
    {
        public static void Add<T>(this List<T> list, T value, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                list.Add(value);
            }
        }

        public static bool ContainsAny(this List<float> list, params float[] values)
        {
            foreach (float value in values)
            {
                if (list.ContainsApprox(value))
                    return true;
            }

            return false;
        }

        public static bool ContainsAll(this List<float> list, params float[] values)
        {
            foreach (float value in values)
            {
                if (!list.ContainsApprox(value))
                    return false;
            }

            return true;
        }

        public static List<T> Reversed<T>(this List<T> list)
        {
            List<T> reverse = new List<T>(list);
            reverse.Reverse();
            return reverse;
        }

        private static bool ContainsApprox(this List<float> list, float value)
        {
            foreach (float listValue in list)
                if (value.Approx(listValue)) return true;
            return false;
        }

        public static void RemoveApprox(this List<float> list, float value)
        {
            foreach (float listValue in list)
            {
                if (!value.Approx(listValue)) continue;
                list.Remove(listValue);
                return;
            }
        }

        public static int Repeated(this List<float> list, float value)
        {
            int amount = 0;
            foreach (float listValue in list)
                if (value.Approx(listValue)) amount++;
            return amount;
        }
    }
}