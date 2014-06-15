using Behavior.Collidables;
using BringBackSociety.Controllers;
using log4net;
using Services;
using UnityEngine;

public class ItemCollector : MonoBehaviour, IStart
{
  /// <summary> Provides logging for the class. </summary>
  private static readonly ILog Log = LogManager.GetLogger(typeof(ItemCollector));

  /// <summary> The handler to which events should be forwarded. </summary>
  private ICollisionHandler _handler;

  public void Start()
  {
    _handler = gameObject.RetrieveInHierarchy<ICollisionHandler>();

    if (_handler == null)
    {
      Log.ErrorFormat("{0} has no associated {1}.  Self-destructing", GetType(), typeof(ICollisionHandler));
      Destroy(this);
    }
  }

  public void OnTriggerEnter(Collider otherCollider)
  {
    _handler.HandleCollision(CollisionType.ItemCollector, otherCollider);
  }
}