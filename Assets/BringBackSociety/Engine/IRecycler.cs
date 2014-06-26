using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine
{
  /// <summary> Recycles a given object. </summary>
  internal interface IRecycler
  {
    /// <summary> Recycles the given instance. </summary>
    /// <param name="instance"> The instance to recycle. </param>
    void Recycle(object instance);
  }
}