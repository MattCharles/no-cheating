public class CoordinatePosition
{
    public static CoordinatePosition Origin = new CoordinatePosition(0, 0);

    public int X = 0;
    public int Y = 0;
    public int Z = 0;

    public CoordinatePosition(int x, int z)
    {
        X = x;
        Z = z;
    }

    public CoordinatePosition(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override bool Equals(object obj)
    {
        if(obj is CoordinatePosition)
        {
            return Equals((CoordinatePosition)obj);
        }
        return base.Equals(obj);
    }

    public bool Equals(CoordinatePosition other)
    {
        return other.X == X &&
            other.Y == Y &&
            other.Z == Z;
    }

    public override int GetHashCode()
    {
        int hash = 13;
        hash = (hash * 269) + X.GetHashCode();
        hash = (hash * 269) + Y.GetHashCode();
        hash = (hash * 269) + Z.GetHashCode();
        return hash;
    }

    public override string ToString()
    {
        return "(" + X + ", " + Y + ", " + Z + ")";
    }

}
