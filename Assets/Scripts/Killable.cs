using Behavior;
using UnityEngine;

/// <summary> MonoBehavoir for a killable object. </summary>
public class Killable : MonoBehaviour, IKillable
{
  /// <summary> The maximum health amount of health for this entity. </summary>
  public int maxHealth = 100;

  /// <summary> The resistance. </summary>
  public Resistance resistance;

  Resistance IKillable.Resistance
  {
    get { return resistance; }
  }

  /// <summary> The maximum amount of health that the entity can have. </summary>
  int IKillable.MaxHealth
  {
    get { return maxHealth; }
  }

  /// <summary> The health of the object. </summary>
  public int Health { get; set; }

  /// <inheritdoc />
  public void Start()
  {
    this.Initialize();
  }

  /// <inheritdoc />
  void IKillable.Destroy()
  {
    Destroy(gameObject);
  }
}