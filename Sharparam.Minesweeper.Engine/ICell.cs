namespace Sharparam.Minesweeper.Engine
{
    public interface ICell
    {
        bool IsMine { get; }
        bool IsFlagged { get; }
        bool IsMarked { get; }
        bool IsEmpty { get; }
        State State { get; }

        int X { get; }
        int Y { get; }

        int Count { get; }
    }
}
