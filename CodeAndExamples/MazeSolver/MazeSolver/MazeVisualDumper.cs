using System;
using System.Text;

namespace MazeSolver
{
    public class MazeVisualDumper
    {
        public string Dump(MazeGrid maze, Point currentPosition = null)
        {
            var sb = new StringBuilder();

            for (int rowIndex = 0; rowIndex < maze.Grid.Length; rowIndex++)
            {
                var row = maze.Grid[rowIndex];

                for (int columnIndex = 0; columnIndex < row.Length; columnIndex++)
                {
                    string pointMarker = row[columnIndex] ? "." : "#";
                    if (maze.StartPosition.X == columnIndex && maze.StartPosition.Y == rowIndex) { pointMarker = "S";}
                    if (maze.Finish.X == columnIndex && maze.Finish.Y == rowIndex) { pointMarker = "F";}
                    if (currentPosition?.X == columnIndex && currentPosition.Y == rowIndex) { pointMarker = "O"; }
                    sb.Append(pointMarker);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }

    public class BlandWalkerStateDumper : IWalkerStateDumper
    {

        public void Dump(IMazeWalker walker)
        {
            Console.WriteLine(walker.CurrentPosition.ToString());
        }
    }

    public class FancyWalkerStateDumper : IWalkerStateDumper
    {
        private readonly MazeVisualDumper _mazeDumper;
        private readonly MazeGrid _maze;

        public FancyWalkerStateDumper(MazeVisualDumper mazeDumper, MazeGrid maze)
        {
            _mazeDumper = mazeDumper;
            _maze = maze;
        }

        public void Dump(IMazeWalker walker)
        {
            Console.WriteLine(_mazeDumper.Dump(_maze, walker.CurrentPosition));
        }
    }
}