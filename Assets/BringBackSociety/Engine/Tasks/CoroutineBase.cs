using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Tasks
{
  /// <summary> Represents a resumable action. </summary>
  public abstract class CoroutineBase<T> : ICoroutine<T>
  {
    /// <summary> The enumerator to process as a coroutine. </summary>
    private readonly IEnumerator<T> _enumerator;

    /// <summary> true if this object has already been disposed. </summary>
    private bool _isDisposed;

    /// <summary> Constructor. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="enumerator"> The enumerator to process as a coroutine. </param>
    internal CoroutineBase(IEnumerator<T> enumerator)
    {
      _enumerator = enumerator;
      if (enumerator == null)
        throw new ArgumentNullException("enumerator");
    }

    /// <inheritdoc />
    object ICoroutine.LastValue
    {
      get { return LastValue; }
    }

    /// <summary> The last value returned by the coroutine. </summary>
    public T LastValue { get; private set; }

    /// <summary> Continues execution of the coroutine. </summary>
    public void Continue()
    {
      // make sure we have not already finished.
      if (IsComplete)
        return;

      try
      {
        IsComplete = !_enumerator.MoveNext();

        // only get the next value if it's not yet complete
        if (!IsComplete)
        {
          LastValue = _enumerator.Current;
        }
      }
      catch (Exception e)
      {
        IsComplete = true;
        ContinuationException = e;
      }

      // if we completed, then dispose
      if (IsComplete)
      {
        Dispose();
        OnCompleted();
      }
    }

    /// <summary>
    ///  True if the Coroutine is completed and if Continue() will have no further effect.
    /// </summary>
    public bool IsComplete { get; private set; }

    /// <summary> ContinuationException ?? DisposalException. </summary>
    public Exception Error
    {
      get { return ContinuationException ?? DisposalException; }
    }

    /// <summary>
    ///  The error that occurred while attempting to continue execution enumerator.  Null if no error
    ///  occurred.  Only valid if IsDone is true.
    /// </summary>
    public Exception ContinuationException { get; private set; }

    /// <summary>
    ///  The error that occurred while attempting to dispose of the enumerator.  Null if no error
    ///  occurred.  Only valid if IsDone is true.
    /// </summary>
    public Exception DisposalException { get; private set; }

    /// <summary> True if Error != null || DisposalException != null. </summary>
    public bool HasError
    {
      get { return Error != null || DisposalException != null; }
    }

    /// <summary> Data specific to the dispatcher which is executing this coroutine. </summary>
    ICoroutineDispatcher ICoroutine.Dispatcher { get; set; }

    /// <summary> Event that occurs when the coroutine completes. </summary>
    public event EventHandler Completed;

    private void OnCompleted()
    {
      EventHandler handler = Completed;
      if (handler != null)
        handler(this, EventArgs.Empty);

      Completed = null;
    }

    /// <summary>
    ///  Dispose of the enumerator.
    /// </summary>
    public void Dispose()
    {
      // be sure to only attempt to dispose once.
      if (_isDisposed)
        return;

      _isDisposed = true;
      try
      {
        _enumerator.Dispose();
      }
      catch (Exception e)
      {
        DisposalException = e;
      }
    }
  }
}