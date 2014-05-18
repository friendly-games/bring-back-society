using System;
using System.Collections.Generic;
using System.Linq;
using Behavior;
using BringBackSociety.Items;
using UnityEngine;

public static class MonoBehaviourExtensions
{
  /// <summary>
  ///  Get the component of the specified type from the game object, without attempting to go
  ///  through the parent component.
  /// </summary>
  public static T GetViaInstance<T>(this GameObject gameObject)
    where T : class
  {
    return gameObject.GetComponent(typeof (T)) as T;
  }

  public static IComponent Get(this GameObject gameObject)
  {
    var instance = gameObject.GetViaInstance<IComponent>();

    // if it wasn't found on the game object itself
    if (instance == null)
    {
      // and it has a parent
      var parent = gameObject.GetParent();
      if (parent != null)
      {
        // that is a provider for the child
        var provider = parent.GetViaInstance<IProviderParent>();
        if (provider != null && provider.IsApplicable(gameObject))
        {
          // then use that instance
          provider.With(gameObject);
          instance = provider.Component;
        }
      }
    }

    return instance;
  }

  /// <summary>
  ///  Get the component of the specified type from the game object, or attempt to get the the
  ///  component via it's parent component.
  /// </summary>
  public static T Get<T>(this GameObject gameObject)
    where T : class, IComponent
  {
    return Get(gameObject) as T;
  }

  /// <summary>
  ///  Get the game object associated with the game object of the parent of this game object.
  /// </summary>
  public static GameObject GetParent(this GameObject child)
  {
    var parent = child.transform.parent;

    return parent == null ? null : child.transform.parent.gameObject;
  }
}