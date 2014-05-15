using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety;
using Extensions;
using log4net;
using UnityEngine;

namespace Behavior
{
  /// <summary> Provides the walls with the ability to be killed. </summary>
  internal class WallProviderParentKillable : ExtendedBehaviour, IProviderParent<IKillable>, IKillable
  {
    private static readonly ILog _log = LogManager.GetLogger(typeof (WallProviderParentKillable));
    private TileCoordinate _tileCoordinate;
    private GameObject _currentChild;

    /// <summary> Create a new instance. </summary>
    /// TODO - this should be done in the Unity Editor somehow
    public WallProviderParentKillable()
    {
      resistance = new Resistance();
    }

    public Resistance resistance;
    public int maxHealth;

    /// <summary> The chunk associated with the killable. </summary>
    public Chunk Chunk { get; set; }

    #region IProviderParent<IKillable> Implementation

    /// <inheritdoc />
    bool IProviderParent<IKillable>.IsApplicable(GameObject child)
    {
      return true;
    }

    /// <inheritdoc />
    void IProviderParent<IKillable>.With(GameObject child)
    {
      var worldPosition = child.transform.position.ToWorldPosition();
      ChunkCoordinate chunkCoordinate;
      worldPosition.CalculateCoordinates(out chunkCoordinate, out _tileCoordinate);

      _currentChild = child;

      _log.InfoFormat("World Position: {0}", child.transform.position.ToWorldPosition());
    }

    /// <inheritdoc />
    IKillable IProviderParent<IKillable>.Implementation
    {
      get { return this; }
    }

    #endregion

    #region IKillable implementation

    /// <inheritdoc />
    Resistance IKillable.Resistance
    {
      get { return resistance; }
    }

    /// <inheritdoc />
    int IKillable.MaxHealth
    {
      get { return maxHealth; }
    }

    /// <inheritdoc />
    int IKillable.Health
    {
      get { return Chunk.Tiles[_tileCoordinate.Index].Type != 0 ? 20 : 0; }
      set { }
    }

    /// <inheritdoc />
    void IKillable.Destroy()
    {
      Chunk.Tiles[_tileCoordinate.Index] = new Tile();
      Destroy(_currentChild);
    }

    #endregion
  }
}