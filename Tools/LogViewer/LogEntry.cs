using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LogViewer.Annotations;

namespace LogViewer
{
  /// <summary> A log entry. </summary>
  public class LogEntry
  {
    /// <summary> The text of what occurred. </summary>
    public string Description { get; set; }
  }
}