﻿using System;
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
      return currentValue.Instance;
    }

    return default(T);
  }

  /// <summary>
  ///  Retrieve the instance of the given interface/object by walking up the chain until it is found.
  /// </summary>
  /// <typeparam name="T"> The interface type to retrieve. </typeparam>
  /// <param name="self"> The game object at which to begin the search. </param>
  /// <returns> An instance of the given interface/object, or null if it could not be found. </returns>
  public static T RetrieveInHierarchy<T>(this GameObject self)
    where T : class
  {
    var instance = self.RetrieveOwnObject<T>();

    while (instance == null)
    {
      self = self.GetParent();

      // we've reached the end, how sad
      if (self == null)
        break;

      instance = self.RetrieveOwnObject<T>();
    }

    return instance;
  }

  /// <summary>
  ///  Get the game object associated with the game object of the parent of this game object.
  /// </summary>
  public static GameObject GetParent(this GameObject child)
  {
    var parent = child.transform.parent;

    return parent == null ? null : child.transform.parent.gameObject;
  }

  /// <summary>
  ///  Get the game object associated with the game object of the parent of this game object.
  /// </summary>
  /// <param name="child"> The child to set the parent of. </param>
  /// <param name="parent"> The parent to set as the child's parent. </param>
  public static void SetParent(this GameObject child, GameObject parent)
  {
    child.transform.parent = parent.transform;
  }
}