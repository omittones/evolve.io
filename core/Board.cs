using System;
using System.Collections.Generic;

namespace core
{
    public class Board : Helpers
    {
        public bool userControl;
        public bool wasPressingB;
        public color buttonColor = color(0.82, 0.8, 0.7);
        public const int LIST_SLOTS = 6;
        public Creature selectedCreature;
        public Creature[] list = new Creature[LIST_SLOTS];
        public double imageSaveInterval = 1;
        public double MANUAL_BIRTH_SIZE = 1.2;
        public double recordPopulationEvery = 0.02;
        public double textSaveInterval = 1;
        public double timeStep;
        public double year;
        public double[] fileSaveTimes;

        public double[] letterFrequencies =
        {
            8.167, 1.492, 2.782, 4.253, 12.702, 2.228, 2.015, 6.094, 6.966, 0.153, 0.772,
            4.025, 2.406, 6.749, 7.507, 1.929, 0.095, 5.987, 6.327, 9.056, 2.758, 0.978, 2.361, 0.150, 1.974, 10000.0
        };

        public float MAX_TEMPERATURE;
        public float MIN_TEMPERATURE;
        public float temperature;
        public int creatureRankMetric = 0;
        public int playSpeed = 1;
        public int POPULATION_HISTORY_LENGTH = 200;
        public int[] fileSaveCounts;
        public int[] populationHistory;
        public List<Creature> creatures;
        public List<SoftBody> rocks;
        public int boardHeight;
        public int boardWidth;
        public int creatureIDUpTo = 0;
        public int creatureMinimum;
        public List<SoftBody>[,] softBodiesInPositions;
        public readonly color BACKGROUND_COLOR = color(0, 0, 0.1);
        public readonly color ROCK_COLOR = color(0, 0, 0.5);
        public readonly double FLASH_SPEED = 80;
        public const float MAX_CREATURE_ENERGY = 2.0f;
        public const float MAX_ROCK_ENERGY_BASE = 1.6f;
        public const float MIN_CREATURE_ENERGY = 1.2f;
        public const float MIN_ROCK_ENERGY_BASE = 0.8f;
        public const float MINIMUM_SURVIVABLE_SIZE = 0.2f;
        public const float OBJECT_TIMESTEPS_PER_YEAR = 100;
        public const float ROCK_DENSITY = 5;
        public const float THERMOMETER_MAX = 2;
        public const float THERMOMETER_MIN = -2;
        public readonly int creatureMinimumIncrement = 5;
        public readonly int ROCKS_TO_ADD;
        public string folder = "TEST";
        public Tile[,] tiles;

        public Board(int w, int h, float stepSize, float minTemp, float maxTemp, int rocksToAdd,
            int minimumCreatures, int seed, string initialFileName, double timeStep)
        {
            Rnd.noiseSeed(seed);
            Rnd.randomSeed(seed);
            boardWidth = w;
            boardHeight = h;
            tiles = new Tile[w, h];
            for (var x = 0; x < boardWidth; x++)
            {
                for (var y = 0; y < boardHeight; y++)
                {
                    var bigForce = Math.Pow(((float) y)/boardHeight, 0.5);
                    var fertility =
                        (float)
                            (Rnd.noise(x*stepSize*3, y*stepSize*3)*(1 - bigForce)*5.0 +
                             Rnd.noise(x*stepSize*0.5, y*stepSize*0.5)*bigForce*5.0 - 1.5);
                    var climateType = (float) (Rnd.noise(x*stepSize + 10000, y*stepSize + 10000)*1.63 - 0.4);
                    climateType = min(max(climateType, 0), 0.8f);
                    tiles[x, y] = new Tile(x, y, fertility, 0, climateType);
                }
            }
            MIN_TEMPERATURE = minTemp;
            MAX_TEMPERATURE = maxTemp;

            softBodiesInPositions = new List<SoftBody>[boardWidth, boardHeight];
            for (var x = 0; x < boardWidth; x++)
            {
                for (var y = 0; y < boardHeight; y++)
                {
                    softBodiesInPositions[x, y] = new List<SoftBody>();
                }
            }

            ROCKS_TO_ADD = rocksToAdd;
            rocks = new List<SoftBody>(0);
            for (var i = 0; i < ROCKS_TO_ADD; i++)
            {
                rocks.Add(new SoftBody(Rnd.Next(0, boardWidth), Rnd.Next(0, boardHeight), 0, 0,
                    getRandomSize(), ROCK_DENSITY, hue(ROCK_COLOR), saturation(ROCK_COLOR), brightness(ROCK_COLOR), this,
                    year));
            }

            creatureMinimum = minimumCreatures;
            creatures = new List<Creature>(0);
            maintainCreatureMinimum(false);
            for (var i = 0; i < LIST_SLOTS; i++)
            {
                list[i] = null;
            }
            folder = initialFileName;
            fileSaveCounts = new int[4];
            fileSaveTimes = new double[4];
            for (var i = 0; i < 4; i++)
            {
                fileSaveCounts[i] = 0;
                fileSaveTimes[i] = -999;
            }
            userControl = true;
            this.timeStep = timeStep;
            populationHistory = new int[POPULATION_HISTORY_LENGTH];
            for (var i = 0; i < POPULATION_HISTORY_LENGTH; i++)
            {
                populationHistory[i] = 0;
            }
        }

        public void drawBoard(float scaleUp, float camZoom, int mX, int mY)
        {
            for (var x = 0; x < boardWidth; x++)
            {
                for (var y = 0; y < boardHeight; y++)
                {
                    tiles[x, y].drawTile(scaleUp, (mX == x && mY == y));
                }
            }
            for (var i = 0; i < rocks.Count; i++)
            {
                rocks[i].drawSoftBody(scaleUp);
            }
            for (var i = 0; i < creatures.Count; i++)
            {
                creatures[i].drawSoftBody(scaleUp, camZoom, true);
            }
        }

        public void drawBlankBoard(float scaleUp)
        {
            fill(BACKGROUND_COLOR);
            rect(0, 0, scaleUp*boardWidth, scaleUp*boardHeight);
        }

        public void drawUI(float scaleUp, double timeStep, int x1, int y1, int x2, int y2, PFont font)
        {
            fill(0, 0, 0);
            noStroke();
            rect(x1, y1, x2 - x1, y2 - y1);

            pushMatrix();
            translate(x1, y1);

            fill(0, 0, 1);
            textAlign(AlignText.LEFT);
            textFont(font, 48);
            var yearText = "Year " + ((float) year).ToString(0, 2);
            text(yearText, 10, 48);
            var seasonTextXCoor = textWidth(yearText) + 50;
            textFont(font, 24);
            text("Population: " + creatures.Count, 10, 80);
            string[] seasons = {"Winter", "Spring", "Summer", "Autumn"};
            text(seasons[(int) (getSeason()*4)], seasonTextXCoor, 30);

            if (selectedCreature == null)
            {
                for (var i = 0; i < LIST_SLOTS; i++)
                {
                    list[i] = null;
                }
                for (var i = 0; i < creatures.Count; i++)
                {
                    var lookingAt = 0;
                    if (creatureRankMetric == 4)
                    {
                        while (lookingAt < LIST_SLOTS && list[lookingAt] != null &&
                               list[lookingAt].name.CompareTo(creatures[i].name) < 0)
                        {
                            lookingAt++;
                        }
                    }
                    else if (creatureRankMetric == 5)
                    {
                        while (lookingAt < LIST_SLOTS && list[lookingAt] != null &&
                               list[lookingAt].name.CompareTo(creatures[i].name) >= 0)
                        {
                            lookingAt++;
                        }
                    }
                    else
                    {
                        while (lookingAt < LIST_SLOTS && list[lookingAt] != null &&
                               list[lookingAt].measure(creatureRankMetric) > creatures[i].measure(creatureRankMetric))
                        {
                            lookingAt++;
                        }
                    }
                    if (lookingAt < LIST_SLOTS)
                    {
                        for (var j = LIST_SLOTS - 1; j >= lookingAt + 1; j--)
                        {
                            list[j] = list[j - 1];
                        }
                        list[lookingAt] = creatures[i];
                    }
                }
                double maxEnergy = 0;
                for (var i = 0; i < LIST_SLOTS; i++)
                {
                    if (list[i] != null && list[i].energy > maxEnergy)
                    {
                        maxEnergy = list[i].energy;
                    }
                }
                for (var i = 0; i < LIST_SLOTS; i++)
                {
                    if (list[i] != null)
                    {
                        list[i].preferredRank += (i - list[i].preferredRank)*0.4f;
                        var y = y1 + 175 + 70*list[i].preferredRank;
                        drawCreature(list[i], 45, y + 5, 0.7f, scaleUp);
                        textFont(font, 24);
                        textAlign(AlignText.LEFT);
                        noStroke();
                        fill(0.333f, 1, 0.4f);
                        float multi = (x2 - x1 - 200);
                        if (list[i].energy > 0)
                        {
                            rect(85, y + 5, (float) (multi*list[i].energy/maxEnergy), 25);
                        }
                        if (list[i].energy > 1)
                        {
                            fill(0.333f, 1, 0.8f);
                            rect(85 + (float) (multi/maxEnergy), y + 5, (float) (multi*(list[i].energy - 1)/maxEnergy), 25);
                        }
                        fill(0, 0, 1);
                        text(list[i].getCreatureName() + " [" + list[i].id + "] (" + toAge(list[i].birthTime) + ")", 90, y);
                        text("Energy: " + (100*(float) (list[i].energy)).ToString(0, 2), 90, y + 25);
                    }
                }
                noStroke();
                fill(buttonColor);
                rect(10, 95, 220, 40);
                rect(240, 95, 220, 40);
                fill(0, 0, 1);
                textAlign(AlignText.CENTER);
                text("Reset zoom", 120, 123);
                string[] sorts =
                {
                    "Biggest", "Smallest", "Youngest", "Oldest", "A to Z", "Z to A", "Highest Gen",
                    "Lowest Gen"
                };
                text("Sort by: " + sorts[creatureRankMetric], 350, 123);

                textFont(font, 19);
                string[] buttonTexts =
                {
                    "Brain Control", "Maintain pop. at " + creatureMinimum,
                    "Screenshot now", "-   Image every " + ((float) imageSaveInterval).ToString(0, 2) + " years   +",
                    "Text file now", "-    Text every " + ((float) textSaveInterval).ToString(0, 2) + " years    +",
                    "-    Play Speed (" + playSpeed + "x)    +", "This button does nothing"
                };
                if (userControl)
                {
                    buttonTexts[0] = "Keyboard Control";
                }
                for (var i = 0; i < 8; i++)
                {
                    float x = (i%2)*230 + 10;
                    var y = (float) (Math.Floor(i/2.0)*50 + 570);
                    fill(buttonColor);
                    rect(x, y, 220, 40);
                    if (i >= 2 && i < 6)
                    {
                        var flashAlpha = 1.0*Math.Pow(0.5, (year - fileSaveTimes[i - 2])*FLASH_SPEED);
                        fill(0, 0, 1, (float) flashAlpha);
                        rect(x, y, 220, 40);
                    }
                    fill(0, 0, 1, 1);
                    text(buttonTexts[i], x + 110, y + 17);
                    if (i == 0)
                    {
                    }
                    else if (i == 1)
                    {
                        text("-" + creatureMinimumIncrement +
                             "                    +" + creatureMinimumIncrement, x + 110, y + 37);
                    }
                    else if (i <= 5)
                    {
                        text(getNextFileName(i - 2), x + 110, y + 37);
                    }
                }
            }
            else
            {
                var energyUsage = (float) selectedCreature.getEnergyUsage(timeStep);
                noStroke();
                if (energyUsage <= 0)
                {
                    fill(0, 1, 0.5f);
                }
                else
                {
                    fill(0.33f, 1, 0.4f);
                }
                var EUbar = 6*energyUsage;
                rect(110, 280, min(max(EUbar, -110), 110), 25);
                if (EUbar < -110)
                {
                    rect(0, 280, 25, (-110 - EUbar)*20 + 25);
                }
                else if (EUbar > 110)
                {
                    var h = (EUbar - 110)*20 + 25;
                    rect(185, 280 - h, 25, h);
                }
                fill(0, 0, 1);
                text("Name: " + selectedCreature.getCreatureName(), 10, 225);
                text("Energy: " + (100*(float) selectedCreature.energy).ToString(0, 2) + " yums", 10, 250);
                text("E Change: " + (100*energyUsage).ToString(0, 2) + " yums/year", 10, 275);

                text("ID: " + selectedCreature.id, 10, 325);
                text("X: " + ((float) selectedCreature.px).ToString(0, 2), 10, 350);
                text("Y: " + ((float) selectedCreature.py).ToString(0, 2), 10, 375);
                text("Rotation: " + ((float) selectedCreature.rotation).ToString(0, 2), 10, 400);
                text("B-day: " + toDate(selectedCreature.birthTime), 10, 425);
                text("(" + toAge(selectedCreature.birthTime) + ")", 10, 450);
                text("Generation: " + selectedCreature.gen, 10, 475);
                text("Parents: " + selectedCreature.parents, 10, 500, 210, 255);
                text("Hue: " + ((float) (selectedCreature.myHue)).ToString(0, 2), 10, 550, 210, 255);
                text("Mouth hue: " + ((float) (selectedCreature.mouthHue)).ToString(0, 2), 10, 575, 210, 255);

                if (userControl)
                {
                    text("Controls:\nUp/Down: Move\nLeft/Right: Rotate\nSpace: Eat\nF: Fight\nV: Vomit\nU,J: Change color" +
                         "\nI,K: Change mouth color\nB: Give birth (Not possible if under " +
                         Math.Round((MANUAL_BIRTH_SIZE + 1)*100) + " yums)", 10, 625, 250, 400);
                }
                pushMatrix();
                translate(400, 80);
                var apX = (float) Math.Round((mouseX - 264 - x1)/26.0);
                var apY = (float) Math.Round((mouseY - 80 - y1)/26.0);
                selectedCreature.drawBrain(font, 52, (int) apX, (int) apY);
                popMatrix();
            }
            drawPopulationGraph(x1, x2, y1, y2);
            fill(0, 0, 0);
            textAlign(AlignText.RIGHT);
            textFont(font, 24);
            text("Population: " + creatures.Count, x2 - x1 - 10, y2 - y1 - 10);
            popMatrix();

            pushMatrix();
            translate(x2, y1);
            textAlign(AlignText.RIGHT);
            textFont(font, 24);
            text("Temperature", -10, 24);
            drawThermometer(-45, 30, 20, 660, temperature, THERMOMETER_MIN, THERMOMETER_MAX, color(0, 1, 1));
            popMatrix();

            if (selectedCreature != null)
            {
                drawCreature(selectedCreature, x1 + 65, y1 + 147, 0.7f, scaleUp);
            }
        }

        private void drawPopulationGraph(float x1, float x2, float y1, float y2)
        {
            var barWidth = (x2 - x1)/POPULATION_HISTORY_LENGTH;
            noStroke();
            fill(0.33333f, 1, 0.6f);
            var maxPopulation = 0;
            for (var i = 0; i < POPULATION_HISTORY_LENGTH; i++)
            {
                if (populationHistory[i] > maxPopulation)
                {
                    maxPopulation = populationHistory[i];
                }
            }
            for (var i = 0; i < POPULATION_HISTORY_LENGTH; i++)
            {
                var h = (((float) populationHistory[i])/maxPopulation)*(y2 - 770);
                rect((POPULATION_HISTORY_LENGTH - 1 - i)*barWidth, y2 - h, barWidth, h);
            }
        }

        private string getNextFileName(int type)
        {
            string[] modes = {"manualImgs", "autoImgs", "manualTexts", "autoTexts"};
            var ending = ".png";
            if (type >= 2)
            {
                ending = ".txt";
            }
            return folder + "/" + modes[type] + "/" + fileSaveCounts[type].ToString(5) + ending;
        }

        public void iterate(double timeStep)
        {
            var prevYear = year;
            year += timeStep;
            if (Math.Floor(year/recordPopulationEvery) != Math.Floor(prevYear/recordPopulationEvery))
            {
                for (var i = POPULATION_HISTORY_LENGTH - 1; i >= 1; i--)
                {
                    populationHistory[i] = populationHistory[i - 1];
                }
                populationHistory[0] = creatures.Count;
            }
            temperature = getGrowableTime();
            for (var x = 0; x < boardWidth; x++)
            {
                for (var y = 0; y < boardHeight; y++)
                {
                    tiles[x, y].iterate(timeStep, getGrowableTime());
                }
            }
            for (var i = 0; i < creatures.Count; i++)
            {
                creatures[i].setPreviousEnergy();
            }
            for (var i = 0; i < rocks.Count; i++)
            {
                rocks[i].collide(timeStep*OBJECT_TIMESTEPS_PER_YEAR);
            }
            maintainCreatureMinimum(false);
            for (var i = 0; i < creatures.Count; i++)
            {
                var me = creatures[i];
                me.collide(timeStep*OBJECT_TIMESTEPS_PER_YEAR);
                me.metabolize(timeStep*OBJECT_TIMESTEPS_PER_YEAR);
                if (userControl)
                {
                    if (me == selectedCreature)
                    {
                        if (keyPressed)
                        {
                            if (key == Key.CODED)
                            {
                                if (keyCode == Key.UP) me.accelerate(0.3, timeStep*OBJECT_TIMESTEPS_PER_YEAR);
                                if (keyCode == Key.DOWN) me.accelerate(-0.3, timeStep*OBJECT_TIMESTEPS_PER_YEAR);
                                if (keyCode == Key.LEFT) me.turn(-0.3, timeStep*OBJECT_TIMESTEPS_PER_YEAR);
                                if (keyCode == Key.RIGHT) me.turn(0.3, timeStep*OBJECT_TIMESTEPS_PER_YEAR);
                            }
                            else
                            {
                                if (key == ' ') me.eat(0.3, timeStep*OBJECT_TIMESTEPS_PER_YEAR);
                                if (key == 'v') me.eat(-0.3, timeStep*OBJECT_TIMESTEPS_PER_YEAR);
                                if (key == 'f') me.fight(0.3, timeStep*OBJECT_TIMESTEPS_PER_YEAR);
                                if (key == 'u') me.setHue(me.myHue + 0.02);
                                if (key == 'j') me.setHue(me.myHue - 0.02);

                                if (key == 'i') me.setMouthHue(me.mouthHue + 0.02);
                                if (key == 'k') me.setMouthHue(me.mouthHue - 0.02);
                                /*if(key == 'i') me.setSaturarion(me.saturation+0.05);
              if(key == 'k') me.setSaturarion(me.saturation-0.05);
              if(key == 'o') me.setBrightness(me.brightness+0.05);
              if(key == 'l') me.setBrightness(me.brightness-0.05);


              if(key == 'w') me.setVisionDistance(me.visionDistance+0.05);
              if(key == 's') me.setVisionDistance(me.visionDistance-0.05);
              if(key == 'a') me.setVisionAngle(me.visionAngle-0.05);
              if(key == 'd') me.setVisionAngle(me.visionAngle+0.05);*/
                                if (key == 'b')
                                {
                                    if (!wasPressingB)
                                    {
                                        me.reproduce(MANUAL_BIRTH_SIZE, timeStep);
                                    }
                                    wasPressingB = true;
                                }
                                else
                                {
                                    wasPressingB = false;
                                }
                            }
                        }
                    }
                }
                me.useBrain(timeStep*OBJECT_TIMESTEPS_PER_YEAR, !userControl);
                if (me.getRadius() < MINIMUM_SURVIVABLE_SIZE)
                {
                    me.returnToEarth();
                    creatures.Remove(me);
                    i--;
                }
            }

            for (var i = 0; i < rocks.Count; i++)
            {
                rocks[i].applyMotions(timeStep*OBJECT_TIMESTEPS_PER_YEAR);
            }

            for (var i = 0; i < creatures.Count; i++)
            {
                creatures[i].applyMotions(timeStep*OBJECT_TIMESTEPS_PER_YEAR);
                creatures[i].see();
            }

            if (Math.Floor(fileSaveTimes[1]/imageSaveInterval) != Math.Floor(year/imageSaveInterval))
            {
                prepareForFileSave(1);
            }

            if (Math.Floor(fileSaveTimes[3]/textSaveInterval) != Math.Floor(year/textSaveInterval))
            {
                prepareForFileSave(3);
            }
        }

        private float getGrowableTime()
        {
            var temperatureRange = MAX_TEMPERATURE - MIN_TEMPERATURE;
            return
                (float)
                    (MIN_TEMPERATURE + temperatureRange*0.5 - temperatureRange*0.5*Math.Cos((float) (getSeason()*2*Math.PI)));
        }

        private double getSeason()
        {
            return (year%1.0);
        }

        private void drawThermometer(float x1, float y1, float w, float h, float prog, float min, float max,
            color fillColor)
        {
            noStroke();
            fill(0, 0, 0.2f);
            rect(x1, y1, w, h);
            fill(fillColor);
            var proportionFilled = (prog - min)/(max - min);
            rect(x1, y1 + h*(1 - proportionFilled), w, proportionFilled*h);

            var zeroHeight = (0 - min)/(max - min);
            var zeroLineY = y1 + h*(1 - zeroHeight);
            textAlign(AlignText.RIGHT);
            stroke(0, 0, 1);
            strokeWeight(3);
            line(x1, zeroLineY, x1 + w, zeroLineY);
            var minY = y1 + h*(1 - (MIN_TEMPERATURE - min)/(max - min));
            var maxY = y1 + h*(1 - (MAX_TEMPERATURE - min)/(max - min));
            fill(0, 0, 0.8f);
            line(x1, minY, x1 + w*1.8, minY);
            line(x1, maxY, x1 + w*1.8, maxY);
            line(x1 + w*1.8, minY, x1 + w*1.8, maxY);

            fill(0, 0, 1);
            text("Zero", x1 - 5, zeroLineY + 8);
            text(MIN_TEMPERATURE.ToString(0, 2), x1 - 5, minY + 8);
            text(MAX_TEMPERATURE.ToString(0, 2), x1 - 5, maxY + 8);
        }

        private void drawVerticalSlider(float x1, float y1, float w, float h, float prog, color fillColor, color antiColor)
        {
            noStroke();
            fill(0, 0, 0.2f);
            rect(x1, y1, w, h);
            if (prog >= 0)
            {
                fill(fillColor);
            }
            else
            {
                fill(antiColor);
            }
            rect(x1, y1 + h*(1 - prog), w, prog*h);
        }

        public bool setMinTemperature(float temp)
        {
            MIN_TEMPERATURE = tempBounds(THERMOMETER_MIN + temp*(THERMOMETER_MAX - THERMOMETER_MIN));
            if (MIN_TEMPERATURE > MAX_TEMPERATURE)
            {
                var placeHolder = MAX_TEMPERATURE;
                MAX_TEMPERATURE = MIN_TEMPERATURE;
                MIN_TEMPERATURE = placeHolder;
                return true;
            }
            return false;
        }

        public bool setMaxTemperature(float temp)
        {
            MAX_TEMPERATURE = tempBounds(THERMOMETER_MIN + temp*(THERMOMETER_MAX - THERMOMETER_MIN));
            if (MIN_TEMPERATURE > MAX_TEMPERATURE)
            {
                var placeHolder = MAX_TEMPERATURE;
                MAX_TEMPERATURE = MIN_TEMPERATURE;
                MIN_TEMPERATURE = placeHolder;
                return true;
            }
            return false;
        }

        private float tempBounds(float temp)
        {
            return min(max(temp, THERMOMETER_MIN), THERMOMETER_MAX);
        }

        public float getHighTempProportion()
        {
            return (MAX_TEMPERATURE - THERMOMETER_MIN)/(THERMOMETER_MAX - THERMOMETER_MIN);
        }

        public float getLowTempProportion()
        {
            return (MIN_TEMPERATURE - THERMOMETER_MIN)/(THERMOMETER_MAX - THERMOMETER_MIN);
        }

        private string toDate(double d)
        {
            return "Year " + ((float) (d)).ToString(0, 2);
        }

        private string toAge(double d)
        {
            return ((float) (year - d)).ToString(0, 2) + " yrs old";
        }

        private void maintainCreatureMinimum(bool choosePreexisting)
        {
            while (creatures.Count < creatureMinimum)
            {
                if (choosePreexisting)
                {
                    var c = getRandomCreature();
                    c.addEnergy(Creature.SAFE_SIZE);
                    c.reproduce(Creature.SAFE_SIZE, timeStep);
                }
                else
                {
                    creatures.Add(new Creature(Rnd.Next(0, boardWidth), Rnd.Next(0, boardHeight), 0, 0,
                        Rnd.Next(MIN_CREATURE_ENERGY, MAX_CREATURE_ENERGY), 1, Rnd.Next(0, 1), 1, 1,
                        this, year, Rnd.Next(0, 2*Math.PI), 0, "", "[PRIMORDIAL]", true, null, null, 1, Rnd.Next(0, 1)));
                }
            }
        }

        private Creature getRandomCreature()
        {
            var index = Rnd.Next(0, creatures.Count);
            return creatures[index];
        }

        private double getRandomSize()
        {
            return Math.Pow(Rnd.Next(MIN_ROCK_ENERGY_BASE, MAX_ROCK_ENERGY_BASE), 4);
        }

        private void drawCreature(Creature c, float x, float y, float scale, float scaleUp)
        {
            pushMatrix();
            var scaleIconUp = scaleUp*scale;
            translate((float) (-c.px*scaleIconUp), (float) (-c.py*scaleIconUp));
            translate(x, y);
            c.drawSoftBody(scaleIconUp, 40.0f/scale, false);
            popMatrix();
        }

        public void prepareForFileSave(int type)
        {
            fileSaveTimes[type] = -999999;
        }

        public void fileSave()
        {
            for (var i = 0; i < 4; i++)
            {
                if (fileSaveTimes[i] < -99999)
                {
                    fileSaveTimes[i] = year;
                    if (i < 2)
                    {
                        saveFrame(getNextFileName(i));
                    }
                    else
                    {
                        var data = this.toBigString();
                        saveStrings(getNextFileName(i), data);
                    }
                    fileSaveCounts[i]++;
                }
            }
        }

        public string[] toBigString()
        {
            string[] placeholder = {"Goo goo", "Ga ga"};
            return placeholder;
        }

        public void unselect()
        {
            selectedCreature = null;
        }
    }
}