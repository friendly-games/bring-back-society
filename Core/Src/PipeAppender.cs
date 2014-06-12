using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using log4net.Appender;
using log4net.Core;

namespace BringBackSociety
{
  internal class PipeAppender : AppenderSkeleton, IDisposable
  {
    private readonly StreamWriter _stream;
    private readonly NamedPipeClientStream _pipe;

    public PipeAppender(string name)
    {
      _pipe = null;
      try
      {
        _pipe = new NamedPipeClientStream(".", name, PipeDirection.Out);
        _stream = new StreamWriter(_pipe);
        _pipe.Connect();
      }
      catch (Exception e)
      {
        if (_pipe != null)
        {
          _pipe.Dispose();
        }

        UnityEngine.Debug.Log(e.Message);
      }
    }

    public void Dispose()
    {
      if (_stream != null)
      {
        _pipe.Dispose();
        _stream.Dispose();
      }
    }

    protected override void Append(LoggingEvent loggingEvent)
    {
      if (_stream == null)
        return;

      try
      {
        _stream.WriteLine(loggingEvent.MessageObject);
        _stream.Flush();
      }
      catch (Exception e)
      {
        UnityEngine.Debug.LogError(e.Message);
      }
    }
  }
}