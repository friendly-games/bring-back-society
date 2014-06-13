using System;
using System.Collections.Generic;
using System.Linq;

namespace LogViewer
{
  /// <summary> A log entry. </summary>
  public class LogEntry
  {
    /// <summary> The text of what occurred. </summary>
    public string Description { get; set; }

    /// <summary> The time at which the entry was added. </summary>
    public DateTime Time { get; set; }

    /// <summary> The thread on which the entry was logged. </summary>
    public string Thread { get; set; }

    /// <summary> The level at which the entry was logged. </summary>
    public string Level { get; set; }
  }
}