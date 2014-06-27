using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> Contains a stack of inventory items. </summary>
  internal struct ItemStack : IEquatable<ItemStack>
  {
    private readonly IItem _item;
    private readonly int _quantity;

    /// <summary> A stack which contains no model and no items. </summary>
    public static ItemStack Empty = new ItemStack(null, 0);

    /// <summary> Constructor. </summary>
    /// <param name="item"> The item which the stack represents. </param>
    /// <param name="quantity"> The quantity of items in the stack. </param>
    public ItemStack(IItem item, int quantity)
    {
      if (quantity < 0)
        throw new ArgumentException("Inventory must be > 0", "quantity");

      _item = item;
      _quantity = quantity;
    }

    /// <summary> The item which the stack represents. </summary>
    public IItem Item
    {
      get { return _item; }
    }

    /// <summary> The quantity of items in the stack. </summary>
    public int Quantity
    {
      get { return _quantity; }
    }

    /// <summary> True if the stack contains no items. </summary>
    public bool IsEmpty
    {
      get { return Item == null || Quantity == 0; }
    }

    /// <summary> Add a given count to this quantity to create a new inventory count. </summary>
    /// <param name="count"> Number of items to add. </param>
    /// <returns> The new inventory stack representing the new quantity. </returns>
    public ItemStack Plus(int count)
    {
      int totalCount = Quantity + count;
      return new ItemStack(Item, totalCount);
    }

    /// <inheritdoc />
    public bool Equals(ItemStack other)
    {
      if (IsEmpty && other.IsEmpty)
        return true;

      return Equals(_item, other._item)
             && _quantity == other._quantity;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;

      return obj is ItemStack && Equals((ItemStack) obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      unchecked
      {
        return ((_item != null ? _item.GetHashCode() : 0) * 397) ^ _quantity;
      }
    }

    /// <summary> Addition operator. </summary>
    /// <param name="stack"> The stack. </param>
    /// <param name="count"> The quantity to add to the existing stack. </param>
    /// <returns> The result of the operation. </returns>
    /// <remarks>
    ///   Equivalent to <code>stack.Plus(count);</code>
    /// </remarks>
    public static ItemStack operator +(ItemStack stack, int count)
    {
      return stack.Plus(count);
    }
  }
}