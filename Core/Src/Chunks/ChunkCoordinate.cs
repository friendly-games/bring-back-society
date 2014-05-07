using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety
{
  /// <summary>
  ///  The position of the chunk in the overall world where each increment in x or z represents
  ///  Chunk.Length tiles.
  /// </summary>
  public struct ChunkCoordinate
  {
    /// <summary> The X coordinate of the chunk among all chunks in the world.</summary>
    public readonly int X;

    /// <summary> The Z coordinate of the chunk among all chunks in the world.</summary>
    public readonly int Z;

    /// <summary> Constructor.</summary>
    /// <param name="x"> The x coordinate of the chunk. </param>
    /// <param name="z"> The z coordinate of the chunk. </param>
    public ChunkCoordinate(int x, int z)
    {
      X = x;
      Z = z;
    }

    /// <summary> Converts the chunk coordinate into a position that is world coordinates. </summary>
    /// <returns> This object as a WorldPosition. </returns>
    public WorldPosition ToWorldPosition()
    {
      return new WorldPosition(X*Chunk.Length, Z*Chunk.Length);
    }

    /// <summary> Tests if this ChunkCoordinate is considered equal to another. </summary>
    public bool Equals(ChunkCoordinate other)
    {
      return X == other.X && Z == other.Z;
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
      return obj is ChunkCoordinate && Equals((ChunkCoordinate) obj);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
      unchecked
      {
        return (X*397) ^ Z;
      }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
      return String.Format("{0}, {1}", X, Z);
    }
  }
}