using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> Manages all of the things for specific tiles. </summary>
  internal class TileThingResourceManager
  {
    private readonly World _world;
    private readonly ResourcePool<TileWallProxy> _wallPool;

    /// <summary> Constructor. </summary>
    /// <param name="world"> The world from which things should be handed out. </param>
    public TileThingResourceManager(World world)
    {
      _world = world;

      _wallPool = new ResourcePool<TileWallProxy>(() => new TileWallProxy(_wallPool));
    }

    /// <summary> Retrieves the thing at the given position. </summary>
    /// <param name="position"> The position of the thing to retrieve. </param>
    /// <returns> The thing that is found in the world at the given world position. </returns>
    public IThing Retrieve(WorldPosition position)
    {
      TileCoordinate tileCoordinate;
      ChunkCoordinate chunkCoordinate;
      position.CalculateCoordinates(out chunkCoordinate, out tileCoordinate);
      var reference = new TileReference(_world.Chunks[chunkCoordinate.Index], tileCoordinate);

      // todo decide type of data to return based on data type
      var type = reference.GetValue().Type;

      var thing = _wallPool.Get();
      thing.Initialize(reference);
      return thing;
    }
  }
}