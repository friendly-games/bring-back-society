using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Grid
{
  /// <summary>
  /// An individual area of the map that is currently loaded into memory.
  /// </summary>
  public class Chunk
  {
    /// <summary>
    /// The length of a single side of the chunk
    /// </summary>
    public const int Length = 128;

    public Chunk(ChunkCoordinate coordinate)
    {
      Coordinate = coordinate;
      Offset = new WorldPosition(coordinate.X*Chunk.Length, coordinate.Z*Chunk.Length);
      Tiles = new Tile[Length*Length*Length];
    }

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

    public Tile this[WorldPosition position]
    {
      get { return this[CoordinateFrom(position)]; }
      set { this[CoordinateFrom(position)] = value; }
    }

    public Tile this[TileCoordinate position]
    {
      get { return Tiles[position.Index]; }
      set { Tiles[position.Index] = value; }
    }

    public TileCoordinate CoordinateFrom(WorldPosition position)
    {
      return new TileCoordinate(position.X - Offset.X, position.Z - Offset.Z);
    }

    public WorldPosition GetWorldLocationFrom(TileCoordinate coordinate)
    {
      var local = coordinate.ToWorldPosition();
      return new WorldPosition(local.X + Offset.X, local.Z + Offset.Z);
    }
  }
}