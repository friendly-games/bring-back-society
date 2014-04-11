using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MonoBehaviourExtensions
{
  public static T Get<T>(this MonoBehaviour monoBehaviour)
    where T : class
  {
    return monoBehaviour.GetComponent(typeof (T)) as T;
  }

  public static T Get<T>(this GameObject gameObject)
    where T : class
  {
    return gameObject.GetComponent(typeof (T)) as T;
  }
}