using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  public interface ICanBeDestroyed : IAspect
  {
    /// <summary> Mark the object as destroyed. </summary>
    void MarkDestroyed();
  }
}