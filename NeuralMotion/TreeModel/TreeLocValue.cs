namespace NeuralMotion.TreeModel
{
    public struct TreeLocValue
    {
        public int NodeIndex;
        public int ParamIndex;
        public double Value;

        public TreeLocValue(int inNodeIndex, int inParamIndex)
        {
            NodeIndex = inNodeIndex;
            ParamIndex = inParamIndex;
            Value = 0;
        }

        public TreeLocValue(int inNodeIndex, int inParamIndex, double inValue)
            : this(inNodeIndex, inParamIndex)
        {
            Value = inValue;
        }
    }
}