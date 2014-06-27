using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> Represents an thing which is stackable. </summary>
  internal enum StackableWeight
  {

    /// <summary> Only 1 item can be in a slot at a time. </summary>
    Single,

    /// <summary> A small number of these items can be stacked. </summary>
    Small,

    /// <summary> A medium number of these items can be stacked. </summary>
    Medium,

    /// <summary> A large number of these items can be stacked. </summary>
    Large
  }
}