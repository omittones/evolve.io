using System;
using System.Drawing;

namespace core
{
    public class hslColor
    {
        // Private data members below are on scale 0-1
        // They are scaled for use externally based on scale
        private double hue = 1.0;
        private double saturation = 1.0;
        private double luminosity = 1.0;

        private const double scale = 240.0;

        public double Hue
        {
            get { return hue*scale; }
            set { hue = CheckRange(value/scale); }
        }

        public double Saturation
        {
            get { return saturation*scale; }
            set { saturation = CheckRange(value/scale); }
        }

        public double Luminosity
        {
            get { return luminosity*scale; }
            set { luminosity = CheckRange(value/scale); }
        }

        public double Alpha { get; set; }

        private double CheckRange(double value)
        {
            if (value < 0.0)
                value = 0.0;
            else if (value > 1.0)
                value = 1.0;
            return value;
        }

        public override string ToString()
        {
            return $"H: {Hue:#0.##} S: {Saturation:#0.##} L: {Luminosity:#0.##}";
        }

        public string ToRGBString()
        {
            var color = (Color) this;
            return $"R: {color.R:#0.##} G: {color.G:#0.##} B: {color.B:#0.##}";
        }

        #region Casts to/from System.Drawing.Color

        public static implicit operator Color(hslColor hslColor)
        {
            double r = 0, g = 0, b = 0;
            if (hslColor.luminosity != 0)
            {
                if (hslColor.saturation == 0)
                    r = g = b = hslColor.luminosity;
                else
                {
                    var temp2 = GetTemp2(hslColor);
                    var temp1 = 2.0*hslColor.luminosity - temp2;

                    r = GetColorComponent(temp1, temp2, hslColor.hue + 1.0/3.0);
                    g = GetColorComponent(temp1, temp2, hslColor.hue);
                    b = GetColorComponent(temp1, temp2, hslColor.hue - 1.0/3.0);
                }
            }
            return Color.FromArgb((int) (255*hslColor.Alpha), (int) (255*r), (int) (255*g), (int) (255*b));
        }

        private static double GetColorComponent(double temp1, double temp2, double temp3)
        {
            temp3 = MoveIntoRange(temp3);
            if (temp3 < 1.0/6.0)
                return temp1 + (temp2 - temp1)*6.0*temp3;
            else if (temp3 < 0.5)
                return temp2;
            else if (temp3 < 2.0/3.0)
                return temp1 + ((temp2 - temp1)*((2.0/3.0) - temp3)*6.0);
            else
                return temp1;
        }

        private static double MoveIntoRange(double temp3)
        {
            if (temp3 < 0.0)
                temp3 += 1.0;
            else if (temp3 > 1.0)
                temp3 -= 1.0;
            return temp3;
        }

        private static double GetTemp2(hslColor hslColor)
        {
            double temp2;
            if (hslColor.luminosity < 0.5) //<=??
                temp2 = hslColor.luminosity*(1.0 + hslColor.saturation);
            else
                temp2 = hslColor.luminosity + hslColor.saturation - (hslColor.luminosity*hslColor.saturation);
            return temp2;
        }

        public static implicit operator hslColor(Color color)
        {
            var hslColor = new hslColor();
            hslColor.hue = color.GetHue()/360.0; // we store hue as 0-1 as opposed to 0-360
            hslColor.luminosity = color.GetBrightness();
            hslColor.saturation = color.GetSaturation();
            hslColor.Alpha = color.A/255.0;
            return hslColor;
        }

        #endregion

        public void SetRGB(int red, int green, int blue)
        {
            var hslColor = (hslColor) Color.FromArgb(red, green, blue);
            this.hue = hslColor.hue;
            this.saturation = hslColor.saturation;
            this.luminosity = hslColor.luminosity;
        }

        private hslColor()
        {
        }

        private hslColor(Color color)
        {
            SetRGB(color.R, color.G, color.B);
            this.Alpha = color.A/255.0;
        }

        private hslColor(int red, int green, int blue, int alpha)
        {
            SetRGB(red, green, blue);
            this.Alpha = alpha/255.0;
        }

        public hslColor(double hue, double saturation, double luminosity, double alpha = 1.0)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.Luminosity = luminosity;
            this.Alpha = alpha;
        }
    }
}