namespace Sharparam.Minesweeper.Engine
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class CellCollection : ICellCollection
    {
        private readonly int _width;

        private readonly int _height;

        private readonly ICell[,] _cells;

        public ICell this[int x, int y]
        {
            get { return _cells[x, y]; }
        }

        public CellCollection(int width, int height)
        {
            _width = width;
            _height = height;
            _cells = new ICell[_width, _height];
            var squareCount = _width * _height;
            var mineCount = (int)(squareCount * Rng.NextDouble(0.2, 0.6));

            for (var i = 0; i < mineCount; i++)
            {
                var x = Rng.Next(0, _width);
                var y = Rng.Next(0, _height);
                while (this[x, y] != null)
                {
                    x = Rng.Next(0, _width);
                    y = Rng.Next(0, _height);
                }
                var square = new Cell(x, y, true);
                _cells[x, y] = square;
            }

            // Assign the rest of the squares
            for (var x = 0; x < _width; x++)
                for (var y = 0; y < _height; y++)
                    if (this[x, y] == null)
                        _cells[x, y] = new Cell(x, y, false);
        }

        public int Width { get { return _width; } }
        public int Height { get { return _height; } }

        public void Flag(int x, int y)
        {
            this[x, y].State = State.Flagged;
        }

        public void Mark(int x, int y)
        {
            this[x, y].State = State.Marked;
        }

        public void Show(int x, int y)
        {
            this[x, y].State = State.Shown;
        }

        IEnumerator<ICell> IEnumerable<ICell>.GetEnumerator()
        {
            return _cells.Cast<ICell>().GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _cells.GetEnumerator();
        }
    }
}
