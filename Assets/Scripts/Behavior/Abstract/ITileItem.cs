using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety;

namespace Behavior
{
  /// <summary> An item that is placed in a tile location. </summary>
  public interface ITileItem : IComponent
  {
    /// <summary> The location of the item in the chunk. </summary>
    TileCoordinate TileCoordinate { get; }

    /// <summary> The position of the item among chunks. </summary>
    ChunkCoordinate ChunkCoordinate { get; }
  }
}