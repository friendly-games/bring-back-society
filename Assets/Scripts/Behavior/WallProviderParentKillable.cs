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
  internal class WallProviderParentKillable : ExtendedBehaviour, IProviderParent<IKillable>
  {
    /// <summary> Create a new instance. </summary>
    /// TODO - this should be done in the Unity Editor somehow
    public WallProviderParentKillable()
    {
      resistance = new Resistance();
      _child = new ChildKillable(this);
    }

    public Resistance resistance;
    public int maxHealth = 100;
    private readonly ChildKillable _child;

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
      TileCoordinate tileCoordinate;
      worldPosition.CalculateCoordinates(out chunkCoordinate, out tileCoordinate);
      _child.SetCurrent(child, tileCoordinate);
    }

    /// <inheritdoc />
    IKillable IProviderParent<IKillable>.Implementation
    {
      get { return _child; }
    }

    #endregion

    #region IKillable implementation

    /// <summary> A returnable Killable instance object </summary>
    private class ChildKillable : IKillable
    {
      private readonly WallProviderParentKillable _parent;
      private TileCoordinate _tileCoordinate;
      private GameObject _currentChild;

      public ChildKillable(WallProviderParentKillable parent)
      {
        _parent = parent;
      }

      /// <inheritdoc />
      Resistance IKillable.Resistance
      {
        get { return _parent.resistance; }
      }

      /// <inheritdoc />
      int IKillable.MaxHealth
      {
        get { return _parent.maxHealth; }
      }

      /// <inheritdoc />
      int IKillable.Health
      {
        get { return _parent.Chunk.Tiles[_tileCoordinate.Index].WallData.Health; }
        set
        {
          if (value <= 0)
          {
            _parent.Chunk.Tiles[_tileCoordinate.Index].WallData.Health = 0;
          }
          else
          {
            _parent.Chunk.Tiles[_tileCoordinate.Index].WallData.Health = (byte) value;
          }
        }
      }

      /// <inheritdoc />
      void IKillable.Destroy()
      {
        _parent.Chunk.Tiles[_tileCoordinate.Index] = new Tile();
        Destroy(_currentChild);
      }

      /// <summary>
      ///  Set the killable to operate on the given child at the given tile coordinate.
      /// </summary>
      /// <param name="child"> The child whose data should be represented by this killable. </param>
      /// <param name="tileCoordinate"> The tile coordinate that the child is located at. </param>
      public void SetCurrent(GameObject child, TileCoordinate tileCoordinate)
      {
        _currentChild = child;
        _tileCoordinate = tileCoordinate;
      }

      /// <inheritdoc />
      public override string ToString()
      {
        return String.Format("[Name='Wall', Chunk={0}, Tile={1}]", _parent.Chunk, _tileCoordinate);
      }
    }

    #endregion
  }
}