using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> Represents an item that can be stacked in inventory. </summary>
  internal interface ICanStack : IAspect
  {
    /// <summary> Gets how many of the items that can be stacked. </summary>
    StackableWeight StackableWeight { get; }
  }
}