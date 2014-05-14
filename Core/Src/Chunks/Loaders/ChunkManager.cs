using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using log4net;

namespace BringBackSociety.Loaders
{
  internal class ChunkManager
  {
    private readonly IChunkLoader _loader;

    private readonly Dictionary<Chunk, ChunkNode> _nodeLookups;
    private readonly Dictionary<ChunkCoordinate, ChunkNode> _nodeLookupByCoordinate;

    private readonly ILog _log = LogManager.GetLogger(typeof (ChunkManager));

    /// <summary> Constructor. </summary>
    /// <param name="loader"> The api to use to load and ave chunks. </param>
    public ChunkManager(IChunkLoader loader)
    {
      _loader = loader;

      _nodeLookups = new Dictionary<Chunk, ChunkNode>();
      _nodeLookupByCoordinate = new Dictionary<ChunkCoordinate, ChunkNode>();
    }

    /// <summary> Load the chunks surrounding the designated node. </summary>
    /// <param name="coordinate"> The coordinate of the node to load surrounding nodes. </param>
    /// <param name="includeSurrounding"> true to include surrounding nodes, false otherwise. </param>
    public ChunkLoadResult Load(ChunkCoordinate coordinate, bool includeSurrounding)
    {
      // how far from the center point should chunks be loaded for
      const int radiusLength = 2;

      var loadedChunks = new List<ChunkNode>();
      var nodesToLoadSiblingsOf = new Queue<ChunkNode>();

      var low = new ChunkCoordinate(coordinate.X - radiusLength, coordinate.Z - radiusLength);
      var high = new ChunkCoordinate(coordinate.X + radiusLength, coordinate.Z + radiusLength);

      var removed = RemoveTooFar(low, high);

      var firstNode = GetOrLoadNode(low, loadedChunks);
      nodesToLoadSiblingsOf.Enqueue(firstNode);

      ChunkNode current = null;

      while (nodesToLoadSiblingsOf.Count > 0)
      {
        var node = nodesToLoadSiblingsOf.Dequeue();
        var curCoordinate = node.Chunk.Coordinate;

        if (node.Chunk.Coordinate.Equals(coordinate))
        {
          current = node;
        }

        if (node.Chunk.Coordinate.X < high.X)
        {
          var right = GetOrLoadNode(new ChunkCoordinate(curCoordinate.X + 1, curCoordinate.Z), loadedChunks);
          ChunkNode.LinkHorizontally(node, right);
          nodesToLoadSiblingsOf.Enqueue(right);
        }

        if (node.Chunk.Coordinate.Z < high.Z)
        {
          var above = GetOrLoadNode(new ChunkCoordinate(curCoordinate.X, curCoordinate.Z + 1), loadedChunks);
          ChunkNode.LinkVertically(above, node);
          nodesToLoadSiblingsOf.Enqueue(above);
        }
      }

      return new ChunkLoadResult(current, loadedChunks, removed);
    }

    /// <summary> Retrieve the node associated with the designated coordinate. </summary>
    /// <param name="coordinate"> The coordinate of the node to retrieve. </param>
    /// <param name="loadedChunks"> A list to add the node to if it was loaded. </param>
    /// <returns> The node that was retrieved. </returns>
    private ChunkNode GetOrLoadNode(ChunkCoordinate coordinate, [CanBeNull] List<ChunkNode> loadedChunks)
    {
      ChunkNode node;
      if (_nodeLookupByCoordinate.TryGetValue(coordinate, out node))
        return node;

      var chunk = _loader.Load(coordinate);
      node = new ChunkNode(chunk);
      _nodeLookups[chunk] = node;
      _nodeLookupByCoordinate[coordinate] = node;

      if (loadedChunks != null)
      {
        loadedChunks.Add(node);
      }

      return node;
    }

    private void Remove(ChunkNode node)
    {
      _nodeLookups.Remove(node.Chunk);
      _nodeLookupByCoordinate.Remove(node.Chunk.Coordinate);
    }

    private List<ChunkNode> RemoveTooFar(ChunkCoordinate low, ChunkCoordinate high)
    {
      var toRemove = _nodeLookups.Values.Where(n =>
      {
        var offset = n.Chunk.Coordinate;

        return offset.X < low.X
               || offset.Z < low.Z
               || offset.X > high.X
               || offset.Z > high.Z;
      }).ToList();

      foreach (var node in toRemove)
      {
        Remove(node);
      }

      return toRemove;
    }

    /// <summary> The result of loading chunks. </summary>
    public class ChunkLoadResult
    {
      /// <summary> Constructor. </summary>
      /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
      /// <param name="center"> The center node that was loaded. </param>
      /// <param name="loaded"> All of the chunks that were loaded dynamically into the system. </param>
      /// <param name="removed"> All of the chunks that are pegged for removal.. </param>
      public ChunkLoadResult(ChunkNode center, List<ChunkNode> loaded, List<ChunkNode> removed)
      {
        if (loaded == null)
          throw new ArgumentNullException("loaded");
        if (center == null)
          throw new ArgumentNullException("center");
        if (removed == null)
          throw new ArgumentNullException("removed");

        Center = center;
        Loaded = loaded;
        Removed = removed;
      }

      /// <summary> The center node that was loaded. </summary>
      public ChunkNode Center { get; private set; }

      /// <summary> All of the chunks that were loaded dynamically into the system. </summary>
      public List<ChunkNode> Loaded { get; private set; }

      public List<ChunkNode> Removed { get; set; }
    }
  }
}