using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety;
using BringBackSociety.Scripts;
using UnityEngine;

namespace Scripts
{
  /// <summary> Part of an object that is owned by a chunk. </summary>
  public class ChunkOwner : ExtendedBehaviour
  {
    /// <summary> Gets the chunk associated with this object. </summary>
    public Chunk AssociatedChunk { get; set; }
  }
}