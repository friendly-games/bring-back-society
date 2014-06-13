using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
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
                          ConversionPattern = "%utcdate [%thread] %-5level %logger - %message\a%newline"
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

    var unityLogger = new UnityAppender
                      {
                        Layout = new PatternLayout(),
                      };

    // Only the temp logger and Errors
    unityLogger.AddFilter(new LoggerMatchFilter()
                          {
                            AcceptOnMatch = true,
                            LoggerToMatch = "Temp",
                          });

    unityLogger.AddFilter(new LevelRangeFilter()
                          {
                            AcceptOnMatch = true,
                            LevelMin = Level.Error
                          });

    unityLogger.AddFilter(new DenyAllFilter());

    unityLogger.ActivateOptions();

    BasicConfigurator.Configure(unityLogger, fileAppender);
  }

  /// <summary> Global log used for temporary logging. </summary>
  public static ILog Log { get; set; }

  /// <summary> An appender which logs to the unity console. </summary>
  private class UnityAppender : AppenderSkeleton
  {
    /// <inheritdoc />
    protected override void Append(LoggingEvent loggingEvent)
    {
      string message = RenderLoggingEvent(loggingEvent);

      if (Level.Compare(loggingEvent.Level, Level.Error) >= 0)
      {
        // everything above or equal to error is an error
        Debug.LogError(message);
      }
      else if (Level.Compare(loggingEvent.Level, Level.Warn) >= 0)
      {
        // everything that is a warning up to error is logged as warning
        Debug.LogWarning(message);
      }
      else
      {
        // everything else we'll just log normally
        Debug.Log(message);
      }
    }
  }
}