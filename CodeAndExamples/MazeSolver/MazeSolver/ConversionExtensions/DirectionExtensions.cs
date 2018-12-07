using System.ComponentModel;
using ThirdPartyLogic;

namespace MazeSolver.ConversionExtensions
{
    public static class DirectionExtensions
    {
        public static Orientation GetOrientation(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Orientation.North;
                case Direction.Right:
                    return Orientation.East;
                case Direction.Down:
                    return Orientation.South;
                case Direction.Left:
                    return Orientation.West;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}