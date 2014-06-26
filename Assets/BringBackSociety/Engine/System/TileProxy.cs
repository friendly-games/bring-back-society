using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> A IThing which derives its data from a tile location. </summary>
  internal abstract class TileProxy : IThing, IProxy
  {
    private readonly IRecycler _recycler;

    /// <summary> The tile whose data should be retrieved. </summary>
    protected TileReference Reference { get; private set; }

    /// <summary> Constructor. </summary>
    protected TileProxy(IRecycler recycler)
    {
      _recycler = recycler;
    }

    /// <summary> Initializes the proxy object. </summary>
    /// <param name="reference"> The reference to the tile. </param>
    public void Initialize(TileReference reference)
    {
      Reference = reference;
    }

    void IDisposable.Dispose()
    {
      _recycler.Recycle(this);
    }

    /// <inheritdoc />
    public override string ToString()
    {
      return "Proxy for " + Reference;
    }
  }
}