using System;
using System.Collections.Generic;
using System.Linq;

namespace Behavior
{
  /// <summary> Represents an object that is killable. </summary>
  public interface IDestroyable : IComponent
  {
    /// <summary> The resistance of the object. </summary>
    Resistance Resistance { get; }

    /// <summary> The maximum amount of health that the entity can have. </summary>
    int MaxHealth { get; }

    /// <summary> The current health of the entity. </summary>
    int Health { get; set; }

    /// <summary> Mark the object as destroyed. </summary>
    void Destroy();
  }
}