﻿using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Extensions;
using BringBackSociety.Services;

namespace BringBackSociety.Engine
{
  /// <summary> Generates new chunks using perlin noise. </summary>
  public class PerlinChunkGenerator : IChunkGenerator
  {
    private readonly IPerlinNoise _noise;

    /// <summary> Constructor. </summary>
    /// <param name="noise"> The noise generator to use.. </param>
    public PerlinChunkGenerator(IPerlinNoise noise)
    {
      if (noise == null)
        throw new ArgumentNullException("noise");

      _noise = noise;
    }

    /// <inheritdoc/>
    Chunk IChunkGenerator.Generate(ChunkCoordinate location)
    {
      // TODO remove short circuit
      if (location.ToWorldPosition().ToVector3().magnitude > 100)
        return new Chunk(location);

      var world = location.ToWorldPosition();
      var chunk = new Chunk(location);
      for (int x = 0; x < Chunk.Length; x++)
        for (int z = 0; z < Chunk.Length; z++)
        {
          var noise = _noise.Noise(world.X + x, world.Z + z);
          chunk[new TileCoordinate(x, z)] = new Tile
                                            {
                                              Type = (byte) (noise > 0.5f ? 1 : 0),
                                              WallData =
                                              {
                                                Health = (byte) (noise * byte.MaxValue)
                                              }
                                            };
        }
      return chunk;
    }
  }
}