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
             * A new algorithm is available in the `AdvancedMazeSolver` class in the ThirdPartyLogic project
             *   - we may not modify or add any code in this project as it is provided by a 3rd party
             *   - write an Adapter to use the existing `MazeWalker` class with the new logic
             * Notes:
             *   - `Point` already has an implicit conversion to ThirdPartyPoint
             *   - Conversion extension methods to/from this project's `Orientation` enum and ThirdPartyLogic's `Direction` enum already exist
             *
             * Part 2
             * ---------
             * We have two different ways of displaying the progress through a maze: `BlandWalkerStateDumper` and `FancyWalkerStateDumper`.
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
             * Notes:
             *   - The `Points` class already has a `FromString` method.
             */
            mazeApp.Run(@"MazeFiles\maze1.txt");
            Console.ReadLine();
        }

        private void Run(string mazeFilePath)
        {
            MazeGrid maze = GetMaze(mazeFilePath);

            IWalkerStateDumper dumper = new BlandWalkerStateDumper();

            var entity = new MazeWalker(maze, dumper);

            Console.Write(MazeVisualDumper.Dump(maze));

            bool mazeSolved = MazeSolver.SolveMaze(entity, maze.Finish);

            Console.WriteLine(mazeSolved ? "Reached end of maze! :)" : "Failed to reach end of maze. :(");
            Console.ReadKey();
        }

        private static MazeGrid GetMaze(string mazeFilePath)
        {
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