using System;
using System.Drawing;
using core.Graphics;

namespace core
{
    public class Tile
    {
        public const float FOOD_GROWTH_RATE = 1.0f;

        public readonly hslColor barrenColor = Color.Brown;
        public readonly hslColor fertileColor = Color.Green;
        public readonly hslColor blackColor = Color.Black;
        public readonly hslColor waterColor = Color.DeepSkyBlue;

        public readonly float fertility;
        private readonly float maxGrowthLevel = 1.0f;
        private readonly int posX;
        private readonly int posY;

        private readonly float climateType;
        public float foodType;
        public float foodLevel;

        public Tile(int x, int y, float maxFertility, float maxFood, float foodType)
        {
            this.posX = x;
            this.posY = y;
            this.fertility = Math.Max(0, maxFertility);
            this.foodLevel = Math.Max(0, maxFood);
            this.climateType = foodType;
            this.foodType = foodType;
        }

        public void drawTile(GraphicsEngine graphics, float scaleUp, bool showEnergy)
        {
            graphics.stroke(0, 0, 0, 1);
            graphics.strokeWeight(2);
            var landColor = getColor();
            graphics.fill(landColor);
            graphics.rect(posX*scaleUp, posY*scaleUp, scaleUp, scaleUp);
            if (showEnergy)
            {
                if (landColor.LuminosityF() >= 0.7)
                {
                    graphics.fill(0, 0, 0, 1);
                }
                else
                {
                    graphics.fill(0, 0, 1, 1);
                }
                graphics.textAlign(AlignText.CENTER);
                graphics.textFont(graphics.font, 21);
                graphics.text((100*foodLevel).ToString(0, 2) + " yums", (posX + 0.5f)*scaleUp, (posY + 0.3f)*scaleUp);
                graphics.text("Clim: " + climateType.ToString(0, 2), (posX + 0.5f)*scaleUp, (posY + 0.6f)*scaleUp);
                graphics.text("Food: " + foodType.ToString(0, 2), (posX + 0.5f)*scaleUp, (posY + 0.9f)*scaleUp);
            }
        }

        public void iterate(double timeStep, float growableTime)
        {
            if (fertility > 1)
            {
                foodLevel = 0;
            }
            else
            {
                if (growableTime > 0)
                {
                    if (foodLevel < maxGrowthLevel)
                    {
                        var foodGrowthAmount =
                            (float) ((maxGrowthLevel - foodLevel)*fertility*FOOD_GROWTH_RATE*timeStep*growableTime);
                        addFood(foodGrowthAmount, climateType);
                    }
                }
                else
                {
                    foodLevel += (float) (maxGrowthLevel*foodLevel*FOOD_GROWTH_RATE*timeStep*growableTime);
                }
            }
            foodLevel = Math.Max(foodLevel, 0);
        }

        public void addFood(float amount, float addedFoodType)
        {
            foodLevel += amount;
            if (foodLevel > 0)
            {
                foodType += (addedFoodType - foodType)*(amount/foodLevel);
                // We're adding new plant growth, so we gotta "mix" the colors of the tile.
            }
        }

        public hslColor getColor()
        {
            var foodColor = new hslColor(foodType, 1, 1);
            if (fertility > 1)
            {
                return waterColor;
            }
            if (foodLevel < maxGrowthLevel)
            {
                return interColorFixedHue(interColor(barrenColor, fertileColor, fertility), foodColor,
                    foodLevel/maxGrowthLevel, foodColor.HueF());
            }
            return interColorFixedHue(foodColor, blackColor, 1.0f - maxGrowthLevel/foodLevel, foodColor.HueF());
        }

        public hslColor interColor(hslColor a, hslColor b, float x)
        {
            var hue1 = inter(a.HueF(), b.HueF(), x);
            var sat = inter(a.SaturationF(), b.SaturationF(), x);
            var bri = inter(a.LuminosityF(), b.LuminosityF(), x); // I know it's dumb to do interpolation with HSL but oh well
            return new hslColor(hue1, sat, bri);
        }

        public hslColor interColorFixedHue(hslColor a, hslColor b, float x, float hue)
        {
            var satB = b.SaturationF();
            if (b.LuminosityF() == 0)
            {
                // I want black to be calculated as 100% saturation
                satB = 1;
            }
            var sat = inter(a.SaturationF(), satB, x);
            var bri = inter(a.LuminosityF(), b.LuminosityF(), x); // I know it's dumb to do interpolation with HSL but oh well
            return new hslColor(hue, sat, bri);
        }

        public float inter(float a, float b, float x)
        {
            return a + (b - a)*x;
        }
    }
}
