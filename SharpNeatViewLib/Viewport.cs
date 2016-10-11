/* ***************************************************************************
 * This file is part of SharpNEAT - Evolution of Neural Networks.
 * 
 * Copyright 2004-2016 Colin Green (sharpneat@gmail.com)
 *
 * SharpNEAT is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * SharpNEAT is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with SharpNEAT.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SharpNeat.View
{
    /// <summary>
    /// A user control that provides a re resizable area that can be painted to by an 
    /// IViewportPainter.
    /// </summary>
    public class Viewport : UserControl
    {
        private const PixelFormat ViewportPixelFormat = PixelFormat.Format16bppRgb565;
        private readonly Brush _brushBackground = new SolidBrush(Color.Lavender);

        private Rectangle _viewportArea;
        private float _zoomFactor = 1f;
        private Image _image;

        #region Component designer variables

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private readonly Container components = null;

        #endregion

        #region Constructor / Disposal

        /// <summary>
        /// Default constructor. Required for user controls.
        /// </summary>
        public Viewport()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            var width = Width;
            var height = Height;
            _viewportArea = new Rectangle(0, 0, width, height);

            // Create a bitmap for the picturebox.
            _image = new Bitmap(width, height, ViewportPixelFormat);
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Component Designer Generated Code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Viewport
            // 
            this.Name = "Viewport";
            this.Size = new Size(216, 232);
            this.ResumeLayout(false);

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the viewport's IViewportPainter.
        /// </summary>
        public IViewportPainter ViewportPainter { get; set; }

        /// <summary>
        /// Gets or sets the viewport's zoom factor.
        /// </summary>
        public float ZoomFactor
        {
            get { return _zoomFactor; }
            set
            {
                _zoomFactor = value;
                RefreshImage();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Refresh/repaint the image being displayed by the control.
        /// </summary>
        public void RefreshImage()
        {
            var g = Graphics.FromImage(_image);
            g.FillRectangle(_brushBackground, 0, 0, _image.Width, _image.Height);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // If a painter has been assigned then paint the graph.
            if (null != ViewportPainter)
                ViewportPainter.Paint(g, _viewportArea, _zoomFactor);

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.DrawImage(this._image, Point.Empty);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            const float ImageSizeChangeDelta = 100f;

            // Track viewport area.
            var width = Width;
            var height = Height;
            _viewportArea.Size = new Size(width, height);

            // Handle calls during control initialization (image may not be created yet).
            if (null == _image)
            {
                return;
            }

            // If the viewport has grown beyond the size of the image then create a new image. 
            // Note. If the viewport shrinks we just paint on the existing (larger) image, this prevents unnecessary 
            // and expensive construction/destrucion of Image objects.
            if (width > _image.Width || height > _image.Height)
            {
                // Reset the image's size. We round up the the nearest __imageSizeChangeDelta. This prevents unnecessary 
                // and expensive construction/destrucion of Image objects as the viewport is resized multiple times.
                var imageWidth = (int) (Math.Ceiling(width/ImageSizeChangeDelta)*ImageSizeChangeDelta);
                var imageHeight = (int) (Math.Ceiling(height/ImageSizeChangeDelta)*ImageSizeChangeDelta);
                _image = new Bitmap(imageWidth, imageHeight, ViewportPixelFormat);
            }

            RefreshImage();
        }

        #endregion
    }
}