namespace NeuralMotion.TreeModel
{
    public class State : BaseNode
    {
        public static BaseNode MakeNew() { return new State(); }

        public State() : base(0, 0) { }
        public override double Evaluate(double inState)
        {
            return inState;
        }
        public override string ToString()
        {
            return "y = state";
        }
    }
}