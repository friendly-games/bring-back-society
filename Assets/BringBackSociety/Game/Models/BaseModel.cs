using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;
using BringBackSociety.ViewModels;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Models
{
  /// <summary> Provides the base, non-generic, implementation for a model. </summary>
  public abstract class BaseModel
  {
    /// <summary> The top-level owner of the behavior/instance.  </summary>
    protected internal readonly GameObject Owner;

    /// <summary> Specialised constructor for use only by derived classes. </summary>
    /// <param name="owner"> The top-level owner of the behavior/instance. </param>
    protected BaseModel(GameObject owner)
    {
      Owner = owner;
    }
  }

  /// <summary> Provides the base implementation for a model. </summary>
  /// <typeparam name="TBehavior"> The type of behavior that owns this model. </typeparam>
  /// <typeparam name="TInstance"> The interface implementation of the model. </typeparam>
  public abstract class BaseModel<TBehavior, TInstance> : BaseModel, ICopyable<TInstance>, IModel
    where TBehavior : Component, IModelProvider<TInstance>
  {
    /// <summary> The behavior that created the instance. </summary>
    protected internal readonly TBehavior Behavoir;

    /// <summary> Specialised constructor for use only by derived classes. </summary>
    /// <param name="owner"> The top-level owner of the behavior/instance. </param>
    /// <param name="behavoir"> The behavior that created the instance. </param>
    protected BaseModel(GameObject owner, TBehavior behavoir)
      : base(owner)
    {
      Behavoir = behavoir;
    }

    /// <inheritdoc />
    public TInstance Copy()
    {
      var newOwner = (GameObject) Object.Instantiate(Owner);
      var newModel = newOwner.GetComponent<TBehavior>().ModelImplementation;

      if (newModel == null)
      {
        Logging.Log.FatalFormat("Object that was cloned has null model.  {0} => {1}", newOwner, newModel);
      }

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