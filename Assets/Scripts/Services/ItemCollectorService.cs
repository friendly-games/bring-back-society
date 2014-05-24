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
    public void Collect(ICollectible collectible)
    {
      var bestMatch = new StorageContainer.Cursor();

      var inventory = _player.Inventory;
      foreach (StorageContainer.Cursor cursor in inventory)
      {
        // find the ammo stack with the lowest count
        if (cursor.Stack.Model is IAmmoModel &&
            (cursor.Stack.ExtraCapacity > bestMatch.Stack.ExtraCapacity))
        {
          bestMatch = cursor;
        }
      }

      if (bestMatch.IsValid)
      {
        // fill it to the top
        var model = bestMatch.Stack.Model;
        var extraItems = new InventoryStack(model, bestMatch.Stack.ExtraCapacity);

        inventory.Fill(bestMatch, extraItems);
      }
    }
  }
}