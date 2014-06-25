using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> A single piece of data associated with a component. </summary>
  public interface IComponent
  {
    /// <summary> Gets the information that describes the component. </summary>
    ComponentInfo GetComponentInfo();
  }
}