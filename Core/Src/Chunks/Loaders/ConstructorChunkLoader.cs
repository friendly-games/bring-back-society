using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Loaders
{
  /// <summary> A chunk loader which simply creates a new chunk by using the default constructor. </summary>
  internal class ConstructorChunkLoader : IChunkLoader
  {
    Chunk IChunkLoader.Load(ChunkCoordinate location)
    {
      return new Chunk(location);
    }

    void IChunkLoader.Save(ChunkCoordinate location, Chunk chunk)
    {
    }
  }
}