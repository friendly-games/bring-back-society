using System;
using BringBackSociety;
using BringBackSociety.Chunks.Generators;
using BringBackSociety.Chunks.Loaders;
using Extensions;
using Services;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
  private GameObject _player;

  public World World { get; set; }

  public void Start()
  {
    var chunkLoader = new SimpleChunkLoader(new PerlinChunkGenerator(new PerlinNoise()));

    //chunkLoader.Loading += HandleChunkLoading;
    //chunkLoader.Saving += HandleChunkSaving;

    _player = GameObject.Find("Player");

    World = new World(chunkLoader);
    World.ChunkChange += HandleChunkChange;
    World.Initialize();
  }

  private void HandleChunkChange(object sender, ChunkChangedArgs args)
  {
    var chunk = args.Chunk;

    if (args.WasLoaded)
    {
      HandleChunkLoading(chunk);
    }
    else
    {
      HandleChunkSaving(chunk);
    }
  }

  private void HandleChunkSaving(Chunk chunk)
  {
    Destroy(chunk.TagValue<GameObject>());
  }

  private void HandleChunkLoading(Chunk chunk)
  {
    var wall = Prefabs.GetPrefab("Wall");
    var allWalls = GameObject.Find("All.Walls");
    var chunkObject = new GameObject("Chunk[" + chunk.Coordinate + "]");

    chunkObject.transform.parent = allWalls.transform;

    var chunkOffset = chunk.Offset.ToVector3();

    for (int x = 0; x < Chunk.Length; x++)
    {
      for (int z = 0; z < Chunk.Length; z++)
      {
        var coordinate = new TileCoordinate(x, z);
        var tile = chunk.Tiles[coordinate.Index];

        if (tile.GroundType > 0)
        {
          var position = chunkOffset + new Vector3(x, wall.transform.localScale.y/2, z)/2;
          var newWall = wall.Clone(position);
          newWall.transform.parent = chunkObject.transform;
        }
      }
    }

    chunk.Tag = chunkObject;

    Logging.Info("Loading chunk {0} at position {1}, current position: {2}",
                 chunk.Coordinate,
                 chunk.Offset,
                 _player.transform.position.ToWorldPosition());
  }

  public void Update()
  {
    World.Recenter((_player.transform.position).ToWorldPosition());
  }
}