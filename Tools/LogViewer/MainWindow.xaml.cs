using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LogViewer
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
      await Task.Factory.StartNew(() => Main());
    }

    private void Main()
    {
      using (var server = new NamedPipeServerStream("Unity", PipeDirection.In))
      {
        while (true)
        {
          server.WaitForConnection();

          try
          {
            using (var sr = new StreamReader(server))
            {
              while (true)
              {
                var line = sr.ReadLine();
                if (!String.IsNullOrEmpty(line))
                {
                  Debug.WriteLine(line);
                }
              }
            }
          }
          catch (Exception e)
          {
            Debug.WriteLine(e);
          }
        }
      }
    }
  }
}