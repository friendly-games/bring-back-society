using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Tasks;

/// <summary> Helper methods that can be used to help on the UI side of things. </summary>
internal static class View
{
  private static CoroutineDispatcher _dispatcher;

  /// <summary> Static constructor. </summary>
  static View()
  {
    Dispatcher = new CoroutineDispatcher();
  }

  /// <summary> The dispatcher for the UI. </summary>
  public static CoroutineDispatcher Dispatcher
  {
    get { return _dispatcher; }
    set
    {
      if (value == null)
        throw new ArgumentNullException("value");

      _dispatcher = value;
    }
  }

  /// <summary> Start a new coroutine based on the given enumerator. </summary>
  public static void Start(Coroutine coroutine)
  {
    Dispatcher.Start(coroutine);
  }

  /// <summary> Start a new coroutine based on the given enumerator. </summary>
  public static void Start(IEnumerator enumerator)
  {
    Dispatcher.Start(enumerator);
  }
}