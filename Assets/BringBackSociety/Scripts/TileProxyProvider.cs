using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Extensions;
using BringBackSociety.Items;
using log4net;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BringBackSociety.Scripts
{
  /// <summary> Provides proxy objects that represent objects at tile locations. </summary>
  internal class TileProxyProvider : ExtendedBehaviour, IStart
  {
    /// <summary> Provides logging for the class. </summary>
    private static readonly ILog Log = LogManager.GetLogger(typeof(TileProxyProvider));

    private Object _parentObject;

    public void Start()
    {
      _parentObject = gameObject;
    }

    /// <summary> The chunk associated with the killable. </summary>
    public Chunk Chunk { get; set; }

    public IComponent RetrieveProxyForLocation(TileCoordinate coordinate)
    {
      var position = new WorldPosition(Chunk.Coordinate, coordinate).ToVector3();
      var matches = Physics.OverlapSphere(position, 0.01f);
      foreach (var match in matches)
      {
        return RetrieveProxyFor(match.gameObject);
      }

      return null;
    }

    /// <summary> Retrieves a proxy for the given game object. </summary>
    /// <param name="gameObject"> The game object for which the proxy is valid. </param>
    /// <returns>
    ///  An IComponent that represents the game object, or null if the proxy cannot be created.
    /// </returns>
    public IComponent RetrieveProxyFor(GameObject gameObject)
    {
      if (gameObject.GetParent() != _parentObject)
      {
        Log.Error("Attempted to get proxy for object that is not a child of this object.");
        return null;
      }

      // TODO
      return null;
    }
  }
}