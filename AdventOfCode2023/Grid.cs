using System.Text;

namespace AdventOfCode2023;
public class Grid<T>(int x, int y)
{
    private readonly T[,] points = new T[x, y];
    public int Width { get; } = x;
    public int Height { get; } = y;

    public Grid(int x, int y, T defaultValue) : this(x, y)
    {
        foreach (var p in AllPositions())
            this[p] = defaultValue;
    }

    public T this[Point c]
    {
        get => points[c.X, c.Y];
        set => points[c.X, c.Y] = value;
    }

    public bool IsInGrid(Point p) => p.X >= 0 && p.X < Width && p.Y >= 0 && p.Y < Height;

    private static readonly IEnumerable<Point> cardinalNeighbours = new Point[] { (0, -1), (1, 0), (0, 1), (-1, 0) };
    private static readonly IEnumerable<Point> diagonalNeighbours = new Point[] { (1, -1), (1, 1), (-1, 1), (-1, -1) };

    public IEnumerable<Point> Neighbours(Point p, bool includeDiagonals = false)
    {
        var neighbours = includeDiagonals ? cardinalNeighbours.Concat(diagonalNeighbours) : cardinalNeighbours;
        return neighbours.Select(n => p + n).Where(IsInGrid);
    }

    public IEnumerable<Point> AllPositions()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                yield return (x, y);
            }
        }
    }

    public IEnumerable<Point> LineOut(Point start, int direction, bool inclusive)
    {
        if (!IsInGrid(start)) yield break;

        if (direction == 0) // North
        {
            for (int i = inclusive ? start.Y : start.Y - 1; i >= 0; i--)
            {
                yield return (start.X, i);
            }
        }
        else if (direction == 2) // South
        {
            for (int i = inclusive ? start.Y : start.Y + 1; i < Height; i++)
            {
                yield return (start.X, i);
            }
        }
        else if (direction == 3) // West
        {
            for (int i = inclusive ? start.X : start.X + 1; i >= 0; i--)
            {
                yield return (i, start.Y);
            }
        }
        else if (direction == 1) // East
        {
            for (int i = inclusive ? start.X : start.X + 1; i < Width; i++)
            {
                yield return (i, start.Y);
            }
        }
        else { throw new ArgumentException("Invalid direction, may only be 0-3 (N,E,S,W)", nameof(direction)); }

    }

    public IEnumerable<Point> LineTo(Point start, Point end, bool inclusive = true)
    {
        if (!IsInGrid(start) || !IsInGrid(end)) yield break;

        if (start.X == end.X)
        {
            int small = Math.Min(start.Y, end.Y);
            int large = Math.Max(start.Y, end.Y);

            for (int i = small; !inclusive && i < large || inclusive && i <= large; i++)
            {
                yield return (start.X, i);
            }
        }
        else if (start.Y == end.Y)
        {
            int small = Math.Min(start.X, end.X);
            int large = Math.Max(start.X, end.X);

            for (int i = small; !inclusive && i < large || inclusive && i <= large; i++)
            {
                yield return (start.X, i);
            }
        }
        else
        {
            throw new Exception($"Not a straight line between {start} and {end}");
        }
    }

    public override string ToString()
    {
        var s = new StringBuilder();
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                s.Append(points[x, y]!.ToString());
            }
            s.AppendLine();
        }
        return s.ToString();
    }
}

public struct Point(int x, int y)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    public readonly int MDistanceTo(Point b) => Math.Abs(b.X - X) + Math.Abs(b.Y - Y);
    public readonly int this[int index] => index == 0 ? X : Y;

    public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
    public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);
    public static Point operator -(Point a) => new(-a.X, a.Y);
    public static bool operator ==(Point? a, Point? b) => a.Equals(b);
    public static bool operator !=(Point? a, Point? b) => !(a == b);

    public override readonly bool Equals(object? obj) => obj is Point p && p.X.Equals(X) && p.Y.Equals(Y);
    public override readonly int GetHashCode() => HashCode.Combine(X, Y);


    public static implicit operator Point((int x, int y) coords) => new(coords.x, coords.y);
    //public static implicit operator (int X, int Y)(Point p) => (p.X, p.Y);
    public override readonly string ToString() => $"({X}, {Y})";
    public readonly int[] ToArray() => [X, Y];
    public readonly void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }
}

public struct Coord(int x, int y, int z)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public int Z { get; set; } = z;

    public static implicit operator Coord((int x, int y, int z) coords) => new(coords.x, coords.y, coords.z);

    static IEnumerable<IEnumerable<T>> GetPermutationsWithRept<T>(IEnumerable<T> list, int length)
    {
        if (length == 1) return list.Select(t => new T[] { t });
        return GetPermutationsWithRept(list, length - 1).SelectMany(t => list, (t1, t2) => t1.Concat(new T[] { t2 }));
    }

    public readonly Coord[] Adjacents() => [this + (0, 0, 1), this - (0, 0, 1), this + (0, 1, 0), this - (0, 1, 0), this + (1, 0, 0), this - (1, 0, 0)];

    public static Coord operator +(Coord a, Coord b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Coord operator -(Coord a, Coord b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public int this[int index]
    {
        readonly get => index == 0 ? X : index == 1 ? Y : index == 2 ? Z : throw new IndexOutOfRangeException();
        set
        {
            switch (index)
            {
                case 0: X = value; break;
                case 1: Y = value; break;
                case 2: Z = value; break;
                default: throw new IndexOutOfRangeException();
            }
        }

    }
    public override readonly bool Equals(object? obj) => obj is Coord p && p.X.Equals(X) && p.Y.Equals(Y) && p.Z.Equals(Z);
    public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z);

    public static bool operator ==(Coord a, Coord b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
    public static bool operator !=(Coord a, Coord b) => a.X != b.X || a.Y != b.Y || a.Z != b.Z;
    public static bool operator <(Coord a, Coord b) => a.X < b.X && a.Y < b.Y && a.Z < b.Z;
    public static bool operator >(Coord a, Coord b) => a.X > b.X && a.Y > b.Y && a.Z > b.Z;
    public static bool operator <=(Coord a, Coord b) => a.X <= b.X && a.Y <= b.Y && a.Z <= b.Z;
    public static bool operator >=(Coord a, Coord b) => a.X >= b.X && a.Y >= b.Y && a.Z >= b.Z;
    public override readonly string ToString() => $"({X}, {Y}, {Z})";
    public readonly int[] ToArray() => [X, Y, Z];
    public readonly void Deconstruct(out int x, out int y, out int z)
    {
        x = X;
        y = Y;
        z = Z;
    }

}