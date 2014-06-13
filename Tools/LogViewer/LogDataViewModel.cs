using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using LogViewer.Annotations;

namespace LogViewer
{
  /// <summary> The model for all of the logging data.. </summary>
  internal class LogDataViewModel : INotifyPropertyChanged
  {
    private LogEntry _selectedItem;
    private bool _autoScroll;

    /// <summary> Default constructor. </summary>
    public LogDataViewModel()
    {
      Entries = new ObservableCollection<LogEntry>();
      _autoScroll = true;
    }

    /// <summary> All of the entries that have been seen thus far. </summary>
    public ObservableCollection<LogEntry> Entries { get; private set; }

    /// <summary> The currently selected log entry. </summary>
    public LogEntry SelectedItem
    {
      get { return _selectedItem; }
      set
      {
        if (Equals(value, _selectedItem)) return;
        _selectedItem = value;
        OnPropertyChanged();
      }
    }

    /// <summary> True if the grid should auto scroll and auto select the last entry added. </summary>
    public bool AutoScroll
    {
      get { return _autoScroll; }
      set
      {
        if (value.Equals(_autoScroll)) return;
        _autoScroll = value;
        OnPropertyChanged();
      }
    }

    /// <summary> Occurs when a property value changes. </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      var handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}