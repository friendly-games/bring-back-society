using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BringBackSociety.Loaders;
using JetBrains.Annotations;

namespace BringBackSociety
{
  /// <summary> Contains all of the chunks in the world. </summary>
  public sealed class World
  {
    private readonly ChunkManager _manager;

    /// <summary> Default constructor. </summary>
    /// <param name="loader"> A factory which can load and save the chunks as needed. </param>
    public World(IChunkLoader loader)
    {
      _manager = new ChunkManager(loader);

      Recenter(new ChunkCoordinate(0, 0));
    }

    /// <summary>
    ///  The chunk which is at the center of the world.  As the player moves, the node chunk be
    ///  changed so that only the required parts of the world are loaded at any time.
    /// </summary>
    public ChunkNode CenterNode { get; private set; }

    /// <summary> Event fired when a new chunk is loaded or saved. </summary>
    public event EventHandler<ChunkChangedArgs> ChunkChange;

    private void OnChunkChange(ChunkChangedArgs args)
    {
      EventHandler<ChunkChangedArgs> handler = ChunkChange;
      if (handler != null)
      {
        handler(this, args);
      }
    }

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
      var result = _manager.Load(coordinate, true);
      CenterNode = result.Center;

      foreach (var node in result.Loaded)
      {
        OnChunkChange(new ChunkChangedArgs(node.Chunk, true));
      }
    }
  }
}