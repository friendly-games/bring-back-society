using System;
using System.Collections.Generic;
using Behavior;
using BringBackSociety;
using BringBackSociety.Chunks.Generators;
using BringBackSociety.Chunks.Loaders;
using BringBackSociety.Tasks;
using Drawing;
using Extensions;
using log4net;
using Scripts;
using Services;
using UnityEngine;

public class Bootstrap : MonoBehaviour, IGui
{
  public GlobalResources GlobalResources;

  private static readonly ILog Log;

  private GameObject _player;

  private readonly PropertyDrawer _currentObjectDrawer = new PropertyDrawer("Current Item", 10, 10);

  static Bootstrap()
  {
    Logging.ConfigureAllLogging();
    Log = LogManager.GetLogger(typeof(Bootstrap));
  }

  public void Start()
  {
    GlobalResources.Initialize(GlobalResources);

    Dispatcher = new CoroutineDispatcher();
    var chunkLoader = new SimpleChunkLoader(new PerlinChunkGenerator(new PerlinNoise()));

    _player = GameObject.Find("Player");

    var processor = new ChunkProcessor(Dispatcher);

    World = new World(chunkLoader);
    World.ChunkChange += processor.HandleChunkChange;

    World.Initialize();
  }

  public World World { get; set; }

  public CoroutineDispatcher Dispatcher { get; private set; }

  public void Update()
  {
    Dispatcher.Continue();

    World.Recenter((_player.transform.position).ToWorldPosition());
  }

  public void OnGUI()
  {
    RaycastHit hitInfo;
    if (Physics.Raycast(new Ray(_player.transform.position, _player.transform.forward),
                        out hitInfo,
                        100))
    {
      var component = hitInfo.collider.gameObject.Get();
      _currentObjectDrawer.Add(component);
      _currentObjectDrawer.Draw();
    }
  }
}