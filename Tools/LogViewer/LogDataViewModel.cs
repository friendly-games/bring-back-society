using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LogViewer
{
  /// <summary> A ViewModel for the log data. </summary>
  internal class LogDataViewModel
  {
    /// <summary> Default constructor. </summary>
    public LogDataViewModel()
    {
      Entries = new ObservableCollection<LogEntry>();
    }

    public ObservableCollection<LogEntry> Entries { get; private set; }
  }

}