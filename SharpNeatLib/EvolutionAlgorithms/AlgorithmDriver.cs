using System;
using System.Threading;
using log4net;
using SharpNeat.Core;

namespace SharpNeat.EvolutionAlgorithms
{
    public class AlgorithmDriver
    {
        public static AlgorithmDriver<TAlgorithm> Drive<TAlgorithm>(TAlgorithm algorithm)
            where TAlgorithm : IAlgorithm
        {
            var driver = new AlgorithmDriver<TAlgorithm>();

            driver.Initialize(algorithm);

            return driver;
        }
    }

    public class AlgorithmDriver<TAlgorithm> : AlgorithmDriver, IAlgorithmDriver 
        where TAlgorithm : IAlgorithm
    {
        private static readonly ILog __log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AlgorithmDriver()
        {
            _runState = RunState.NotReady;
            _updateScheme = new UpdateScheme(new TimeSpan(0, 0, 1));
        }
        
        // Algorithm state data.
        RunState _runState;
        
        // Update event scheme / data.
        UpdateScheme _updateScheme;
        
        uint _prevUpdateGeneration;
        long _prevUpdateTimeTick;
        
        // Misc working variables.
        Thread _algorithmThread;
        bool _pauseRequestFlag;
        readonly AutoResetEvent _awaitPauseEvent = new AutoResetEvent(false);
        readonly AutoResetEvent _awaitRestartEvent = new AutoResetEvent(false);
        IAlgorithm algorithm;

        /// <summary>
        /// Gets or sets the algorithm's update scheme.
        /// </summary>
        public UpdateScheme UpdateScheme
        {
            get { return _updateScheme; }
            set { _updateScheme = value; }
        }

        /// <summary>
        /// Gets the current execution/run state of the IEvolutionAlgorithm.
        /// </summary>
        public RunState RunState
        {
            get { return _runState; }
        }

        /// <summary>
        /// Notifies listeners that some state change has occured.
        /// </summary>
        public event EventHandler UpdateEvent;
        /// <summary>
        /// Notifies listeners that the algorithm has paused.
        /// </summary>
        public event EventHandler PausedEvent;

        /// <summary>
        /// Initiazlies driver with algorithm instance
        /// </summary>
        /// <param name="algorithm"></param>
        public void Initialize(IAlgorithm algorithm)
        {
            this.algorithm = algorithm;

            this._runState = RunState.Ready;
        }

        /// <summary>
        /// Starts the algorithm running. The algorithm will switch to the Running state from either
        /// the Ready or Paused states.
        /// </summary>
        public void StartContinue()
        {
            // RunState must be Ready or Paused.
            if (RunState.Ready == _runState)
            {
                // Create a new thread and start it running.
                _algorithmThread = new Thread(AlgorithmThreadMethod);
                _algorithmThread.Name = "AlgorithmThreadMethod";
                _algorithmThread.IsBackground = true;
                _algorithmThread.Priority = ThreadPriority.BelowNormal;
                _runState = RunState.Running;
                OnUpdateEvent();
                _algorithmThread.Start();
            }
            else if (RunState.Paused == _runState)
            {
                // Thread is paused. Resume execution.
                _runState = RunState.Running;
                OnUpdateEvent();
                _awaitRestartEvent.Set();
            }
            else if (RunState.Running == _runState)
            {
                // Already running. Log a warning.
                __log.Warn("StartContinue() called but algorithm is already running.");
            }
            else
            {
                throw new SharpNeatException($"StartContinue() call failed. Unexpected RunState [{_runState}]");
            }
        }

        /// <summary>
        /// Alias for RequestPause().
        /// </summary>
        public void Stop()
        {
            RequestPause();
        }

        /// <summary>
        /// Requests that the algorithm pauses but doesn't wait for the algorithm thread to stop.
        /// The algorithm thread will pause when it is next convenient to do so, and will notify
        /// listeners via an UpdateEvent.
        /// </summary>
        public void RequestPause()
        {
            if (RunState.Running == _runState)
            {
                _pauseRequestFlag = true;
            }
            else
            {
                __log.Warn("RequestPause() called but algorithm is not running.");
            }
        }

        /// <summary>
        /// Request that the algorithm pause and waits for the algorithm to do so. The algorithm
        /// thread will pause when it is next convenient to do so and notifies any UpdateEvent 
        /// listeners prior to returning control to the caller. Therefore it's generally a bad idea 
        /// to call this method from a GUI thread that also has code that may be called by the
        /// UpdateEvent - doing so will result in deadlocked threads.
        /// </summary>
        public void RequestPauseAndWait()
        {
            if (RunState.Running == _runState)
            {
                // Set a flag that tells the algorithm thread to enter the paused state and wait 
                // for a signal that tells us the thread has paused.
                _pauseRequestFlag = true;
                _awaitPauseEvent.WaitOne();
            }
            else
            {
                __log.Warn("RequestPauseAndWait() called but algorithm is not running.");
            }
        }

        private void AlgorithmThreadMethod()
        {
            try
            {
                _prevUpdateGeneration = 0;
                _prevUpdateTimeTick = DateTime.Now.Ticks;

                for (;;)
                {
                    algorithm.Execute();

                    if (UpdateTest())
                    {
                        _prevUpdateGeneration = algorithm.CurrentGeneration;
                        _prevUpdateTimeTick = DateTime.Now.Ticks;
                        OnUpdateEvent();
                    }

                    // Check if a pause has been requested. 
                    // Access to the flag is not thread synchronized, but it doesn't really matter if
                    // we miss it being set and perform one other generation before pausing.
                    if (_pauseRequestFlag || algorithm.StopConditionSatisfied)
                    {
                        // Signal to any waiting thread that we are pausing
                        _awaitPauseEvent.Set();

                        // Reset the flag. Update RunState and notify any listeners of the state change.
                        _pauseRequestFlag = false;
                        _runState = RunState.Paused;
                        OnUpdateEvent();
                        OnPausedEvent();

                        // Wait indefinitely for a signal to wake up and continue.
                        _awaitRestartEvent.WaitOne();
                    }
                }
            }
            catch (ThreadAbortException)
            {
                // Quietly exit thread.
            }
            catch (Exception ex)
            {
                __log.Error(ex);
            }

            if (_pauseRequestFlag)
                _awaitPauseEvent.Set();
        }

        /// <summary>
        /// Returns true if it is time to raise an update event.
        /// </summary>
        private bool UpdateTest()
        {
            if (UpdateMode.Generational == _updateScheme.UpdateMode)
            {
                return (algorithm.CurrentGeneration - _prevUpdateGeneration) >= _updateScheme.Generations;
            }

            return (DateTime.Now.Ticks - _prevUpdateTimeTick) >= _updateScheme.TimeSpan.Ticks;
        }

        private void OnUpdateEvent()
        {
            if (null != UpdateEvent)
            {
                // Catch exceptions thrown by even listeners. This prevents listener exceptions from terminating the algorithm thread.
                try
                {
                    UpdateEvent(this.algorithm, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    __log.Error("UpdateEvent listener threw exception", ex);
                }
            }
        }

        private void OnPausedEvent()
        {
            if (null != PausedEvent)
            {
                // Catch exceptions thrown by even listeners. This prevents listener exceptions from terminating the algorithm thread.
                try
                {
                    PausedEvent(this.algorithm, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    __log.Error("PausedEvent listener threw exception", ex);
                }
            }
        }
    }
}