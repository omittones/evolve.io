namespace core
{
    public static class StringEx
    {
        public static string ToString(this float num, int lead, int trail)
        {
            var trailf = new string('0', trail);
            return num.ToString("0." + trailf);
        }

        public static string ToString(this int num, int trail)
        {
            return num.ToString().PadLeft(trail);
        }
    }
}