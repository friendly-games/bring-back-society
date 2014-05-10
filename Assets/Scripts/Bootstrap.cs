using System;
using System.Collections.Generic;
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

    var processor = new ChunkProcessor(this);

    World = new World(chunkLoader);
    World.ChunkChange += processor.HandleChunkChange;

    World.Initialize();
  }

  public void Update()
  {
    World.Recenter((_player.transform.position).ToWorldPosition());
  }
}