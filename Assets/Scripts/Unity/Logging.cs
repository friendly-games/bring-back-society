using System;
using System.Collections.Generic;
using System.Linq;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using UnityEngine;

public class Logging
{
  /// <summary>
  ///  Configure logging to write to Logs\EventLog.txt and the Unity console output.
  /// </summary>
  public static void ConfigureAllLogging()
  {
    var patternLayout = new PatternLayout
                        {
                          ConversionPattern = "%date [%thread] %-5level %logger - %message%newline"
                        };
    patternLayout.ActivateOptions();

    // setup the appender that writes to Log\EventLog.txt
    var fileAppender = new RollingFileAppender
                       {
                         AppendToFile = false,
                         File = @"Logs\EventLog.txt",
                         Layout = patternLayout,
                         MaxSizeRollBackups = 5,
                         MaximumFileSize = "1GB",
                         RollingStyle = RollingFileAppender.RollingMode.Size,
                         StaticLogFileName = true
                       };
    fileAppender.ActivateOptions();

    var unityLogger = new UnityLogger()
                      {
                        Layout = new PatternLayout()
                      };
    unityLogger.ActivateOptions();

    BasicConfigurator.Configure(unityLogger, fileAppender);
  }

  /// <summary> An appender which logs to the unity console. </summary>
  private class UnityLogger : AppenderSkeleton
  {
    /// <inheritdoc />
    protected override void Append(LoggingEvent loggingEvent)
    {
      string message = RenderLoggingEvent(loggingEvent);

      if (loggingEvent.Level == Level.Error)
      {
        Debug.LogError(message);
      }
      else if (loggingEvent.Level == Level.Warn)
      {
        Debug.LogWarning(message);
      }
      else
      {
        Debug.Log(message);
      }
    }
  }
}