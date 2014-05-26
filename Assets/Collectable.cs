using System.Linq;
using BringBackSociety.Items;
using BringBackSociety.Services;
using log4net;
using Services;
using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour
{
  /// <summary> Provides logging for the class. </summary>
  private static readonly ILog Log = LogManager.GetLogger(typeof(Collectable));

  private IItemCollectionService _collectionService;
  private GameObject _player;

  // Use this for initialization
  public void Start()
  {
    _player = GameObject.Find("Player").gameObject;

    _collectionService = AllServices.CollectionService;
    if (_collectionService == null)
    {
      Log.Error("Collection service is null");
    }
  }

  // Update is called once per frame
  public void Update()
  {
  }

  public void OnTriggerEnter(Collider otherCollider)
  {
    var otherGameObject = otherCollider.gameObject;

    // only the player should be hit
    if (otherGameObject != _player)
      return;

    Log.Info("Collision detected");

    _collectionService.Collect(null);

    // Object.Destroy(gameObject);
  }
}