using System;
using System.Windows.Forms;
using System.Diagnostics;
using NeuralMotion.Parallel;

namespace NeuralMotion
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Debugger.IsAttached)
            {
                ParallelProxy.GlobalDegreeOfParallelism = 1;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
