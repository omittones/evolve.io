namespace NeuralMotion.TreeModel
{
    public class Minus : BaseNode
    {
        public static BaseNode MakeNew() { return new Minus(); }

        public Minus() : base(2, 0) { }

        public override double Evaluate(double inState)
        {
            return this.Children[0].Evaluate(inState) - this.Children[1].Evaluate(inState);
        }

        public override string ToString()
        {
            return "y = x1 - x2";
        }
    }
}