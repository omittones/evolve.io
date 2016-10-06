using System;
using System.Collections.Generic;
using core.Graphics;

namespace core
{
    public class Creature : SoftBody
    {
        public const double ACCELERATION_BACK_ENERGY = 0.05;
        public const double ACCELERATION_ENERGY = 0.03;
        public const double AXON_START_MUTABILITY = 0.0005;
        public const double EAT_ENERGY = 0.04;
        public const double EAT_SPEED = 0.9; // 1 is instant, 0 is nonexistent, 0.001 is verrry slow.
        public const double FIGHT_ENERGY = 0.03;
        public const double FOOD_SENSITIVITY = 0.3;
        public const double INJURED_ENERGY = 0.25;
        public const double MATURE_AGE = 0.01;
        public const double METABOLISM_ENERGY = 0.004;
        public const double SAFE_SIZE = 1.25;
        public const double STARTING_AXON_VARIABILITY = 1.0;
        public const double SWIM_ENERGY = 0.008;
        public const double TURN_ENERGY = 0.01;
        public const float BRIGHTNESS_THRESHOLD = 0.7f;
        public const float CROSS_SIZE = 0.05f;
        public const int BRAIN_HEIGHT = 12;
        public const int BRAIN_WIDTH = 3;
        public const int ENERGY_HISTORY_LENGTH = 6;
        public const int MAX_NAME_LENGTH = 10;
        public const int MIN_NAME_LENGTH = 3;

        public string name;
        public string parents;
        public int gen;
        public int id;
        private readonly double[] previousEnergy = new double[ENERGY_HISTORY_LENGTH];
        private double vr;
        public double rotation;
        private readonly Axon[,,] axons;
        private readonly double[,] neurons;

        public float preferredRank = 8;
        private static readonly double[] visionAngles = {0, -0.4, 0.4};
        private static readonly double[] visionDistances = {0, 1.42, 1.42};
        private readonly double[] visionOccludedX = new double[visionAngles.Length];
        private readonly double[] visionOccludedY = new double[visionAngles.Length];
        private readonly double[] visionResults = new double[9];
        private readonly int MEMORY_COUNT = 1;
        private readonly double[] memories;
        public double mouthHue;

        public Creature(
            GraphicsEngine graphics,
            double tpx, double tpy, double tvx, double tvy, double tenergy,
            double tdensity, double thue, double tsaturation, double tbrightness, Board tb, double bt,
            double rot, double tvr, string tname, string tparents, bool mutateName1,
            Axon[,,] tbrain, double[,] tneurons, int tgen, double tmouthHue)
            : base(graphics, tpx, tpy, tvx, tvy, tenergy, tdensity, thue, tsaturation, tbrightness, tb, bt)
        {

            if (tbrain == null)
            {
                axons = new Axon[BRAIN_WIDTH - 1, BRAIN_HEIGHT, BRAIN_HEIGHT - 1];
                neurons = new double[BRAIN_WIDTH, BRAIN_HEIGHT];
                for (var x = 0; x < BRAIN_WIDTH - 1; x++)
                {
                    for (var y = 0; y < BRAIN_HEIGHT; y++)
                    {
                        for (var z = 0; z < BRAIN_HEIGHT - 1; z++)
                        {
                            double startingWeight = 0;
                            if (y == BRAIN_HEIGHT - 1)
                            {
                                startingWeight = (Rnd.next()*2 - 1)*STARTING_AXON_VARIABILITY;
                            }
                            axons[x, y, z] = new Axon(startingWeight, AXON_START_MUTABILITY);
                        }
                    }
                }
                neurons = new double[BRAIN_WIDTH, BRAIN_HEIGHT];
                for (var x = 0; x < BRAIN_WIDTH; x++)
                {
                    for (var y = 0; y < BRAIN_HEIGHT; y++)
                    {
                        if (y == BRAIN_HEIGHT - 1)
                        {
                            neurons[x, y] = 1;
                        }
                        else
                        {
                            neurons[x, y] = 0;
                        }
                    }
                }
            }
            else
            {
                axons = tbrain;
                neurons = tneurons;
            }
            rotation = rot;
            vr = tvr;
            isCreature = true;
            id = board.creatureIDUpTo + 1;
            if (tname.Length >= 1)
            {
                if (mutateName1)
                {
                    name = mutateName(tname);
                }
                else
                {
                    name = tname;
                }
                name = sanitizeName(name);
            }
            else
            {
                name = createNewName();
            }
            parents = tparents;
            board.creatureIDUpTo++;
            //visionAngle = 0;
            //visionDistance = 0;
            //visionEndX = getVisionStartX();
            //visionEndY = getVisionStartY();
            for (var i = 0; i < 9; i++)
            {
                visionResults[i] = 0;
            }
            memories = new double[MEMORY_COUNT];
            for (var i = 0; i < MEMORY_COUNT; i++)
            {
                memories[i] = 0;
            }
            gen = tgen;
            mouthHue = tmouthHue;
        }

        public void drawBrain(PFont font, float scaleUp, int mX, int mY)
        {
            const float neuronSize = 0.4f;
            this.graphics.noStroke();
            this.graphics.fill(0, 0, 0.4f);
            this.graphics.rect((-1.7f - neuronSize)*scaleUp, -neuronSize*scaleUp, (2.4f + BRAIN_WIDTH + neuronSize*2)*scaleUp,
                (BRAIN_HEIGHT + neuronSize*2)*scaleUp);

            this.graphics.ellipseMode(EllipseMode.RADIUS);
            this.graphics.strokeWeight(2);
            this.graphics.textFont(font, 0.58f*scaleUp);
            this.graphics.fill(0, 0, 1);
            string[] inputLabels =
            {
                "0Hue", "0Sat", "0Bri", "1Hue",
                "1Sat", "1Bri", "2Hue", "2Sat", "2Bri", "Size", "Mem", "Const."
            };
            string[] outputLabels =
            {
                "BHue", "MHue", "Accel.", "Turn", "Eat", "Fight", "Birth", "How funny?",
                "How popular?", "How generous?", "Mem", "Const."
            };
            for (var y = 0; y < BRAIN_HEIGHT; y++)
            {
                this.graphics.textAlign(AlignText.RIGHT);
                this.graphics.text(inputLabels[y], (-neuronSize - 0.1f)*scaleUp, (y + (neuronSize*0.6f))*scaleUp);
                this.graphics.textAlign(AlignText.LEFT);
                this.graphics.text(outputLabels[y], (BRAIN_WIDTH - 1 + neuronSize + 0.1f)*scaleUp, (y + (neuronSize*0.6f))*scaleUp);
            }
            this.graphics.textAlign(AlignText.CENTER);
            for (var x = 0; x < BRAIN_WIDTH; x++)
            {
                for (var y = 0; y < BRAIN_HEIGHT; y++)
                {
                    this.graphics.noStroke();
                    var val = neurons[x, y];
                    this.graphics.fill(neuronFillColor(val));
                    this.graphics.ellipse(x*scaleUp, y*scaleUp, neuronSize*scaleUp, neuronSize*scaleUp);
                    this.graphics.fill(neuronTextColor(val));
                    this.graphics.text(((float) val).ToString(0, 1), x*scaleUp, (y + (neuronSize*0.6f))*scaleUp);
                }
            }
            if (mX >= 0 && mX < BRAIN_WIDTH && mY >= 0 && mY < BRAIN_HEIGHT)
            {
                for (var y = 0; y < BRAIN_HEIGHT; y++)
                {
                    if (mX >= 1 && mY < BRAIN_HEIGHT - 1)
                    {
                        drawAxon(mX - 1, y, mX, mY, scaleUp);
                    }
                    if (mX < BRAIN_WIDTH - 1 && y < BRAIN_HEIGHT - 1)
                    {
                        drawAxon(mX, mY, mX + 1, y, scaleUp);
                    }
                }
            }
        }

        public void drawAxon(int x1, int y1, int x2, int y2, float scaleUp)
        {
            this.graphics.stroke(neuronFillColor(axons[x1, y1, y2].weight*neurons[x1, y1]));

            this.graphics.line(x1*scaleUp, y1*scaleUp, x2*scaleUp, y2*scaleUp);
        }

        public void useBrain(double timeStep, bool useOutput)
        {
            for (var i = 0; i < 9; i++)
            {
                neurons[0, i] = visionResults[i];
            }
            neurons[0, 9] = energy;
            for (var i = 0; i < MEMORY_COUNT; i++)
            {
                neurons[0, 10 + i] = memories[i];
            }
            for (var x = 1; x < BRAIN_WIDTH; x++)
            {
                for (var y = 0; y < BRAIN_HEIGHT - 1; y++)
                {
                    double total = 0;
                    for (var input = 0; input < BRAIN_HEIGHT; input++)
                    {
                        total += neurons[x - 1, input]*axons[x - 1, input, y].weight;
                    }
                    if (x == BRAIN_WIDTH - 1)
                    {
                        neurons[x, y] = total;
                    }
                    else
                    {
                        neurons[x, y] = sigmoid(total);
                    }
                }
            }
            if (useOutput)
            {
                var end = BRAIN_WIDTH - 1;
                myHue = Math.Min(Math.Max(neurons[end, 0], 0), 1);
                mouthHue = Math.Min(Math.Max(neurons[end, 1], 0), 1);
                accelerate(neurons[end, 2], timeStep);
                turn(neurons[end, 3], timeStep);
                eat(neurons[end, 4], timeStep);
                fight(neurons[end, 5], timeStep);
                if (neurons[end, 6] > 0 && board.year - birthTime >= MATURE_AGE && energy > SAFE_SIZE)
                {
                    reproduce(SAFE_SIZE, timeStep);
                }
                for (var i = 0; i < MEMORY_COUNT; i++)
                {
                    memories[i] = neurons[end, 10 + i];
                }
            }
        }

        public double sigmoid(double input)
        {
            return 1.0/(1.0 + Math.Pow(2.71828182846, -input));
        }

        public hslColor neuronFillColor(double d)
        {
            if (d >= 0)
            {
                return new hslColor(0, 0, 1, (float) (d));
            }
            return new hslColor(0, 0, 0, (float) (-d));
        }

        public hslColor neuronTextColor(double d)
        {
            if (d >= 0)
            {
                return new hslColor(0, 0, 0);
            }
            return new hslColor(0, 0, 1);
        }

        public void drawSoftBody(float scaleUp, float camZoom, bool showVision)
        {
            this.graphics.ellipseMode(EllipseMode.RADIUS);
            var radius = getRadius();
            if (showVision)
            {
                for (var i = 0; i < visionAngles.Length; i++)
                {
                    var visionUIcolor = new hslColor(0, 0, 1);
                    if (visionResults[i*3 + 2] > BRIGHTNESS_THRESHOLD)
                    {
                        visionUIcolor = new hslColor(0, 0, 0);
                    }
                    this.graphics.stroke(visionUIcolor);
                    this.graphics.strokeWeight(2);
                    var endX = (float) getVisionEndX(i);
                    var endY = (float) getVisionEndY(i);
                    this.graphics.line((float) (px*scaleUp), (float) (py*scaleUp), endX*scaleUp, endY*scaleUp);
                    this.graphics.noStroke();
                    this.graphics.fill(visionUIcolor);
                    this.graphics.ellipse((float) (visionOccludedX[i]*scaleUp), (float) (visionOccludedY[i]*scaleUp),
                        2*CROSS_SIZE*scaleUp, 2*CROSS_SIZE*scaleUp);
                    this.graphics.stroke((float) (visionResults[i*3]), (float) (visionResults[i*3 + 1]),
                        (float) (visionResults[i*3 + 2]));
                    this.graphics.strokeWeight(2);
                    this.graphics.line((float) ((visionOccludedX[i] - CROSS_SIZE)*scaleUp),
                        (float) ((visionOccludedY[i] - CROSS_SIZE)*scaleUp),
                        (float) ((visionOccludedX[i] + CROSS_SIZE)*scaleUp),
                        (float) ((visionOccludedY[i] + CROSS_SIZE)*scaleUp));
                    this.graphics.line((float) ((visionOccludedX[i] - CROSS_SIZE)*scaleUp),
                        (float) ((visionOccludedY[i] + CROSS_SIZE)*scaleUp),
                        (float) ((visionOccludedX[i] + CROSS_SIZE)*scaleUp),
                        (float) ((visionOccludedY[i] - CROSS_SIZE)*scaleUp));
                }
            }
            this.graphics.noStroke();
            if (fightLevel > 0)
            {
                this.graphics.fill(0, 1, 1, (float) (fightLevel*0.8));
                this.graphics.ellipse((float) (px*scaleUp), (float) (py*scaleUp), (float) (FIGHT_RANGE*radius*scaleUp),
                    (float) (FIGHT_RANGE*radius*scaleUp));
            }
            this.graphics.strokeWeight(2);
            this.graphics.stroke(0, 0, 1);
            this.graphics.fill(0, 0, 1);
            if (this == board.selectedCreature)
            {
                this.graphics.ellipse((float) (px*scaleUp), (float) (py*scaleUp),
                    (float) (radius*scaleUp + 1 + 75.0/camZoom), (float) (radius*scaleUp + 1 + 75.0/camZoom));
            }
            base.drawSoftBody(scaleUp);
            this.graphics.noFill();
            this.graphics.strokeWeight(2);
            this.graphics.stroke(0, 0, 1);
            this.graphics.ellipseMode(EllipseMode.RADIUS);
            this.graphics.ellipse((float) (px*scaleUp), (float) (py*scaleUp),
                Board.MINIMUM_SURVIVABLE_SIZE*scaleUp, Board.MINIMUM_SURVIVABLE_SIZE*scaleUp);
            this.graphics.pushMatrix();
            this.graphics.translate((float) (px*scaleUp), (float) (py*scaleUp));
            this.graphics.scale((float) radius);
            this.graphics.rotate((float) rotation);
            this.graphics.strokeWeight((float) (2.0/radius));
            this.graphics.stroke(0, 0, 0);
            this.graphics.fill((float) mouthHue, 1.0f, 1.0f);
            this.graphics.ellipse(0.6f*scaleUp, 0, 0.37f*scaleUp, 0.37f*scaleUp);
            this.graphics.popMatrix();
            if (showVision)
            {
                this.graphics.fill(0, 0, 1);
                this.graphics.textFont(this.graphics.font, 0.3f*scaleUp);
                this.graphics.textAlign(AlignText.CENTER);
                this.graphics.text(getCreatureName(), (float) (px*scaleUp), (float) ((py - getRadius()*1.4)*scaleUp));
            }
        }

        public void metabolize(double timeStep)
        {
            loseEnergy(energy*METABOLISM_ENERGY*timeStep);
        }

        public void accelerate(double amount, double timeStep)
        {
            var multiplied = amount*timeStep/getMass();
            vx += Math.Cos(rotation)*multiplied;
            vy += Math.Sin(rotation)*multiplied;
            if (amount >= 0)
            {
                loseEnergy(amount*ACCELERATION_ENERGY*timeStep);
            }
            else
            {
                loseEnergy(Math.Abs(amount*ACCELERATION_BACK_ENERGY*timeStep));
            }
        }

        public void turn(double amount, double timeStep)
        {
            vr += 0.04*amount*timeStep/getMass();
            loseEnergy(Math.Abs(amount*TURN_ENERGY*energy*timeStep));
        }

        public Tile getRandomCoveredTile()
        {
            double radius = (float) getRadius();
            double choiceX = 0;
            double choiceY = 0;
            while (MathEx.Distance((float) px, (float) py, (float) choiceX, (float) choiceY) > radius)
            {
                choiceX = (Rnd.next()*2*radius - radius) + px;
                choiceY = (Rnd.next()*2*radius - radius) + py;
            }
            var x = xBound((int) choiceX);
            var y = yBound((int) choiceY);
            return board.tiles[x, y];
        }

        public void eat(double attemptedAmount, double timeStep)
        {
            var amount = attemptedAmount/(1.0 + distance(0, 0, vx, vy));

            // The faster you're moving, the less efficiently you can eat.
            if (amount < 0)
            {
                dropEnergy(-amount*timeStep);
                loseEnergy(-attemptedAmount*EAT_ENERGY*timeStep);
            }
            else
            {
                var coveredTile = getRandomCoveredTile();
                var foodToEat = coveredTile.foodLevel*(1 - Math.Pow((1 - EAT_SPEED), amount*timeStep));
                if (foodToEat > coveredTile.foodLevel)
                {
                    foodToEat = coveredTile.foodLevel;
                }
                coveredTile.foodLevel -= (float) foodToEat;
                var foodDistance = Math.Abs(coveredTile.foodType - mouthHue);
                var multiplier = 1.0 - foodDistance/FOOD_SENSITIVITY;
                if (multiplier >= 0)
                {
                    addEnergy(foodToEat*multiplier);
                }
                else
                {
                    loseEnergy(-foodToEat*multiplier);
                }
                loseEnergy(attemptedAmount*EAT_ENERGY*timeStep);
            }
        }

        public void fight(double amount, double timeStep)
        {
            if (amount > 0 && board.year - birthTime >= MATURE_AGE)
            {
                fightLevel = amount;
                loseEnergy(fightLevel*FIGHT_ENERGY*energy*timeStep);
                for (var i = 0; i < colliders.Count; i++)
                {
                    var collider = colliders[i];
                    if (collider.isCreature)
                    {
                        var distance = MathEx.Distance((float) px, (float) py, (float) collider.px, (float) collider.py);
                        var combinedRadius = getRadius()*FIGHT_RANGE + collider.getRadius();
                        if (distance < combinedRadius)
                        {
                            ((Creature) collider).dropEnergy(fightLevel*INJURED_ENERGY*timeStep);
                        }
                    }
                }
            }
            else
            {
                fightLevel = 0;
            }
        }

        public void loseEnergy(double energyLost)
        {
            if (energyLost > 0)
            {
                energy -= energyLost;
            }
        }

        public void dropEnergy(double energyLost)
        {
            if (energyLost > 0)
            {
                energyLost = Math.Min(energyLost, energy);
                energy -= energyLost;
                getRandomCoveredTile().addFood((float) energyLost, (float) myHue);
            }
        }

        public void see()
        {
            for (var k = 0; k < visionAngles.Length; k++)
            {
                var visionStartX = px;
                var visionStartY = py;
                var visionTotalAngle = rotation + visionAngles[k];

                var endX = getVisionEndX(k);
                var endY = getVisionEndY(k);

                visionOccludedX[k] = endX;
                visionOccludedY[k] = endY;
                var c = getColorAt(endX, endY);
                visionResults[k*3] = c.HueF();
                visionResults[k*3 + 1] = c.SaturationF();
                visionResults[k*3 + 2] = c.LuminosityF();

                var prevTileX = -1;
                var prevTileY = -1;
                var potentialVisionOccluders = new List<SoftBody>();
                for (var DAvision = 0; DAvision < visionDistances[k] + 1; DAvision++)
                {
                    var tileX = (int) (visionStartX + Math.Cos(visionTotalAngle)*DAvision);
                    var tileY = (int) (visionStartY + Math.Sin(visionTotalAngle)*DAvision);
                    if (tileX != prevTileX || tileY != prevTileY)
                    {
                        addPVOs(tileX, tileY, potentialVisionOccluders);
                        if (prevTileX >= 0 && tileX != prevTileX && tileY != prevTileY)
                        {
                            addPVOs(prevTileX, tileY, potentialVisionOccluders);
                            addPVOs(tileX, prevTileY, potentialVisionOccluders);
                        }
                    }
                    prevTileX = tileX;
                    prevTileY = tileY;
                }
                var rotationMatrix = new double[2, 2];
                rotationMatrix[1, 1] = rotationMatrix[0, 0] = Math.Cos(-visionTotalAngle);
                rotationMatrix[0, 1] = Math.Sin(-visionTotalAngle);
                rotationMatrix[1, 0] = -rotationMatrix[0, 1];
                var visionLineLength = visionDistances[k];
                for (var i = 0; i < potentialVisionOccluders.Count; i++)
                {
                    var body = potentialVisionOccluders[i];
                    var x = body.px - px;
                    var y = body.py - py;
                    var r = body.getRadius();
                    var translatedX = rotationMatrix[0, 0]*x + rotationMatrix[1, 0]*y;
                    var translatedY = rotationMatrix[0, 1]*x + rotationMatrix[1, 1]*y;
                    if (Math.Abs(translatedY) <= r)
                    {
                        if ((translatedX >= 0 && translatedX < visionLineLength && translatedY < visionLineLength) ||
                            distance(0, 0, translatedX, translatedY) < r ||
                            distance(visionLineLength, 0, translatedX, translatedY) < r)
                        {
                            // YES! There is an occlussion.
                            visionLineLength = translatedX - Math.Sqrt(r*r - translatedY*translatedY);
                            visionOccludedX[k] = visionStartX + visionLineLength*Math.Cos(visionTotalAngle);
                            visionOccludedY[k] = visionStartY + visionLineLength*Math.Sin(visionTotalAngle);
                            visionResults[k*3] = body.myHue;
                            visionResults[k*3 + 1] = body.mySaturation;
                            visionResults[k*3 + 2] = body.myBrightness;
                        }
                    }
                }
            }
        }

        public hslColor getColorAt(double x, double y)
        {
            if (x >= 0 && x < board.boardWidth && y >= 0 && y < board.boardHeight)
            {
                return board.tiles[(int) (x), (int) (y)].getColor();
            }
            return board.BACKGROUND_COLOR;
        }

        public double distance(double x1, double y1, double x2, double y2)
        {
            return (Math.Sqrt((x2 - x1)*(x2 - x1) + (y2 - y1)*(y2 - y1)));
        }

        public void addPVOs(int x, int y, List<SoftBody> PVOs)
        {
            if (x >= 0 && x < board.boardWidth && y >= 0 && y < board.boardHeight)
            {
                for (var i = 0; i < board.softBodiesInPositions[x, y].Count; i++)
                {
                    var newCollider = board.softBodiesInPositions[x, y][i];
                    if (!PVOs.Contains(newCollider) && newCollider != this)
                    {
                        PVOs.Add(newCollider);
                    }
                }
            }
        }

        public void returnToEarth()
        {
            var pieces = 20;
            for (var i = 0; i < pieces; i++)
            {
                getRandomCoveredTile().addFood((float) energy/pieces, (float) myHue);
            }
            for (var x = SBIPMinX; x <= SBIPMaxX; x++)
            {
                for (var y = SBIPMinY; y <= SBIPMaxY; y++)
                {
                    board.softBodiesInPositions[x, y].Remove(this);
                }
            }
            if (board.selectedCreature == this)
            {
                board.unselect();
            }
        }

        public void reproduce(double babySize, double timeStep)
        {
            if (colliders == null)
            {
                collide(timeStep);
            }
            var highestGen = 0;
            if (babySize >= 0)
            {
                var parents = new List<Creature>(0);
                parents.Add(this);
                var availableEnergy = getBabyEnergy();
                for (var i = 0; i < colliders.Count; i++)
                {
                    var possibleParent = colliders[i];
                    if (possibleParent.isCreature && ((Creature) possibleParent).neurons[BRAIN_WIDTH - 1, 9] > -1)
                    {
                        // Must be a WILLING creature to also give birth.
                        var distance = MathEx.Distance((float) px, (float) py, (float) possibleParent.px, (float) possibleParent.py);
                        var combinedRadius = getRadius()*FIGHT_RANGE + possibleParent.getRadius();
                        if (distance < combinedRadius)
                        {
                            parents.Add((Creature) possibleParent);
                            availableEnergy += ((Creature) possibleParent).getBabyEnergy();
                        }
                    }
                }
                if (availableEnergy > babySize)
                {
                    double newPX = Rnd.next(-0.01, 0.01);
                    double newPY = Rnd.next(-0.01, 0.01);
                        //To avoid landing directly on parents, resulting in division by 0)
                    double newHue = 0;
                    double newSaturation = 0;
                    double newBrightness = 0;
                    double newMouthHue = 0;
                    var parentsTotal = parents.Count;
                    var parentNames = new string[parentsTotal];
                    var newBrain = new Axon[BRAIN_WIDTH - 1, BRAIN_HEIGHT, BRAIN_HEIGHT - 1];
                    var newNeurons = new double[BRAIN_WIDTH, BRAIN_HEIGHT];
                    float randomParentRotation = Rnd.next(0, 1);
                    for (var x = 0; x < BRAIN_WIDTH - 1; x++)
                    {
                        for (var y = 0; y < BRAIN_HEIGHT; y++)
                        {
                            for (var z = 0; z < BRAIN_HEIGHT - 1; z++)
                            {
                                var axonAngle =
                                    (float)
                                        (Math.Atan2((y + z)/2.0 - BRAIN_HEIGHT/2.0, x - BRAIN_WIDTH/2)/(2*Math.PI) +
                                         Math.PI);
                                var parentForAxon =
                                    parents[((int) (((axonAngle + randomParentRotation)%1.0)*parentsTotal))];
                                newBrain[x, y, z] = parentForAxon.axons[x, y, z].mutateAxon();
                            }
                        }
                    }
                    for (var x = 0; x < BRAIN_WIDTH; x++)
                    {
                        for (var y = 0; y < BRAIN_HEIGHT; y++)
                        {
                            var axonAngle =
                                (float) (Math.Atan2(y - BRAIN_HEIGHT/2.0, x - BRAIN_WIDTH/2)/(2*Math.PI) + Math.PI);
                            var parentForAxon = parents[(int) (((axonAngle + randomParentRotation)%1.0)*parentsTotal)];
                            newNeurons[x, y] = parentForAxon.neurons[x, y];
                        }
                    }
                    for (var i = 0; i < parentsTotal; i++)
                    {
                        var chosenIndex = Rnd.next(0, parents.Count);
                        var parent = parents[(chosenIndex)];
                        parents.RemoveAt(chosenIndex);
                        parent.energy -= babySize*(parent.getBabyEnergy()/availableEnergy);
                        newPX += parent.px/parentsTotal;
                        newPY += parent.py/parentsTotal;
                        newHue += parent.myHue/parentsTotal;
                        newSaturation += parent.mySaturation/parentsTotal;
                        newBrightness += parent.myBrightness/parentsTotal;
                        newMouthHue += parent.mouthHue/parentsTotal;
                        parentNames[i] = parent.name;
                        if (parent.gen > highestGen)
                        {
                            highestGen = parent.gen;
                        }
                    }
                    newSaturation = 1;
                    newBrightness = 1;
                    board.creatures.Add(new Creature(this.graphics, newPX, newPY, 0, 0,
                        babySize, density, newHue, newSaturation, newBrightness, board, board.year,
                        Rnd.next(0, 2*Math.PI), 0,
                        stitchName(parentNames), andifyParents(parentNames), true,
                        newBrain, newNeurons, highestGen + 1, newMouthHue));
                }
            }
        }

        public string stitchName(string[] parts)
        {
            var result = "";
            for (var i = 0; i < parts.Length; i++)
            {
                var portion = ((float) parts[i].Length)/parts.Length;
                var start = (int) Math.Min(Math.Max((float) Math.Round(portion*i), 0), parts[i].Length);
                var end = (int) Math.Min(Math.Max((float) Math.Round(portion*(i + 1)), 0), parts[i].Length);
                result = result + parts[i].Substr(start, end);
            }
            return result;
        }

        public string andifyParents(string[] parts)
        {
            var result = "";
            for (var i = 0; i < parts.Length; i++)
            {
                if (i >= 1)
                {
                    result = result + " & ";
                }
                result = result + capitalize(parts[i]);
            }
            return result;
        }

        public string createNewName()
        {
            var nameSoFar = "";
            var chosenLength = Rnd.next(MIN_NAME_LENGTH, MAX_NAME_LENGTH);
            for (var i = 0; i < chosenLength; i++)
            {
                nameSoFar += getRandomChar();
            }
            return sanitizeName(nameSoFar);
        }

        public char getRandomChar()
        {
            float letterFactor = Rnd.next(0, 100);
            var letterChoice = 0;
            while (letterFactor > 0)
            {
                letterFactor -= (float) board.letterFrequencies[letterChoice];
                letterChoice++;
            }
            return (char) (letterChoice + 96);
        }

        public string sanitizeName(string input)
        {
            var output = "";
            var vowelsSoFar = 0;
            var consonantsSoFar = 0;
            for (var i = 0; i < input.Length; i++)
            {
                var ch = input[i];
                if (isVowel(ch))
                {
                    consonantsSoFar = 0;
                    vowelsSoFar++;
                }
                else
                {
                    vowelsSoFar = 0;
                    consonantsSoFar++;
                }
                if (vowelsSoFar <= 2 && consonantsSoFar <= 2)
                {
                    output = output + ch;
                }
                else
                {
                    var chanceOfAddingChar = 0.5;
                    if (input.Length <= MIN_NAME_LENGTH)
                    {
                        chanceOfAddingChar = 1.0;
                    }
                    else if (input.Length >= MAX_NAME_LENGTH)
                    {
                        chanceOfAddingChar = 0.0;
                    }
                    if (Rnd.next(0, 1) < chanceOfAddingChar)
                    {
                        var extraChar = ' ';
                        while (extraChar == ' ' || (isVowel(ch) == isVowel(extraChar)))
                        {
                            extraChar = getRandomChar();
                        }
                        output = output + extraChar + ch;
                        if (isVowel(ch))
                        {
                            consonantsSoFar = 0;
                            vowelsSoFar = 1;
                        }
                        else
                        {
                            consonantsSoFar = 1;
                            vowelsSoFar = 0;
                        }
                    }
                }
            }
            return output;
        }

        public string getCreatureName()
        {
            return capitalize(name);
        }

        public string capitalize(string n)
        {
            return n.Substr(0, 1).ToUpper() + n.Substr(1, n.Length);
        }

        public bool isVowel(char a)
        {
            return (a == 'a' || a == 'e' || a == 'i' || a == 'o' || a == 'u' || a == 'y');
        }

        public string mutateName(string input)
        {
            if (input.Length >= 3)
            {
                if (Rnd.next(0, 1) < 0.2)
                {
                    var removeIndex = Rnd.next(0, input.Length);
                    input = input.Substr(0, removeIndex) + input.Substr(removeIndex + 1, input.Length);
                }
            }
            if (input.Length <= 9)
            {
                if (Rnd.next(0, 1) < 0.2)
                {
                    var insertIndex = Rnd.next(0, input.Length + 1);
                    input = input.Substr(0, insertIndex) + getRandomChar() + input.Substr(insertIndex, input.Length);
                }
            }
            var changeIndex = Rnd.next(0, input.Length);
            input = input.Substr(0, changeIndex) + getRandomChar() + input.Substr(changeIndex + 1, input.Length);
            return input;
        }

        public override void applyMotions(double timeStep)
        {
            if (getRandomCoveredTile().fertility > 1)
            {
                loseEnergy(SWIM_ENERGY*energy);
            }

            base.applyMotions(timeStep);

            rotation += vr;
            vr *= (1 - FRICTION/getMass());
        }

        public double getEnergyUsage(double timeStep)
        {
            return (energy - previousEnergy[ENERGY_HISTORY_LENGTH - 1])/ENERGY_HISTORY_LENGTH/timeStep;
        }

        public double getBabyEnergy()
        {
            return energy - SAFE_SIZE;
        }

        public void addEnergy(double amount)
        {
            energy += amount;
        }

        public void setPreviousEnergy()
        {
            for (var i = ENERGY_HISTORY_LENGTH - 1; i >= 1; i--)
            {
                previousEnergy[i] = previousEnergy[i - 1];
            }
            previousEnergy[0] = energy;
        }

        public double measure(int choice)
        {
            var sign = 1 - 2*(choice%2);
            if (choice < 2)
            {
                return sign*energy;
            }
            if (choice < 4)
            {
                return sign*birthTime;
            }
            if (choice == 6 || choice == 7)
            {
                return sign*gen;
            }
            return 0;
        }

        public void setHue(double set)
        {
            myHue = Math.Min(Math.Max(set, 0), 1);
        }

        public void setMouthHue(double set)
        {
            mouthHue = Math.Min(Math.Max(set, 0), 1);
        }

        public void setSaturation(double set)
        {
            mySaturation = Math.Min(Math.Max(set, 0), 1);
        }

        public void setBrightness(double set)
        {
            myBrightness = Math.Min(Math.Max(set, 0), 1);
        }

        public double getVisionEndX(int i)
        {
            var visionTotalAngle = rotation + visionAngles[i];
            return px + visionDistances[i]*Math.Cos(visionTotalAngle);
        }

        public double getVisionEndY(int i)
        {
            var visionTotalAngle = rotation + visionAngles[i];
            return py + visionDistances[i]*Math.Sin(visionTotalAngle);
        }
    }
}