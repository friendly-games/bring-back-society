﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BringBackSociety;
using BringBackSociety.Chunks.Generators;
using BringBackSociety.Chunks.Loaders;
using BringBackSociety.Controllers;
using BringBackSociety.Items;
using BringBackSociety.Services;
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

  private readonly PropertyDrawer _currentObjectDrawer = new PropertyDrawer("Current Item", 10, 10);
  private readonly PropertyDrawer _weaponDrawer = new PropertyDrawer("Weapons", Screen.width - 120, 10);

  private FireableWeaponController _firableWeaponController;
  private Player _player;
  private UnitySynchronizationContext _syncContext;

  static Bootstrap()
  {
    Logging.ConfigureAllLogging();
    Log = LogManager.GetLogger(typeof(Bootstrap));
  }

  public void Start()
  {
    GlobalResources.Initialize(GlobalResources);

    _player = new Player(GameObject.Find("Player"));

    InitializeServices();
    InitializeViews();
    GenerateWorld();
  }

  public World World { get; set; }

  /// <summary> Initializes all of the services for the game. </summary>
  private void InitializeServices()
  {
    SynchronizationContext.SetSynchronizationContext(_syncContext = new UnitySynchronizationContext());
    AllServices.SynchronizationContext = SynchronizationContext.Current;
    TaskExtensions.InitializeSchedularForUiThread(TaskScheduler.FromCurrentSynchronizationContext());

    AllServices.RaycastService = new RaycastService();
    AllServices.Dispatcher = new CoroutineDispatcher();
  }

  /// <summary> Initializes the views for the game. </summary>
  private void InitializeViews()
  {
    var weaponView = new FirableWeaponView(this);

    _firableWeaponController = new FireableWeaponController(AllServices.RaycastService, weaponView);

    var weapons = GlobalResources.Instance.Weapons.Cast<IItemModel>().Take(5);
    var ammos = GlobalResources.Instance.Ammos.Cast<IItemModel>().Take(5);

    weapons.Concat(ammos)
           .Take(10)
           .Select(w => new InventoryStack(w, w.StackAmount))
           .ToList()
           .ForEach(itemStack => _player.Inventory.AddToStorage(itemStack));

    SwitchWeapons(0);
  }

  private void GenerateWorld()
  {
    var processor = new ChunkProcessor(AllServices.Dispatcher);
    var chunkLoader = new SimpleChunkLoader(new PerlinChunkGenerator(new PerlinNoise()));

    World = new World(chunkLoader);
    World.ChunkChange += processor.HandleChunkChange;

    World.Initialize();
  }

  public void Fire()
  {
    _firableWeaponController.FireWeapon(_player);
  }

  public void SwitchWeapons(int weapon)
  {
    var inventory = _player.Inventory;
    if (weapon < 0 || weapon >= inventory.Slots.Count)
      return;

    _player.EquippedWeapon = inventory.GetCursor(weapon);

    Log.InfoFormat("Switched weapon to {0}", inventory.Slots[weapon]);
  }

  public void Update()
  {
    AllServices.Dispatcher.Continue();

    World.Recenter(_player.Transform.position.ToWorldPosition());
    _syncContext.Process();
  }

  public void OnGUI()
  {
    DrawLookAtItem();
    DrawWeaponList();
  }

  private void DrawLookAtItem()
  {
    var component = AllServices.RaycastService.Raycast<IComponent>(_player, 100);
    if (component != null)
    {
      _currentObjectDrawer.Add(component);
      _currentObjectDrawer.Draw();
    }
  }

  private void DrawWeaponList()
  {
    _weaponDrawer.Start();

    var countController = new InventoryCountController(_player.Inventory);

    var inventory = _player.Inventory;
    for (var i = 0; i < inventory.Slots.Count; i++)
    {
      var slot = inventory.Slots[i];

      string label = (i + 1).ToString();

      var count = countController.GetDisplayCount(slot.Model);

      if (slot.IsEmpty)
      {
        _weaponDrawer.AddItem(label + " - ", " - ");
      }
      else if (i == _player.EquippedWeapon.SlotNumber)
      {
        _weaponDrawer.AddItem("*" + " " + slot.Model.Name, count.ToString());
      }
      else
      {
        _weaponDrawer.AddItem(label + " " + slot.Model.Name, count.ToString());
      }
    }

    _weaponDrawer.Draw();
  }
}