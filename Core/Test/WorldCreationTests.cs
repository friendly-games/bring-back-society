using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BringBackSociety;
using BringBackSociety.Chunks.Generators;
using BringBackSociety.Chunks.Loaders;
using BringBackSociety.Services;
using Services;
using Xunit;

namespace Tests
{
  public class WorldCreationTests
  {
    [Fact]
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