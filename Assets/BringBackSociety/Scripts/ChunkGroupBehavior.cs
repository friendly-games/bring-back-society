using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Engine.System;
using BringBackSociety.Extensions;
using BringBackSociety.Game.System;
using JetBrains.Annotations;
using UnityEngine;

namespace BringBackSociety.Scripts
{
  /// <summary> Behavior for a chunk that contains a series of children. </summary>
  internal class ChunkGroupBehavior : ExtendedBehaviour, IProxyParent
  {
    /// <summary> The resource manager to use to provide proxies to requesters. </summary>
    private TileThingResourceManager _resourceManager;

    /// <summary>
    ///  Initialize this behavior with the required parameters (takes the place of a constructor).
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="chunk"> The chunk associated with this group behavior. </param>
    /// <param name="resourceManager"> The resource manager to use to provide proxies to requesters. </param>
    public void Initialize([NotNull] Chunk chunk, TileThingResourceManager resourceManager)
    {
      if (chunk == null)
        throw new ArgumentNullException("chunk");

      Chunk = chunk;
      _resourceManager = resourceManager;
    }

    /// <summary> The chunk associated with this group behavior. </summary>
    public Chunk Chunk { get; private set; }

    /// <inheritdoc />
    public IThing RetrieveProxyFor(GameObject child)
    {
      var worldPosition = child.transform.position.ToWorldPosition();
      return _resourceManager.Retrieve(worldPosition);
    }

    public GameObject GetGameObjectFor(TileCoordinate coordinate)
    {
      // TODO fix this crap
      var position = new WorldPosition(Chunk.Coordinate, coordinate).ToVector3();
      var matches = Physics.OverlapSphere(position, 0.01f);
      foreach (var match in matches)
      {
        return match.gameObject;
      }

      return null;
    }
  }
}