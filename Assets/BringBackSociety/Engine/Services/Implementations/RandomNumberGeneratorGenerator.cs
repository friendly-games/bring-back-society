using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Services
{
  /// <summary> Generates random numbers from the System.Random class. </summary>
  internal class RandomNumberGeneratorGenerator : IRandomNumberGenerator
  {
    private readonly Random _random;

    /// <summary> Constructor. </summary>
    /// <param name="random"> (Optional) The random class to use, or null to use a newly generated
    ///  Random class. </param>
    public RandomNumberGeneratorGenerator(Random random = null)
    {
      _random = random ?? new Random();
    }

    /// <inheritdoc />
    float IRandomNumberGenerator.NextFloat(float minimum, float maximum)
    {
      return (float) _random.NextDouble() * (maximum - minimum) + minimum;
    }
  }
}