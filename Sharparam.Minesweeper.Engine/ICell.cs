namespace Sharparam.Minesweeper.Engine
{
    public interface ICell
    {
        bool IsMine { get; }
        State State { get; set; }

        int X { get; }
        int Y { get; }

        int Count { get; }
    }
}
