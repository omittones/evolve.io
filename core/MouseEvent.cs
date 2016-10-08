namespace core
{
    public class MouseEvent
    {
        private readonly int count;

        public MouseEvent(int count)
        {
            this.count = count;
        }

        public int Count()
        {
            return count;
        }
    }
}