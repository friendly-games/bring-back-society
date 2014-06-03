using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BringBackSociety;
using BringBackSociety.Chunks.Generators;
using BringBackSociety.Chunks.Loaders;
using BringBackSociety.Controllers;
using BringBackSociety.Items;
using BringBackSociety.Items.Weapons;
using BringBackSociety.Services;
using BringBackSociety.Tasks;
using Drawing;
using Extensions;
using Items;
using log4net;
using Models;
using Scripts;
using Services;
using UnityEngine;
using ViewModels;

internal class Bootstrap : MonoBehaviour, IGui
{
  private static readonly ILog Log;

  private readonly PropertyDrawer _currentObjectDrawer = new PropertyDrawer("Current Item", 10, 10);
  private readonly PropertyDrawer _weaponDrawer = new PropertyDrawer("Weapons", Screen.width - 120, 10);

  private FireableWeaponController _firableWeaponController;
  private Player _player;
  private UnitySynchronizationContext _syncContext;
  private DisplayStorageViewModel _containerViewModel;
  private PlayerController _playerController;

  static Bootstrap()
  {
    Logging.ConfigureAllLogging();
    Log = LogManager.GetLogger(typeof(Bootstrap));
    Logging.Log = LogManager.GetLogger("Temp");
  }

  public void Start()
  {
    var playerBehavior = GameObject.Find("Player").GetComponent<PlayerMonoBehaviour>();
    _player = playerBehavior.CreatePlayer();

    InitializeServices();
    GenerateWorld();

    Invoke("Initialize", 0.1f);
  }

  public void Initialize()
  {
    InitializeViews();
    _containerViewModel = new DisplayStorageViewModel(_player.Inventory);

    var model = UnitySystem.RetrieveModel<IFireableWeaponModel>("Weapon");
    _player.WeaponHost.Use(model);
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
    AllServices.CollectionService = new ItemCollectorService(_player);
  }

  /// <summary> Initializes the views for the game. </summary>
  private void InitializeViews()
  {
    _firableWeaponController = new FireableWeaponController(AllServices.RaycastService);
    _playerController = new PlayerController(_player, _firableWeaponController);

    var weapons = GlobalResources.WeaponStats.Take(5)
                                 .Select(s => new FireableWeapon(s))
                                 .Cast<IItemModel>();
    var ammos = GlobalResources.Ammos.Cast<IItemModel>().Take(5);

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
    _playerController.FireWeapon();
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
    if (_containerViewModel == null)
      return;

    _weaponDrawer.Start();

    _containerViewModel.Refresh();

    var items = _containerViewModel.Items;
    for (var i = 0; i < items.Count; i++)
    {
      var item = items[i];

      string label = (i + 1).ToString();

      if (i == _player.EquippedWeapon.SlotNumber)
      {
        _weaponDrawer.AddItem("*" + " " + item.DisplayName, item.QuantityText);
      }
      else
      {
        _weaponDrawer.AddItem(label + " " + item.DisplayName, item.QuantityText);
      }
    }

    _weaponDrawer.Draw();
  }
}