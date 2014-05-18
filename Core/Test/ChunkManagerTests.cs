using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BringBackSociety;
using BringBackSociety.Loaders;
using Xunit;

namespace Tests
{
  public class ChunkManagerTests
  {
    private ChunkManager _grid;
    private readonly ChunkManager.ChunkLoadResult _result;

    public ChunkManagerTests()
    {
      _grid = new ChunkManager(new ConstructorChunkLoader());

      _result = _grid.Load(new ChunkCoordinate(0, 0), true);
    }

    [Fact]
    [Description("Verify grid of chunks")]
    public void Verify_grid_of_chunks()
    {
      Assert.NotNull(_result.Center);
    }

    [Fact]
    [Description("Verify loaded chunks")]
    public void Verify_loaded_chunks()
    {
      Assert.NotNull(_result.Center.Back);
      Assert.NotNull(_result.Center.Left);
      Assert.NotNull(_result.Center.Right);
      Assert.NotNull(_result.Center.Front);
    }

    [Fact]
    [Description("Verify locations of chunks")]
    public void Verify_locations_of_chunks()
    {
      Assert.Equal(new ChunkCoordinate(0, 1), _result.Center.Back.Chunk.Coordinate);
      Assert.Equal(new ChunkCoordinate(-1, 0), _result.Center.Left.Chunk.Coordinate);
      Assert.Equal(new ChunkCoordinate(1, 0), _result.Center.Right.Chunk.Coordinate);
      Assert.Equal(new ChunkCoordinate(0, -1), _result.Center.Front.Chunk.Coordinate);

      Assert.Equal(new ChunkCoordinate(1, 1), _result.Center.Right.Back.Chunk.Coordinate);
      Assert.Equal(new ChunkCoordinate(1, -1), _result.Center.Right.Front.Chunk.Coordinate);

      Assert.Equal(new ChunkCoordinate(-1, 1), _result.Center.Left.Back.Chunk.Coordinate);
      Assert.Equal(new ChunkCoordinate(-1, -1), _result.Center.Left.Front.Chunk.Coordinate);
    }
  }
}