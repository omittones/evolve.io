using System;
using System.Drawing.Drawing2D;

namespace core.Graphics
{
    public class MatrixScope : IDisposable
    {
        private readonly Matrix current;
        private readonly System.Drawing.Graphics engine;

        public MatrixScope(System.Drawing.Graphics engine)
        {
            this.engine = engine;
            this.current = engine.Transform.Clone();
        }

        public void Dispose()
        {
            this.engine.Transform = this.current;
        }
    }
}