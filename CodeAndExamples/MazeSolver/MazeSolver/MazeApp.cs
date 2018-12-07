using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThirdPartyLogic;

namespace MazeSolver
{
    public class MazeApp
    {
        public MazeVisualDumper MazeVisualDumper { get; } = new MazeVisualDumper();

        public static void Main()
        {
            var mazeApp = new MazeApp();
            /*
             * Part 1
             * ---------
             * maze1.txt works
             * maze2.txt gets stuck
             * A new algorithm is available in the AdvancedMazeSolver class in the ThirdPartyLogic project
             *   - we may not modify or add any code in this project as it is provided by a 3rd party
             *   - write an Adapter to use the existing MazeWalker class with the new logic
             * Notes:
             *   - Point already has an implicit conversion to ThirdPartyPoint
             *   - Conversion extension methods to/from this project's Orientation enum and ThirdPartyLogic's Direction enum already exist
             *
             * Part 2
             * ---------
             * We have two different ways of displaying the progress through a maze: BlandWalkerStateDumper and FancyWalkerStateDumper.
             * The former outputs the position of the walker at each stage, the later outputs a pictoral view of the whole maze and the walker's position within it.
             * Both implement the same interface. Both currently dump to the console.
             * Now we want both classes to be able to write both to the console and to file.
             * Apply the Bridge pattern to allow both classes to write to both console and file.
             * Notes:
             *   - Yes, there are better ways of doing this than using the Bridge pattern since both output strings, but imagine the output for one was an .png, or an audio file.
             *
             * Part 3
             * ---------
             * We have an upcoming requirment to support multiple formats for maze input. In preparation, apply the Builder pattern to the maze creation logic.
             * Rewrite the input to accept input in the form given in MazeFiles/points1.txt
             */
            mazeApp.Run(@"MazeFiles\maze1.txt");
            Console.ReadLine();
        }

        private void Run(string mazeFilePath)
        {
//            MazeGrid maze = GetMaze(mazeFilePath);
            MazeGrid maze = GetMaze2(mazeFilePath);

            IWalkerStateDumper dumper = new BlandWalkerStateDumper();
//            IWalkerStateDumper dumper = new FancyWalkerStateDumper(MazeVisualDumper, maze);

            var entity = new MazeWalker(maze, dumper);

            Console.Write(MazeVisualDumper.Dump(maze));

//            bool mazeSolved = MazeSolver.SolveMaze(entity, maze.Finish);
            // TODO: Delete
            bool mazeSolved = AdvancedMazeSolver.SolveMaze(new MazeWalkerAdapter(entity), maze.Finish);

            Console.WriteLine(mazeSolved ? "Reached end of maze! :)" : "Failed to reach end of maze. :(");
            Console.ReadKey();
        }

        // TODO: Delete
        public class MazeBuilder
        {
            private Point _start;
            private Point _end;
            private readonly List<Point> _closedPoints = new List<Point>();
            private readonly List<Point> _openPoints = new List<Point>();

            public MazeApp.MazeBuilder WithStartPoint(int x, int y)
            {
                _start = new Point(x, y);
                return this;
            }

            public MazeApp.MazeBuilder WithEndPoint(int x, int y)
            {
                _end = new Point(x, y);
                return this;
            }

            public MazeApp.MazeBuilder WithClosedPoint(int x, int y)
            {
                _closedPoints.Add(new Point(x, y));
                return this;
            }

            public MazeApp.MazeBuilder WithOpenPoint(int x, int y)
            {
                _openPoints.Add(new Point(x, y));
                return this;
            }

            public MazeGrid Build()
            {
                if (_start == null) throw new Exception("Maze should have a start position set.");
                if (_end == null) throw new Exception("Maze should have a finish position set.");
                return new MazeGrid(GetGrid(), _start, _end);
            }

            private bool[][] GetGrid()
            {
                bool[] PadTo(bool[] existing, int newLength)
                {
                    int oldLength = existing.Length;
                    return existing.Concat(Enumerable.Repeat(true, newLength - oldLength + 1)).ToArray();
                }

                int height =  _closedPoints.Max(p => p.Y) + 1;
                var grid = new bool[height][];

                foreach (Point openPoint in _openPoints.Concat(new []{_start, _end}))
                {
                    if (grid[openPoint.Y] == null || grid[openPoint.Y].Length <= openPoint.X)
                    {
                        grid[openPoint.Y] = Enumerable.Repeat(true, openPoint.X + 1).ToArray();
                    }
                }

                foreach (Point closedPoint in _closedPoints)
                {
                    if (grid[closedPoint.Y] == null)
                    {
                        grid[closedPoint.Y] = Enumerable.Repeat(true, closedPoint.X + 1).ToArray();
                    }

                    int rowLength = grid[closedPoint.Y].Length;
                    if (rowLength <= closedPoint.X)
                    {
                        grid[closedPoint.Y] = PadTo(grid[closedPoint.Y], closedPoint.X);
                    }

                    grid[closedPoint.Y][closedPoint.X] = false;
                }


                return grid;
            }
        }

        // TODO: Delete
        private static MazeGrid GetMaze2(string mazeFilePath)
        {
            // todo: handle \n newline characters instead of Environment.NewLine when you download a zip file
            var lines = new StreamReader(new FileStream(mazeFilePath, FileMode.Open)).ReadToEnd().Replace(" ", "")
                .Split(new[] {Environment.NewLine}, StringSplitOptions.None);

            int currentRow = 0;
            var builder = new MazeApp.MazeBuilder();

            foreach (var line in lines)
            {
                int currentCol = 0;

                foreach (var point in line)
                {
                    switch (point)
                    {
                        case '#':
                            builder.WithClosedPoint(currentCol, currentRow);
                            break;
                        case 'S':
                            builder.WithStartPoint(currentCol, currentRow);
                            break;
                        case 'F':
                            builder.WithEndPoint(currentCol, currentRow);
                            break;
                        case '.':
                            builder.WithOpenPoint(currentCol, currentRow);
                            break;
                        default:
                            throw new Exception("Maze input string contains invalid characters");
                    }

                    currentCol++;
                }

                currentRow++;
            }

            return builder.Build();
        }

        private static MazeGrid GetMaze(string mazeFilePath)
        {
            // todo: handle \n newline characters instead of Environment.NewLine when you download a zip file
            var lines = new StreamReader(new FileStream(mazeFilePath, FileMode.Open)).ReadToEnd().Replace(" ", "")
                .Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            Point start = null;
            Point finish = null;

            var grid = new bool[lines.Length][];
            int currentRow = 0;

            foreach (var line in lines)
            {
                grid[currentRow] = new bool[line.Length];
                int currentCol = 0;

                foreach (var point in line)
                {
                    switch (point)
                    {
                        case '#':
                            grid[currentRow][currentCol] = false;
                            break;
                        case '.':
                            grid[currentRow][currentCol] = true;
                            break;
                        case 'S':
                            grid[currentRow][currentCol] = true;
                            start = new Point(currentCol, currentRow);
                            break;
                        case 'F':
                            grid[currentRow][currentCol] = true;
                            finish = new Point(currentCol, currentRow);
                            break;
                        default:
                            throw new Exception("Maze input string contains invalid characters");
                    }

                    currentCol++;
                }

                currentRow++;
            }

            if (start == null) throw new Exception("Maze should have a start position set.");
            if (finish == null) throw new Exception("Maze should have a finish position set.");

            var maze = new MazeGrid(grid, start, finish);
            return maze;
        }
    }
}