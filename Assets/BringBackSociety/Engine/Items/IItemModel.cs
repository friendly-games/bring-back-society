using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Items
{
  /// <summary> The model of an item. </summary>
  public interface IItemModel
  {
    /// <summary> The name of the item. </summary>
    string Name { get; }

    /// <summary> Gets the number of these items that can be stacked together. </summary>
    int StackAmount { get; }
  }
}