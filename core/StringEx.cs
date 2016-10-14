using System;
using System.Linq;

namespace core
{
    public static class StringEx
    {
        public static string ToString(this float num, int lead, int trail)
        {
            var trailf = new string('0', trail);
            return num.ToString("0." + trailf);
        }

        public static string ToString(this double num, int lead, int trail)
        {
            var trailf = new string('0', trail);
            return num.ToString("0." + trailf);
        }

        public static string ToString(this int num, int trail)
        {
            return num.ToString().PadLeft(trail);
        }

        public static string Substr(this string value, int start, int end)
        {
            if (end >= value.Length)
                end = value.Length - 1;
            return value.Substring(start, end - start + 1);
        }

        public static void OutputToConsole(this double[] values)
        {
            var text = string.Join(", ", values.Select(v => v.ToString("0.00")));
            Console.WriteLine(text);
        }

        public static string Capitalize(this string n)
        {
            return n.Substring(0, 1).ToUpper() + n.Substr(1, n.Length);
        }
    }
}