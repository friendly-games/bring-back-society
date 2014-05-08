using BringBackSociety;
using BringBackSociety.Chunks.Generators;
using BringBackSociety.Chunks.Loaders;
using Extensions;
using Services;
using UnityEngine;
using System.Collections;

public class Bootstrap : MonoBehaviour
{
  private GameObject _player;

  public World World { get; set; }

  public void Start()
  {
    var chunkLoader = new SimpleChunkLoader(new PerlinChunkGenerator(new PerlinNoise()));

    chunkLoader.Loading += HandleChunkLoading;
    chunkLoader.Saving += HandleChunkSaving;

    _player = GameObject.Find("Player");

    World = new World(chunkLoader);
  }

  private void HandleChunkSaving(Chunk chunk)
  {
  }

  private void HandleChunkLoading(Chunk chunk)
  {
    var wall = Prefabs.GetPrefab("Wall");
    var allWalls = GameObject.Find("All.Walls");

    var chunkOffset = chunk.Offset.ToVector3();

    for (int x = 0; x < Chunk.Length; x++)
    {
      for (int z = 0; z < 10; z++)
      {
        var coordinate = new TileCoordinate(x, z);
        var tile = chunk.Tiles[coordinate.Index];

        if (tile.GroundType > 0)
        {
          var position = chunkOffset + new Vector3(x, wall.transform.localScale.y/2, z)/2;
          var newWall = wall.Clone(position);
          newWall.transform.parent = allWalls.transform;
        }
      }
    }

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