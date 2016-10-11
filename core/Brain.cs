using System;
using core.Graphics;

namespace core
{
    public class Brain
    {
        private const int BRAIN_HEIGHT = 12;
        private const int BRAIN_WIDTH = 3;
        private const double STARTING_AXON_VARIABILITY = 1.0;
        private const double AXON_START_MUTABILITY = 0.0005;
        private const int MEMORY_COUNT = 1;

        private readonly Axon[,,] axons;
        private readonly double[] memories;
        private readonly double[,] neurons;
        private readonly double[] outputs;

        public Brain()
        {
            outputs = new double[BRAIN_HEIGHT];
            memories = new double[MEMORY_COUNT];
            axons = new Axon[BRAIN_WIDTH - 1, BRAIN_HEIGHT, BRAIN_HEIGHT - 1];
            neurons = new double[BRAIN_WIDTH, BRAIN_HEIGHT];
            for (var x = 0; x < BRAIN_WIDTH - 1; x++)
            {
                for (var y = 0; y < BRAIN_HEIGHT; y++)
                {
                    for (var z = 0; z < BRAIN_HEIGHT - 1; z++)
                    {
                        var startingWeight = 0.0;
                        if (y == BRAIN_HEIGHT - 1)
                            startingWeight = Rnd.nextFloat(-1, 1)*STARTING_AXON_VARIABILITY;
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

        public Brain(Axon[,,] axons, double[,] neurons)
        {
            this.outputs = new double[BRAIN_HEIGHT];
            this.memories = new double[MEMORY_COUNT];
            this.axons = axons;
            this.neurons = neurons;
        }

        public static Brain spawnFrom(Brain[] parents)
        {
            var newBrain = new Axon[BRAIN_WIDTH - 1, BRAIN_HEIGHT, BRAIN_HEIGHT - 1];
            var newNeurons = new double[BRAIN_WIDTH, BRAIN_HEIGHT];
            var randomParentRotation = Rnd.nextFloat(0, 1);

            for (var x = 0; x < BRAIN_WIDTH - 1; x++)
            {
                for (var y = 0; y < BRAIN_HEIGHT; y++)
                {
                    for (var z = 0; z < BRAIN_HEIGHT - 1; z++)
                    {
                        var axonAngle = Math.Atan2((y + z)/2.0f - BRAIN_HEIGHT/2.0f, x - BRAIN_WIDTH/2)/(2*Math.PI) + Math.PI;
                        var parentForAxon = parents[((int) (((axonAngle + randomParentRotation)%1.0)*parents.Length))];
                        newBrain[x, y, z] = parentForAxon.axons[x, y, z].mutateAxon();
                    }
                }
            }

            for (var x = 0; x < BRAIN_WIDTH; x++)
            {
                for (var y = 0; y < BRAIN_HEIGHT; y++)
                {
                    var axonAngle = Math.Atan2(y - BRAIN_HEIGHT/2.0, x - BRAIN_WIDTH/2)/(2*Math.PI) + Math.PI;
                    var parentForAxon = parents[(int) (((axonAngle + randomParentRotation)%1.0)*parents.Length)];
                    newNeurons[x, y] = parentForAxon.neurons[x, y];
                }
            }

            return new Brain(newBrain, newNeurons);
        }

        public bool wantsToGiveBirth()
        {
            return this.neurons[BRAIN_WIDTH - 1, 9] > -1;
        }

        public double[] useBrain(double energy, double[] visionResults)
        {
            const int end = BRAIN_WIDTH - 1;

            if (visionResults.Length != 9)
                throw new ApplicationException();

            for (var i = 0; i < 9; i++)
                neurons[0, i] = visionResults[i];
            neurons[0, 9] = energy;
            for (var i = 0; i < memories.Length; i++)
                neurons[0, 10 + i] = memories[i];

            for (var x = 1; x < BRAIN_WIDTH; x++)
            {
                for (var y = 0; y < BRAIN_HEIGHT - 1; y++)
                {
                    double total = 0;
                    for (var input = 0; input < BRAIN_HEIGHT; input++)
                        total += neurons[x - 1, input]*axons[x - 1, input, y].weight;

                    if (x == BRAIN_WIDTH - 1)
                        neurons[x, y] = total;
                    else
                        neurons[x, y] = sigmoid(total);
                }
            }

            for (var i = 0; i < MEMORY_COUNT; i++)
                this.memories[i] = neurons[end, 10 + i];

            for (var i = 0; i < outputs.Length; i++)
                outputs[i] = neurons[end, i];

            return outputs;
        }

        private double sigmoid(double input)
        {
            return 1.0 / (1.0 + Math.Pow(2.71828182846, -input));
        }

        private const float neuronSize = 0.3f;
        private const float widthToHeightScale = (float)BRAIN_WIDTH / BRAIN_HEIGHT;

        public void draw(GraphicsEngine graphics, int highlightAtX, int highlightAtY)
        {
            graphics.noStroke();
            graphics.fill(0, 0, 0.4f);
            graphics.rect(0, 0, widthToHeightScale + 0.35f, 1);

            graphics.pushMatrix();
            graphics.scale(1.0f/BRAIN_HEIGHT);
            graphics.translate(1.7f, 0.5f);

            this.drawLabels(graphics);

            var coord = graphics.transformToWorld(highlightAtX, highlightAtY);
            var highX = (int)Math.Round(coord.X);
            var highY = (int)Math.Round(coord.Y);

            this.showAxons(graphics, highX, highY);

            graphics.textAlign(AlignText.CENTER);
            for (var x = 0; x < BRAIN_WIDTH; x++)
            {
                for (var y = 0; y < BRAIN_HEIGHT; y++)
                {
                    var val = neurons[x, y];
                    var textColor = neuronTextColor(val);

                    graphics.noStroke();
                    graphics.fill(neuronFillColor(val));
                    graphics.ellipse(x, y, neuronSize, neuronSize);
                    graphics.textSize(neuronSize*1.2f);
                    graphics.fill(textColor);
                    graphics.text(val.ToString("0.00"), x, (y + (neuronSize*0.6f)));

                    if (highX == x && highY == y)
                    {
                        graphics.noFill();
                        graphics.stroke(textColor);
                        graphics.strokeWeight(0.1f);
                        graphics.ellipse(x, y, neuronSize, neuronSize);
                    }
                }
            }

            graphics.popMatrix();
        }

        private void showAxons(GraphicsEngine graphics, int neuronX, int neuronY)
        {
            if (neuronX >= 0 && neuronX < BRAIN_WIDTH)
            {
                var maxY = axons.GetUpperBound(neuronX);
                if (neuronY >= 0 && neuronY < maxY)
                {
                    if (neuronX > 0)
                        for (var i = 0; i < axons.GetUpperBound(neuronX - 1); i++)
                            drawAxon(graphics, neuronX - 1, i, neuronX, neuronY);
                    if (neuronX < BRAIN_WIDTH - 1)
                        for (var i = 0; i < axons.GetUpperBound(neuronX + 1); i++)
                            drawAxon(graphics, neuronX, neuronY, neuronX + 1, i);
                }
            }
        }

        private void drawLabels(GraphicsEngine graphics)
        {
            graphics.strokeWeight(2);
            graphics.textSize(0.58f);
            graphics.fill(0, 0, 1);
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
                graphics.textAlign(AlignText.RIGHT);
                graphics.text(inputLabels[y], (-neuronSize - 0.1f), (y + (neuronSize*0.6f)));
                graphics.textAlign(AlignText.LEFT);
                graphics.text(outputLabels[y], (BRAIN_WIDTH - 1 + neuronSize + 0.1f), (y + (neuronSize*0.6f)));
            }
        }

        private void drawAxon(GraphicsEngine graphics, int xFrom, int yFrom, int xTo, int yTo)
        {
            graphics.stroke(neuronFillColor(axons[xFrom, yFrom, yTo].weight));
            graphics.strokeWeight(0.05f);
            graphics.line(xFrom, yFrom, xTo, yTo);
        }

        private HSBColor neuronFillColor(double d)
        {
            return new HSBColor(0, 0, (float) d/2.0f + 0.5f);
        }

        private HSBColor neuronTextColor(double d)
        {
            if (d >= 0)
                return new HSBColor(0, 0, 0);
            return new HSBColor(0, 0, 1);
        }
    }
}