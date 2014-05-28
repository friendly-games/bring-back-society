using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Items
{
  /// <summary> An item which can be displayed. </summary>
  public interface IDisplayableItem
  {
    /// <summary> The UI-specific resources for the item. </summary>
    IUiResource Resource { get; }
  }
}