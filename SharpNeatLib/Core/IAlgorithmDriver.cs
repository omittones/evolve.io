using System;

namespace SharpNeat.Core
{
    public interface IAlgorithmDriver
    {
        /// <summary>
        /// Gets the current execution/run state of the IEvolutionAlgorithm.
        /// </summary>
        RunState RunState { get; }
        
        /// <summary>
        /// Notifies listeners that some state change has occured.
        /// </summary>
        event EventHandler UpdateEvent;

        /// <summary>
        /// Notifies listeners that driver was paused.
        /// </summary>
        event EventHandler PausedEvent;

        /// <summary>
        /// Gets or sets the algorithm's update scheme.
        /// </summary>
        UpdateScheme UpdateScheme { get; set; }

        /// <summary>
        /// Starts the algorithm running. The algorithm will switch to the Running state from either
        /// the Ready or Paused states.
        /// </summary>
        void StartContinue();

        /// <summary>
        /// Requests that the algorithm pauses but doesn't wait for the algorithm thread to stop.
        /// The algorithm thread will pause when it is next convenient to do so, and notifies
        /// listeners via an UpdateEvent.
        /// </summary>
        void RequestPause();

        /// <summary>
        /// Request that the algorithm pause and waits for the algorithm to do so. The algorithm
        /// thread will pause when it is next convenient to do so and notifies any UpdateEvent 
        /// listeners prior to returning control to the caller. Therefore it's generally a bad idea 
        /// to call this method from a GUI thread that also has code that may be called by the
        /// UpdateEvent - doing so will result in deadlocked threads.
        /// </summary>
        void RequestPauseAndWait();
    }
}