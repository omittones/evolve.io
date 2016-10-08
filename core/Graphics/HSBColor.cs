using System;
using System.Drawing;

namespace core.Graphics
{
    public struct HSBColor
    {
        const float rgbMax = 255.0f;

        public float Hue;
        public float Saturation;
        public float Brightness;
        public float Alpha;

        public static implicit operator Color(HSBColor hsbColor)
        {
            return hsbColor.ToColor();
        }

        public static implicit operator HSBColor(Color color)
        {
            return HSBColor.FromColor(color);
        }

        public HSBColor(float hue, float saturation, float brightness, float alpha)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.Brightness = brightness;
            this.Alpha = alpha;
        }

        public HSBColor(float hue, float saturation, float brightness)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.Brightness = brightness;
            this.Alpha = 1f;
        }

        public HSBColor(Color col)
        {
            var hSlColor = HSBColor.FromColor(col);

            this.Hue = hSlColor.Hue;
            this.Saturation = hSlColor.Saturation;
            this.Brightness = hSlColor.Brightness;
            this.Alpha = hSlColor.Alpha;
        }

        public static HSBColor FromColor(Color color)
        {
            var r = color.R / rgbMax;
            var g = color.G / rgbMax;
            var b = color.B / rgbMax;
            var result = new HSBColor(0f, 0f, 0f, color.A/ rgbMax);

            var num = Math.Max(r, Math.Max(g, b));
            if (num <= 0f)
            {
                return result;
            }
            var num2 = Math.Min(r, Math.Min(g, b));
            var num3 = num - num2;
            if (num > num2)
            {
                if (g == num)
                {
                    result.Hue = (b - r) / num3 * 60f + 120f;
                }
                else if (b == num)
                {
                    result.Hue = (r - g) / num3 * 60f + 240f;
                }
                else if (b > g)
                {
                    result.Hue = (g - b) / num3 * 60f + 360f;
                }
                else
                {
                    result.Hue = (g - b) / num3 * 60f;
                }
                if (result.Hue < 0f)
                {
                    result.Hue += 360f;
                }
            }
            else
            {
                result.Hue = 0f;
            }
            result.Hue *= 0.00277777785f;
            result.Saturation = num3 / num * 1f;
            result.Brightness = num;
            return result;
        }

        public static Color ToColor(HSBColor hsbColor)
        {
            var red = hsbColor.Brightness;
            var blue = hsbColor.Brightness;
            var green = hsbColor.Brightness;
            if (hsbColor.Saturation == 0f)
            {
                return Color.FromArgb(MathEx.From01To255(hsbColor.Alpha),
                    MathEx.From01To255(red),
                    MathEx.From01To255(blue),
                    MathEx.From01To255(green));
            }

            var b = hsbColor.Brightness;
            var num = hsbColor.Brightness*hsbColor.Saturation;
            var num2 = hsbColor.Brightness - num;
            var num3 = hsbColor.Hue*360f;
            if (num3 < 60f)
            {
                red = b;
                blue = num3*num/60f + num2;
                green = num2;
            }
            else if (num3 < 120f)
            {
                red = -(num3 - 120f)*num/60f + num2;
                blue = b;
                green = num2;
            }
            else if (num3 < 180f)
            {
                red = num2;
                blue = b;
                green = (num3 - 120f)*num/60f + num2;
            }
            else if (num3 < 240f)
            {
                red = num2;
                blue = -(num3 - 240f)*num/60f + num2;
                green = b;
            }
            else if (num3 < 300f)
            {
                red = (num3 - 240f)*num/60f + num2;
                blue = num2;
                green = b;
            }
            else if (num3 <= 360f)
            {
                red = b;
                blue = num2;
                green = -(num3 - 360f)*num/60f + num2;
            }
            else
            {
                red = 0f;
                blue = 0f;
                green = 0f;
            }
            return Color.FromArgb(MathEx.From01To255(hsbColor.Alpha),
                MathEx.From01To255(red),
                MathEx.From01To255(blue),
                MathEx.From01To255(green));
        }

        public Color ToColor()
        {
            return HSBColor.ToColor(this);
        }

        public override string ToString()
        {
            return string.Concat(new object[]
            {
            "H:",
            this.Hue,
            " S:",
            this.Saturation,
            " B:",
            this.Brightness
            });
        }

        public static HSBColor Lerp(HSBColor a, HSBColor b, float t)
        {
            float h;
            float s;
            if (a.Brightness == 0f)
            {
                h = b.Hue;
                s = b.Saturation;
            }
            else if (b.Brightness == 0f)
            {
                h = a.Hue;
                s = a.Saturation;
            }
            else
            {
                if (a.Saturation == 0f)
                {
                    h = b.Hue;
                }
                else if (b.Saturation == 0f)
                {
                    h = a.Hue;
                }
                else
                {
                    float num;
                    for (num = MathEx.LerpAngle(a.Hue*360f, b.Hue*360f, t); num < 0f; num += 360f)
                    {
                    }
                    while (num > 360f)
                    {
                        num -= 360f;
                    }
                    h = num/360f;
                }
                s = MathEx.Lerp(a.Saturation, b.Saturation, t);
            }

            return new HSBColor(h, s, MathEx.Lerp(a.Brightness, b.Brightness, t), MathEx.Lerp(a.Alpha, b.Alpha, t));
        }
    }
}