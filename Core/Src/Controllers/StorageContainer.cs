using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BringBackSociety.Controllers
{
  /// <summary> Controls the items in some sort of storage controller. </summary>
  public class StorageContainer
  {
    private readonly InventoryStack[] _slots;

    /// <summary> Constructor. </summary>
    /// <param name="size"> The number of items that the container should hold. </param>
    public StorageContainer(int size)
    {
      _slots = new InventoryStack[size];
      Slots = new ReadOnlyCollection<InventoryStack>(_slots);
    }

    /// <summary> All of the inventory slots for the container. </summary>
    public ReadOnlyCollection<InventoryStack> Slots { get; private set; }

    /// <summary> Adds the given inventory item to the storage. </summary>
    /// <param name="stack"> The stack to add to the storage. </param>
    /// <returns> The items that could not be added to the storage. </returns>
    public InventoryStack AddToStorage(InventoryStack stack)
    {
      // first look for any existing stacks that have room
      for (var i = 0; i < _slots.Length && !stack.IsEmpty; i++)
      {
        var slot = _slots[i];

        if (!slot.IsEmpty && slot.Model == stack.Model && slot.ExtraCapacity > 0)
        {
          stack = Fill(i, stack);
        }
      }

      // fill any remaining empty slots with remaining quantities
      if (!stack.IsEmpty)
      {
        for (var i = 0; i < _slots.Length && !stack.IsEmpty; i++)
        {
          stack = Put(i, stack);
        }
      }

      // return however many are left
      return stack;
    }

    /// <summary> Get a cursor looking at the designated slot. </summary>
    /// <param name="slotNumber"> The slot number. </param>
    /// <returns> The cursor to the designated slot. </returns>
    public Cursor GetCursor(int slotNumber)
    {
      return new Cursor(this, slotNumber);
    }

    /// <summary> Gets the stack at the current designated cursor. </summary>
    /// <param name="cursor"> The position from which to get the stack. </param>
    /// <returns> The stack of the items at the designated location. </returns>
    public InventoryStack GetStack(Cursor cursor)
    {
      if (cursor.Parent != this)
        return InventoryStack.Empty;

      return _slots[cursor.SlotNumber];
    }

    /// <summary> Decrements the inventory by one unit </summary>
    /// <param name="cursor"> The position from which to get the stack. </param>
    /// <returns> true if it succeeds, false if it fails. </returns>
    public bool Decrement(Cursor cursor)
    {
      if (cursor.Parent != this)
        return false;

      var value = _slots[cursor.SlotNumber];
      if (value.IsEmpty)
      {
        _slots[cursor.SlotNumber] = InventoryStack.Empty;
        return false;
      }
      else
      {
        _slots[cursor.SlotNumber] = _slots[cursor.SlotNumber].Plus(-1);
        return true;
      }
    }

    /// <summary> Remove the stack at the designated location. </summary>
    /// <param name="cursor"> The position from which to remove the stack. </param>
    /// <returns> The stack of the items at the designated location, removed from this container. </returns>
    public InventoryStack RemoveStack(Cursor cursor)
    {
      if (cursor.Parent != this)
        return InventoryStack.Empty;

      var value = _slots[cursor.SlotNumber];
      _slots[cursor.SlotNumber] = InventoryStack.Empty;

      return value;
    }

    /// <summary> Puts items of the designated into the designated empty slot. </summary>
    /// <param name="cursor"> The position in which to put the stack. </param>
    /// <param name="stack"> The stack to add to the storage. </param>
    /// <returns> The remaining items that could not be placed in the designated slot. </returns>
    public InventoryStack Put(Cursor cursor, InventoryStack stack)
    {
      if (cursor.Parent != this)
        return stack;

      return Put(cursor.SlotNumber, stack);
    }

    /// <summary> Fills the designated slot with the items of the same time. </summary>
    /// <param name="cursor"> The position in which to fill the stack. </param>
    /// <param name="stack"> The stack to add to the storage. </param>
    /// <returns> The remaining items which could not be added. </returns>
    public InventoryStack Fill(Cursor cursor, InventoryStack stack)
    {
      if (cursor.Parent != this)
        return stack;

      return Fill(cursor.SlotNumber, stack);
    }

    /// <summary> Puts items of the designated into the designated empty slot. </summary>
    /// <param name="slotNumber"> The position in which to put the stack. </param>
    /// <param name="stack"> The stack to add to the storage. </param>
    /// <returns> The remaining items that could not be placed in the designated slot. </returns
    private InventoryStack Put(int slotNumber, InventoryStack stack)
    {
      if (!_slots[slotNumber].IsEmpty)
        return stack;

      int numberToAdd = Math.Min(stack.Model.StackAmount, stack.Quantity);

      // add that much to the current slot
      _slots[slotNumber] = new InventoryStack(stack.Model, numberToAdd);

      // reduce the current stack by that much
      stack = stack.Plus(-numberToAdd);
      return stack;
    }

    /// <summary> Fills the designated slot with the items of the same time. </summary>
    /// <param name="slotNumber"> Zero-based index of the slot to fill. </param>
    /// <param name="stack"> The stack to add to the storage. </param>
    /// <returns> The remaining items which could not be added. </returns>
    private InventoryStack Fill(int slotNumber, InventoryStack stack)
    {
      var slot = _slots[slotNumber];

      int numberToAdd = Math.Min(slot.ExtraCapacity, stack.Quantity);

      // add that much to the current slot
      _slots[slotNumber] = slot.Plus(numberToAdd);

      // reduce the current stack by that much
      return stack.Plus(-numberToAdd);
    }

    /// <summary> References a specific slot on the container. </summary>
    public struct Cursor
    {
      /// <summary> Constructor. </summary>
      /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
      /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
      ///  illegal values. </exception>
      /// <param name="parent"> The parent of the slot. </param>
      /// <param name="slot"> The slot. </param>
      internal Cursor(StorageContainer parent, int slot)
      {
        if (parent == null)
          throw new ArgumentNullException("parent");
        if (slot >= parent._slots.Length)
          throw new ArgumentException("slotNumber is >= _slots.Length", "slot");

        Parent = parent;
        SlotNumber = slot;
      }

      /// <summary> The parent. </summary>
      public readonly StorageContainer Parent;

      /// <summary> The slot number. </summary>
      public readonly int SlotNumber;
    }
  }
}