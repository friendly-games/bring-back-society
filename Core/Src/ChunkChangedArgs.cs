using System;
using System.Collections.Generic;
using System.Linq;

namespace Grid
{
  /// <summary> Arguments for chunk changed event. </summary>
  public class ChunkChangedArgs : EventArgs
  {
    /// <summary> Constructor. </summary>
    /// <param name="chunk"> The chunk that was either loaded or saved. </param>
    /// <param name="wasLoaded"> true if the chunk was loaded, false if it was saved. </param>
    public ChunkChangedArgs(Chunk chunk, bool wasLoaded)
    {
      Chunk = chunk;
      WasLoaded = wasLoaded;
    }

    /// <summary> Gets or sets a value indicating whether the chunk was was loaded. </summary>
    public bool WasLoaded { get; private set; }

    /// <summary> The chunk that had the operation applied to it. </summary>
    public Chunk Chunk { get; private set; }
  }
}