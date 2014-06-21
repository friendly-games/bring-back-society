using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Extensions;
using BringBackSociety.Scripts;
using BringBackSociety.Tasks;
using log4net;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BringBackSociety.Game
{
  /// <summary> Adds the physics and game objects to the world when a new chunk is created. </summary>
  internal class ChunkProcessor
  {
    private readonly CoroutineDispatcher _dispatcher;
    private readonly World _world;
    private readonly LinkedList<Chunk> _chunksToLoad;
    private readonly LinkedList<Chunk> _chunksToUnload;
    private readonly LinkedList<Chunk> _loadedChunks;
    private bool _isChunkProcessorActive;

    private readonly YieldInstruction _waiter = null;
    private readonly GameObject _wall;
    private readonly GameObject _allWalls;
    private readonly ILog _log = LogManager.GetLogger(typeof(ChunkProcessor));
    private Chunk _oldCenterChunk;

    /// <summary> Constructor. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="dispatcher"> The MonoBehavior that created this processor. </param>
    public ChunkProcessor(CoroutineDispatcher dispatcher, World world)
    {
      if (dispatcher == null)
        throw new ArgumentNullException("dispatcher");

      _dispatcher = dispatcher;
      _world = world;
      _chunksToLoad = new LinkedList<Chunk>();
      _chunksToUnload = new LinkedList<Chunk>();
      _loadedChunks = new LinkedList<Chunk>();

      _isChunkProcessorActive = false;

      _wall = Prefabs.GetPrefab("Wall");
      _allWalls = GameObject.Find("All.Walls");
    }

   
    /// <summary> Process an incoming chunk change. </summary>
    public void HandleChunkChange()
    {
      if (_world.CenterChunk == _oldCenterChunk)
        return;

      var center = _world.CenterChunk;

      int minX = Math.Max(center.Coordinate.X - 1, World.Min.X);
      int maxX = Math.Min(center.Coordinate.X + 1, World.Max.X);

      int minZ = Math.Max(center.Coordinate.Z - 1, World.Min.Z);
      int maxZ = Math.Min(center.Coordinate.Z + 1, World.Max.Z);

      // remove currently loaded chunks that are no longer needed
      foreach (var chunk in _loadedChunks)
      {
        if (chunk.Coordinate.X < minX
            || chunk.Coordinate.X > maxX
            || chunk.Coordinate.Z < minZ
            || chunk.Coordinate.Z > maxZ)
        {
          _chunksToUnload.AddLast(chunk);
        }
      }

      // remove any that are no longer necessary from the "to-load" list
      foreach (var chunk in _chunksToLoad.ToArray())
      {
        if (chunk.Coordinate.X < minX
            || chunk.Coordinate.X > maxX
            || chunk.Coordinate.Z < minZ
            || chunk.Coordinate.Z > maxZ)
        {
          _chunksToLoad.Remove(chunk);
        }
      }

      for (int x = minX; x <= maxX; x++)
      {
        for (int z = minZ; z <= maxZ; z++)
        {
          var chunk = _world.Chunks[new ChunkCoordinate(x, z).Index];

          _chunksToLoad.AddLast(chunk);
          // remove it if its in the remove list
          _chunksToUnload.Remove(chunk);
        }
      }

      // if there isn't already a co-routine, make one
      if (!_isChunkProcessorActive)
      {
        _dispatcher.Start(DoChunkProcessing());
      }

      _oldCenterChunk = _world.CenterChunk;
    }

    /// <summary>
    ///  Co-routine that performs the heavy lifting of adding and removing game objects for the chunks.
    /// </summary>
    private IEnumerator DoChunkProcessing()
    {
      _log.Info("Processing chunk change");

      _isChunkProcessorActive = true;

      const int numRowsPerAdd = 256 / Chunk.Length;

      do
      {
        if (_chunksToLoad.Count > 0)
        {
          var chunk = _chunksToLoad.First.Value;
          _chunksToLoad.RemoveFirst();

          if (!_loadedChunks.Contains(chunk))
          {
            _loadedChunks.AddLast(chunk);

            var chunkObject = new GameObject("Chunk[" + chunk.Coordinate + "]");
            var chunkOffset = chunk.Offset.ToVector3();

            chunkObject.transform.parent = _allWalls.transform;

            // go through all the tiles and create walls in all the right locations
            for (int x = 0; x < Chunk.Length; x++)
            {
              ProcessRow(chunk, chunkObject, chunkOffset, x);

              // always yield somewhere in between
              if (x % numRowsPerAdd == 0)
              {
                yield return _waiter;
              }
            }

            chunk.Tag = chunkObject;
            var wallParentKillable = chunkObject.AddComponent<WallParentProviderKillableBehavior>();
            wallParentKillable.Chunk = chunk;
          }
        }

        while (_chunksToUnload.Count > 0)
        {
          var chunk = _chunksToUnload.First.Value;
          _chunksToUnload.RemoveFirst();
          Object.Destroy(chunk.TagValue<GameObject>());
          _loadedChunks.Remove(chunk);
        }

        yield return _waiter;
      } while (_chunksToLoad.Count > 0 || _chunksToUnload.Count > 0);

      _log.Info("Ending processing chunk change");
      _isChunkProcessorActive = false;
    }

    /// <summary> Process a single row of tiles. </summary>
    /// <param name="chunk"> The chunk that is being processed. </param>
    /// <param name="chunkObject"> The game object that represents the tile. </param>
    /// <param name="chunkOffset"> The world offset of the chunk to add to the tiles local location. </param>
    /// <param name="x"> The x coordinates of the tile row to process. </param>
    private void ProcessRow(Chunk chunk, GameObject chunkObject, Vector3 chunkOffset, int x)
    {
      for (int z = 0; z < Chunk.Length; z++)
      {
        var coordinate = new TileCoordinate(x, z);
        var tile = chunk[coordinate];

        if (tile.Type > 0)
        {
          var position = chunkOffset + new Vector3(x, _wall.transform.position.y, z);
          var newWall = _wall.Clone(position);
          newWall.transform.parent = chunkObject.transform;
        }
      }
    }
  }
}