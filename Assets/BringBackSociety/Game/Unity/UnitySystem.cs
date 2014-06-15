using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using UnityEngine;

/// <summary> Provides unity specific utility methods. </summary>
internal class UnitySystem
{
  /// <summary> Retrieve the given model implementation from the game object with the specified name. </summary>
  /// <param name="name"> The name of the GameObject that contains the model provider. </param>
  /// <returns> The model implementation of type T, on the GameObject with the given name. </returns>
  public static T RetrieveModel<T>(string name)
  {
    return GameObject.Find(name)
                     .RetrieveComponent<IModelProvider<T>>()
                     .ModelImplementation;
  }
}