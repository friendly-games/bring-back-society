using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;
using UnityEngine;

namespace BringBackSociety.Scripts
{
  /// <summary> MonoBehavoir for a killable object. </summary>
  public class KillableBehavior : MonoBehaviour, IDestroyable
  {
    /// <summary> The maximum health amount of health for this entity. </summary>
    public int maxHealth = 100;

    /// <summary> The resistance. </summary>
    public Resistance resistance;

    private int _health;

    Resistance IDestroyable.Resistance
    {
      get { return resistance; }
    }

    /// <summary> The maximum amount of health that the entity can have. </summary>
    int IDestroyable.MaxHealth
    {
      get { return maxHealth; }
    }

    /// <summary> The health of the object. </summary>
    int IDestroyable.Health
    {
      get { return _health; }
      set { _health = value; }
    }

    /// <inheritdoc />
    public void Start()
    {
      IDestroyable destroyable = this;
      destroyable.Health = destroyable.MaxHealth;
    }

    /// <inheritdoc />
    void IDestroyable.Destroy()
    {
      Destroy(gameObject);
    }
  }
}