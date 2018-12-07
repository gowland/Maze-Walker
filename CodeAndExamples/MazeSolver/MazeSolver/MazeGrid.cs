using System;
using System.Text;

namespace MazeSolver
{
    public class MazeGrid
    {
        // +--------> +ve X
        // |
        // |
        // |
        // |
        // v
        // +ve Y

        public MazeGrid(bool[][] grid, Point start, Point finish)
        {
            Grid = grid;
            StartPosition = start;
            Finish = finish;
        }

        public Point StartPosition { get; }

        public bool AtFinish(IMazeWalker walker)
        {
            return Finish.X == walker.CurrentPosition.X && Finish.Y == walker.CurrentPosition.Y;
        }

        public bool IsWithin(Point point)
        {
            return point.X >= 0 && point.Y >= 0 && point.Y < Grid.Length && point.X < Grid[point.X].Length;
        }

        public int Rows => Grid.GetLength(0);
        public int Columns => Grid.GetLength(1);

        public Point Finish { get; }

        public bool[][] Grid { get; }

    }
}