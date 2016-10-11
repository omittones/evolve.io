using System;

namespace NeuralMotion.TreeModel
{
    public class Cos : BaseNode
    {
        public static BaseNode MakeNew() { return new Cos(); }

        public Cos() : base(1, 3) { }

        public override double Evaluate(double inState)
        {
            return this.Parameters[0] * Math.Cos(this.Children[0].Evaluate(inState) / this.Parameters[1] + this.Parameters[2]);
        }

        public override string ToString()
        {
            return string.Format("{0} * cos ( x1 / {1} + {2} )", this.Parameters[0], this.Parameters[1], this.Parameters[2]);
        }
    }
}