using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BringBackSociety.Tasks
{
  /// <summary> Preserves the stack trace of an exception. </summary>
  public static class ExceptionHelper
  {
    private static readonly FieldInfo _stackTraceField;

    static ExceptionHelper()
    {
      // Get the _remoteStackTraceString of the Exception class
      var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
      // MS.Net

      _stackTraceField = typeof (Exception).GetField("_remoteStackTraceString", bindingFlags)
                         ?? typeof (Exception).GetField("remote_stack_trace", bindingFlags);
    }

    /// <summary> Preserve stack trace. </summary>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    /// <param name="exception"> The exception. </param>
    /// <returns> An Exception. </returns>
    public static Exception PreserveStackTrace<T>(this T exception)
      where T : Exception
    {
      if (_stackTraceField == null)
        return new Exception("Exception was thrown", exception);

      // Set the InnerException._remoteStackTraceString
      // to the current InnerException.StackTrace
      _stackTraceField.SetValue(exception, exception.StackTrace + Environment.NewLine);

      // Throw the new exception
      return exception;
    }
  }
}