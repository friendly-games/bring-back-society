using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BringBackSociety.Items;

namespace BringBackSociety.Controllers
{
  /// <summary> Controls the items in some sort of storage controller. </summary>
  public class StorageContainer : ISnapshotable
  {
    private readonly InventoryStack[] _slots;

    /// <summary> Constructor. </summary>
    /// <param name="size"> The number of items that the container should hold. </param>
    public StorageContainer(int size)
    {
      _slots = new InventoryStack[size];
      Slots = new ReadOnlyCollection<InventoryStack>(_slots);

      ForceSnapshot();
    }

    /// <summary> All of the inventory slots for the container. </summary>
    public ReadOnlyCollection<InventoryStack> Slots { get; private set; }

    /// <summary>
    ///  A token indicating the contents of the storage at a moment in time.  If an item is added or
    ///  removed, this token changes.
    /// </summary>
    /// <remarks>
    ///  If an item within the storage changes, the external change should inform the storage that the
    ///  change has occurred by invoking the ForceSnapshot() method.
    /// </remarks>
    public SnapshotToken SnapshotToken { get; private set; }

    /// <inheritdoc />
    public void ForceSnapshot()
    {
      SnapshotToken = SnapshotToken.Next();
    }

    /// <summary> Return a cursor to the first slot. </summary>
    public Cursor FirstSlot
    {
      get { return new Cursor(this, 0); }
    }

    /// <summary>
    ///  Adds the given inventory item to the storage, placing it with other items of the item, or
    ///  placing it into an existing item if needed.
    /// </summary>
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
        Write(cursor.SlotNumber, InventoryStack.Empty);
        return false;
      }
      else
      {
        Write(cursor.SlotNumber, _slots[cursor.SlotNumber].Plus(-1));
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
      Write(cursor.SlotNumber, InventoryStack.Empty);

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
    /// <returns> The remaining items that could not be placed in the designated slot. </returns>
    private InventoryStack Put(int slotNumber, InventoryStack stack)
    {
      if (!_slots[slotNumber].IsEmpty)
        return stack;

      int numberToAdd = Math.Min(stack.Model.StackAmount, stack.Quantity);

      // add that much to the current slot
      Write(slotNumber, new InventoryStack(stack.Model, numberToAdd));

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
      Write(slotNumber, slot.Plus(numberToAdd));

      // reduce the current stack by that much
      return stack.Plus(-numberToAdd);
    }

    /// <summary> Writes a given stack to a slot. </summary>
    /// <param name="slotNumber"> The slot number to which the value should be written. </param>
    /// <param name="value"> The value of the slot. </param>
    private void Write(int slotNumber, InventoryStack value)
    {
      if (value.IsEmpty)
        value = InventoryStack.Empty;

      _slots[slotNumber] = value;
      // increment the change count
      ForceSnapshot();
    }

    /// <returns> An enumerable for iterating through the collection. </returns>
    public Enumerator GetEnumerator()
    {
      return new Enumerator(FirstSlot);
    }

    /// <summary> References a specific slot on the container. </summary>
    public struct Cursor : IEquatable<Cursor>
    {
      /// <summary> The parent. </summary>
      private readonly StorageContainer _parent;

      /// <summary> The slot number. </summary>
      private readonly int _slotNumber;

      /// <summary> The default cursor value. </summary>
      public static readonly Cursor Empty = new Cursor();

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

        _parent = parent;
        _slotNumber = slot;
      }

      /// <summary> The parent. </summary>
      public StorageContainer Parent
      {
        get { return _parent; }
      }

      /// <summary> The slot number. </summary>
      public int SlotNumber
      {
        get { return _slotNumber; }
      }

      /// <summary> True if the cursor is that last item in the storage container. </summary>
      public bool IsValid
      {
        get { return Parent != null && SlotNumber >= 0 && SlotNumber < Parent._slots.Length; }
      }

      /// <summary>
      ///  Gets a cursor that points to the next slot after this one, or the first one if this is the
      ///  last slot.
      /// </summary>
      public Cursor GetNextCursor()
      {
        if (Parent == null || SlotNumber + 1 >= Parent._slots.Length)
          return Cursor.Empty;

        return new Cursor(Parent, SlotNumber + 1);
      }

      /// <summary> The value of this position in the container. </summary>
      public InventoryStack Stack
      {
        get
        {
          if (!IsValid)
            return InventoryStack.Empty;

          return Parent._slots[SlotNumber];
        }
      }

      /// <summary> InventoryStack casting operator. </summary>
      public static implicit operator InventoryStack(Cursor cursor)
      {
        return cursor.Stack;
      }

      /// <inheritdoc />
      public override string ToString()
      {
        return Stack.ToString();
      }

      /// <inheritdoc />
      public bool Equals(Cursor other)
      {
        return _parent == other._parent
               && _slotNumber == other._slotNumber;
      }

      /// <inheritdoc />
      public override bool Equals(object obj)
      {
        if (ReferenceEquals(null, obj))
          return false;

        return obj is Cursor && Equals((Cursor) obj);
      }

      /// <inheritdoc />
      public override int GetHashCode()
      {
        unchecked
        {
          return ((_parent != null ? _parent.GetHashCode() : 0) * 397) ^ _slotNumber;
        }
      }

      /// <inheritdoc />
      public static bool operator ==(Cursor left, Cursor right)
      {
        return left.Equals(right);
      }

      /// <inheritdoc />
      public static bool operator !=(Cursor left, Cursor right)
      {
        return !left.Equals(right);
      }
    }

    /// <summary> Mutable enumerator for the container. </summary>
    public struct Enumerator
    {
      private Cursor _cursor;

      /// <summary> Constructor. </summary>
      /// <param name="cursor"> The cursor. </param>
      internal Enumerator(Cursor cursor)
      {
        _cursor = cursor;
      }

      /// <inheritdoc />
      public bool MoveNext()
      {
        _cursor = _cursor.GetNextCursor();
        return _cursor.IsValid;
      }

      /// <inheritdoc />
      public Cursor Current
      {
        get { return _cursor; }
      }
    }
  }
}