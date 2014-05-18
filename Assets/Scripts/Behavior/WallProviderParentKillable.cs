using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety;
using BringBackSociety.Items;
using Extensions;
using log4net;
using UnityEngine;

namespace Behavior
{
  /// <summary> Provides the walls with the ability to be killed. </summary>
  internal class WallProviderParentKillable : ExtendedBehaviour, IProviderParent
  {
    /// <summary> Create a new instance. </summary>
    /// TODO - this should be done in the Unity Editor somehow
    public WallProviderParentKillable()
    {
      resistance = new Resistance();
      _child = new ChildDestroyable(this);
    }

    public Resistance resistance;
    public int maxHealth = 100;
    private readonly ChildDestroyable _child;

    /// <summary> The chunk associated with the killable. </summary>
    public Chunk Chunk { get; set; }

    #region IProviderParent<IKillable> Implementation

    /// <inheritdoc />
    bool IProviderParent.IsApplicable(GameObject child)
    {
      return true;
    }

    /// <inheritdoc />
    void IProviderParent.With(GameObject child)
    {
      var worldPosition = child.transform.position.ToWorldPosition();
      ChunkCoordinate chunkCoordinate;
      TileCoordinate tileCoordinate;
      worldPosition.CalculateCoordinates(out chunkCoordinate, out tileCoordinate);
      _child.SetCurrent(child, tileCoordinate);
    }

    public IComponent Component
    {
      get { return _child; }
    }

    #endregion

    #region IKillable implementation

    /// <summary> A returnable Killable instance object </summary>
    private class ChildDestroyable : IDestroyable, INamed, ITileItem
    {
      private readonly WallProviderParentKillable _parent;
      private TileCoordinate _tileCoordinate;
      private GameObject _currentChild;

      public ChildDestroyable(WallProviderParentKillable parent)
      {
        _parent = parent;
      }

      /// <inheritdoc />
      Resistance IDestroyable.Resistance
      {
        get { return _parent.resistance; }
      }

      /// <inheritdoc />
      int IDestroyable.MaxHealth
      {
        get { return _parent.maxHealth; }
      }

      /// <inheritdoc />
      int IDestroyable.Health
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
      void IDestroyable.Destroy()
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

      public string Name
      {
        get { return "Wall"; }
      }

      public TileCoordinate TileCoordinate
      {
        get { return _tileCoordinate; }
      }

      public ChunkCoordinate ChunkCoordinate
      {
        get { return _parent.Chunk.Coordinate; }
      }
    }

    #endregion
  }
}