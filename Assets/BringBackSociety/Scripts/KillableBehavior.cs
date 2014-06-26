using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Engine.System;
using BringBackSociety.Items;
using UnityEngine;

namespace BringBackSociety.Scripts
{
  /// <summary> MonoBehavoir for a killable object. </summary>
  public class KillableBehavior : MonoBehaviour, IThing, IHpHolder, ICanBeDestroyed, IResist
  {
    /// <summary> The maximum health amount of health for this entity. </summary>
    public int maxHealth = 100;

    /// <summary> The resistance. </summary>
    public Resistance resistance;

    private int _health;

    Resistance IResist.Resistance
    {
      get { return resistance; }
    }

    /// <summary> The maximum amount of health that the entity can have. </summary>
    int IHpHolder.MaxHealth
    {
      get { return maxHealth; }
    }

    /// <summary> The health of the object. </summary>
    int IHpHolder.Health
    {
      get { return _health; }
      set { _health = value; }
    }

    /// <inheritdoc />
    public void Start()
    {
      IHpHolder destroyable = this;
      destroyable.Health = destroyable.MaxHealth;
    }

    /// <inheritdoc />
    void ICanBeDestroyed.MarkDestroyed()
    {
      Destroy(gameObject);
    }
  }
}