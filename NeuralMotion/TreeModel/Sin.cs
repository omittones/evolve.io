using System;

namespace NeuralMotion.TreeModel
{
    public class Sin : BaseNode
    {
        public static BaseNode MakeNew() { return new Sin(); }

        public Sin() : base(1, 3) { }

        public override double Evaluate(double inState)
        {
            return this.Parameters[0] * Math.Sin(this.Children[0].Evaluate(inState) / this.Parameters[1] + this.Parameters[2]);
        }

        public override string ToString()
        {
            return string.Format("{0} * sin ( x1 / {1} + {2} )", this.Parameters[0], this.Parameters[1], this.Parameters[2]);
        }
    }
}