using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Chunks.Generators
{
  /// <summary> Generates a new chunk for a designated area in the world. </summary>
  public interface IChunkGenerator
  {
    /// <summary> Generates the chunk for the give location. </summary>
    /// <param name="location"> The location of the chunk to generate. </param>
    /// <returns> The chunk for the given position. </returns>
    Chunk Generate(ChunkCoordinate location);
  }
}