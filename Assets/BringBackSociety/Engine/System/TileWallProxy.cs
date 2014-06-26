using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;

namespace BringBackSociety.Engine.System
{
  /// <summary> A wall which is located at a specific tile. </summary>
  internal class TileWallProxy : TileProxy, IHpHolder, ICanBeDestroyed, IResist
  {
    /// <summary> Constructor. </summary>
    public TileWallProxy(IRecycler recycler)
      : base(recycler)
    {
    }

    /// <inheritdoc />
    public Resistance Resistance
    {
      get
      {
        // TODO use lookup table
        return new Resistance()
               {
                 BulletResistance = 3
               };
      }
    }

    /// <inheritdoc />
    public int MaxHealth
    {
      // TODO use lookup table
      get { return 99; }
    }

    /// <inheritdoc />
    public int Health
    {
      get { return Reference.GetValue().WallData.Health; }
      set
      {
        var current = Health;
        if (value == current)
          return;

        var data = Reference.GetValue();
        data.WallData.Health = (byte) Math.Max(0, value);
        Reference.SetValue(data);
      }
    }

    /// <inheritdoc />
    public void MarkDestroyed()
    {
      Reference.SetValue(Tile.Empty);
    }

    /// <inheritdoc />
    public override string ToString()
    {
      return "Wall @ " + Reference;
    }
  }
}