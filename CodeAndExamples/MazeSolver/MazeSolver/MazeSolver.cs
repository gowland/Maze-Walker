using System;
using System.ComponentModel;
using MazeSolver.ConversionExtensions;
using ThirdPartyLogic;

namespace MazeSolver
{
    public class MazeSolver
    {
        public static bool SolveMaze(IMazeWalker entity, Point mazeFinish)
        {
            bool endOfMazeReached = false;

            while (!endOfMazeReached)
            {
                bool couldMoveForward = entity.MoveForward();

                if (!couldMoveForward)
                {
                    entity.TurnRight();
                }
                else
                {
                    if (entity.CanSeeLeftTurning())
                    {
                        entity.TurnLeft();
                    }
                }

                endOfMazeReached = Equals(entity.CurrentPosition, mazeFinish);
                entity.DumpState();
            }

            return true;
        }
    }

// TODO: Delete, this is just to test that it's doable/easy enough
    public class MazeWalkerAdapter : IMazeTraverser
    {
        private readonly IMazeWalker _impl;

        public MazeWalkerAdapter(IMazeWalker impl)
        {
            _impl = impl;
        }

        public Direction Orientation => _impl.Direction.GetDirection();
        public bool IsLegalMove(Direction direction) => _impl.CanMove(direction.GetOrientation());
        public ThirdPartyPoint Location => _impl.CurrentPosition;

        public void Face(Direction direction)
        {
            while (_impl.Direction != direction.GetOrientation())
            {
                _impl.TurnLeft();
            }
        }

        public void Step() => _impl.MoveForward();
        public ThirdPartyPoint GetNext(Direction direction) => _impl.CurrentPosition.GetNext(direction.GetOrientation());

        public void DumpState()
        {
            _impl.DumpState();
        }
    }
}