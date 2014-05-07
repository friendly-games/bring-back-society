using System;
using System.Collections.Generic;
using System.Linq;
using Grid;
using Grid.Loaders;
using NUnit.Framework;

namespace Tests
{
  internal class ChunkManagerTests
  {
    private ChunkManager _grid;
    private ChunkManager.ChunkLoadResult _result;

    [SetUp]
    public void Setup()
    {
      _grid = new ChunkManager(new ConstructorChunkLoader());

      _result = _grid.Load(new ChunkCoordinate(0, 0), true);
    }

    [Test]
    [Description("Verify grid of chunks")]
    public void Verify_grid_of_chunks()
    {
      Assert.IsNotNull(_result.Center);
    }

    [Test]
    [Description("Verify loaded chunks")]
    public void Verify_loaded_chunks()
    {
      Assert.IsNotNull(_result.Center.Back);
      Assert.IsNotNull(_result.Center.Left);
      Assert.IsNotNull(_result.Center.Right);
      Assert.IsNotNull(_result.Center.Front);
    }

    [Test]
    [Description("Verify locations of chunks")]
    public void Verify_locations_of_chunks()
    {
      Assert.AreEqual(new ChunkCoordinate(0, 1), _result.Center.Back.Chunk.Coordinate);
      Assert.AreEqual(new ChunkCoordinate(-1, 0), _result.Center.Left.Chunk.Coordinate);
      Assert.AreEqual(new ChunkCoordinate(1, 0), _result.Center.Right.Chunk.Coordinate);
      Assert.AreEqual(new ChunkCoordinate(0, -1), _result.Center.Front.Chunk.Coordinate);

      Assert.AreEqual(new ChunkCoordinate(1, 1), _result.Center.Right.Back.Chunk.Coordinate);
      Assert.AreEqual(new ChunkCoordinate(1, -1), _result.Center.Right.Front.Chunk.Coordinate);

      Assert.AreEqual(new ChunkCoordinate(-1, 1), _result.Center.Left.Back.Chunk.Coordinate);
      Assert.AreEqual(new ChunkCoordinate(-1, -1), _result.Center.Left.Front.Chunk.Coordinate);
    }
  }
}