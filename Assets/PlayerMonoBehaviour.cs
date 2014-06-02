using Behavior;
using Behavior.Collidables;
using BringBackSociety.Controllers;
using BringBackSociety.Items;
using BringBackSociety.Services;
using Items;
using log4net;
using Models;
using Services;
using UnityEngine;
using System.Collections;

internal class PlayerMonoBehaviour : MonoBehaviour, ICollisionHandler, IStart
{
  /// <summary> Provides logging for the class. </summary>
  private static readonly ILog Log = LogManager.GetLogger(typeof(PlayerMonoBehaviour));

  private IItemCollectionService _collectionService;

  public void Start()
  {
    _collectionService = AllServices.CollectionService;
  }

  /// <summary> Creates the player object for the game. </summary>
  /// <returns> The player that was created for this behavior. </returns>
  public Player CreatePlayer()
  {
    Player = new Player(gameObject, new ModelHost(gameObject, new Vector3(.55f, .35f, .35f)));

    return Player;
  }

  /// <summary> The player associated with this object. </summary>
  public Player Player { get; private set; }

  /// <inheritdoc />
  public void HandleCollision(CollisionType collision, Collider rhs)
  {
    switch (collision)
    {
      case CollisionType.ItemCollector:
      {
        var stack = rhs.gameObject.RetrieveObject<InventoryStack>();
        _collectionService.Collect(stack);
      }
        break;
    }

    Log.Info("Collision Detected in Player");
  }
}