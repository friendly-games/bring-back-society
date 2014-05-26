using Behavior;
using BringBackSociety.Controllers;
using UnityEngine;

internal class Collectible : MonoBehaviour, IStart, IObjectProvider<InventoryStack>
{
  public void Start()
  {
    // TODO modify this so that it does not initialize always the same
    Instance = new InventoryStack(GlobalResources.Instance.Ammos[0], 10);
  }

  /// <summary> The item to be given to the actor who takes the collectible. </summary>
  public InventoryStack Instance { get; set; }
}