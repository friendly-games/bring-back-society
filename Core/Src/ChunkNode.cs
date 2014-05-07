using System;
using System.Diagnostics;

namespace Grid
{
  public class ChunkNode
  {
    /// <summary>
    /// Wrap a new chunk in a node.
    /// </summary>
    /// <param name="chunk"> The chunk to wrap into the node. </param>
    public ChunkNode(Chunk chunk)
    {
      if (chunk == null)
        throw new ArgumentNullException("chunk");

      Chunk = chunk;
    }

    /// <summary>
    /// The chunk node with lower x values, that is directly to the left of this chunk
    /// </summary>
    public ChunkNode Left { get; set; }

    /// <summary>
    /// The chunk node with higher x values, that is directly to the right of this chunk.
    /// </summary>
    public ChunkNode Right { get; set; }

    /// <summary>
    /// The chunk node with lower z values, that is directly in front of this chunk.
    /// </summary>
    public ChunkNode Front { get; set; }

    /// <summary>
    /// The chunk node with higher z values, that is directly behind this chunk
    /// </summary>
    public ChunkNode Back { get; set; }

    /// <summary> The chunk associated with this node. </summary>
    public Chunk Chunk { get; set; }

    public static void LinkHorizontally(ChunkNode lhs, ChunkNode rhs)
    {
      Debug.Assert(lhs.Chunk.Offset.Z == rhs.Chunk.Offset.Z);
      Debug.Assert(lhs.Chunk.Offset.X == rhs.Chunk.Offset.X - Chunk.Length);

      lhs.Right = rhs;
      rhs.Left = lhs;
    }

    public static void LinkVertically(ChunkNode upper, ChunkNode lower)
    {
      Debug.Assert(upper.Chunk.Offset.X == lower.Chunk.Offset.X);
      Debug.Assert(upper.Chunk.Offset.Z == lower.Chunk.Offset.Z + Chunk.Length);

      lower.Back = upper;
      upper.Front = lower;
    }
  }
}