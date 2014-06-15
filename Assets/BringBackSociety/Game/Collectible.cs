using Behavior;
using BringBackSociety.Controllers;
using UnityEngine;

internal class Collectible : MonoBehaviour, IObjectProvider<InventoryStack>, IStartAndUpdate
{
  private Transform _transform;

  /// <inheritdoc />
  public void Start()
  {
    // TODO modify this so that it does not initialize always the same
    Instance = new InventoryStack(GlobalResources.Ammos[0], 10);

    _transform = transform;
  }

  /// <inheritdoc />
  public void Update()
  {
    _transform.Rotate(0, 60 * Time.deltaTime, 0);
  }

  /// <summary> The item to be given to the actor who takes the collectible. </summary>
  public InventoryStack Instance { get; set; }
}