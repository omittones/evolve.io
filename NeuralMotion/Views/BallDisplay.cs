using System;
using System.Drawing;
using System.Windows.Forms;
using NeuralMotion.Simulator;
using Util;

namespace NeuralMotion.Views
{
    public partial class BallDisplay : UserControl
    {
        public bool ShowPreviewFlag { get; set; }

        private readonly Pen penRed = new Pen(Color.Red);
        private readonly Pen penBlue = new Pen(Color.Blue);
        private readonly Pen penGreen = new Pen(Color.Green);
        private Font fontText;
        
        public BoxArena Arena { get; set; }

        public BallDisplay()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
                          ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (this.Arena != null)
            {
                this.penRed.Width = this.Arena.BallRadius/5;
                this.penBlue.Width = this.penRed.Width;
                this.penGreen.Width = 0.005f;
                this.fontText = new Font(FontFamily.GenericSansSerif, this.Arena.BallRadius, FontStyle.Regular,
                    GraphicsUnit.Point);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (this.Arena != null)
            {
                var smallerClientSize = Math.Min(this.ClientSize.Width, this.ClientSize.Height);

                e.Graphics.ScaleTransform(smallerClientSize, smallerClientSize);
                e.Graphics.ScaleTransform(0.5f, 0.5f);
                e.Graphics.TranslateTransform(1.0f, 1.0f);

                var radius = Arena.BallRadius*2;
                var halfRadius = Arena.BallRadius;

                var time = $"{this.Arena.CurrentSimulationTime} {(ShowPreviewFlag ? "PREVIEW" : "")}";
                e.Graphics.DrawString(time, this.fontText, penRed.Brush, new PointF(-0.9f, -0.9f));

                for (var index = -1; index < Arena.EngineBalls.Length; index++)
                {
                    PointF point;
                    if (index == -1)
                        point = new PointF(0, 0);
                    else
                        point = Arena.EngineBalls[index].Position;

                    if (index == -1)
                    {
                        e.Graphics.DrawLine(penRed, point.Offset(-0.1f, 0), point.Offset(0.1f, 0));
                        e.Graphics.DrawLine(penRed, point.Offset(0, -0.1f), point.Offset(0, 0.1f));
                    }
                    else
                    {
                        var ball = Arena.EngineBalls[index];
                        e.Graphics.DrawLine(penGreen, point, point.Offset(ball.Acceleration));

                        if (Arena.CurrentSimulationTime - ball.LastCollisionTime < 0.1)
                            e.Graphics.DrawEllipse(penBlue, point.X - halfRadius, point.Y - halfRadius, radius, radius);
                        else
                            e.Graphics.DrawEllipse(penRed, point.X - halfRadius, point.Y - halfRadius, radius, radius);

                        var totalKicks = ball.KicksToBall + ball.KicksToBorder;

                        var totalKicksText = totalKicks.ToString();
                        var offsetX = (-fontText.SizeInPoints*0.6f)*totalKicksText.Length;
                        e.Graphics.DrawString(totalKicksText, fontText, penRed.Brush,
                            point.Offset(offsetX, -fontText.SizeInPoints*0.7f));
                    }
                }
            }
        }
    }
}
