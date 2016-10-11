using System;
using System.Windows.Forms;

namespace NeuralMotion
{
    public partial class Settings : Form
    {
        public new Main Owner { get; private set; }

        public Settings()
        {
            InitializeComponent();

            this.uiToggleSpeed.Click += OnToggleSpeed;
            this.uiWriteLog.Checked = true;
            this.uiToggleSpeed.Text = "Fast";
        }

        public bool WriteStatus => uiWriteLog.Checked;
        public bool DontPreviewBest => uiDontPreviewBest.Checked;
        public bool EnableNetExpansion => uiEnableNetExpansion.Checked;
        public bool DontShowSim => uiDontShowSim.Checked;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Owner.BoxArena.RealTime)
                uiToggleSpeed.Text = "Fast";
            else
                uiToggleSpeed.Text = "Slow";

            base.OnPaint(e);
        }

        private void OnToggleSpeed(object sender, EventArgs e)
        {
            if (Owner.BoxArena.RealTime)
                Owner.BoxArena.RealTime = false;
            else
                Owner.BoxArena.RealTime = true;
            Refresh();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Owner = (Main) base.Owner;
        }
    }
}
