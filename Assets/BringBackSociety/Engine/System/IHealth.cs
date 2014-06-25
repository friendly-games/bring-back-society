using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> An object that contains health. </summary>
  internal interface IHealth : IReadWriteComponent
  {
    /// <summary> The current health of the entity. </summary>
    int Health { get; set; }

    /// <summary> The maximum amount of health the entity can have. </summary>
    int MaxHealth { get; }
  }
}