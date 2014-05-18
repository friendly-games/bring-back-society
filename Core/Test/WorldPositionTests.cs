using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BringBackSociety;
using Xunit;

namespace Tests
{
  public class WorldPositionTests
  {
    [Fact]
    [Description("RoundTrip")]
    public void RoundTrip()
    {
      var values = new EntryList()
                   {
                     {
                       new WorldPosition(5, 5),
                       new ChunkCoordinate(0, 0),
                       new TileCoordinate(5, 5)
                     },
                     {
                       new WorldPosition(Chunk.Length + 5, 5),
                       new ChunkCoordinate(1, 0),
                       new TileCoordinate(5, 5)
                     },
                     {
                       new WorldPosition(-5, 5),
                       new ChunkCoordinate(-1, 0),
                       new TileCoordinate(Chunk.Length - 5, 5)
                     },
                   };

      int i = 0;
      foreach (var value in values)
      {
        var actualWorld = new WorldPosition(value.ChunkCoordinate, value.TileCoordinate);
        Assert.Equal(value.WorldPosition, actualWorld);

        ChunkCoordinate actualChunk;
        TileCoordinate actualTile;

        actualWorld.CalculateCoordinates(out actualChunk, out actualTile);
        Assert.Equal(value.ChunkCoordinate, actualChunk);
        Assert.Equal(value.TileCoordinate, actualTile);
        i++;
      }
    }

    private class EntryList : List<Entry>
    {
      public void Add(WorldPosition world, ChunkCoordinate chunk, TileCoordinate tile)
      {
        Add(new Entry()
            {
              WorldPosition = world,
              ChunkCoordinate = chunk,
              TileCoordinate = tile,
            });
      }
    }

    private class Entry
    {
      public WorldPosition WorldPosition;
      public ChunkCoordinate ChunkCoordinate;
      public TileCoordinate TileCoordinate;
    }
  }
}