namespace core.Graphics
{
    public static class ColorEx
    {
        public static float HueF(this hslColor clr)
        {
            return (float)clr.Hue;
        }

        public static float LuminosityF(this hslColor clr)
        {
            return (float)clr.Luminosity;
        }

        public static float SaturationF(this hslColor clr)
        {
            return (float)clr.Saturation;
        }
    }
}