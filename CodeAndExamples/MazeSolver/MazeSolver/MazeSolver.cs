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
}