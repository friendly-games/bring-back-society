namespace BringBackSociety
{
  /// <summary>
  /// Information about a single location in a chunk.
  /// </summary>
  public struct Tile
  {
    /// <summary>
    /// The raw type of the tile.
    /// </summary>
    public byte Type;

    /// <summary>
    /// The type of material the ground is made of.  Various different types of grounds
    /// have different attributes such as speed, build strengths, and material types
    /// </summary>
    public byte GroundType;
  }
}