using System;
using System.Collections.Generic;
using Behavior;
using BringBackSociety;
using BringBackSociety.Chunks.Generators;
using BringBackSociety.Chunks.Loaders;
using BringBackSociety.Tasks;
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

  private readonly GuiDrawer drawer = new GuiDrawer();

  public void OnGUI()
  {
    RaycastHit hitInfo;
    if (Physics.Raycast(new Ray(_player.transform.position, _player.transform.forward),
                        out hitInfo,
                        100))
    {
      var component = hitInfo.collider.gameObject.Get();

      drawer.Start();

      var name = component as INamed;
      if (name != null)
      {
        drawer.DrawItem("Name:", name.Name);
      }

      var destroyable = component as IDestroyable;
      if (destroyable != null)
      {
        drawer.DrawItem("Health:", destroyable.Health.ToString());
      }

      var tileItem = component as ITileItem;
      if (tileItem != null)
      {
        drawer.DrawItem("Chunk:", tileItem.TileCoordinate.ToString());
        drawer.DrawItem("Tile:", tileItem.ChunkCoordinate.ToString());
        drawer.DrawItem("World:",
                        new WorldPosition(tileItem.ChunkCoordinate, tileItem.TileCoordinate).ToString());
      }
    }
  }

  private class GuiDrawer
  {
    private int _line = 0;

    public void Start()
    {
      _line = 30;
      GUI.Box(new Rect(10, 10, 120, 120), "Current Item");
    }

    public void DrawItem(string name, string value)
    {
      GUI.Label(new Rect(20, _line, 50, 20), name);
      GUI.Label(new Rect(72, _line, 50, 20), value);

      _line += 18;
    }
  }
}