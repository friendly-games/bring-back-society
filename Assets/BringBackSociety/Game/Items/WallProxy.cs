using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;

namespace BringBackSociety.Game.Items
{
  /// <summary> A proxy for a game object that is destroyable. </summary>
  public class WallProxy : IDestroyable, INamed
  {
    private TileReference _tileReference;
    private WallTemplate _template;

    /// <summary> Have the proxy use the specified data. </summary>
    /// <param name="tileReference"> The reference to the tile being proxied. </param>
    /// <param name="template"> The template for the type of wall to use. </param>
    public void Initialize(TileReference tileReference, WallTemplate template)
    {
      _tileReference = tileReference;
      _template = template;
    }

    /// <inheritdoc />
    public string Name
    {
      get { return _template.Name; }
    }

    /// <inheritdoc />
    public Resistance Resistance
    {
      get { return _template.Resistance; }
    }

    /// <inheritdoc />
    public int MaxHealth
    {
      get { return _template.MaxHealth; }
    }

    /// <inheritdoc />
    public int Health
    {
      get { return _tileReference.Value.WallData.Health; }
      set
      {
        var current = Health;
        if (value == current)
          return;

        var data = _tileReference.Value;
        data.WallData.Health = (byte) Math.Max(0, value);
        _tileReference.Value = data;
      }
    }

    /// <inheritdoc />
    public void Destroy()
    {
      _tileReference.Value = Tile.Empty;
    }
  }
}