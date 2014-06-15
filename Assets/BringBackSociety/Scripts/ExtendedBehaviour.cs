using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;

namespace BringBackSociety.Scripts
{
  /// <summary> Custom implementation of a mono behavior. </summary>
  public class ExtendedBehaviour : MonoBehaviour
  {
    public static TInstance Copy<TModel, TInstance>(GameObject owner, Func<TModel, TInstance> getter)
      where TModel : Component
    {
      var newOwner = (GameObject) UnityEngine.Object.Instantiate(owner);
      return getter(newOwner.GetComponent<TModel>());
    }
  }

  internal class ModelProvidingBehavoir<T> : ExtendedBehaviour, IModelProvider<T>
  {
    private T _implementation;

    T IModelProvider<T>.ModelImplementation
    {
      get { return _implementation; }
    }

    protected void SetImplementation(T implementation)
    {
      if (implementation == null)
        throw new ArgumentNullException("implementation");

      _implementation = implementation;
    }
  }
}