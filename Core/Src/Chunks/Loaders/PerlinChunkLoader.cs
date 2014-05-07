using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BringBackSociety.Services;

namespace BringBackSociety.Chunks.Loaders
{
  /// <summary> Loads chunks via perlin noise. </summary>
  public class PerlinChunkLoader : IChunkLoader
  {
    private readonly IPerlinNoise _noise;

    /// <summary> Constructor. </summary>
    /// <param name="noise"> The noise generator to use.. </param>
    public PerlinChunkLoader(IPerlinNoise noise)
    {
      if (noise == null)
        throw new ArgumentNullException("noise");

      _noise = noise;
    }

    public Chunk Load(ChunkCoordinate location)
    {
      var chunk = new Chunk(location);
      for (int x = 0; x < Chunk.Length; x++)
        for (int z = 0; z < Chunk.Length; z++)
        {
          var noise = _noise.Noise(x, z);
          chunk.Tiles[new TileCoordinate(x, z).Index] = new Tile
                                                        {
                                                          GroundType = (byte) (noise*255)
                                                        };
        }
      return chunk;
    }

    public void Save(ChunkCoordinate location, Chunk chunk)
    {
      // noop
    }
  }
}