using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BringBackSociety;
using BringBackSociety.Controllers;
using BringBackSociety.Items;
using log4net;

namespace ViewModels
{
  /// <summary> A ViewModel for the display storage. </summary>
  internal class DisplayStorageViewModel
  {
    private readonly StorageContainer _container;
    private readonly ItemSlotViewModel[] _items;
    private readonly InventoryCountController _inventoryCounter;
    private ChangeCount _lastRefresh;

    /// <summary> Constructor. </summary>
    /// <param name="container"> The container of items. </param>
    public DisplayStorageViewModel(StorageContainer container)
    {
      if (container == null)
        throw new ArgumentNullException("container");

      _container = container;
      _items = new ItemSlotViewModel[_container.Slots.Count];
      Items = new ReadOnlyCollection<ItemSlotViewModel>(_items);

      _inventoryCounter = new InventoryCountController(_container);

      Refresh(true);
    }

    /// <summary> The items in the slot. </summary>
    /// <remarks> Call Refresh() to get the latest results. </remarks>
    public ReadOnlyCollection<ItemSlotViewModel> Items { get; private set; }

    /// <summary> Refreshes the cached values. </summary>
    /// <param name="force"> (Optional) true to force, false to only refresh if necessary. </param>
    public void Refresh(bool force = false)
    {
      if (!force && _lastRefresh == _container.ChangeCount)
        return;

      _lastRefresh = _container.ChangeCount;

      for (int i = 0; i < _container.Slots.Count; i++)
      {
        var slot = _container.Slots[i];
        int count = _inventoryCounter.GetDisplayCount(slot);

        if (count > 0)
        {
          _items[i] = new ItemSlotViewModel(slot.Model.Name, count.ToString(), slot.Model.Resource);
        }
        else
        {
          _items[i] = new ItemSlotViewModel("<>", "-", null);
        }
      }
    }
  }

  /// <summary> A ViewModel for an item within a slot. </summary>
  public struct ItemSlotViewModel
  {
    private readonly string _displayName;
    private readonly string _quantityText;
    private readonly IUiResource _uiResource;

    public ItemSlotViewModel(string displayName, string quantityText, IUiResource uiResource)
    {
      _displayName = displayName;
      _quantityText = quantityText;
      _uiResource = uiResource;
    }

    /// <summary> The text to display regarding the name of the item. </summary>
    public string DisplayName
    {
      get { return _displayName; }
    }

    /// <summary> The text to display related to quantity. </summary>
    public string QuantityText
    {
      get { return _quantityText; }
    }

    /// <summary> The UI Resources associated with the item.  </summary>
    public IUiResource UiResource
    {
      get { return _uiResource; }
    }
  }
}