using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BringBackSociety
{
  /// <summary> Utility methods. </summary>
  public static class Util
  {
    private static readonly FieldInfo _remoteStackTraceStringField;

    /// <summary> Static constructor. </summary>
    static Util()
    {
      // Get the _remoteStackTraceString of the Exception class
      _remoteStackTraceStringField = typeof(Exception)
        .GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);

      if (_remoteStackTraceStringField == null)
      {
        _remoteStackTraceStringField = typeof(Exception)
          .GetField("remote_stack_trace", BindingFlags.Instance | BindingFlags.NonPublic);
      } // Mono
    }

    /// <summary> Tell an exception to keep its stack trace when its rethrown.  </summary>
    /// <param name="exception"> The exception whose stack trace should be kept. </param>
    [DebuggerStepThrough]
    public static Exception MaintainStackTrace(Exception exception)
    {
      if (_remoteStackTraceStringField != null)
      {
        _remoteStackTraceStringField.SetValue(exception, exception.StackTrace + Environment.NewLine);
        return exception;
      }
      else
      {
        return new Exception("Thrown Exception", exception);
      }
    }
  }
}