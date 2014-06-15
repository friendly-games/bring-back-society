using System.Diagnostics;

namespace BringBackSociety
{
  /// <summary> Coordinate of tiles within a chunk. </summary>
  public struct TileCoordinate
  {
    private readonly int _index;

    /// <summary> The index into the tile array to the tile at the given index. </summary>
    public int Index
    {
      get { return _index; }
    }

    /// <summary> Constructor. </summary>
    /// <param name="x"> The x coordinate. </param>
    /// <param name="z"> The z coordinate. </param>
    public TileCoordinate(int x, int z)
    {
      Debug.Assert(x >= 0 && x < Chunk.Length);
      Debug.Assert(z >= 0 && z < Chunk.Length);

      _index = z*Chunk.Length + x;
    }

    /// <summary> Convert this tile coordinate into a world position. </summary>
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

    /// <inheritdoc/>
    public override int GetHashCode()
    {
      return _index;
    }

    /// <summary> Check for equality.. </summary>
    public bool Equals(TileCoordinate other)
    {
      return _index == other._index;
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
      return obj is TileCoordinate
             && Equals((TileCoordinate) obj);
    }
  }
}