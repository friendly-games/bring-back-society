using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.ViewModels;
using UnityEngine;

namespace Models
{
  /// <summary> An object which can hold a model and display it in the world. </summary>
  internal class ModelHost : IModelHost
  {
    /// <summary> The game object that shall be the parent of the model. </summary>
    private readonly GameObject _owner;

    /// <summary> The offset, in local space, that the model should have from the owner. </summary>
    private readonly Vector3 _offset;

    /// <summary> Constructor. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="owner"> The game object that shall be the parent of the model. </param>
    /// <param name="offset"> The offset, in local space, that the model should have from the owner. </param>
    public ModelHost(GameObject owner, Vector3 offset)
    {
      if (owner == null)
        throw new ArgumentNullException("owner");

      _owner = owner;
      _offset = offset;
    }

    /// <inheritdoc />
    void IModelHost.Use(IModel model)
    {
      var baseModel = (BaseModel) model;
      var gameObject = baseModel.Owner;

      gameObject.SetParent(_owner, _offset);
    }
  }
}