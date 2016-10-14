using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using core.Graphics;

namespace core
{
    public class Board
    {
        public bool userControl;
        public bool wasPressingB;
        public HSBColor buttonColor = new HSBColor(0.82f, 0.8f, 0.7f);
        public Creature selectedCreature;
        public double imageSaveInterval = 1;
        public double MANUAL_BIRTH_SIZE = 1.2;
        public double recordPopulationEvery = 0.02;
        public double textSaveInterval = 1;
        public double timeStep;
        public double year;
        public double[] fileSaveTimes;
        
        private float maxTemperature;
        private float minTemperature;
        private float temperature;
        private readonly InputEngine input;
        private readonly GraphicsEngine graphics;
        private readonly int[] fileSaveCounts;
        private readonly int[] populationHistory;
        private readonly List<SoftBody> rocks;
        public const float MAX_CREATURE_ENERGY = 2.0f;
        public const float MAX_ROCK_ENERGY_BASE = 1.6f;
        public const float MIN_CREATURE_ENERGY = 1.2f;
        public const float MIN_ROCK_ENERGY_BASE = 0.8f;
        public const float MINIMUM_SURVIVABLE_SIZE = 0.2f;
        public const float OBJECT_TIMESTEPS_PER_YEAR = 100;
        public const float ROCK_DENSITY = 5;
        public const float THERMOMETER_MAX = 2;
        public const float THERMOMETER_MIN = -2;
        public const int POPULATION_HISTORY_LENGTH = 200;
        public int creatureIDUpTo = 0;
        public int creatureMinimum;
        public int creatureRankMetric = 0;
        public int playSpeed = 1;
        public readonly double FLASH_SPEED = 80;
        public readonly HSBColor BACKGROUND_COLOR = new HSBColor(0, 0, 0.1f);
        public readonly HSBColor ROCK_COLOR = Color.LightGray;
        public readonly int boardHeight;
        public readonly int boardWidth;
        public readonly int creatureMinimumIncrement = 5;
        public readonly List<Creature> creatures;
        public readonly List<SoftBody>[,] softBodiesInPositions;
        public readonly Tile[,] tiles;
        public readonly string folder;

        public Board(
            InputEngine input,
            GraphicsEngine graphics,
            int width, int height, float stepSize, float minTemp, float maxTemp, int rocksToAdd,
            int minimumCreatures, int seed, string initialFileName, double timeStep)
        {
            this.input = input;
            this.graphics = graphics;

            Rnd.noiseSeed(seed);
            Rnd.randomSeed(seed);

            boardWidth = width;
            boardHeight = height;
            tiles = new Tile[width, height];
            for (var x = 0; x < boardWidth; x++)
            {
                for (var y = 0; y < boardHeight; y++)
                {
                    var bigForce = Math.Pow(((float) y)/boardHeight, 0.5);
                    var fertility = (float)
                        (Rnd.noise(x*stepSize*3f, y*stepSize*3f)*(1 - bigForce)*5.0f +
                         Rnd.noise(x*stepSize*0.5f, y*stepSize*0.5f)*bigForce*5.0f - 1.5f);
                    var climateType = (float) (Rnd.noise(x*stepSize + 10000, y*stepSize + 10000)*1.63f - 0.4f);
                    climateType = Math.Min(Math.Max(climateType, 0), 0.8f);
                    tiles[x, y] = new Tile(x, y, fertility, 0, climateType);
                }
            }
            minTemperature = minTemp;
            maxTemperature = maxTemp;

            softBodiesInPositions = new List<SoftBody>[boardWidth, boardHeight];
            for (var x = 0; x < boardWidth; x++)
            {
                for (var y = 0; y < boardHeight; y++)
                {
                    softBodiesInPositions[x, y] = new List<SoftBody>();
                }
            }

            rocks = new List<SoftBody>(0);
            for (var i = 0; i < rocksToAdd; i++)
            {
                rocks.Add(new SoftBody(this.graphics, Rnd.nextFloat(0, boardWidth), Rnd.nextFloat(0, boardHeight), 0, 0,
                    getRandomSize(), ROCK_DENSITY, ROCK_COLOR, this, year));
            }

            creatureMinimum = minimumCreatures;
            creatures = new List<Creature>(0);
            maintainCreatureMinimum(false);
            folder = initialFileName;
            fileSaveCounts = new int[4];
            fileSaveTimes = new double[4];
            for (var i = 0; i < 4; i++)
            {
                fileSaveCounts[i] = 0;
                fileSaveTimes[i] = -999;
            }
            this.userControl = false;
            this.timeStep = timeStep;
            this.populationHistory = new int[POPULATION_HISTORY_LENGTH];
            for (var i = 0; i < POPULATION_HISTORY_LENGTH; i++)
            {
                populationHistory[i] = 0;
            }
        }

        public void drawBoard(float scaleUp, float camZoom, int mX, int mY)
        {
            for (var x = 0; x < boardWidth; x++)
                for (var y = 0; y < boardHeight; y++)
                    tiles[x, y].drawTile(this.graphics, scaleUp, (mX == x && mY == y));

            foreach (var rock in rocks)
                rock.drawSoftBody(scaleUp);

            foreach (var creature in creatures)
                creature.drawSoftBody(scaleUp, camZoom, true);
        }

        public void drawBlankBoard(float scaleUp)
        {
            this.graphics.fill(BACKGROUND_COLOR);
            this.graphics.rect(0, 0, scaleUp*boardWidth, scaleUp*boardHeight);
        }

        public void drawUI(float scaleUp, double timeStep, int x1, int y1, int x2, int y2)
        {
            this.graphics.fill(0, 0, 0);
            this.graphics.noStroke();
            this.graphics.rect(x1, y1, x2 - x1, y2 - y1);

            this.drawInfo(x1, y1);

            if (selectedCreature == null)
            {
                drawTopCreatures(scaleUp, x1, y1, x2);
            }
            else
            {
                drawSelectedCreature(x1, y1);
            }

            this.drawPopulationGraph(x1, x2, y1, y2);

            this.drawCurrentPopAndTemp(x1, y1, x2, y2);

            this.drawThermometer(-45, 30, 20, 660, temperature, THERMOMETER_MIN, THERMOMETER_MAX, new HSBColor(0, 1, 1));

            this.graphics.popMatrix();

            if (selectedCreature != null)
            {
                this.drawCreature(selectedCreature, x1 + 65, y1 + 147, 0.7f, scaleUp);
            }
        }

        private void drawInfo(int x1, int y1)
        {
            this.graphics.pushMatrix();
            this.graphics.translate(x1, y1);

            this.graphics.fill(0, 0, 1);
            this.graphics.textAlign(AlignText.LEFT);
            this.graphics.textSize(48);
            var yearText = "Year " + year.ToString(0, 2);
            this.graphics.text(yearText, 10, 48);
            var seasonTextXCoor = this.graphics.textWidth(yearText) + 50;
            this.graphics.textSize(24);
            this.graphics.text("Population: " + creatures.Count, 10, 80);
            string[] seasons = {"Winter", "Spring", "Summer", "Autumn"};
            this.graphics.text(seasons[(int) (getSeason()*4)], seasonTextXCoor, 30);
        }

        private void drawCurrentPopAndTemp(int x1, int y1, int x2, int y2)
        {
            this.graphics.fill(0, 0, 0);
            this.graphics.textAlign(AlignText.RIGHT);
            this.graphics.textSize(24);
            this.graphics.text("Population: " + creatures.Count, x2 - x1 - 10, y2 - y1 - 10);
            this.graphics.popMatrix();

            this.graphics.pushMatrix();
            this.graphics.translate(x2, y1);
            this.graphics.textAlign(AlignText.RIGHT);
            this.graphics.textSize(24);
            this.graphics.text("Temperature", -10, 24);
        }

        private void drawSelectedCreature(int x1, int y1)
        {
            if (this.selectedCreature.dead)
                return;

            var energyUsage = (float) selectedCreature.getEnergyUsage(timeStep);

            this.graphics.noStroke();
            if (energyUsage <= 0)
            {
                this.graphics.fill(0, 1, 0.5f);
            }
            else
            {
                this.graphics.fill(0.33f, 1, 0.4f);
            }

            var eBar = 6*energyUsage;
            this.graphics.rect(110, 280, Math.Min(Math.Max(eBar, -110), 110), 25);

            if (eBar < -110)
            {
                this.graphics.rect(0, 280, 25, (-110 - eBar)*20 + 25);
            }
            else if (eBar > 110)
            {
                var h = (eBar - 110)*20 + 25;
                this.graphics.rect(185, 280 - h, 25, h);
            }

            this.graphics.fill(0, 0, 1);
            var text = "";
            text += "Name: " + selectedCreature.name.Name;
            text += "\nID: " + selectedCreature.id;
            text += "\nEnergy: " + (100*(float) selectedCreature.energy).ToString(0, 2) + " yums";
            text += "\nEn. change: " + (100*energyUsage).ToString(0, 2) + " yums/year";
            text += "\nX: " + ((float) selectedCreature.px).ToString(0, 2);
            text += "\nY: " + ((float) selectedCreature.py).ToString(0, 2);
            text += "\nRotation: " + ((float) selectedCreature.rotation).ToString(0, 2);
            text += "\nB-day: " + toDate(selectedCreature.birthTime) + " (" + toAge(selectedCreature.birthTime) + ")";
            text += "\nGeneration: " + selectedCreature.gen;
            text += "\nParents: " + selectedCreature.name.Parents;
            text += "\nHue: " + ((float) (selectedCreature.myColor.Hue)).ToString(0, 2);
            text += "\nMouth hue: " + ((float) (selectedCreature.mouthHue)).ToString(0, 2);
            this.graphics.text(text, 0, 550);

            if (userControl)
            {
                this.graphics.text(
                    "Controls:\nUp/Down: Move\nLeft/Right: Rotate\nSpace: Eat\nF: Fight\nV: Vomit\nU,J: Change color" +
                    "\nI,K: Change mouth color\nB: Give birth (Not possible if under " +
                    Math.Round((MANUAL_BIRTH_SIZE + 1)*100) + " yums)", 10, 625, 250, 400);
            }

            using (this.graphics.newTransformScope())
            {
                this.graphics.translate(250, 100);
                this.graphics.scale(500.0f);

                selectedCreature.brain.draw(this.graphics, this.input.MouseX, this.input.MouseY);
            }
        }

        private void drawTopCreatures(float scaleUp, int x1, int y1, int x2)
        {
            var top = getCreaturesOrderdByCriteria();
            var maxEnergy = top.Max(e => e.energy);

            for (var i = 0; i < top.Length; i++)
            {
                top[i].preferredRank += (i - top[i].preferredRank)*0.4f;
                var y = y1 + 175 + 70*top[i].preferredRank;
                this.drawCreature(top[i], 45, y + 5, 0.7f, scaleUp);
                this.graphics.textSize(24);
                this.graphics.textAlign(AlignText.LEFT);
                this.graphics.noStroke();
                this.graphics.fill(0.333f, 1, 0.4f);
                float multi = (x2 - x1 - 200);
                if (top[i].energy > 0)
                {
                    this.graphics.rect(85, y + 5, (float) (multi*top[i].energy/maxEnergy), 25);
                }
                if (top[i].energy > 1)
                {
                    this.graphics.fill(0.333f, 1, 0.8f);
                    this.graphics.rect(85 + (float) (multi/maxEnergy), y + 5,
                        (float) (multi*(top[i].energy - 1)/maxEnergy),
                        25);
                }
                this.graphics.fill(0, 0, 1);
                this.graphics.text(
                    top[i].name.Name + " [" + top[i].id + "] (" + toAge(top[i].birthTime) + ")",
                    90, y);
                this.graphics.text("Energy: " + (100*(float) (top[i].energy)).ToString(0, 2), 90, y + 25);
            }

            this.graphics.noStroke();
            this.graphics.fill(buttonColor);
            this.graphics.rect(10, 95, 220, 40);
            this.graphics.rect(240, 95, 220, 40);
            this.graphics.fill(0, 0, 1);
            this.graphics.textAlign(AlignText.CENTER);
            this.graphics.text("Reset zoom", 120, 123);
            string[] sorts =
            {
                "Biggest", "Smallest", "Youngest", "Oldest", "A to Z", "Z to A", "Highest Gen",
                "Lowest Gen"
            };
            this.graphics.text("Sort by: " + sorts[creatureRankMetric], 350, 123);

            this.graphics.textSize(19);
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
                this.graphics.fill(buttonColor);
                this.graphics.rect(x, y, 220, 40);
                if (i >= 2 && i < 6)
                {
                    var flashAlpha = 1.0*Math.Pow(0.5, (year - fileSaveTimes[i - 2])*FLASH_SPEED);
                    this.graphics.fill(0, 0, 1, (float) flashAlpha);
                    this.graphics.rect(x, y, 220, 40);
                }
                this.graphics.fill(0, 0, 1, 1);
                this.graphics.text(buttonTexts[i], x + 110, y + 17);

                if (i == 0)
                {
                }
                else if (i == 1)
                {
                    this.graphics.text("-" + creatureMinimumIncrement +
                                       "                    +" + creatureMinimumIncrement, x + 110, y + 37);
                }
                else if (i <= 5)
                {
                    this.graphics.text(getNextFileName(i - 2), x + 110, y + 37);
                }
            }
        }

        public Creature[] getCreaturesOrderdByCriteria()
        {
            IEnumerable<Creature> comparison;
            if (creatureRankMetric == 4)
                comparison = creatures.OrderBy(c => c.name);
            else if (creatureRankMetric == 5)
                comparison = creatures.OrderBy(c => c.name).Reverse();
            else
                comparison = creatures.OrderBy(c => c.measure(creatureRankMetric));

            return comparison.Take(6).ToArray();
        }

        private void drawPopulationGraph(float x1, float x2, float y1, float y2)
        {
            var barWidth = (x2 - x1)/POPULATION_HISTORY_LENGTH;
            this.graphics.noStroke();
            this.graphics.fill(0.33333f, 1, 0.6f);
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
                this.graphics.rect((POPULATION_HISTORY_LENGTH - 1 - i)*barWidth, y2 - h, barWidth, h);
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

            foreach (var creature in creatures)
                creature.setPreviousEnergy();

            foreach (var rock in rocks)
                rock.collide(timeStep*OBJECT_TIMESTEPS_PER_YEAR);

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
                        if (this.input.KeyPressed)
                        {
                            if (this.input.Key == char.MaxValue)
                            {
                                if (this.input.KeyCode == Keys.Up)
                                    me.accelerate(0.3, timeStep*OBJECT_TIMESTEPS_PER_YEAR);
                                if (this.input.KeyCode == Keys.Down)
                                    me.accelerate(-0.3, timeStep*OBJECT_TIMESTEPS_PER_YEAR);
                                if (this.input.KeyCode == Keys.Left)
                                    me.turn(-0.3, timeStep*OBJECT_TIMESTEPS_PER_YEAR);
                                if (this.input.KeyCode == Keys.Right)
                                    me.turn(0.3, timeStep*OBJECT_TIMESTEPS_PER_YEAR);
                            }
                            else
                            {
                                if (this.input.Key == ' ') me.eat(0.3, timeStep*OBJECT_TIMESTEPS_PER_YEAR);
                                if (this.input.Key == 'v') me.eat(-0.3, timeStep*OBJECT_TIMESTEPS_PER_YEAR);
                                if (this.input.Key == 'f') me.fight(0.3, timeStep*OBJECT_TIMESTEPS_PER_YEAR);
                                if (this.input.Key == 'u') me.setHue(me.myColor.Hue + 0.02);
                                if (this.input.Key == 'j') me.setHue(me.myColor.Hue - 0.02);
                                if (this.input.Key == 'i') me.setMouthHue(me.mouthHue + 0.02);
                                if (this.input.Key == 'k') me.setMouthHue(me.mouthHue - 0.02);
                                if (this.input.Key == 'b')
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

                me.decideWhatToDo(timeStep*OBJECT_TIMESTEPS_PER_YEAR, !userControl);

                if (me.getRadius() < MINIMUM_SURVIVABLE_SIZE)
                {
                    this.killCreature(me);
                    i--;
                }
            }

            foreach (var rock in rocks)
                rock.applyMotions(timeStep*OBJECT_TIMESTEPS_PER_YEAR);

            foreach (var creature in creatures)
            {
                creature.applyMotions(timeStep*OBJECT_TIMESTEPS_PER_YEAR);
                creature.see();
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

        private void killCreature(Creature creature)
        {
            creature.killAndReturnToEarth();
            creatures.Remove(creature);
            while (this.selectedCreature == creature)
            {
                var rndSelection = Rnd.nextInt(0, creatures.Count);
                this.selectedCreature = creatures[rndSelection];
            }
        }

        private float getGrowableTime()
        {
            var temperatureRange = maxTemperature - minTemperature;
            return
                (float)
                    (minTemperature + temperatureRange*0.5 -
                     temperatureRange*0.5*Math.Cos((float) (getSeason()*2*Math.PI)));
        }

        private double getSeason()
        {
            return (year%1.0);
        }

        private void drawThermometer(float x1, float y1, float w, float h, float prog, float min, float max, HSBColor fillColor)
        {
            this.graphics.noStroke();
            this.graphics.fill(0, 0, 0.2f);
            this.graphics.rect(x1, y1, w, h);
            this.graphics.fill(fillColor);
            var proportionFilled = (prog - min)/(max - min);
            this.graphics.rect(x1, y1 + h*(1 - proportionFilled), w, proportionFilled*h);

            var zeroHeight = (0 - min)/(max - min);
            var zeroLineY = y1 + h*(1 - zeroHeight);
            this.graphics.textAlign(AlignText.RIGHT);
            this.graphics.stroke(0, 0, 1);
            this.graphics.strokeWeight(3);
            this.graphics.line(x1, zeroLineY, x1 + w, zeroLineY);
            var minY = y1 + h*(1 - (minTemperature - min)/(max - min));
            var maxY = y1 + h*(1 - (maxTemperature - min)/(max - min));
            this.graphics.fill(0, 0, 0.8f);
            this.graphics.line(x1, minY, x1 + w*1.8, minY);
            this.graphics.line(x1, maxY, x1 + w*1.8, maxY);
            this.graphics.line(x1 + w*1.8, minY, x1 + w*1.8, maxY);

            this.graphics.fill(0, 0, 1);
            this.graphics.text("Zero", x1 - 5, zeroLineY + 8);
            this.graphics.text(minTemperature.ToString(0, 2), x1 - 5, minY + 8);
            this.graphics.text(maxTemperature.ToString(0, 2), x1 - 5, maxY + 8);
        }

        private void drawVerticalSlider(float x1, float y1, float w, float h, float prog, HSBColor fillColor, HSBColor antiColor)
        {
            this.graphics.noStroke();
            this.graphics.fill(0, 0, 0.2f);
            this.graphics.rect(x1, y1, w, h);
            if (prog >= 0)
            {
                this.graphics.fill(fillColor);
            }
            else
            {
                this.graphics.fill(antiColor);
            }
            this.graphics.rect(x1, y1 + h*(1 - prog), w, prog*h);
        }

        public bool setMinTemperature(float temp)
        {
            minTemperature = tempBounds(THERMOMETER_MIN + temp*(THERMOMETER_MAX - THERMOMETER_MIN));
            if (minTemperature > maxTemperature)
            {
                var placeHolder = maxTemperature;
                maxTemperature = minTemperature;
                minTemperature = placeHolder;
                return true;
            }
            return false;
        }

        public bool setMaxTemperature(float temp)
        {
            maxTemperature = tempBounds(THERMOMETER_MIN + temp*(THERMOMETER_MAX - THERMOMETER_MIN));
            if (minTemperature > maxTemperature)
            {
                var placeHolder = maxTemperature;
                maxTemperature = minTemperature;
                minTemperature = placeHolder;
                return true;
            }
            return false;
        }

        private float tempBounds(float temp)
        {
            return Math.Min(Math.Max(temp, THERMOMETER_MIN), THERMOMETER_MAX);
        }

        public float getHighTempProportion()
        {
            return (maxTemperature - THERMOMETER_MIN)/(THERMOMETER_MAX - THERMOMETER_MIN);
        }

        public float getLowTempProportion()
        {
            return (minTemperature - THERMOMETER_MIN)/(THERMOMETER_MAX - THERMOMETER_MIN);
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
                    var color = new HSBColor(0, 1, 1);
                    color.Hue = Rnd.nextFloat(0, 1);

                    creatures.Add(new Creature(this.graphics, Rnd.nextFloat(0, boardWidth),
                        Rnd.nextFloat(0, boardHeight), 0, 0,
                        Rnd.nextFloat(MIN_CREATURE_ENERGY, MAX_CREATURE_ENERGY), 1, color,
                        this, year, Rnd.nextFloat(0, 2*Math.PI), 0, null, null, 1,
                        Rnd.nextFloat(0, 1)));
                }
            }
        }

        private Creature getRandomCreature()
        {
            var index = Rnd.nextInt(0, creatures.Count);
            return creatures[index];
        }

        private double getRandomSize()
        {
            return Math.Pow(Rnd.nextFloat(MIN_ROCK_ENERGY_BASE, MAX_ROCK_ENERGY_BASE), 4);
        }

        private void drawCreature(Creature creature, float x, float y, float scale, float scaleUp)
        {
            this.graphics.pushMatrix();
            var scaleIconUp = scaleUp*scale;
            this.graphics.translate((float) (-creature.px*scaleIconUp), (float) (-creature.py*scaleIconUp));
            this.graphics.translate(x, y);

            creature.drawSoftBody(scaleIconUp, 40.0f/scale, false);

            this.graphics.popMatrix();
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
                        this.graphics.saveFrame(getNextFileName(i));
                    }
                    else
                    {
                        var data = this.toBigString();
                        this.graphics.saveStrings(getNextFileName(i), data);
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