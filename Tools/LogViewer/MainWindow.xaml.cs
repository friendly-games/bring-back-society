using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace LogViewer
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private Thread _backgroundThread;
    private CancellationTokenSource _cancellationToken;
    private SynchronizationContext _syncContext;

    private readonly object _modelLock = new object();

    /// <summary> Default constructor. </summary>
    public MainWindow()
    {
      InitializeComponent();

      Loaded += OnLoaded;
      Closing += OnClosing;

      _model = new LogDataViewModel();

      BindingOperations.EnableCollectionSynchronization(_model.Entries, _modelLock);

      DataContext = _model;
    }

    private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
      _syncContext = SynchronizationContext.Current;
      _cancellationToken = new CancellationTokenSource();
      _backgroundThread = new Thread(Main)
                          {Name = "Server"};
      _backgroundThread.Start();
    }

    private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
    {
      var result = MessageBox.Show("Are you sure you would like to quit?",
                                   "Are you sure?",
                                   MessageBoxButton.YesNo);

      if (result == MessageBoxResult.No)
      {
        cancelEventArgs.Cancel = true;
        return;
      }

      _cancellationToken.Cancel();
      _backgroundThread.Join();
    }

    private void Main()
    {
      try
      {
        while (!_cancellationToken.IsCancellationRequested)
        {
          WatchFile();
        }
      }
      catch (Exception e)
      {
        Debug.WriteLine("Exception:");
        Debug.WriteLine(e);
      }
    }

    private void WatchFile()
    {
      const string filename = @"W:\Unity\Bring Back Society\Logs\EventLog.txt";

      using (var stream = new StreamReader(new FileStream(filename,
                                                          FileMode.Open,
                                                          FileAccess.Read,
                                                          FileShare.ReadWrite)))
      {
        long lastFileSize = 0;
        long newLength = 0;

        do
        {
          lastFileSize = newLength;

          var line = stream.ReadLine();
          if (!string.IsNullOrEmpty(line))
          {
            ProcessLine(line);
          }
          else
          {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
          }

          var info = new FileInfo(filename);

          newLength = info.Length;
        } while (newLength >= lastFileSize && !_cancellationToken.IsCancellationRequested);

        _model.Entries.Clear();
        Debug.WriteLine("Starting over");
      }
    }

    private StringBuilder _primaryBuffer = new StringBuilder();
    private StringBuilder _remainingBuffer = new StringBuilder();
    private readonly LogDataViewModel _model;

    private void ProcessLine(string contents)
    {
      _primaryBuffer.AppendLine(contents);

      int lastLocation = -1;

      for (int i = 0; i < _primaryBuffer.Length; i++)
      {
        if (_primaryBuffer[i] == '\a')
        {
          var length = i - lastLocation + 1;

          var line = _primaryBuffer.ToString(lastLocation + 1, length);

          ProcessEntry(line);
          lastLocation = i;

          if (i < _primaryBuffer.Length - 1 && _primaryBuffer[i + 1] == '\n')
          {
            lastLocation++;
          }
        }
      }

      _remainingBuffer.Clear();
      var length2 = _primaryBuffer.Length - lastLocation - 1;
      _remainingBuffer.Append(_primaryBuffer.ToString(lastLocation + 1, length2));

      // swap them
      var tmp = _primaryBuffer;
      _primaryBuffer = _remainingBuffer;
      _remainingBuffer = tmp;
    }

    private void ProcessEntry(string line)
    {
      var data = line.Trim().Split(new char[] {' '}, 5, StringSplitOptions.RemoveEmptyEntries);

      var dateTime = data[0] + "T" + data[1].Replace(',', '.') + "-0";

      var nameParts = data[4].Split(new char[] {'-'}, 2);

      var entry = new LogEntry()
                  {
                    Time = DateTime.Parse(dateTime),
                    Thread = data[2].Trim('[', ']'),
                    Level = data[3],
                    Name = nameParts[0].Trim(),
                    Description = nameParts[1].Trim(),
                  };

      _syncContext.Post(AddLogEntry, entry);
    }

    private void AddLogEntry(object state)
    {
      var entry = (LogEntry) state;
      _model.Entries.Add(entry);

      if (LogGrid.Items.Count > 0 && _model.AutoScroll)
      {
        var border = VisualTreeHelper.GetChild(LogGrid, 0) as Decorator;
        if (border != null)
        {
          var scroll = border.Child as ScrollViewer;
          if (scroll != null)
            scroll.ScrollToEnd();

          LogGrid.SelectedIndex = LogGrid.Items.Count - 1;
        }
      }
    }
  }
}