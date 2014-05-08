using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Chunks.Loaders;
using BringBackSociety.Services;

namespace BringBackSociety.Chunks.Generators
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
      var chunk = new Chunk(location);
      for (int x = 0; x < Chunk.Length; x++)
        for (int z = 0; z < Chunk.Length; z++)
        {
          var noise = _noise.Noise(x, z);
          chunk.Tiles[new TileCoordinate(x, z).Index] = new Tile
                                                        {
                                                          GroundType = (byte) (noise > 0.5f ? 1 : 0)
                                                        };
        }
      return chunk;
    }
  }
}