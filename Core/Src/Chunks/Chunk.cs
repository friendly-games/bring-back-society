using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BringBackSociety
{
  /// <summary>
  /// An individual area of the map that is currently loaded into memory.
  /// </summary>
  public class Chunk
  {
    /// <summary>
    /// The length of a single side of the chunk
    /// </summary>
    public const int Length = 64;

    /// <summary> Constructor. </summary>
    /// <param name="coordinate"> The coordinate associated with the chunk. </param>
    public Chunk(ChunkCoordinate coordinate)
    {
      Coordinate = coordinate;
      Offset = new WorldPosition(coordinate.X*Chunk.Length, coordinate.Z*Chunk.Length);
      Tiles = new Tile[Length*Length];
    }

    /// <summary> The coordinate associated with the chunk. </summary>
    public ChunkCoordinate Coordinate { get; private set; }

    /// <summary>
    /// The offset to the tiles within the chunk.  This offset represents the position
    /// of the tile at (0,0) within the chunk.
    /// </summary>
    public WorldPosition Offset { get; private set; }

    /// <summary>
    /// The position of the chunk in the overall world where each increment in x or z represents
    /// Chunk.Length tiles.
    /// </summary>
    public WorldPosition MacroChunkPosition { get; private set; }

    /// <summary>
    /// The chunk with lower x values, that is directly to the left of this chunk
    /// </summary>
    public Chunk Left { get; set; }

    /// <summary>
    /// The chunk with higher x values, that is directly to the right of this chunk.
    /// </summary>
    public Chunk Right { get; set; }

    /// <summary>
    /// The chunk with lower z values, that is directly in front of this chunk.
    /// </summary>
    public Chunk Front { get; set; }

    /// <summary>
    /// The chunk with higher z values, that is directly behind this chunk
    /// </summary>
    public Chunk Back { get; set; }

    /// <summary>
    /// The tiles that belong to the chunk.
    /// </summary>
    public Tile[] Tiles { get; private set; }

    /// <summary>
    ///  The tile at the specified world position.
    /// </summary>
    public Tile this[WorldPosition position]
    {
      get { return this[CoordinateFrom(position)]; }
      set { this[CoordinateFrom(position)] = value; }
    }

    /// <summary>
    ///  The tile at the specified tile position.
    /// </summary>
    public Tile this[TileCoordinate position]
    {
      get { return Tiles[position.Index]; }
      set { Tiles[position.Index] = value; }
    }

    /// <summary> Get a tile coordinate from the given position, for this chunk. </summary>
    public TileCoordinate CoordinateFrom(WorldPosition position)
    {
      return new TileCoordinate(position.X - Offset.X, position.Z - Offset.Z);
    }

    /// <summary> Convert a tile coordinate for this chunk into a world position. </summary>
    public WorldPosition GetWorldLocationFrom(TileCoordinate coordinate)
    {
      var local = coordinate.ToWorldPosition();
      return new WorldPosition(local.X + Offset.X, local.Z + Offset.Z);
    }
  }
}