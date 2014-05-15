using System;
using System.Collections.Generic;
using System.Linq;
using Behavior;
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

  /// <summary>
  ///  Get the component of the specified type from the game object, or attempt to get the the
  ///  component via it's parent component.
  /// </summary>
  public static T Get<T>(this GameObject gameObject)
    where T : class
  {
    var instance = gameObject.GetViaInstance<T>();

    // if it wasn't found on the game object itself
    if (instance == null)
    {
      // and it has a parent
      var parent = gameObject.GetParent();
      if (parent != null)
      {
        // that is a provider for the child
        var provider = parent.GetViaInstance<IProviderParent<T>>();
        if (provider != null && provider.IsApplicable(gameObject))
        {
          // then use that instance
          provider.With(gameObject);
          instance = provider.Implementation;
        }
      }
    }

    return instance;
  }

  /// <summary>
  ///  Get the game object associated with the game object of the parent of this game object.
  /// </summary>
  public static GameObject GetParent(this GameObject child)
  {
    return child.transform.parent.gameObject;
  }
}