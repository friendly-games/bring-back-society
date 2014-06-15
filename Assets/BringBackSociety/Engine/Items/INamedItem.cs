using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Items
{
  /// <summary> An item that has a name. </summary>
  public interface INamedItem
  {
    /// <summary> The name of the item. </summary>
    string Name { get; }
  }
}