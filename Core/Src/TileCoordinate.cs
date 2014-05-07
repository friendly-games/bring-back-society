using System.Diagnostics;

namespace Grid
{
  public struct TileCoordinate
  {
    private readonly int _index;

    public int Index
    {
      get { return _index; }
    }

    public TileCoordinate(int x, int y)
    {
      Debug.Assert(x >= 0 && x < Chunk.Length);
      Debug.Assert(y >= 0 && y < Chunk.Length);

      _index = y*Chunk.Length + x;
    }

    public WorldPosition ToWorldPosition()
    {
      int x = _index%Chunk.Length;
      return new WorldPosition(x, (_index - x)/Chunk.Length);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
      return ToWorldPosition().ToString();
    }

    public override int GetHashCode()
    {
      return _index;
    }

    public bool Equals(TileCoordinate other)
    {
      return _index == other._index;
    }

    public override bool Equals(object obj)
    {
      return obj is TileCoordinate
             && Equals((TileCoordinate) obj);
    }
  }
}