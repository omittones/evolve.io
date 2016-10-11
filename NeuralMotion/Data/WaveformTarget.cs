using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Util;

namespace NeuralMotion.Data
{
    public class WaveformTarget : IFunctionResponse
    {
        public double[] Output { get; }

        public double[] Input { get; }

        public WaveformTarget(double biasX, double biasY)
        {
            Input = new double[64];
            Output = new double[64];
            
            Enumerable
                .Range(0, this.Output.Length)
                .ToArray()
                .Normalize(-1, 1)
                .CopyTo(this.Input, 0);

            var file = Assembly.GetExecutingAssembly().GetManifestResourceStream("NeuralMotion.Data.Resources.light.wav");
            var fileValues = new byte[this.Output.Length*2];
            file.Seek(2000, SeekOrigin.Begin);
            file.Read(fileValues, 0, this.Output.Length*2);
            file.Close();
            for (var i = 0; i < this.Output.Length; i++)
            {
                //waveform
                this.Output[i] = BitConverter.ToInt16(fileValues, i*2)/300.0;
                this.Output[i] = this.Output[i]/-40.0 - 1.0;

                ////custom tanh
                //var x = i/(this.target.Length - 1.0f);
                //x = (x - 0.5f)*2.0f;
                //var y1 = Math.Tanh(x*3.0f)*1.5f;
                //var y2 = -Math.Tanh((x + 0.5f)*12.0f)*0.3f + 0.3f;
                //var y3 = Math.Tanh((-x + 0.3f)*12.0f)*0.4f + 0.4f;
                //target[i] = (float) (y1 + y2 + y3);
            }

            this.Input.Bias(biasX).CopyTo(this.Input, 0);
            this.Output.Normalize(-1, 1).Bias(biasY).CopyTo(this.Output, 0);
        }
    }
}
