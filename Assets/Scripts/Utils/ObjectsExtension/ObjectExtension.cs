namespace Utils.ObjectsExtension
{
    public static class ObjectExtension
    {
        /// <summary>
        /// Is object null?
        /// </summary>
        /// <returns>Return object == null</returns>
        public static bool Null(this object obj)
        {
            return obj == null;
        }
    }
}