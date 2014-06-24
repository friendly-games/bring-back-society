using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> The component that can be read & written. </summary>
  public interface IReadWriteComponent : IComponent
  {
    /// <summary> Makes a deep copy of this object. </summary>
    /// <returns> A copy of this object, with writable properties copied. </returns>
    IReadWriteComponent Clone();
  }
}