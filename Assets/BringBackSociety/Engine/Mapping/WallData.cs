using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BringBackSociety.Mapping
{
  /// <summary> Contains all information needed for a single wall. </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct WallData
  {
    /// <summary> The style of the wall. </summary>
    public byte Style;

    /// <summary> The current health of the wall. </summary>
    public byte Health;
  }
}