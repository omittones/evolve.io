using NeuralMotion.Views;

namespace NeuralMotion
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.bwEngine = new System.ComponentModel.BackgroundWorker();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.bwBrains = new System.ComponentModel.BackgroundWorker();
            this.uiArena = new NeuralMotion.Views.BallDisplay();
            this.uiFitnessPlot = new NeuralMotion.Views.FitnessPlot();
            this.SuspendLayout();
            // 
            // refreshTimer
            // 
            this.refreshTimer.Interval = 10;
            this.refreshTimer.Tick += new System.EventHandler(this.OnRefreshTimer);
            // 
            // uiArena
            // 
            this.uiArena.Arena = null;
            this.uiArena.BackColor = System.Drawing.Color.Black;
            this.uiArena.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiArena.Location = new System.Drawing.Point(0, 0);
            this.uiArena.Name = "uiArena";
            this.uiArena.ShowPreviewFlag = false;
            this.uiArena.Size = new System.Drawing.Size(1220, 981);
            this.uiArena.TabIndex = 0;
            // 
            // uiFitnessPlot
            // 
            this.uiFitnessPlot.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiFitnessPlot.Location = new System.Drawing.Point(0, 981);
            this.uiFitnessPlot.Name = "uiFitnessPlot";
            this.uiFitnessPlot.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.uiFitnessPlot.Size = new System.Drawing.Size(1220, 344);
            this.uiFitnessPlot.TabIndex = 2;
            this.uiFitnessPlot.Text = "Fitness";
            this.uiFitnessPlot.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.uiFitnessPlot.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.uiFitnessPlot.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1220, 1325);
            this.Controls.Add(this.uiArena);
            this.Controls.Add(this.uiFitnessPlot);
            this.DoubleBuffered = true;
            this.Location = new System.Drawing.Point(20, 50);
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Main";
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bwEngine;
        private System.Windows.Forms.Timer refreshTimer;
        private System.ComponentModel.BackgroundWorker bwBrains;
        private BallDisplay uiArena;
        private FitnessPlot uiFitnessPlot;
    }
}

