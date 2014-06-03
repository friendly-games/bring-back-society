using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items.Weapons;
using BringBackSociety.ViewModels;

namespace BringBackSociety.Items
{
  /// <summary> A UI component for the fireable weapon. </summary>
  internal interface IFireableWeaponModel : IModel, ICopyable<IFireableWeaponModel>
  {
    /// <summary> Determine if we can enter the designated state. </summary>
    /// <param name="state"> The state that might be entered. </param>
    /// <returns> true if we can enter state, false if not. </returns>
    bool CanEnterState(FireableWeaponState state);

    /// <summary> Transition the weapon into the given state. </summary>
    /// <param name="state"> The state to enter. </param>
    void TransitionToState(FireableWeaponState state);

    /// <summary> The fireable weapon instance which is associated with this model. </summary>
    FireableWeapon FireableWeapon { get; set; }
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
}