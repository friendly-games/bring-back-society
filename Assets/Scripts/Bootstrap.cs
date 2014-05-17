using System;
using System.Collections.Generic;
using BringBackSociety;
using BringBackSociety.Chunks.Generators;
using BringBackSociety.Chunks.Loaders;
using BringBackSociety.Tasks;
using Drawing;
using Extensions;
using log4net;
using Services;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
  private static readonly ILog _log;

  private GameObject _player;

  public World World { get; set; }

  public CoroutineDispatcher Dispatcher { get; private set; }

  static Bootstrap()
  {
    Logging.ConfigureAllLogging();
    _log = LogManager.GetLogger(typeof (Bootstrap));
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

  private readonly PropertyDrawer drawer = new PropertyDrawer();

  public void OnGUI()
  {
    RaycastHit hitInfo;
    if (Physics.Raycast(new Ray(_player.transform.position, _player.transform.forward),
                        out hitInfo,
                        100))
    {
      var component = hitInfo.collider.gameObject.Get();
      drawer.With(component);
      drawer.Draw();
    }
  }
}