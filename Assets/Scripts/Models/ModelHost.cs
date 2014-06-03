using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.ViewModels;
using log4net;
using UnityEngine;

namespace Models
{
  /// <summary> An object which can hold a model and display it in the world. </summary>
  internal class ModelHost<T> : IModelHost<T>
    where T : class, IModel
  {
    /// <summary> Provides logging for the class. </summary>
    private static readonly ILog Log = LogManager.GetLogger(typeof(ModelHost<T>));

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

    /// <summary> The current model being used in the host. </summary>
    public T CurrentModel { get; private set; }

    /// <inheritdoc />
    void IModelHost<T>.Use(T model)
    {
      if (model == null)
        throw new ArgumentNullException("model");

      // we always know that T is going to a BaseModel
      var baseModel = model as BaseModel;
      if (baseModel == null)
      {
        Log.FatalFormat("BaseModel was null.  Model is {0}", model);
        return;
      }

      var gameObject = baseModel.Owner;
      gameObject.SetParent(_owner);
      gameObject.transform.localPosition = _offset;
      gameObject.transform.localRotation = Quaternion.identity;

      CurrentModel = model;
    }
  }
}