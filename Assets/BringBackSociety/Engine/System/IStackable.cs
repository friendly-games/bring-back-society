using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> An item that can be stacked in inventory. </summary>
  internal interface IStackable : IComponent
  {
    /// <summary> The amount of items that can be stacked together. </summary>
    int StackAmount { get; }
  }
}