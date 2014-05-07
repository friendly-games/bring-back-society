using System;
using System.Collections.Generic;
using System.Linq;

namespace Grid
{
  /// <summary> Loads chunks for the world. </summary>
  public interface IChunkLoader
  {
    /// <summary> Loads the chunk for the give location. </summary>
    /// <param name="location"> The location of the chunk to load. </param>
    /// <returns> The chunk for the given position. </returns>
    Chunk Load(ChunkCoordinate location);

    /// <summary> Saves a chunk for the given location. </summary>
    /// <param name="location"> The location of the chunk that should be saved. </param>
    /// <param name="chunk"> The chunk to save. </param>
    void Save(ChunkCoordinate location, Chunk chunk);
  }
}