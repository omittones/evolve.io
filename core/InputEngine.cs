using System;
using System.Windows.Forms;

namespace core
{
    public class InputEngine
    {
        private readonly Form container;

        public bool KeyPressed { get; private set; }
        public Keys KeyCode { get; private set; }
        public char Key { get; private set; }
        public int MouseX { get; private set; }
        public int MouseY { get; private set; }
        public event Action<MouseEvent> OnMouseWheel;
        public event Action OnMousePressed;
        public event Action OnMouseReleased;

        public InputEngine(Form container)
        {
            this.container = container;

            container.MouseMove += UpdateMouse;
            container.KeyDown += SetKeyCode;
            container.KeyPress += SetKeyChar;
            container.KeyUp += RemoveKeyCode;

            container.MouseWheel += (sender, e) =>
            {
                OnMouseWheel?.Invoke(new MouseEvent(e.Delta));
            };
            container.MouseDown += (sender, e) =>
            {
                this.MouseX = e.X;
                this.MouseY = e.Y;
                this.OnMousePressed?.Invoke();
            };
            container.MouseUp += (sender, e) =>
            {
                this.MouseX = e.X;
                this.MouseY = e.Y;
                this.OnMouseReleased?.Invoke();
            };

            container.Disposed += Detach;
        }

        private void Detach(object sender, EventArgs e)
        {
            container.MouseMove -= UpdateMouse;
            container.KeyDown -= SetKeyCode;
            container.KeyPress -= SetKeyChar;
            container.KeyUp -= RemoveKeyCode;
        }

        private void SetKeyChar(object sender, KeyPressEventArgs e)
        {
            this.Key = e.KeyChar;
        }

        private void SetKeyCode(object sender, KeyEventArgs e)
        {
            this.KeyPressed = true;
            this.KeyCode = e.KeyCode;
            this.Key = char.MaxValue;
        }

        private void RemoveKeyCode(object sender, KeyEventArgs e)
        {
            this.KeyPressed = false;
        }

        private void UpdateMouse(object sender, MouseEventArgs e)
        {
            this.MouseX = e.X;
            this.MouseY = e.Y;
        }
    }
}