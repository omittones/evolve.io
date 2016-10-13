using System;
using System.Drawing;
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
            memories = new double[MEMORY_COUNT];
            outputs = new double[BRAIN_HEIGHT];
            axons = new Axon[BRAIN_WIDTH - 1, BRAIN_HEIGHT, BRAIN_HEIGHT];
            neurons = new double[BRAIN_WIDTH, BRAIN_HEIGHT];
            for (var x = 0; x < BRAIN_WIDTH - 1; x++)
            {
                for (var y = 0; y < BRAIN_HEIGHT; y++)
                {
                    for (var z = 0; z < BRAIN_HEIGHT; z++)
                    {
                        var startingWeight = Rnd.nextFloat(-1, 1)*STARTING_AXON_VARIABILITY;
                        axons[x, y, z] = new Axon(startingWeight, AXON_START_MUTABILITY);
                    }
                }
            }

            neurons = new double[BRAIN_WIDTH, BRAIN_HEIGHT];
            for (var x = 0; x < BRAIN_WIDTH; x++)
                for (var y = 0; y < BRAIN_HEIGHT; y++)
                    neurons[x, y] = 0;
        }

        public Brain(Axon[,,] axons, double[,] neurons)
        {
            this.memories = new double[MEMORY_COUNT];
            this.outputs = new double[BRAIN_HEIGHT];
            this.axons = axons;
            this.neurons = neurons;
        }

        public static Brain spawnFrom(Brain[] parents)
        {
            var newBrain = new Axon[BRAIN_WIDTH - 1, BRAIN_HEIGHT, BRAIN_HEIGHT];
            var newNeurons = new double[BRAIN_WIDTH, BRAIN_HEIGHT];
            var randomParentRotation = Rnd.nextFloat(0, 1);

            for (var x = 0; x < BRAIN_WIDTH; x++)
            {
                for (var y = 0; y < BRAIN_HEIGHT; y++)
                {
                    for (var z = 0; z < BRAIN_HEIGHT; z++)
                    {
                        var axonAngle = Math.Atan2((y + z)/2.0f - BRAIN_HEIGHT/2.0f, x - BRAIN_WIDTH/2)/(2*Math.PI) + Math.PI;
                        var parentForAxon = parents[(int) (((axonAngle + randomParentRotation)%1.0)*parents.Length)];
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

            for (var i = 0; i < visionResults.Length; i++)
                neurons[0, i] = visionResults[i];
            neurons[0, 9] = energy;
            for (var i = 0; i < memories.Length; i++)
                neurons[0, 10 + i] = memories[i];
            neurons[0, 10 + memories.Length] = 1;

            for (var xLayer = 1; xLayer < BRAIN_WIDTH; xLayer++)
            {
                for (var xNeuron = 0; xNeuron < BRAIN_HEIGHT; xNeuron++)
                {
                    double total = 0;
                    for (var xInputNeuron = 0; xInputNeuron < BRAIN_HEIGHT; xInputNeuron++)
                        total += neurons[xLayer - 1, xInputNeuron]*axons[xLayer - 1, xInputNeuron, xNeuron].weight;
                    neurons[xLayer, xNeuron] = sigmoid(total);
                }
            }

            for (var i = 0; i < MEMORY_COUNT; i++)
                this.memories[i] = neurons[end, 10 + i];

            for (var i = 0; i < outputs.Length; i++)
                outputs[i] = neurons[end, i];

            return outputs;
        }

        private static double sigmoid(double input)
        {
            return 1.0/(1.0 + Math.Pow(2.71828182846, -input));
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
            Console.WriteLine($"{neuronX} - {neuronY}");

            if (neuronX >= 0 && neuronX < BRAIN_WIDTH)
                if (neuronY >= 0 && neuronY < BRAIN_HEIGHT)
                {
                    if (neuronX > 0)
                        for (var i = 0; i < BRAIN_HEIGHT; i++)
                        {
                            var axon = axons[neuronX - 1, i, neuronY];
                            drawAxon(graphics, axon, neuronX - 1, i, neuronX, neuronY);
                        }

                    if (neuronX < BRAIN_WIDTH - 1)
                        for (var i = 0; i < BRAIN_HEIGHT; i++)
                        {
                            var axon = axons[neuronX, neuronY, i];
                            drawAxon(graphics, axon, neuronX, neuronY, neuronX + 1, i);
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

        private void drawAxon(GraphicsEngine graphics, Axon axon, int xFrom, int yFrom, int xTo, int yTo)
        {
            graphics.strokeWeight(0.05f);
            if (axon != null)
                graphics.stroke(neuronFillColor(axon.weight));
            else
                graphics.stroke(Color.Red);
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