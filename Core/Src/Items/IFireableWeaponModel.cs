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

  /// <summary> Instance template for a fireable weapon. </summary>
  internal struct FireableWeaponTemplate
  {
    private readonly IFireableWeaponModel _model;
    private readonly IUiResource _resource;
    private readonly FireableWeaponStats _stats;

    public FireableWeaponTemplate(IFireableWeaponModel model, IUiResource resource, FireableWeaponStats stats)
    {
      _stats = stats;
      _resource = resource;
      _model = model;
    }

    /// <summary> The model for the template. </summary>
    public IFireableWeaponModel Model
    {
      get { return _model; }
    }

    /// <summary> The UI Resource associated with the firable weapon. </summary>
    public IUiResource Resource
    {
      get { return _resource; }
    }

    /// <summary> The stats for the weapon. </summary>
    public FireableWeaponStats Stats
    {
      get { return _stats; }
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
}