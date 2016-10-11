using System;

namespace Util
{
    public class RandomEx : Random
    {
        public static RandomEx Global = new RandomEx();

        public RandomEx() : base(DateTime.Now.Millisecond)
        {
        }

        public RandomEx(int seed) : base(seed)
        {
        }

        public int RouletteSelection(int excludedMaximum)
        {
            var choice = this.NextDouble();
            choice = Math.Pow(choice, 1.4f);
            return (int) Math.Floor(excludedMaximum*choice);
        }

        public sbyte GetRandomSign()
        {
            if (this.NextDouble() < 0.5)
                return -1;
            else
                return 1;
        }

        //vraća random vrijednost
        //0-9, 10-99,100-999,1000-9999 imaju istu vjerojatnost da se pojave
        //negativni također       
        public double GetUniformChange(int inMaxOrder, bool inAllowNegative)
        {
            bool isNegative = (this.NextDouble() < 0.5) && inAllowNegative;
            int order = this.Next(inMaxOrder);

            //odredi base vrijednost
            double value = this.NextDouble();
            if (isNegative) value = -value;

            //povećaj red vrijednosti
            for (int cOrder = 0; cOrder < order; cOrder++)
                value = value*10;

            return value;
        }

        public float NextF(float min, float max)
        {
            return (float) (min + this.NextDouble()*(max - min));
        }
    }
}
