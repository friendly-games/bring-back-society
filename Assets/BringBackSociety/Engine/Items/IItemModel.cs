using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Items
{
  /// <summary> The model of an item. </summary>
  public interface IItemModel : IDisplayableItem, INamedItem
  {
    /// <summary> Gets the number of these items that can be stacked together. </summary>
    int StackAmount { get; }
  }
}