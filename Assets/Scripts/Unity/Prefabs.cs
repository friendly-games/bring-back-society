using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary> Utility methods for getting prefabs. </summary>
public static class Prefabs
{
  /// <summary> Get the prefab with the designated name </summary>
  /// <param name="name"> The name of the prefab to get. </param>
  /// <returns> The prefab. </returns>
  public static GameObject GetPrefab(string name)
  {
    return (GameObject) Resources.Load("Prefabs/" + name);
  }

  /// <summary> Clone a prefab object and return a new instance of it.  </summary>
  /// <param name="gameObject"> The prefab to clone. </param>
  /// <param name="position"> The position of the new object. </param>
  /// <returns> The copy of the prefab. </returns>
  public static GameObject Clone(this GameObject gameObject, Vector3 position)
  {
    return (GameObject) Object.Instantiate(gameObject, position, Quaternion.identity);
  }
}