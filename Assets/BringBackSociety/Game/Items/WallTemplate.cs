using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;

namespace BringBackSociety.Game.Items
{
  /// <summary> A template that represents a specific type of wall. </summary>
  public class WallTemplate
  {
    /// <summary> Constructor. </summary>
    /// <param name="name"> The name of the designated wall. </param>
    /// <param name="maxHealth"> The maximum amount of health that the wall can have. </param>
    /// <param name="resistance"> The resistance for the wall. </param>
    public WallTemplate(string name, int maxHealth, Resistance resistance)
    {
      if (name == null)
        throw new ArgumentNullException("name");
      if (resistance == null)
        throw new ArgumentNullException("resistance");

      Name = name;
      Resistance = resistance;
      MaxHealth = maxHealth;
    }

    /// <summary> The resistance for the wall. </summary>
    public Resistance Resistance { get; private set; }

    /// <summary> The maximum amount of health that the wall can have. </summary>
    public int MaxHealth { get; private set; }

    /// <summary> The name of the designated wall. </summary>
    public string Name { get; private set; }
  }
}