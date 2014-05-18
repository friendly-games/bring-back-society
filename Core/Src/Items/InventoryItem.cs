using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BringBackSociety.Items
{
  /// <summary> Contains a single item in the inventory. </summary>
  internal class InventoryItem
  {

    /// <summary> The model of the item in the inventory. </summary>
    public IItemModel ItemModel { get; private set; }

    public InventoryItem(IItemModel itemModel)
    {
      if (itemModel == null)
        throw new ArgumentNullException("itemModel");

      ItemModel = itemModel;
    }
  }

}