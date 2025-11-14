using System.Linq;

namespace MySystem.Base.Helpers
{
    public static class NumberHelper
    {
        public static bool IsBitSet(this int b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }

        public static int BitOn(int pos) => 1 << pos;

        public static int BitOn(int[] poses) => poses.Aggregate(0, (total, next) => total | BitOn(next));
        public static void Swap<T>(ref T num1, ref T num2)
        {
            T tmp = num1;
            num1 = num2;
            num2 = tmp;
        }
    }
}