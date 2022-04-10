namespace Utils.MathR
{
    public static class MathR
    {
        public static int Mod(int number, int max)
        {
            return (number % max + max) % max;
        }
    }
}