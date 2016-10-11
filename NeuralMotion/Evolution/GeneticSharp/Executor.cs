using System;
using System.Threading.Tasks;
using GeneticSharp.Infrastructure.Framework.Threading;

namespace NeuralMotion.Evolution.GeneticSharp
{
    internal class Executor : ITaskExecutor
    {
        public Task Task { get; private set; }

        public void Add(Action action)
        {
            if (this.Task == null)
                this.Task = new Task(action);
            else
                this.Task = this.Task.ContinueWith(t => action());
        }

        public void Clear()
        {
        }

        public bool Start()
        {
            if (this.Task != null)
            {
                this.IsRunning = true;
                this.Task.Start();
            }
            return true;
        }

        public void Stop()
        {
            if (this.Task != null)
                this.Task.Wait();
            this.IsRunning = false;
        }

        public TimeSpan Timeout { get; set; }

        public bool IsRunning { get; private set; }
    }
}