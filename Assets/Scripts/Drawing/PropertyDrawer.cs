using System;
using System.Collections.Generic;
using System.Linq;
using Behavior;
using BringBackSociety;
using UnityEngine;

namespace Drawing
{
  /// <summary> Draws the properties of an item in the upper left. </summary>
  public class PropertyDrawer
  {
    private const int LineHeight = 18;
    private readonly List<KeyValuePair<string, string>> _items;

    /// <summary> Draws the properties of a component in the upper left of the screen. </summary>
    public PropertyDrawer()
    {
      _items = new List<KeyValuePair<string, string>>();
    }

    /// <summary> Use the details from this component for drawing purposes. </summary>
    /// <param name="component"> The component whose information should be processed. </param>
    public void With(IComponent component)
    {
      _items.Clear();

      var name = component as INamed;
      if (name != null)
      {
        AddItem("Name:", name.Name);
      }

      var destroyable = component as IDestroyable;
      if (destroyable != null)
      {
        AddItem("Health:", destroyable.Health.ToString());
      }

      var tileItem = component as ITileItem;
      if (tileItem != null)
      {
        AddItem("Chunk:", tileItem.TileCoordinate.ToString());
        AddItem("Tile:", tileItem.ChunkCoordinate.ToString());
        AddItem("World:", new WorldPosition(tileItem.ChunkCoordinate, tileItem.TileCoordinate).ToString());
      }
    }

    /// <summary> Add a key value pair to be drawn. </summary>
    /// <param name="name"> The name of the item to draw. </param>
    /// <param name="value"> The value of the item to draw. </param>
    private void AddItem(string name, string value)
    {
      _items.Add(new KeyValuePair<string, string>(name, value));
    }

    /// <summary> Draw the actual properties associated with the current item. </summary>
    public void Draw()
    {
      int line = 30;

      GUI.Box(new Rect(10, 10, 120, LineHeight * (_items.Count + 1) + 10), "Current Item");

      foreach (var kvp in _items)
      {
        GUI.Label(new Rect(20, line, 50, 20), kvp.Key);
        GUI.Label(new Rect(72, line, 50, 20), kvp.Value);
        line += LineHeight;
      }
    }
  }
}