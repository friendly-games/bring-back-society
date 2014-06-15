using System;

namespace BringBackSociety
{
  /// <summary> Represents a position aligned to the grid. </summary>
  public struct WorldPosition
  {
    /// <summary> The X coordinate of the position. </summary>
    public readonly int X;

    /// <summary> The Z coordinate of the position. </summary>
    public readonly int Z;

    /// <summary> Constructor. </summary>
    /// <param name="x"> The x coordinate of the position. </param>
    /// <param name="z"> The x coordinate of the position. </param>
    public WorldPosition(int x, int z)
    {
      X = x;
      Z = z;
    }

    /// <summary> Constructor. </summary>
    public WorldPosition(ChunkCoordinate chunkCoordinate, TileCoordinate tileCoordinate)
    {
      this = chunkCoordinate.ToWorldPosition() + tileCoordinate.ToWorldPosition();
    }

    /// <summary> Calculates the chunk coordinate and tile coordinate from this world coordinate. </summary>
    public void CalculateCoordinates(out ChunkCoordinate chunkCoordinate, out TileCoordinate tileCoordinate)
    {
      int tileX = (X % Chunk.Length + Chunk.Length) % Chunk.Length;
      int tileZ = (Z % Chunk.Length + Chunk.Length) % Chunk.Length;

      int chunkX = (X - tileX) / Chunk.Length;
      int chunkZ = (Z - tileZ) / Chunk.Length;

      chunkCoordinate = new ChunkCoordinate(chunkX, chunkZ);
      tileCoordinate = new TileCoordinate(tileX, tileZ);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      unchecked
      {
        return (X * 397) ^ Z;
      }
    }

    /// <summary> Tests if this WorldPosition is considered equal to another. </summary>
    public bool Equals(WorldPosition other)
    {
      return X == other.X && Z == other.Z;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      return obj is WorldPosition
             && Equals((WorldPosition) obj);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
      return String.Format("{0}, {1}", X, Z);
    }

    /// <summary> Add two world positions together. </summary>
    public static WorldPosition operator +(WorldPosition lhs, WorldPosition rhs)
    {
      return new WorldPosition(lhs.X + rhs.X, lhs.Z + rhs.Z);
    }

    /// <summary> Subtract two world positions </summary>
    public static WorldPosition operator -(WorldPosition lhs, WorldPosition rhs)
    {
      return new WorldPosition(lhs.X - rhs.X, lhs.Z - rhs.Z);
    }
  }
}