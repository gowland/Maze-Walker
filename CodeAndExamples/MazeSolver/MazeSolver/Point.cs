using System;
using System.Collections.Generic;
using System.Linq;
using ThirdPartyLogic;

namespace MazeSolver
{
    public class Point : IEquatable<Point>
    {
        public int X { set; get; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"Point({X}, {Y})";
        }

        public bool Equals(Point other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Point) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public Point GetNext(Orientation orientation)
        {
            var desiredPoint = new Point(X, Y);

            switch (orientation)
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

            return desiredPoint;
        }

        public static Point FromString(string pointAsText)
        {
            var coords = StringsToInts(pointAsText.Split(',')).ToList();
            if (coords.Count == 2)
            {
                return new Point(coords[0], coords[1]);
            }

            throw new ArgumentException($"{pointAsText} is not a valid point");
        }

        private static IEnumerable<int> StringsToInts(IEnumerable<string> intsAsText)
        {
            foreach (string intAsText in intsAsText)
            {
                if (int.TryParse(intAsText, out int result))
                {
                    yield return result;
                }
            }
        }

        public static implicit operator ThirdPartyPoint(Point point)
        {
            return new ThirdPartyPoint(point.X, point.Y);
        }
    }
}