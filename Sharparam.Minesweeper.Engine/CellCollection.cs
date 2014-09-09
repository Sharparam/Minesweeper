namespace Sharparam.Minesweeper.Engine
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class CellCollection : ICellCollection
    {
        private readonly int _width;

        private readonly int _height;

        private readonly int _cellCount;

        private readonly int _mineCount;

        private readonly ICell[,] _cells;

        private bool _firstCell = true;

        public ICell this[int x, int y]
        {
            get { return _cells[x, y]; }
            private set { _cells[x, y] = value; }
        }

        public CellCollection(int width, int height)
        {
            _width = width;
            _height = height;
            _cells = new ICell[_width, _height];
            _cellCount = _width * _height;
            _mineCount = (int)(_cellCount * Rng.NextDouble(0.2, 0.6));

            for (var i = 0; i < _mineCount; i++)
            {
                var x = Rng.Next(0, _width);
                var y = Rng.Next(0, _height);
                while (this[x, y] != null)
                {
                    x = Rng.Next(0, _width);
                    y = Rng.Next(0, _height);
                }
                var square = new Cell(x, y, true);
                this[x, y] = square;
            }

            // Assign the rest of the squares
            for (var x = 0; x < _width; x++)
                for (var y = 0; y < _height; y++)
                    if (this[x, y] == null)
                        this[x, y] = new Cell(x, y, false);
        }

        public int Width { get { return _width; } }
        public int Height { get { return _height; } }
        public int CellCount { get { return _cellCount; } }
        public int MineCount { get { return _mineCount; } }

        public int Flagged
        {
            get
            {
                return this.Aggregate(0, (i, cell) => i + (cell.IsFlagged ? 1 : 0));
            }
        }

        public void Flag(int x, int y)
        {
            var cell = ((Cell)this[x, y]);
            cell.State = cell.IsFlagged ? State.Hidden : State.Flagged;
        }

        public void Mark(int x, int y)
        {
            var cell = ((Cell)this[x, y]);
            cell.State = cell.IsMarked ? State.Hidden : State.Marked;
        }

        public void Show(int x, int y)
        {
            var cell = (Cell)this[x, y];

            cell.State = State.Shown;

            if (_firstCell)
            {
                if (cell.IsMine)
                {
                    // Reassign the mine to be nice to the player
                    // This can probably be done better
                    cell = new Cell(x, y, false);
                    this[x, y] = cell;

                    var assigned = false;
                    while (!assigned)
                    {
                        var i = Rng.Next(0, Width);
                        var j = Rng.Next(0, Height);

                        if ((i == x && j == x) || this[i, j].IsMine)
                            continue;

                        this[i, j] = new Cell(i, j, true);
                        assigned = true;
                    }
                }
                
                CalculateCounts();

                _firstCell = false;
            }

            if (cell.IsMine)
                return;

            // Call Show on neighboring cells that are currently hidden and
            // are not mines
            for (var i = x - 1; i < x + 2; i++)
            {
                for (var j = y - 1; j < y + 2; j++)
                {
                    if ((i == x && j == y)
                        || i < 0 || i >= Width
                        || j < 0 || j >= Height
                        || (i == x - 1 && j == y - 1)
                        || (i == x - 1 && j == y + 1)
                        || (i == x + 1 && j == y - 1)
                        || (i == x + 1 && j == y + 1))
                        continue;

                    var neighbor = this[i, j];
                    if (neighbor.State == State.Hidden && !neighbor.IsMine)
                        Show(i, j);
                }
            }
        }

        public void ShowAll()
        {
            foreach (Cell cell in this)
                cell.State = State.Shown;
        }

        private void CalculateCounts()
        {
            foreach (Cell cell in this)
            {
                var count = 0;
                var x = cell.X;
                var y = cell.Y;
                for (var i = x - 1; i < x + 2; i++)
                {
                    for (var j = y - 1; j < y + 2; j++)
                    {
                        if ((i == x && j == y)
                            || i < 0 || i >= Width
                            || j < 0 || j >= Height)
                            continue;
                        if (this[i, j].IsMine)
                            count++;
                    }
                }

                cell.Count = count;
            }
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
