namespace Sharparam.Minesweeper.Engine
{
    internal class Cell : ICell
    {
        private readonly bool _isMine;

        private State _state;

        private readonly int _x;

        private readonly int _y;

        internal Cell(int x, int y, bool isMine)
        {
            _isMine = isMine;
            _state = State.Hidden;
            _x = x;
            _y = y;
        }

        public bool IsMine { get { return _isMine; } }
        public bool IsFlagged { get { return State == State.Flagged; } }
        public bool IsMarked { get {return State == State.Marked; } }
        public bool IsEmpty { get { return !IsMine; } }

        public State State
        {
            get { return _state; }
            internal set { _state = value; }
        }

        public int X { get { return _x; } }

        public int Y { get { return _y; } }

        public int Count { get; internal set; }
    }
}
