namespace Util
{
    public static class FloatExtensions
    {
        public static bool IsInside(this float num, float min, float max)
        {
            return num <= max && num >= min;
        }
    }
}