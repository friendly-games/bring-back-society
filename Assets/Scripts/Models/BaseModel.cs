using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;
using BringBackSociety.ViewModels;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Models
{
  /// <summary> Provides the base implementation for a model. </summary>
  /// <typeparam name="TBehavior"> The type of behavior that owns this model. </typeparam>
  /// <typeparam name="TInstance"> The interface implementation of the model. </typeparam>
  public abstract class BaseModel<TBehavior, TInstance> : ICopyable<TInstance>, IModel
    where TBehavior : Component, IModelProvider<TInstance>
  {
    /// <summary> The top-level owner of the behavior/instance.  </summary>
    protected readonly GameObject Owner;

    /// <summary> The behavior that created the instance. </summary>
    protected readonly TBehavior Behavoir;

    /// <summary> Specialised constructor for use only by derived classes. </summary>
    /// <param name="owner"> The top-level owner of the behavior/instance. </param>
    /// <param name="behavoir"> The behavior that created the instance. </param>
    protected BaseModel(GameObject owner, TBehavior behavoir)
    {
      Owner = owner;
      Behavoir = behavoir;
    }

    /// <inheritdoc />
    public TInstance Copy()
    {
      var newOwner = (GameObject) Object.Instantiate(Owner);
      return newOwner.GetComponent<TBehavior>().ModelImplementation;
    }

    /// <inheritdoc />
    object ICopyable.Copy()
    {
      return Copy();
    }

    /// <inheritdoc />
    public void Destroy()
    {
      Object.Destroy(Owner);
    }
  }
}