using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BringBackSociety;
using BringBackSociety.Chunks.Generators;
using BringBackSociety.Chunks.Loaders;
using BringBackSociety.Controllers;
using BringBackSociety.Extensions;
using BringBackSociety.Items;
using BringBackSociety.Items.Weapons;
using BringBackSociety.Services;
using BringBackSociety.Tasks;
using BringBackSociety.ViewModels;
using Drawing;
using Items;
using log4net;
using Scripts;
using Services;
using UnityEngine;
using ViewModels;

internal class Bootstrap : MonoBehaviour, IGui
{
  private static ILog Log;

  private readonly PropertyDrawer _currentObjectDrawer = new PropertyDrawer("Current Item", 10, 10);
  private readonly PropertyDrawer _weaponDrawer = new PropertyDrawer("Weapons", Screen.width - 120, 10);

  private FireableWeaponController _firableWeaponController;
  private Player _player;
  private DisplayStorageViewModel _containerViewModel;
  private PlayerController _playerController;
  private ChunkProcessor _processor;

  static Bootstrap()
  {
  }

  public void Start()
  {
    Logging.ConfigureAllLogging();
    Log = LogManager.GetLogger(typeof(Bootstrap));
    Logging.Log = LogManager.GetLogger("Temp");

    Logging.Log.Info("What");

    var playerBehavior = GameObject.Find("Player").GetComponent<PlayerMonoBehaviour>();
    _player = playerBehavior.CreatePlayer();

    InitializeServices();
    GenerateWorld();

    Invoke("Initialize", 0.1f);
  }

  public void OnApplicationQuit()
  {
    Debug.Log("Quitting");
  }

  public void Initialize()
  {
    InitializeViews();
    _containerViewModel = new DisplayStorageViewModel(_player.Inventory);
  }

  public World World { get; set; }

  /// <summary> Initializes all of the services for the game. </summary>
  private void InitializeServices()
  {
    AllServices.SynchronizationContext = SynchronizationContext.Current;

    AllServices.RaycastService = new RaycastService();
    AllServices.Dispatcher = new CoroutineDispatcher();
    AllServices.CollectionService = new ItemCollectorService(_player);
    AllServices.RandomNumberGenerator = new RandomNumberGeneratorGenerator();
  }

  /// <summary> Initializes the views for the game. </summary>
  private void InitializeViews()
  {
    var pistolModel = UnitySystem.RetrieveModel<IFireableWeaponModel>("Weapon/Pistol");
    var shotgunModel = UnitySystem.RetrieveModel<IFireableWeaponModel>("Weapon/Shotgun");

    _firableWeaponController = new FireableWeaponController(AllServices.RaycastService,
                                                            AllServices.RandomNumberGenerator);
    _playerController = new PlayerController(_player, _firableWeaponController);

    var weapons = from stat in GlobalResources.WeaponStats.Take(5)
                  let model = stat.AmmoType == AmmoType.Pistol ? pistolModel : shotgunModel
                  let weapon = new FireableWeapon(new FireableWeaponTemplate(model, null, stat))
                  select (IItemModel) weapon;

    var ammos = GlobalResources.Ammos.Cast<IItemModel>().Take(5);

    weapons.Concat(ammos)
           .Take(10)
           .Select(w => new InventoryStack(w, w.StackAmount))
           .ToList()
           .ForEach(itemStack => _player.Inventory.AddToStorage(itemStack));

    SwitchWeapons(1);
  }

  private void GenerateWorld()
  {
    var chunkLoader = new SimpleChunkLoader(new PerlinChunkGenerator(new PerlinNoise()));
    World = new World(chunkLoader);

    _processor = new ChunkProcessor(AllServices.Dispatcher, World);

    World.Initialize();
  }

  public void Fire()
  {
    _playerController.FireWeapon();
  }

  public void SwitchWeapons(int weapon)
  {
    var inventory = _player.Inventory;
    var currentWeapon = _player.EquippedWeapon;

    if (weapon < 0 || weapon >= inventory.Slots.Count)
      return;

    var newWeapon = inventory.GetCursor(weapon);

    // if we're not actually switching, bail out
    if (newWeapon == currentWeapon)
      return;

    _player.EquippedWeapon = newWeapon;

    var actualWeapon = _player.EquippedWeapon.Stack.Model as FireableWeapon;

    if (actualWeapon != null)
    {
      _player.WeaponHost.SwitchToCopyOf(actualWeapon.Template.Model);
    }

    Log.InfoFormat("Switched weapon to {0}", inventory.Slots[weapon]);
  }

  public void Update()
  {
    AllServices.Dispatcher.Continue();

    UpdateWorld();
  }

  private void UpdateWorld()
  {
    World.Recenter(_player.Transform.position.ToWorldPosition());
    _processor.HandleChunkChange();
  }

  public void OnGUI()
  {
    DrawLookAtItem();
    DrawWeaponList();
  }

  private void DrawLookAtItem()
  {
    var component = AllServices.RaycastService.Raycast<IComponent>(_player.Transform.ToRay(), 100);
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