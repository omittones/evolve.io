using ZedGraph;

namespace NeuralMotion.Views
{
    partial class MultipleFunctionView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private ZedGraphControl graph;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.graph = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // graph
            // 
            this.graph.BackColor = System.Drawing.SystemColors.Control;
            this.graph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graph.Location = new System.Drawing.Point(0, 0);
            this.graph.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.graph.Name = "graph";
            this.graph.ScrollGrace = 0D;
            this.graph.ScrollMaxX = 0D;
            this.graph.ScrollMaxY = 0D;
            this.graph.ScrollMaxY2 = 0D;
            this.graph.ScrollMinX = 0D;
            this.graph.ScrollMinY = 0D;
            this.graph.ScrollMinY2 = 0D;
            this.graph.Size = new System.Drawing.Size(973, 806);
            this.graph.TabIndex = 11;
            this.graph.UseExtendedPrintDialog = true;
            // 
            // FunctionView
            // 
            this.Controls.Add(this.graph);
            this.Name = "FunctionView";
            this.Size = new System.Drawing.Size(973, 806);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
