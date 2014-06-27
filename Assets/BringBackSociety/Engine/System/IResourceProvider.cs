using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> A thing which provides a certain type of resource. </summary>
  internal interface IResourceProvider : IAspect
  {
    /// <summary> The type of resource that the item provides. </summary>
    Resource Provides { get; }
  }
}