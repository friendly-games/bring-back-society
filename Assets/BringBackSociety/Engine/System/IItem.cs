using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary>
  ///  An object which has no individual properties but instead can be combined with other items of
  ///  the same type.
  /// </summary>
  internal interface IItem
  {
    /// <summary> Gets how many of the items that can be stacked. </summary>
    StackableWeight StackableWeight { get; }
  }
}