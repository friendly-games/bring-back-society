using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;

namespace BringBackSociety.Engine.System
{
  /// <summary> Represents an object that has resistance against various effects. </summary>
  public interface IResist : IAspect
  {
    /// <summary> The values of resistance for the entity. </summary>
    Resistance Resistance { get; }
  }
}