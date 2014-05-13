using System;
using System.Collections.Generic;
using BringBackSociety;
using BringBackSociety.Chunks.Generators;
using BringBackSociety.Chunks.Loaders;
using BringBackSociety.Tasks;
using Extensions;
using Services;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
  private GameObject _player;

  public World World { get; set; }

  public CoroutineDispatcher Dispatcher { get; private set; }

  static Bootstrap()
  {
    Logging.ConfigureAllLogging();
  }

  public void Start()
  {
    Dispatcher = new CoroutineDispatcher();
    var chunkLoader = new SimpleChunkLoader(new PerlinChunkGenerator(new PerlinNoise()));

    _player = GameObject.Find("Player");

    var processor = new ChunkProcessor(Dispatcher);

    World = new World(chunkLoader);
    World.ChunkChange += processor.HandleChunkChange;

    World.Initialize();
  }

  public void Update()
  {
    Dispatcher.Continue();

    World.Recenter((_player.transform.position).ToWorldPosition());
  }
}