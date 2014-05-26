using System;
using System.Collections.Generic;
using System.Linq;
using Behavior;
using BringBackSociety.Items;
using log4net;
using UnityEngine;

public static class MonoBehaviourExtensions
{
  /// <summary>
  ///  Get the component of the specified type from the game object, without attempting to go
  ///  through the parent component.
  /// </summary>
  public static T RetrieveOwnObject<T>(this GameObject gameObject)
    where T : class
  {
    return gameObject.GetComponent(typeof(T)) as T;
  }

  public static IComponent RetrieveComponent(this GameObject gameObject)
  {
    var instance = gameObject.RetrieveOwnObject<IComponent>();

    // if it wasn't found on the game object itself
    if (instance == null)
    {
      // and it has a parent
      var parent = gameObject.GetParent();
      if (parent != null)
      {
        // that is a provider for the child
        var provider = parent.RetrieveOwnObject<IParentProvider>();
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
  public static T RetrieveComponent<T>(this GameObject gameObject)
    where T : class, IComponent
  {
    return RetrieveComponent(gameObject) as T;
  }

  /// <summary> Retrieve an object provided by a game object in the hierarchy. </summary>
  /// <typeparam name="T"> The type of object to retrieve. </typeparam>
  /// <param name="gameObject"> The gameObject where the hierarchal search should begin for the
  ///  object. </param>
  /// <returns> A object found, or null if the object could not be found. </returns>
  public static T RetrieveObject<T>(this GameObject gameObject)
    where T : class
  {
    IObjectProvider<T> currentValue;
    GameObject currentGameObject = gameObject;

    do
    {
      // try to retrieve it from ourselves
      currentValue = currentGameObject.RetrieveOwnObject<IObjectProvider<T>>();

      // if that doesn't work, set our self to our parent
      if (currentValue == null)
      {
        currentGameObject = currentGameObject.GetParent();
      }

      // and keep going until we have no parent or until we have a value
    } while (currentValue == null && currentGameObject != null);

    if (currentValue != null)
    {
      return currentValue.Component;
    }

    return null;
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