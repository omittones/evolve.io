namespace NeuralMotion.TreeModel
{
    public class Constant : BaseNode
    {
        public static BaseNode MakeNew() { return new Constant(); }

        public Constant() : base(0, 1) { }
        public override double Evaluate(double inState)
        {
            return this.Parameters[0];
        }
        public override string ToString()
        {
            return "y = " + this.Parameters[0];
        }
    }
}