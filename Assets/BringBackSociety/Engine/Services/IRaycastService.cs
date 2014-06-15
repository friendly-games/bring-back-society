using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;
using UnityEngine;

namespace BringBackSociety.Services
{
  /// <summary> Performs raycasting. </summary>
  internal interface IRaycastService
  {
    /// <summary> Ray cast from the given position and direction, until an object is hit </summary>
    /// <typeparam name="T"> The type of instance to raycast for. </typeparam>
    /// <param name="ray"> The starting position and direction of the ray cast. </param>
    /// <param name="maxDistance"> The maximum distance that the raycast can travel. </param>
    /// <returns> The instance hit or null if that type of instance could not be hit. </returns>
    T Raycast<T>(Ray ray, float maxDistance)
      where T : class, IComponent;

    /// <summary> Ray cast from the given position and direction, until an object is hit. </summary>
    /// <typeparam name="T"> The type of instance to raycast for. </typeparam>
    /// <param name="ray"> The starting position and direction of the ray cast. </param>
    /// <param name="maxDistance"> The maximum distance that the raycast can travel. </param>
    /// <param name="distance"> [out] The distance to the object that was hit. </param>
    /// <returns> The instance hit or null if that type of instance could not be hit. </returns>
    T Raycast<T>(Ray ray, float maxDistance, out float distance)
      where T : class, IComponent;
  }
}