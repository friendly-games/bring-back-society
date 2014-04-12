using System;
using System.Annotations;
using System.Collections.Generic;
using System.Linq;

/// <summary> Performs logging from within unity. </summary>
public static class Logging
{
  /// <summary> Outputs a debugging message. </summary>
  /// <param name="message"> The message to output. </param>
  public static void Error(string message)
  {
    UnityEngine.Debug.LogError(message);
  }

  /// <summary> Outputs a debugging message. </summary>
  /// <param name="formatString"> The format string for the message. </param>
  /// <param name="args"> A variable-length parameters list containing arguments. </param>
  [StringFormatMethod("formatString")]
  public static void Error(string formatString, params object[] args)
  {
    UnityEngine.Debug.LogError(String.Format(formatString, args));
  }

  /// <summary> Outputs a debugging message. </summary>
  /// <param name="message"> The message to output. </param>
  public static void Info(string message)
  {
    UnityEngine.Debug.Log(message);
  }

  /// <summary> Outputs a debugging message. </summary>
  /// <param name="formatString"> The format string for the message. </param>
  /// <param name="args"> A variable-length parameters list containing arguments. </param>
  [StringFormatMethod("formatString")]
  public static void Info(string formatString, params object[] args)
  {
    UnityEngine.Debug.Log(String.Format(formatString, args));
  }

  /// <summary> Outputs a debugging message. </summary>
  /// <param name="message"> The message to output. </param>
  public static void Warning(string message)
  {
    UnityEngine.Debug.LogWarning(message);
  }

  /// <summary> Outputs a debugging message. </summary>
  /// <param name="formatString"> The format string for the message. </param>
  /// <param name="args"> A variable-length parameters list containing arguments. </param>
  [StringFormatMethod("formatString")]
  public static void Warning(string formatString, params object[] args)
  {
    UnityEngine.Debug.LogWarning(String.Format(formatString, args));
  }
}