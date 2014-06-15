using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Actions
{
  /// <summary> A single action which can be executed from an external input. </summary>
  public interface IAction
  {
    /// <summary> The name of the action, used for logging purposes. </summary>
    string Name { get; }

    /// <summary> Executes the given action. </summary>
    void Execute();
  }
}