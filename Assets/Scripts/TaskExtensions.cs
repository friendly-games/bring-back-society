using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BringBackSociety;
using BringBackSociety.Services;

/// <summary> Task extension methods. </summary>
public static class TaskExtensions
{
  private static TaskScheduler _unityTaskScheduler;

  /// <summary> Set the task scheduler for the UI thread. </summary>
  public static void InitializeSchedularForUiThread(TaskScheduler unityTaskScheduler)
  {
    if (unityTaskScheduler == null)
      throw new ArgumentNullException("unityTaskScheduler");

    _unityTaskScheduler = unityTaskScheduler;
  }

  /// <summary> Continue the task with on the UI thread using the given callback. </summary>
  public static void ContinueOnUiThread(this Task task, Action<Task> callback)
  {
    task.ContinueWith(callback);
  }

  /// <summary> Continue the task with on the UI thread using the given callback. </summary>
  public static void ContinueOnUiThread<T>(this Task<T> task, Action<Task<T>> runner)
  {
    task.ContinueWith(runner, _unityTaskScheduler);
  }

  /// <summary> Throw any exceptions on the UI thread </summary>
  /// <param name="task"> The task to act on. </param>
  public static void PropegateExceptions(this Task task)
  {
    task.ContinueWith(PropegateException);
  }

  private static void PropegateException(Task task)
  {
    if (task.Exception != null)
    {
      AllServices.SynchronizationContext
                 .Post(state =>
                 {
                   var exception = task.Exception;
                   Util.MaintainStackTrace(exception);
                   throw exception;
                 },
                       null);
    }
  }
}