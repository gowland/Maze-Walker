using System;
using System.ComponentModel;

namespace MazeSolver
{
/*
    public class ReWalker
    {
        private readonly Func<Point, bool> _isOpenFunc;

        public ReWalker(Func<Point, bool> isOpenFunc)
        {
            _isOpenFunc = isOpenFunc;
        }
        public bool CanMove(Point point, Direction direction)
        {
            Point newPoint = GetPoint(point, direction);
            return _isOpenFunc(newPoint);
        }

        public Point Move(Point point, Direction direction)
        {
            return GetPoint(point, direction);
        }

        private Point GetPoint(Point point, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return new Point(point.X -1, point.Y);
                case Direction.Right:
                    return new Point(point.X +1, point.Y);
                case Direction.Up:
                    return new Point(point.X, point.Y - 1);
                case Direction.Down:
                    return new Point(point.X, point.Y + 1);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }

*/

    public interface IMazeWalker
    {
        bool CanSeeLeftTurning();
        Point CurrentPosition { get; set; }
        void TurnRight();
        void TurnLeft();
        bool MoveForward();
        bool CanMove(Orientation orientation);
        Orientation Direction { get; }
        void DumpState();
    }

    public class MazeWalker : IMazeWalker
    {
        private readonly MazeGrid _mMazeGrid;
        private readonly IWalkerStateDumper _stateDumper;

        public MazeWalker(MazeGrid mazeGrid, IWalkerStateDumper stateDumper)
        {
            _mMazeGrid = mazeGrid;
            _stateDumper = stateDumper;
            CurrentPosition = _mMazeGrid.StartPosition;
            Direction = Orientation.South;
        }

        public bool CanSeeLeftTurning()
        {
            var pointToOurLeft = new Point(CurrentPosition.X, CurrentPosition.Y);

            switch (Direction)
            {
                case Orientation.North:
                    pointToOurLeft.X -= 1;
                    break;
                case Orientation.South:
                    pointToOurLeft.X += 1;
                    break;
                case Orientation.East:
                    pointToOurLeft.Y -= 1;
                    break;
                case Orientation.West:
                    pointToOurLeft.Y += 1;
                    break;
                default:
                    throw new Exception();
            }

            return _mMazeGrid.Grid[pointToOurLeft.Y][pointToOurLeft.X];
        }

        public Point CurrentPosition { get; set; }

        public Orientation Direction { get; private set; }

        public void TurnRight()
        {
            switch (Direction)
            {
                case Orientation.North:
                    Direction = Orientation.East;
                    break;
                case Orientation.East:
                    Direction = Orientation.South;
                    break;
                case Orientation.South:
                    Direction = Orientation.West;
                    break;
                case Orientation.West:
                    Direction = Orientation.North;
                    break;
                default:
                    throw new Exception();
            }
        }

        public void TurnLeft()
        {
            switch (Direction)
            {
                case Orientation.North:
                    Direction = Orientation.West;
                    break;
                case Orientation.West:
                    Direction = Orientation.South;
                    break;
                case Orientation.South:
                    Direction = Orientation.East;
                    break;
                case Orientation.East:
                    Direction = Orientation.North;
                    break;
                default:
                    throw new Exception();
            }
        }

        public bool MoveForward()
        {
            var desiredPoint = new Point(CurrentPosition.X, CurrentPosition.Y);

            switch (Direction)
            {
                case Orientation.North:
                    desiredPoint.Y -= 1;
                    break;
                case Orientation.South:
                    desiredPoint.Y += 1;
                    break;
                case Orientation.East:
                    desiredPoint.X += 1;
                    break;
                case Orientation.West:
                    desiredPoint.X -= 1;
                    break;
                default:
                    throw new Exception();
            }

            var canMoveForward = _mMazeGrid.Grid[desiredPoint.Y][desiredPoint.X];
            if (canMoveForward) CurrentPosition = desiredPoint;
            return canMoveForward;
        }

        public bool CanMove(Orientation orientation)
        {
            Point desiredPoint = CurrentPosition.GetNext(orientation);

            return _mMazeGrid.IsWithin(desiredPoint) && _mMazeGrid.Grid[desiredPoint.Y][desiredPoint.X];
        }

        public void DumpState()
        {
            _stateDumper.Dump(this);
        }
    }
}