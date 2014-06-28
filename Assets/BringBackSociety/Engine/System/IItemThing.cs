using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> An item that represents an instance of a IThing. </summary>
  internal interface IItemThing : IItem
  {
    /// <summary> The thing that the item represents. </summary>
    IThing Thing { get; }
  }
}