using System.ComponentModel;
using ThirdPartyLogic;

namespace MazeSolver.ConversionExtensions
{
    public static class OrientationExtensions
    {
        public static Direction GetDirection(this Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.North:
                    return Direction.Up;
                case Orientation.East:
                    return Direction.Right;
                case Orientation.South:
                    return Direction.Down;
                case Orientation.West:
                    return Direction.Left;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}