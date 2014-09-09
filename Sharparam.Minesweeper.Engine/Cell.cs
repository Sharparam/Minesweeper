namespace Sharparam.Minesweeper.Engine
{
    internal class Cell : ICell
    {
        private bool _isMine;

        private State _state;

        private int _x;

        private int _y;

        internal Cell(int x, int y, bool isMine)
        {
            _isMine = isMine;
            _state = State.Hidden;
            _x = x;
            _y = y;
        }

        public bool IsMine { get { return _isMine; } }

        public State State
        {
            get { return _state; }
            set { _state = value; }
        }

        public int X { get { return _x; } }

        public int Y { get { return _y; } }

        public int Count { get; internal set; }
    }
}
