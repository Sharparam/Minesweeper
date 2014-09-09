namespace Sharparam.Minesweeper.Engine
{
    using System.Collections.Generic;

    public interface ICellCollection : IEnumerable<ICell>
    {
        int Width { get; }
        int Height { get; }

        ICell this[int x, int y] { get; }

        void Flag(int x, int y);

        void Mark(int x, int y);

        void Show(int x, int y);
    }
}
