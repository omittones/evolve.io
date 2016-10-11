using System.Linq;

namespace Util
{
    public static class ArrayExtensions
    {
        public static T[] SetAll<T>(this T[] inArray, T inValue)
        {
            for (int cNum = 0; cNum < inArray.Length; cNum++)
                inArray[cNum] = inValue;
            return inArray;
        }

        public static int GetLinearLength<T>(this T[,] inArray)
        {
            return inArray.GetLength(0)*inArray.GetLength(1);
        }

        public static T GetLinear<T>(this T[,] inArray, int inIndex)
        {
            int x = inIndex/inArray.GetLength(1);
            int y = inIndex - x*inArray.GetLength(1);
            return inArray[x, y];
        }

        public static void SetLinear<T>(this T[,] inArray, int inIndex, T inValue)
        {
            int x = inIndex/inArray.GetLength(1);
            int y = inIndex - x*inArray.GetLength(1);
            inArray[x, y] = inValue;
        }

        public static double[] Normalize(this double[] array, double min, double max)
        {
            var oMin = array.Min();
            var oMax = array.Max();
            var oRange = oMax - oMin;

            if (oRange == 0.0)
                return array;

            var nRange = max - min;

            return array
                .Select(v => (v - oMin)/oRange)
                .Select(v => min + v*nRange)
                .ToArray();
        }

        public static float[] Normalize(this float[] array, float min, float max)
        {
            var oMin = array.Min();
            var oMax = array.Max();
            var oRange = oMax - oMin;
            var nRange = max - min;

            return array
                .Select(v => (v - oMin)/oRange)
                .Select(v => min + v*nRange)
                .ToArray();
        }

        public static double[] Normalize(this int[] array, double min, double max)
        {
            double oMin = array.Min();
            double oMax = array.Max();
            double oRange = oMax - oMin;
            double nRange = max - min;

            return array
                .Select(v => (v - oMin)/oRange)
                .Select(v => min + v*nRange)
                .ToArray();
        }

        public static double[] Bias(this double[] array, double bias)
        {
            return array
                .Select(i => i + bias)
                .ToArray();
        }

        public static double[] Bias(this double[] array, decimal bias)
        {
            return array
                .Select(i => i + (double) bias)
                .ToArray();
        }
    }
}