using System;

namespace core
{
    public class Tile : Helpers
    {
        public const float FOOD_GROWTH_RATE = 1.0f;

        public readonly color barrenColor = color(0, 0, 1);
        public readonly color fertileColor = color(0, 0, 0.2);
        public readonly color blackColor = color(0, 1, 0);
        public readonly color waterColor = color(0, 0, 0);
        
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

        public void drawTile(float scaleUp, bool showEnergy)
        {
            stroke(0, 0, 0, 1);
            strokeWeight(2);
            var landColor = getColor();
            fill(landColor);
            rect(posX*scaleUp, posY*scaleUp, scaleUp, scaleUp);
            if (showEnergy)
            {
                if (brightness(landColor) >= 0.7)
                {
                    fill(0, 0, 0, 1);
                }
                else
                {
                    fill(0, 0, 1, 1);
                }
                textAlign(AlignText.CENTER);
                textFont(font, 21);
                text((100*foodLevel).ToString(0, 2) + " yums", (posX + 0.5f)*scaleUp, (posY + 0.3f)*scaleUp);
                text("Clim: " + climateType.ToString(0, 2), (posX + 0.5f)*scaleUp, (posY + 0.6f)*scaleUp);
                text("Food: " + foodType.ToString(0, 2), (posX + 0.5f)*scaleUp, (posY + 0.9f)*scaleUp);
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

        public color getColor()
        {
            var foodColor = color(foodType, 1, 1);
            if (fertility > 1)
            {
                return waterColor;
            }
            if (foodLevel < maxGrowthLevel)
            {
                return interColorFixedHue(interColor(barrenColor, fertileColor, fertility), foodColor,
                    foodLevel/maxGrowthLevel, hue(foodColor));
            }
            return interColorFixedHue(foodColor, blackColor, 1.0f - maxGrowthLevel/foodLevel, hue(foodColor));
        }

        public color interColor(color a, color b, float x)
        {
            var hue1 = inter(hue(a), hue(b), x);
            var sat = inter(saturation(a), saturation(b), x);
            var bri = inter(brightness(a), brightness(b), x); // I know it's dumb to do interpolation with HSL but oh well
            return color(hue1, sat, bri);
        }

        public color interColorFixedHue(color a, color b, float x, float hue)
        {
            var satB = saturation(b);
            if (brightness(b) == 0)
            {
                // I want black to be calculated as 100% saturation
                satB = 1;
            }
            var sat = inter(saturation(a), satB, x);
            var bri = inter(brightness(a), brightness(b), x); // I know it's dumb to do interpolation with HSL but oh well
            return color(hue, sat, bri);
        }

        public float inter(float a, float b, float x)
        {
            return a + (b - a)*x;
        }
    }
}
