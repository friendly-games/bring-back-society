using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Services
{
  /// <summary> Generates perlin noise. </summary>
  public interface IPerlinNoise
  {
    /// <summary> The current seed for the perlin noise generator. </summary>
    long Seed { get; }

    /// <summary> Retrieve the noise at the given coordinate. </summary>
    /// <param name="x"> The x coordinate. </param>
    /// <param name="y"> The y coordinate. </param>
    /// <returns> A float representing the noise at the given location. </returns>
    float Noise(float x, float y);
  }
}