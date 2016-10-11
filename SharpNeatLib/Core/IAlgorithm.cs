namespace SharpNeat.Core
{
    public interface IAlgorithm
    {
        /// <summary>
        /// Gets a value indicating whether some goal fitness has been achieved and that the algorithm has therefore stopped.
        /// </summary>
        bool StopConditionSatisfied { get; }

        void Execute();

        /// <summary>
        /// Number of times Execute was called. 
        /// </summary>
        uint CurrentGeneration { get; }
    }
}