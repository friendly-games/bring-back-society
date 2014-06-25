using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> Represents an object that is killable. </summary>
  public interface IHpHolder : IAspect
  {
    /// <summary> The maximum amount of health that the entity can have. </summary>
    int MaxHealth { get; }

    /// <summary> The current health of the entity. </summary>
    int Health { get; set; }
  }
}