namespace NeuralMotion.TreeModel
{
    public class Linear : BaseNode
    {
        public static BaseNode MakeNew() { return new Linear(); }

        public Linear() : base(1, 2) { }
        public override double Evaluate(double inState)
        {
            return this.Parameters[0] * this.GetChild(0).Evaluate(inState) + this.Parameters[1];
        }
        public override string ToString()
        {
            return string.Format("y = {0} * x1 + {1}", this.Parameters[0], this.Parameters[1]);
        }
    }
}