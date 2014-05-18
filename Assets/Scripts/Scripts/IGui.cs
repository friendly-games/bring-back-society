using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts
{
  /// <summary> A behavior which has a gui component. </summary>
  public interface IGui
  {
    /// <summary> Called when the gui is being rendered. </summary>
    void OnGUI();
  }
}