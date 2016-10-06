using System;
using System.Windows.Forms;

namespace core
{
    public partial class Main : Form
    {
        private EvolvioColor app;

        public Main()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.app = new EvolvioColor();
            this.app.setup();

            Application.Idle += ExecuteStep;
        }

        private void ExecuteStep(object sender, EventArgs e)
        {
            this.app.draw();
        }
    }
}
