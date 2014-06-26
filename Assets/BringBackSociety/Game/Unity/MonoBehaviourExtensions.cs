using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Engine.System;
using BringBackSociety.Game.System;
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

  /// <summary>
  ///  Get the component of the specified type from the game object, or attempt to get the the
  ///  component via it's parent component.
  /// </summary>
  public static T RetrieveComponent<T>(this GameObject gameObject)
    where T : class, IComponent
  {
    var instance = gameObject.RetrieveOwnObject<IComponent>();
    return instance as T;
  }

  /// <summary>
  ///  Retrieve the thing associated with the game object, or attempt to retrieve a proxy thing from
  ///  the game object's parent.
  /// </summary>
  /// <param name="gameObject"> The gameObject where the hierarchal search should begin for the
  ///  object. </param>
  /// <returns> An IThing. </returns>
  internal static IThing RetrieveThing(this GameObject gameObject)
  {
    IThing thing;

    // if the game object is itself a thing, return it directly
    thing = gameObject.RetrieveOwnObject<IThing>();
    if (thing != null)
      return thing;

    // otherwise try to get its parent
    var parent = gameObject.GetParent();
    if (parent == null)
      return null;

    // and make sure it's a candidate to return a proxy for this child
    var proxyParent = parent.RetrieveOwnObject<IProxyParent>();
    if (proxyParent == null)
      return null;

    // and return that proxy
    return proxyParent.RetrieveProxyFor(gameObject);
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