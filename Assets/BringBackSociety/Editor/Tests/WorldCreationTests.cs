using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety;
using BringBackSociety.Engine;
using BringBackSociety.Services;
using NUnit.Framework;

namespace Tests
{
  public class WorldCreationTests
  {
    [Test]
    [Description("Verify world creation is successful")]
    public void Verify_world_creation_is_successful()
    {
      IChunkLoader fakeLoader = new SimpleChunkLoader(new PerlinChunkGenerator(new FakeNoise()));
      var world = new World(fakeLoader);

      Assert.True(world.Chunks.All(c => c != null));
    }

    public class FakeNoise : IPerlinNoise
    {
      public long Seed
      {
        get { return 38; }
      }

      public float Noise(float x, float y)
      {
        return (float) (x + y * Math.Cos(x) * Math.Sin(y));
      }
    }
  }
}