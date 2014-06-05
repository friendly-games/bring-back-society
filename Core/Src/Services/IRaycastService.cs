using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;
using BringBackSociety.Maths;

namespace BringBackSociety.Services
{
  /// <summary> Performs raycasting. </summary>
  internal interface IRaycastService
  {
    /// <summary> Ray cast from the given actor, until an object is hit </summary>
    /// <typeparam name="T"> The type of instance to raycast for. </typeparam>
    /// <param name="player"> The actor for which the ray cast should be started. </param>
    /// <param name="maxDistance"> The maximum distance that the raycast can travel. </param>
    /// <returns> The instance hit or null if that type of instance could not be hit. </returns>
    T Raycast<T>(IPlayer player, float maxDistance)
      where T : class, IComponent;

    /// <summary> Ray cast from the given position and direction, until an object is hit </summary>
    /// <typeparam name="T"> The type of instance to raycast for. </typeparam>
    /// <param name="ray"> The starting position and direction of the ray cast. </param>
    /// <param name="maxDistance"> The maximum distance that the raycast can travel. </param>
    /// <returns> The instance hit or null if that type of instance could not be hit. </returns>
    T Raycast<T>(ARay ray, float maxDistance)
      where T : class, IComponent;
  }
}