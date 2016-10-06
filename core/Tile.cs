using System;

namespace core
{
    public class Tile
    {
        public const float FOOD_GROWTH_RATE = 1.0f;

        public readonly hslColor barrenColor = new hslColor(0, 0, 1);
        public readonly hslColor fertileColor = new hslColor(0, 0, 0.2f);
        public readonly hslColor blackColor = new hslColor(0, 1, 0);
        public readonly hslColor waterColor = new hslColor(0, 0, 0);

        public readonly float fertility;
        private readonly float maxGrowthLevel = 1.0f;
        private readonly GraphicsEngine graphics;
        private readonly int posX;
        private readonly int posY;

        private readonly float climateType;
        public float foodType;
        public float foodLevel;

        public Tile(GraphicsEngine graphics, int x, int y, float maxFertility, float maxFood, float foodType)
        {
            this.graphics = graphics;
            this.posX = x;
            this.posY = y;
            this.fertility = Math.Max(0, maxFertility);
            this.foodLevel = Math.Max(0, maxFood);
            this.climateType = foodType;
            this.foodType = foodType;
        }

        public void drawTile(float scaleUp, bool showEnergy)
        {
            this.graphics.stroke(0, 0, 0, 1);
            this.graphics.strokeWeight(2);
            var landColor = getColor();
            this.graphics.fill(landColor);
            this.graphics.rect(posX*scaleUp, posY*scaleUp, scaleUp, scaleUp);
            if (showEnergy)
            {
                if (this.graphics.brightness(landColor) >= 0.7)
                {
                    this.graphics.fill(0, 0, 0, 1);
                }
                else
                {
                    this.graphics.fill(0, 0, 1, 1);
                }
                this.graphics.textAlign(AlignText.CENTER);
                this.graphics.textFont(this.graphics.font, 21);
                this.graphics.text((100*foodLevel).ToString(0, 2) + " yums", (posX + 0.5f)*scaleUp, (posY + 0.3f)*scaleUp);
                this.graphics.text("Clim: " + climateType.ToString(0, 2), (posX + 0.5f)*scaleUp, (posY + 0.6f)*scaleUp);
                this.graphics.text("Food: " + foodType.ToString(0, 2), (posX + 0.5f)*scaleUp, (posY + 0.9f)*scaleUp);
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
                    foodLevel/maxGrowthLevel, this.graphics.hue(foodColor));
            }
            return interColorFixedHue(foodColor, blackColor, 1.0f - maxGrowthLevel/foodLevel, this.graphics.hue(foodColor));
        }

        public hslColor interColor(hslColor a, hslColor b, float x)
        {
            var hue1 = inter(this.graphics.hue(a), this.graphics.hue(b), x);
            var sat = inter(this.graphics.saturation(a), this.graphics.saturation(b), x);
            var bri = inter(this.graphics.brightness(a), this.graphics.brightness(b), x); // I know it's dumb to do interpolation with HSL but oh well
            return new hslColor(hue1, sat, bri);
        }

        public hslColor interColorFixedHue(hslColor a, hslColor b, float x, float hue)
        {
            var satB = this.graphics.saturation(b);
            if (this.graphics.brightness(b) == 0)
            {
                // I want black to be calculated as 100% saturation
                satB = 1;
            }
            var sat = inter(this.graphics.saturation(a), satB, x);
            var bri = inter(this.graphics.brightness(a), this.graphics.brightness(b), x); // I know it's dumb to do interpolation with HSL but oh well
            return new hslColor(hue, sat, bri);
        }

        public float inter(float a, float b, float x)
        {
            return a + (b - a)*x;
        }
    }
}
