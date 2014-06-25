using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;

namespace BringBackSociety.Engine.System
{
  /// <summary> An object that has resistance against certain effects. </summary>
  internal interface IResistable : IComponent
  {
    /// <summary> The resistance for the object. </summary>
    Resistance Resistance { get; }
  }
}