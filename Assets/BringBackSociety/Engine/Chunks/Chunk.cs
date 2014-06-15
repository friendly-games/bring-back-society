using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BringBackSociety.Engine;

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
    public const int Length = 32;

    /// <summary> Constructor. </summary>
    /// <param name="coordinate"> The coordinate associated with the chunk. </param>
    public Chunk(ChunkCoordinate coordinate)
    {
      Coordinate = coordinate;
      Offset = new WorldPosition(coordinate.X * Chunk.Length, coordinate.Z * Chunk.Length);
      Tiles = new Tile[Length * Length];
    }

    /// <summary> User defined data. </summary>
    public object Tag { get; set; }

    /// <summary> Gets the tag object casted to the correct type. </summary>
    public T TagValue<T>()
    {
      return (T) Tag;
    }

    /// <summary> The coordinate associated with the chunk. </summary>
    public ChunkCoordinate Coordinate { get; private set; }

    /// <summary>
    /// The offset to the tiles within the chunk.  This offset represents the position
    /// of the tile at (0,0) within the chunk.
    /// </summary>
    public WorldPosition Offset { get; private set; }

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

    /// <inheritdoc />
    public override string ToString()
    {
      return Coordinate.ToString();
    }

    /// <summary> Set the left and right properties to point to each other. </summary>
    public static void LinkHorizontally(Chunk lhs, Chunk rhs)
    {
      Debug.Assert(lhs.Offset.Z == rhs.Offset.Z);
      Debug.Assert(lhs.Offset.X == rhs.Offset.X - Chunk.Length);

      lhs.Right = rhs;
      rhs.Left = lhs;
    }

    /// <summary> Set the Back and Front properties to point to each other. </summary>
    public static void LinkVertically(Chunk upper, Chunk lower)
    {
      Debug.Assert(upper.Offset.X == lower.Offset.X);
      Debug.Assert(upper.Offset.Z == lower.Offset.Z + Chunk.Length);

      lower.Back = upper;
      upper.Front = lower;
    }
  }
}