using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Services
{
  /// <summary> Generates random numbers. </summary>
  internal interface IRandomNumberGenerator
  {
    /// <summary> Generate the next float between min and max </summary>
    /// <param name="min"> The minimum value that the value can take. </param>
    /// <param name="max"> The maximum value that the value can take. </param>
    /// <returns> A float between min and max. </returns>
    float NextFloat(float min = 0.0f, float max = 0.0f);
  }
}