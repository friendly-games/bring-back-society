using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;

namespace BringBackSociety.Controllers
{
  /// <summary> Contains a stack of inventory items. </summary>
  public struct InventoryStack : IEquatable<InventoryStack>
  {
    private readonly IItemModel _model;
    private readonly int _quantity;

    /// <summary> A stack which contains no model and no items. </summary>
    public static InventoryStack Empty = new InventoryStack(null, 0);

    /// <summary> Constructor. </summary>
    /// <param name="model"> The item which the stack represents. </param>
    /// <param name="quantity"> The quantity of items in the stack. </param>
    public InventoryStack(IItemModel model, int quantity)
    {
      if (quantity < 0)
        throw new ArgumentException("Inventory must be > 0", "quantity");

      _model = model;
      _quantity = quantity;
    }

    /// <summary> The item which the stack represents. </summary>
    public IItemModel Model
    {
      get { return _model; }
    }

    /// <summary> The quantity of items in the stack. </summary>
    public int Quantity
    {
      get { return _quantity; }
    }

    /// <summary> True if the stack contains no items. </summary>
    public bool IsEmpty
    {
      get { return Model == null || Quantity == 0; }
    }

    /// <summary>
    ///  The number of items which can be added to this stack without exceeding the model stack limit.
    ///  Only valid if IsEmpty is false.
    /// </summary>
    public int ExtraCapacity
    {
      get
      {
        if (IsEmpty)
          return 0;

        return Model.StackAmount - Quantity;
      }
    }

    /// <summary> Add a given count to this quantity to create a new inventory count. </summary>
    /// <param name="count"> Number of items to add. </param>
    /// <returns> The new inventory stack representing the new quantity. </returns>
    public InventoryStack Plus(int count)
    {
      int totalCount = Quantity + count;
      return new InventoryStack(Model, totalCount);
    }

    /// <inheritdoc />
    public bool Equals(InventoryStack other)
    {
      if (IsEmpty && other.IsEmpty)
        return true;

      return Equals(_model, other._model)
             && _quantity == other._quantity;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;

      return obj is InventoryStack && Equals((InventoryStack) obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      unchecked
      {
        return ((_model != null ? _model.GetHashCode() : 0) * 397) ^ _quantity;
      }
    }

    /// <inheritdoc />
    public override string ToString()
    {
      if (IsEmpty)
        return "<Empty>";

      return String.Format("{0}: {1}", Model.Name, Quantity);
    }
  }
}