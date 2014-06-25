using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;

namespace BringBackSociety.Engine.System
{
  /// <summary> A wall which is located at a specific tile. </summary>
  internal class TileWall : IThing, IHpHolder, ICanBeDestroyed, IResist
  {
    private TileReference _reference;

    /// <summary> Constructor. </summary>
    /// <param name="reference"> The reference to the tile. </param>
    public TileWall(TileReference reference)
    {
      _reference = reference;
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
      get { return _reference.Value.WallData.Health; }
      set
      {
        var current = Health;
        if (value == current)
          return;

        var data = _reference.Value;
        data.WallData.Health = (byte) Math.Max(0, value);
        _reference.Value = data;
      }
    }

    /// <inheritdoc />
    public void MarkDestroyed()
    {
      _reference.Value = Tile.Empty;
    }
  }
}