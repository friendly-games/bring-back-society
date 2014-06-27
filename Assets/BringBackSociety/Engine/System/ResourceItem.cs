using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> Holds a wall that can be placed. </summary>
  internal class ResourceItem : IItem, IResourceProvider
  {
    /// <summary> Constructor. </summary>
    /// <param name="resource"> The resource that the item provides.. </param>
    public ResourceItem(Resource resource)
    {
      Provides = resource;
    }

    /// <inheritdoc />
    public StackableWeight StackableWeight
    {
      get { return StackableWeight.Large; }
    }

    /// <inheritdoc />
    public Resource Provides { get; private set; }
  }
}