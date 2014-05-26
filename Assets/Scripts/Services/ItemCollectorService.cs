using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Controllers;
using BringBackSociety.Items;
using BringBackSociety.Services;
using log4net;

namespace Services
{
  /// <summary> An item collector service. </summary>
  internal class ItemCollectorService : IItemCollectionService
  {
    private readonly Player _player;

    /// <summary> Constructor. </summary>
    /// <param name="player"> The player for which the item should be collected. </param>
    public ItemCollectorService(Player player)
    {
      _player = player;
    }

    /// <inheritdoc />
    public void Collect(InventoryStack stack)
    {
      if (stack.IsEmpty)
        return;

      var remainder = _player.Inventory.AddToStorage(stack);
      // TODO do something with the remainder
    }
  }
}