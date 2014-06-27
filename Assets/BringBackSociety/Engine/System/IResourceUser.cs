using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> A thing which uses a specific type of resource. </summary>
  internal interface IResourceUser : IAspect
  {
    /// <summary> The resource that is required. </summary>
    Resource RequiredResource { get; }
  }
}