using System;
using System.Collections.Generic;
using System.Linq;

namespace ThirdPartyLogic
{
    public class AdvancedMazeSolver
    {
        public static bool SolveMaze(IMazeTraverser entity, ThirdPartyPoint mazeFinish)
        {
            IEnumerable<Direction> GetLegalMoves(IMazeTraverser walker)
            {
                foreach (Direction direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
                {
                    if (walker.IsLegalMove(direction))
                    {
                        yield return direction;
                    }
                }
            }

            void IncrementVisitedCount(IDictionary<ThirdPartyPoint, int> history, ThirdPartyPoint point)
            {
                if (history.ContainsKey(point))
                {
                    history[point] += 1;
                }
                else
                {
                    history[point] = 1;
                }
            }

            Direction GetDirectionOfLeastVisitedLegalMove(IMazeTraverser walker, IDictionary<ThirdPartyPoint, int> history, IEnumerable<Direction> legalMoves)
            {
                return legalMoves.Select(move => new {Dir = move, NewPoint = walker.GetNext(move)})
                    .Select(move => new
                    {
                        Dir = move.Dir,
                        Visits = history.ContainsKey(move.NewPoint) ? history[move.NewPoint] : 0
                    })
                    .OrderBy(move => move.Visits)
                    .Select(move => move.Dir)
                    .First();
            }

            Dictionary<ThirdPartyPoint, int> vistedPoints = new Dictionary<ThirdPartyPoint, int>();

            while (!Equals(entity.Location, mazeFinish))
            {
                IncrementVisitedCount(vistedPoints, entity.Location);

                entity.Face(GetDirectionOfLeastVisitedLegalMove(entity, vistedPoints, GetLegalMoves(entity).ToArray()));

                entity.Step();

                entity.DumpState();
            }

            return true;
        }
    }
}