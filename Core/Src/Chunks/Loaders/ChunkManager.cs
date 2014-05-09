using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BringBackSociety.Loaders
{
  internal class ChunkManager
  {
    private readonly IChunkLoader _loader;

    private readonly Dictionary<Chunk, ChunkNode> _nodeLookups;
    private readonly Dictionary<ChunkCoordinate, ChunkNode> _nodeLookupByCoordinate;

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
      /*
       * UL UC UR
       * ML MM MR
       * BL BM BR
       */
      var loadedChunks = new List<ChunkNode>();

      var upperLeft = GetOrLoadNode(new ChunkCoordinate(coordinate.X - 1, coordinate.Z + 1), loadedChunks);
      var upperMiddle = GetOrLoadNode(new ChunkCoordinate(coordinate.X, coordinate.Z + 1), loadedChunks);
      var upperRight = GetOrLoadNode(new ChunkCoordinate(coordinate.X + 1, coordinate.Z + 1), loadedChunks);

      var middleLeft = GetOrLoadNode(new ChunkCoordinate(coordinate.X - 1, coordinate.Z), loadedChunks);
      var middleMiddle = GetOrLoadNode(new ChunkCoordinate(coordinate.X, coordinate.Z), loadedChunks);
      var middleRight = GetOrLoadNode(new ChunkCoordinate(coordinate.X + 1, coordinate.Z), loadedChunks);

      var bottomLeft = GetOrLoadNode(new ChunkCoordinate(coordinate.X - 1, coordinate.Z - 1), loadedChunks);
      var bottomMiddle = GetOrLoadNode(new ChunkCoordinate(coordinate.X, coordinate.Z - 1), loadedChunks);
      var bottomRight = GetOrLoadNode(new ChunkCoordinate(coordinate.X + 1, coordinate.Z - 1), loadedChunks);

      ChunkNode.LinkHorizontally(upperLeft, upperMiddle);
      ChunkNode.LinkHorizontally(upperMiddle, upperRight);

      ChunkNode.LinkHorizontally(middleLeft, middleMiddle);
      ChunkNode.LinkHorizontally(middleMiddle, middleRight);

      ChunkNode.LinkHorizontally(bottomLeft, bottomMiddle);
      ChunkNode.LinkHorizontally(bottomMiddle, bottomRight);

      ChunkNode.LinkVertically(upperLeft, middleLeft);
      ChunkNode.LinkVertically(middleLeft, bottomLeft);

      ChunkNode.LinkVertically(upperMiddle, middleMiddle);
      ChunkNode.LinkVertically(middleMiddle, bottomMiddle);

      ChunkNode.LinkVertically(upperRight, middleRight);
      ChunkNode.LinkVertically(middleRight, bottomRight);

      var currentChunks = new ChunkNode[]
                          {
                            upperLeft,
                            upperMiddle,
                            upperRight,
                            middleLeft,
                            middleMiddle,
                            middleRight,
                            bottomLeft,
                            bottomMiddle,
                            bottomRight,
                          };

      var removed = RemoveUnneeded(currentChunks);

      return new ChunkLoadResult(middleMiddle, loadedChunks, removed);
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

    private List<ChunkNode> RemoveUnneeded(IEnumerable<ChunkNode> currentChunks)
    {
      var nodesToRemove = _nodeLookups.Values.ToList().Except(currentChunks).ToList();

      foreach (var node in nodesToRemove)
      {
        _nodeLookups.Remove(node.Chunk);
        _nodeLookupByCoordinate.Remove(node.Chunk.Coordinate);
      }

      return nodesToRemove;
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