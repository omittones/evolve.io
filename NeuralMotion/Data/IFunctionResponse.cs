namespace NeuralMotion.Data
{
    public interface IFunctionResponse
    {
        double[] Output { get; }

        double[] Input { get; }
    }
}