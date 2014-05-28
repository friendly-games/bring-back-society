using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace BringBackSociety.Items.Weapons
{
  /// <summary> An instance of a weapon that can be fired. </summary>
  internal class FireableWeapon : IItemModel
  {
    private readonly IFireableWeaponModel _model;

    /// <summary> Constructor. </summary>
    /// <param name="model"> The general type of firable weapon that this weapon is base on. </param>
    public FireableWeapon(IFireableWeaponModel model)
    {
      if (model == null)
        throw new ArgumentNullException("model");

      _model = model;
    }

    /// <inheritdoc />
    public string Name
    {
      get { return _model.Name; }
    }

    /// <inheritdoc />
    public int StackAmount
    {
      // we can only ever hold 1 weapon in a slot at a time
      get { return 1; }
    }

    /// <inheritdoc />
    public IUiResource Resource
    {
      get { return _model.Resource; }
    }

    /// <summary> The number of shots in each clip. </summary>
    public int ClipSize
    {
      get { return _model.ClipSize; }
    }

    /// <summary> How much damage each shot does to an object. </summary>
    public int DamagePerShot
    {
      get { return _model.DamagePerShot; }
    }

    /// <summary> The type of ammo that the weapon uses. </summary>
    public AmmoType AmmoType
    {
      get { return _model.AmmoType; }
    }

    /// <summary> The maximum distance the weapon can be fired. </summary>
    public float MaxDistance
    {
      get { return _model.MaxDistance; }
    }

    /// <summary> The number of shots remaining until the clip needs to be refilled. </summary>
    public int ShotsRemaining { get; set; }

    /// <summary> Get the model associated with this weapon. </summary>
    /// <returns> The base model which serves as the base for the weapon. </returns>
    public IFireableWeaponModel GetModel()
    {
      return _model;
    }
  }
}