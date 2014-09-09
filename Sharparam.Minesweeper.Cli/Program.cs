namespace Sharparam.Minesweeper.Cli
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    using Sharparam.Minesweeper.Engine;

    public class Program
    {
        private const char MineChar = 'X';

        private const char CellSeparator = ' ';

        private const char FieldCorner = ' ';

        private const char CornerConnector = ' ';

        private static readonly Dictionary<State, char> StateMapping = new Dictionary<State, char>
        {
            {State.Hidden, '.'},
            {State.Marked, '?'},
            {State.Flagged, '!'},
            {State.Shown, ' '}
        };

        private static readonly Regex CommandRegex = new Regex(@"(?<mode>[MmFf])?\D*(?<x>\d+)\D*(?<y>\d+)");

        private ICellCollection _cells;

        private string _colHeaders;
        private string _rowSeparator;

        internal static void Main(string[] args)
        {
            while (true) new Program().Run();
        }

        public void Run()
        {
            _cells = new CellCollection(12, 9);

            var leftPadding = new string(' ', _cells.Height.ToString(CultureInfo.InvariantCulture).Length + 3);

            var fieldWidth = _cells.Width * 4 + 1;

            var colHeaderBuilder = new StringBuilder(fieldWidth - 2);
            for (var i = 0; i < fieldWidth - 2; i++)
            {
                if (i % 2 == 0 && i % 4 != 0)
                    colHeaderBuilder.Append((i - 2) / 4 + 1);
                else
                    colHeaderBuilder.Append(' ');
            }

            _colHeaders = leftPadding + colHeaderBuilder;

            var rowSeparatorBuilder = new StringBuilder(fieldWidth);
            for (var i = 0; i < fieldWidth; i++)
            {
                if (i % 2 == 0 && i % 4 == 0)
                    rowSeparatorBuilder.Append(FieldCorner);
                else
                    rowSeparatorBuilder.Append(CornerConnector);
            }

            _rowSeparator = leftPadding + rowSeparatorBuilder;

            var running = true;

            while (running)
            {
                PrintField();

                Console.WriteLine();
                Console.Write("X,Y to test square, prefix with m or f to mark or flag: ");

                var command = Console.ReadLine();

                if (command == "quit")
                    running = false;
                else if (!string.IsNullOrEmpty(command))
                {
                    var match = CommandRegex.Match(command);
                    if (match.Success)
                    {
                        var mode = match.Groups["mode"];

                        var x = int.Parse(match.Groups["x"].Value);
                        var y = int.Parse(match.Groups["y"].Value);

                        // Adjust for actual index
                        x = x - 1;
                        y = y - 1;

                        if (x < 0 || x >= _cells.Width || y < 0 || y >= _cells.Height)
                        {
                            Console.WriteLine("Invalid index.");
                        }
                        else if (_cells[x, y].State != State.Shown)
                        {
                            if (mode.Success)
                            {
                                switch (mode.Value.ToLower()[0])
                                {
                                    case 'm':
                                        _cells.Mark(x, y);
                                        break;
                                    case 'f':
                                        _cells.Flag(x, y);
                                        break;
                                    default:
                                        Console.WriteLine("Unknown mode.");
                                        break;
                                }
                            }
                            else
                            {
                                var cell = _cells[x, y];
                                if (cell.IsMarked || cell.IsFlagged)
                                    continue;
                                if (cell.IsMine)
                                {
                                    _cells.ShowAll();
                                    PrintField();
                                    running = false;
                                }
                                else
                                    _cells.Show(x, y);
                            }
                        }
                    }
                    else
                        Console.WriteLine("Invalid command.");
                }
            }
        }

        private void PrintField()
        {
            Console.WriteLine();
            Console.WriteLine("{0} cells, {1} mines, {2} left to discover", _cells.CellCount, _cells.MineCount, _cells.MineCount - _cells.Flagged);
            Console.WriteLine();
            Console.WriteLine(_colHeaders);
            Console.WriteLine(_rowSeparator);

            for (var row = 0; row < _cells.Height; row++)
            {
                Console.Write("{0,3} {1}", row + 1, CellSeparator);
                for (var col = 0; col < _cells.Width; col++)
                {
                    var square = _cells[col, row];
                    Console.Write(' ');
                    if (square.State == State.Shown)
                    {
                        if (square.IsMine)
                            Console.Write(MineChar);
                        else if (square.Count > 0)
                            Console.Write(square.Count);
                        else
                            Console.Write(StateMapping[square.State]);
                    }
                    else
                        Console.Write(StateMapping[square.State]);
                    Console.Write(" {0}", CellSeparator);
                }
                Console.WriteLine();
                Console.WriteLine(_rowSeparator);
            }
        }
    }
}
