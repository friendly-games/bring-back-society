using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Items
{
  /// <summary> An item that is placed in a tile location. </summary>
  public interface INamed : IComponent
  {
    /// <summary> The name of the item. </summary>
    string Name { get; }
  }
}