﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine
{
  /// <summary> Loads chunks via perlin noise. </summary>
  public class SimpleChunkLoader : IChunkLoader
  {
    private readonly IChunkGenerator _generator;

    /// <summary> Constructor. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="generator"> The chunk generator to use when a chunk has not yet been created. </param>
    public SimpleChunkLoader(IChunkGenerator generator)
    {
      if (generator == null)
        throw new ArgumentNullException("generator");
      _generator = generator;
    }

    /// <summary> Called when a chunk is being loaded. </summary>
    public event Action<Chunk> Loading;

    /// <summary> Called before a chunk is saved. </summary>
    public event Action<Chunk> Saving;

    /// <inheritdoc />
    Chunk IChunkLoader.Load(ChunkCoordinate location)
    {
      var chunk = _generator.Generate(location);
      return chunk;
    }

    /// <inheritdoc />
    void IChunkLoader.Save(ChunkCoordinate location, Chunk chunk)
    {
      // noop
    }
  }
}