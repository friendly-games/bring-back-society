using BringBackSociety.Services;
using log4net;
using Services;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
  /// <summary> Provides logging for the class. </summary>
  private static readonly ILog Log = LogManager.GetLogger(typeof(ItemCollector));

  public void OnTriggerEnter(Collider otherCollider)
  {
    var player = this.gameObject.RetrieveObject<Player>();
  }
}