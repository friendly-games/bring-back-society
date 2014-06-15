using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BringBackSociety.Tasks
{
  /// <summary> Interface for coroutine. </summary>
  internal interface ICoroutine : IDisposable
  {
    /// <summary> The last value returned by the coroutine. </summary>
    [CanBeNull]
    object LastValue { get; }

    /// <summary> Continue the coroutine. </summary>
    void Continue();

    /// <summary>
    ///  True if the Coroutine is completed and if Continue() will have no further effect.
    /// </summary>
    bool IsComplete { get; }

    /// <summary>
    ///  The error that occurred while executing the coroutine or disposing of the coroutine.  Only
    ///  valid if IsDone is true.  Equivalent to (ContinuationException ?? DisposalException)
    /// </summary>
    [CanBeNull]
    Exception Error { get; }

    /// <summary>
    ///  The exception that occurred while attempting to dispose of the co-routine.  Only valid if
    ///  IsDone is true.  Can be null.
    /// </summary>
    [CanBeNull]
    Exception DisposalException { get; }

    /// <summary>
    ///  The error that occurred while attempting to continue execution enumerator.  Null if no error
    ///  occurred.  Only valid if IsDone is true.
    /// </summary>
    [CanBeNull]
    Exception ContinuationException { get; }

    /// <summary> True if either Error or DisposalException is true. </summary>
    bool HasError { get; }

    /// <summary> Event that occurs when the coroutine completes. </summary>
    event EventHandler Completed;

    /// <summary> Data specific to the dispatcher which is executing this coroutine. </summary>
    ICoroutineDispatcher Dispatcher { get; set; }
  }

  /// <summary> Interface for coroutine that returns a specific type of value. </summary>
  /// <typeparam name="T"> The type of value that the coroutine returns. </typeparam>
  internal interface ICoroutine<T> : ICoroutine
  {
    /// <summary> The last value returned by the coroutine. </summary>
    new T LastValue { get; }
  }
}