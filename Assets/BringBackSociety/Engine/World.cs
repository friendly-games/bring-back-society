using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BringBackSociety.Engine;

namespace BringBackSociety
{
  /// <summary> Contains all of the chunks in the world. </summary>
  public sealed class World
  {
    private readonly IChunkLoader _loader;

    /// <summary> The number of chunks that span the world in one direction. </summary>
    public const int NumberOfChunksWide = 128;

    static World()
    {
      const int half = NumberOfChunksWide / 2;
      Max = new ChunkCoordinate(half - 1, half - 1);
      Min = new ChunkCoordinate(-half, -half);
    }

    /// <summary> Default constructor. </summary>
    /// <param name="loader"> A factory which can load and save the chunks as needed. </param>
    public World(IChunkLoader loader)
    {
      _loader = loader;
      Chunks = new Chunk[NumberOfChunksWide * NumberOfChunksWide];

      LoadChunks();
    }

    public static ChunkCoordinate Max { get; private set; }

    public static ChunkCoordinate Min { get; private set; }

    /// <summary> All of the chunks in the world. </summary>
    public Chunk[] Chunks { get; private set; }

    /// <summary> Loads the chunks from memory. </summary>
    private void LoadChunks()
    {
      const int half = NumberOfChunksWide / 2;

      // create the upper edge
      for (int x = -half; x < half; x++)
      {
        Chunk chunk = _loader.Load(new ChunkCoordinate(x, -half));
        Chunks[chunk.Coordinate.Index] = chunk;
      }

      // create the lower edge
      for (int z = -half + 1; z < half; z++)
      {
        Chunk chunk = _loader.Load(new ChunkCoordinate(-half, z));
        Chunks[chunk.Coordinate.Index] = chunk;
      }

      // create the rest (and link the neighbor nodes
      for (int x = -half + 1; x < half; x++)
        for (int z = -half + 1; z < half; z++)
        {
          var centerNode = _loader.Load(new ChunkCoordinate(x, z));
          Chunks[centerNode.Coordinate.Index] = centerNode;

          var nodeToLeft = Chunks[new ChunkCoordinate(x - 1, z).Index];
          var nodeAbove = Chunks[new ChunkCoordinate(x, z - 1).Index];

          Chunk.LinkHorizontally(nodeToLeft, centerNode);
          Chunk.LinkVertically(centerNode, nodeAbove);
        }
    }

    /// <summary> Initialize the world. </summary>
    public void Initialize()
    {
      Recenter(new ChunkCoordinate(0, 0));
    }

    /// <summary>
    ///  The chunk which is at the center of the world.  As the player moves, the node chunk be
    ///  changed so that only the required parts of the world are loaded at any time.
    /// </summary>
    public Chunk CenterChunk { get; private set; }

    /// <summary>
    ///  Re-centers the world on the given chunk, loading neighboring chunks when needed.
    /// </summary>
    /// <param name="position"> The position to recenter on. </param>
    public void Recenter(WorldPosition position)
    {
      TileCoordinate _1;
      ChunkCoordinate chunkCoordinate;
      position.CalculateCoordinates(out chunkCoordinate, out _1);
      Recenter(chunkCoordinate);
    }

    /// <summary>
    ///  Re-centers the world on the given chunk, loading neighboring chunks when needed.
    /// </summary>
    /// <param name="coordinate"> The position of the chunk to make the center of the world. </param>
    private void Recenter(ChunkCoordinate coordinate)
    {
      CenterChunk = Chunks[coordinate.Index];
    }
  }
}