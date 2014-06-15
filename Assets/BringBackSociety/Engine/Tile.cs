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
    /// <summary>
    /// The raw type of the tile.
    /// </summary>
    [FieldOffset(0)]
    public byte Wall;

    /// <summary> Data about the wall. </summary>
    [FieldOffset(4)]
    public WallData WallData;
  }
}