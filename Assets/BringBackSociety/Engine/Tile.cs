using System.Runtime.InteropServices;
using BringBackSociety.Mapping;

namespace BringBackSociety
{
  /// <summary>
  /// Information about a single location in a chunk.
  /// </summary>
  [StructLayout(LayoutKind.Explicit)]
  public struct Tile
  {
    /// <summary> An empty tile. </summary>
    public static Tile Empty;

    /// <summary>
    /// The raw type of the tile.
    /// </summary>
    [FieldOffset(0)]
    public byte Type;

    /// <summary> Data about the wall. </summary>
    [FieldOffset(4)]
    public WallData WallData;

    /// <summary> Check whether the tile is empty. </summary>
    public bool IsEmpty
    {
      get { return Type == 0; }
    }
  }

  /// <summary> Represents a specific tile on a specific chunk. </summary>
  public struct TileReference
  {
    /// <summary> The chunk to which this proxy should point to. </summary>
    private readonly Chunk _chunk;

    /// <summary> The coordinate of the tile that is being observed. </summary>
    private readonly TileCoordinate _coordinate;

    /// <summary> Constructor. </summary>
    /// <param name="chunk"> The chunk to which this proxy should point to. </param>
    /// <param name="coordinate"> The coordinate of the tile that is being observed. </param>
    public TileReference(Chunk chunk, TileCoordinate coordinate)
    {
      _chunk = chunk;
      _coordinate = coordinate;
    }

    /// <summary> The tile data of the tile at the given position. </summary>
    public Tile Value
    {
      get { return _chunk[_coordinate]; }
      set { _chunk[_coordinate] = value; }
    }
  }
}