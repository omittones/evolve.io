using System;
using System.Linq;
using System.Windows.Forms;
using SharpNeat.Core;
using SharpNeat.Domains;
using SharpNeat.Genomes.Neat;

namespace SharpNeatGUI.Controllers
{
    public class GenomeDisplayController
    {
        private readonly Form main;
        private readonly Control container;
        private AbstractView view;
        private IAlgorithmDriver driver;

        public GenomeDisplayController(Form main, Control container)
        {
            this.main = main;
            this.container = container;

            main.FormClosing += HandleClose;
        }

        public void Reconnect(AbstractView newView, IAlgorithmDriver newDriver)
        {
            if (this.view != null)
            {
                this.container.Controls.Remove(this.view);
                this.view.Dispose();
            }

            if (newView != null)
            {
                this.view = newView;
                this.container.Controls.Add(this.view);
                this.view.Dock = DockStyle.Fill;

                // Clean up.
                if (null != this.driver)
                {
                    this.driver.UpdateEvent -= HandleUpdate;
                }

                // Reconnect.
                this.driver = newDriver;
                this.driver.UpdateEvent += HandleUpdate;
            }
        }

        public void HandleUpdate(object sender, EventArgs e)
        {
            var algorithm = (IEvolutionAlgorithm<NeatGenome>) sender;

            // Switch execution to GUI thread if necessary.
            if (this.view.InvokeRequired)
            {
                // Must use Invoke(). BeginInvoke() will execute asynchronously and the evolution algorithm therefore 
                // may have moved on and will be in an intermediate and indeterminate (between generations) state.
                this.view.Invoke(new MethodInvoker(delegate
                {
                    if (this.view.IsDisposed)
                        return;

                    var currentChamp = algorithm.CurrentChampGenome;
                    var currentPopulation = algorithm.GenomeList.Cast<object>().ToArray();

                    view.RefreshView(currentChamp, currentPopulation);
                }));
            }
        }

        private void HandleClose(object sender, FormClosingEventArgs e)
        {
            if (null != driver)
            {
                driver.UpdateEvent -= HandleUpdate;
            }
        }
    }
}
