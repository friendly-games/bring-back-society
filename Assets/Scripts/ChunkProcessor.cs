﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behavior;
using BringBackSociety;
using BringBackSociety.Tasks;
using Extensions;
using log4net;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary> Adds the physics and game objects to the world when a new chunk is created. </summary>
internal class ChunkProcessor
{
  private readonly CoroutineDispatcher _dispatcher;
  private readonly LinkedList<Chunk> _chunksToLoad;
  private readonly LinkedList<Chunk> _chunksToUnload;
  private bool _isChunkProcessorActive;

  private readonly YieldInstruction _waiter = null;
  private readonly GameObject _wall;
  private readonly GameObject _allWalls;
  private readonly ILog _log = LogManager.GetLogger(typeof (ChunkProcessor));

  /// <summary> Constructor. </summary>
  /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
  /// <param name="dispatcher"> The MonoBehavior that created this processor. </param>
  public ChunkProcessor(CoroutineDispatcher dispatcher)
  {
    if (dispatcher == null)
      throw new ArgumentNullException("dispatcher");

    _dispatcher = dispatcher;
    _chunksToLoad = new LinkedList<Chunk>();
    _chunksToUnload = new LinkedList<Chunk>();

    _isChunkProcessorActive = false;

    _wall = Prefabs.GetPrefab("Wall");
    _allWalls = GameObject.Find("All.Walls");
  }

  /// <summary> Process an incoming chunk change. </summary>
  public void HandleChunkChange(object sender, ChunkChangedArgs args)
  {
    var chunk = args.Chunk;

    // TODO make the node have a boolean indicating if it should still be loaded
    if (args.WasLoaded)
    {
      // if we're trying to load one that was just unloaded, remove it from the unload list
      if (_chunksToUnload.Contains(chunk))
      {
        _chunksToUnload.Remove(chunk);
      }
      else
      {
        _chunksToLoad.AddLast(chunk);
      }
    }
    else
    {
      // if we're trying to unload one that was just loaded, remove it from the load list
      if (_chunksToLoad.Contains(chunk))
      {
        _chunksToLoad.Remove(chunk);
      }
      else
      {
        _chunksToUnload.AddLast(chunk);
      }
    }

    // if there isn't already a co-routine, make one
    if (!_isChunkProcessorActive)
    {
      _dispatcher.Start(DoChunkProcessing());
    }
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
        var wallParentKillable = chunkObject.AddComponent<WallProviderParentKillable>();
        wallParentKillable.Chunk = chunk;
      }

      while (_chunksToUnload.Count > 0)
      {
        var chunk = _chunksToUnload.First.Value;
        _chunksToUnload.RemoveFirst();
        Object.Destroy(chunk.TagValue<GameObject>());
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
      var tile = chunk.Tiles[coordinate.Index];

      if (tile.Wall > 0)
      {
        var position = chunkOffset + new Vector3(x, _wall.transform.position.y, z) / 2;
        var newWall = _wall.Clone(position);
        newWall.transform.parent = chunkObject.transform;
      }
    }
  }
}