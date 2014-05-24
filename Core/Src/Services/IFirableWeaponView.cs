using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;

namespace BringBackSociety.Services
{
  /// <summary> Performs the view tasks related to firing projectile weapons. </summary>
  public interface IFirableWeaponView
  {
    /// <summary> Start the effects for firing the designated weapon </summary>
    /// <param name="player"> The actor who fired the weapon. </param>
    /// <param name="weapon"> The weapon that was fired. </param>
    void FireWeapon(IPlayer player, IFireableWeaponModel weapon);
  }
}