using System;
using System.Collections.Generic;
using System.Linq;
using Behavior;
using BringBackSociety;
using BringBackSociety.Engine.System;
using BringBackSociety.Items;
using UnityEngine;

namespace Drawing
{
  /// <summary> Draws the properties of an item in the upper left. </summary>
  public class PropertyDrawer
  {
    private readonly string _titleText;
    private readonly int _xLoc;
    private readonly int _yLoc;
    private const int LineHeight = 18;
    private readonly List<KeyValuePair<string, string>> _items;

    /// <summary> Draws the properties of a component in the upper left of the screen. </summary>
    public PropertyDrawer(string titleText, int xLoc, int yLoc)
    {
      _titleText = titleText;
      _xLoc = xLoc;
      _yLoc = yLoc;
      _items = new List<KeyValuePair<string, string>>();
    }

    /// <summary> Use the details from this component for drawing purposes. </summary>
    /// <param name="thing"> The component whose information should be processed. </param>
    internal void Add(IThing thing)
    {
      Start();

      AddItem("Name:", thing.ToString());

      var hpHolder = thing as IHpHolder;
      if (hpHolder != null)
      {
        AddItem("Health:", hpHolder.Health.ToString());
      }
    }

    /// <summary> Start drawing a new scene. </summary>
    public void Start()
    {
      _items.Clear();
    }

    /// <summary> Add a key value pair to be drawn. </summary>
    /// <param name="name"> The name of the item to draw. </param>
    /// <param name="value"> The value of the item to draw. </param>
    public void AddItem(string name, string value)
    {
      _items.Add(new KeyValuePair<string, string>(name, value));
    }

    /// <summary> Draw the actual properties associated with the current item. </summary>
    public void Draw()
    {
      int height = LineHeight * (_items.Count + 1) + 10;
      GUILayout.BeginArea(new Rect(_xLoc, _yLoc, 120, height));
      GUI.Box(new Rect(0, 0, 120, height), _titleText);

      int line = 18;

      foreach (var kvp in _items)
      {
        GUI.Label(new Rect(10, line, 50, 20), kvp.Key);
        GUI.Label(new Rect(60, line, 50, 20), kvp.Value);
        line += LineHeight;
      }
      GUILayout.EndArea();
    }
  }
}