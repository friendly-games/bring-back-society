using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety;
using Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary> Adds the physics and game objects to the world when a new chunk is created. </summary>
internal class ChunkProcessor
{
  private readonly MonoBehaviour _owner;
  private readonly Queue<Chunk> _chunksToLoad;
  private readonly Queue<Chunk> _chunksToUnload;
  private bool _isChunkProcessorActive;

  private readonly YieldInstruction _waiter = null;
  private readonly GameObject _wall;
  private readonly GameObject _allWalls;

  /// <summary> Constructor. </summary>
  /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
  /// <param name="owner"> The MonoBehavior that created this processor. </param>
  public ChunkProcessor(MonoBehaviour owner)
  {
    if (owner == null)
      throw new ArgumentNullException("owner");

    _owner = owner;
    _chunksToLoad = new Queue<Chunk>();
    _chunksToUnload = new Queue<Chunk>();

    _isChunkProcessorActive = false;

    _wall = Prefabs.GetPrefab("Wall");
    _allWalls = GameObject.Find("All.Walls");
  }

  /// <summary> Process an incoming chunk change. </summary>
  public void HandleChunkChange(object sender, ChunkChangedArgs args)
  {
    var chunk = args.Chunk;

    if (args.WasLoaded)
    {
      _chunksToLoad.Enqueue(chunk);
    }
    else
    {
      _chunksToUnload.Enqueue(chunk);
    }

    // if there isn't already a co-routine, make one
    if (!_isChunkProcessorActive)
    {
      _owner.StartCoroutine(DoChunkProcessing());
    }
  }

  /// <summary>
  ///  Co-routine that performs the heavy lifting of adding and removing game objects for the chunks.
  /// </summary>
  private IEnumerator DoChunkProcessing()
  {
    Logging.Info("Starting queue");

    _isChunkProcessorActive = true;

    const int numRowsPerAdd = 256 / Chunk.Length;

    do
    {
      if (_chunksToLoad.Count > 0)
      {
        var chunk = _chunksToLoad.Dequeue();

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
      }
      else if (_chunksToUnload.Count > 0)
      {
        var chunk = _chunksToUnload.Dequeue();
        Object.Destroy(chunk.TagValue<GameObject>());
      }

      yield return _waiter;
    } while (_chunksToLoad.Count > 0 || _chunksToUnload.Count > 0);

    Logging.Info("Existing processing queue");
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

      if (tile.GroundType > 0)
      {
        var position = chunkOffset + new Vector3(x, _wall.transform.localScale.y / 2, z) / 2;
        var newWall = _wall.Clone(position);
        newWall.transform.parent = chunkObject.transform;
      }
    }
  }
}