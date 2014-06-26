using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine
{
  /// <summary> Allocates a number of things which can be reused later on. </summary>
  /// <typeparam name="T"> Generic type parameter. </typeparam>
  internal class ResourcePool<T> : IRecycler
  {
    private readonly Func<T> _factory;
    private readonly int _maxItems;

    private readonly Queue<T> _items;

    /// <summary> Default constructor. </summary>
    /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
    ///  illegal values. </exception>
    /// <param name="factory"> The factory to use to construct new items. </param>
    /// <param name="maxItems"> (Optional) the maximum number of items that can be stored before
    ///  releasing newer items. </param>
    public ResourcePool(Func<T> factory, int maxItems = int.MaxValue)
    {
      if (factory == null)
        throw new ArgumentNullException("factory");
      if (maxItems <= 0)
        throw new ArgumentException("maxItems must be > 0", "maxItems");

      _factory = factory;
      _maxItems = maxItems;
      _items = new Queue<T>();
    }

    /// <summary> Gets an instance of T. </summary>
    /// <returns> An instance of T. </returns>
    public T Get()
    {
      if (_items.Count > 0)
      {
        return _items.Dequeue();
      }

      return _factory();
    }

    /// <summary> Puts back an instance of T </summary>
    /// <param name="item"> The item to put back. </param>
    public void Put(T item)
    {
      // only add it if we haven't gone over the max items
      if (_items.Count >= _maxItems)
        return;

      _items.Enqueue(item);
    }

    /// <inheritdoc />
    void IRecycler.Recycle(object instance)
    {
      if (instance is T)
      {
        Put((T)instance);
      }
    }
  }
}