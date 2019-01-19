using System;

namespace Mastermind
{
    public static class Extensions
    {
        public static int GetListHashCode<T>(this T[] array) where T : struct
        {
            unchecked
            {
                int hash = 37;
                foreach (var item in array)
                {
                    hash = (hash * 73) + item.GetHashCode();
                }
                return hash;
            }
        }

        public static int GetWidth(this int value)
        {
            if (value < 0)
            {
                throw new NotSupportedException();
            }
            if (value == 0 || value == 1)
            {
                return 1;
            }
            return (int)Math.Ceiling(Math.Log10(value));
        }
    }
}
