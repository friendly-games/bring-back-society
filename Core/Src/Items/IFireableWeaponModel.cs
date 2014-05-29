using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items.Weapons;
using BringBackSociety.Services;

namespace BringBackSociety.Items
{
  /// <summary> A single weapon which can be fired against an enemy. </summary>
  public interface IFireableWeaponModel : IDisplayableItem, INamedItem
  {
    /// <summary> The amount of damage done per bullet hit. </summary>
    int DamagePerShot { get; }

    /// <summary> The number of bullets in each clip. </summary>
    int ClipSize { get; }

    /// <summary> The maximum distance the weapon can be fired </summary>
    float MaxDistance { get; }

    /// <summary> The type of ammo that the weapon uses. </summary>
    AmmoType AmmoType { get; }
  }

  internal interface IFireableWeaponTemplate : ICopyable<IFireableWeaponTemplate>, IDisplayableItem
  {
    /// <summary> The stats associated with the weapon. </summary>
    FireableWeaponStats Stats { get; }

    /// <summary> The view associated with the weapon. </summary>
    IFireableWeaponView View { get; }
  }

  internal class FireableWeaponTemplate
  {
    /// <summary> Constructor. </summary>
    /// <param name="view"> The ui component associated with the item. </param>
    /// <param name="stats"> The damage stats for the item. </param>
    /// <param name="displayItem"> Info about the weapon that is passed to the ui. </param>
    public FireableWeaponTemplate(IFireableWeaponView view,
                                  FireableWeaponStats stats,
                                  IDisplayableItem displayItem)
    {
      View = view;
      Stats = stats;
      DisplayItem = displayItem;
    }

    /// <summary> The ui component associated with the item. </summary>
    public IFireableWeaponView View { get; private set; }

    /// <summary> The damage stats for the item. </summary>
    public FireableWeaponStats Stats { get; private set; }

    /// <summary> Info about the weapon that is passed to the ui. </summary>
    public IDisplayableItem DisplayItem { get; private set; }

    /// <summary> Creates a new instance of the object. </summary>
    /// <returns> A copy of the given object. </returns>
    public FireableWeaponTemplate Copy()
    {
      return new FireableWeaponTemplate(View.Copy(), Stats, DisplayItem);
    }
  }

  /// <summary> All of the states that can be activated for a fireable weapon. </summary>
  internal enum FireableWeaponState
  {
    /// <summary> The empty state. </summary>
    None,

    /// <summary> The weapon can be fired. </summary>
    Fired,

    /// <summary> The weapon can be reloaded. </summary>
    Reloaded,

    /// <summary> The weapon can be put away. </summary>
    PutAway,

    /// <summary> The weapon can be taken out. </summary>
    TakeOut,
  }

  /// <summary> A UI component for the fireable weapon. </summary>
  internal interface IFireableWeaponView : ICopyable<IFireableWeaponView>
  {
    /// <summary> Determine if we can enter the designated state. </summary>
    /// <param name="state"> The state that might be entered. </param>
    /// <returns> true if we can enter state, false if not. </returns>
    bool CanEnterState(FireableWeaponState state);

    /// <summary> Transition the weapon into the given state. </summary>
    /// <param name="state"> The state to enter. </param>
    void TransitionToState(FireableWeaponState state);

    /// <summary> Attach the weapon to the given actor. </summary>
    /// <param name="player"> The player. </param>
    void AttachTo(IPlayer player);
  }

  /// <summary> Stats about a fireable weapon. </summary>
  public struct FireableWeaponStats
  {
    /// <summary> The amount of damage done per bullet hit. </summary>
    public int DamagePerShot;

    /// <summary> The number of bullets in each clip. </summary>
    public int ClipSize;

    /// <summary> The maximum distance the weapon can be fired </summary>
    public float MaxDistance;

    /// <summary> The type of ammo that the weapon uses. </summary>
    public AmmoType AmmoType;
  }
}