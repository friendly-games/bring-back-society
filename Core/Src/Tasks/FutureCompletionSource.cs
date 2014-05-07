using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Tasks
{
  /// <summary> Creates a value that can be used to trigger a future. </summary>
  /// <typeparam name="TResult"> The type of data that the future holds. </typeparam>
  public class FutureCompletionSource<TResult> : IFutureCompletionSource<TResult>
  {
    private ResultOrException<TResult> _actionResultOrException;

    /// <summary> Default constructor. </summary>
    internal FutureCompletionSource()
    {
      Future = new Future<TResult>(this);
    }

    /// <summary> Create a new completion source. </summary>
    public static FutureCompletionSource<TResult> Create()
    {
      return new FutureCompletionSource<TResult>();
    }

    /// <summary> The future tied to this value. </summary>
    public Future<TResult> Future { get; private set; }

    /// <summary> True if the future has completed, false if it has not. </summary>
    public bool IsComplete
    {
      get
      {
        lock (this)
        {
          return _actionResultOrException != null;
        }
      }
    }

    /// <summary>
    ///  The value associated with the future.  Throws if the future has not yet been set.
    /// </summary>
    public TResult Result
    {
      get
      {
        lock (this)
        {
          if (_actionResultOrException == null)
            throw new Exception("Result is not yet complete");

          if (_actionResultOrException.Exception != null)
            throw new Exception("Exception in future", _actionResultOrException.Exception);

          return _actionResultOrException.Value;
        }
      }
    }

    /// <summary>
    ///  The error associated with the value.  Throws if the future has not yet been set.
    /// </summary>
    public Exception Error
    {
      get
      {
        lock (this)
        {
          if (_actionResultOrException == null)
            throw new Exception("Result is not yet complete");

          return _actionResultOrException.Exception;
        }
      }
    }

    /// <summary> Execute the designated action when the future completes. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="action"> The action to execute when the value is set. </param>
    public void ContinueWith(Action<Future<TResult>> action)
    {
      if (action == null)
        throw new ArgumentNullException("action");

      lock (this)
      {
        if (_actionResultOrException == null)
        {
          Complated += action;
        }
        else
        {
          action(Future);
        }
      }
    }

    private event Action<Future<TResult>> Complated;

    private void OnComplated(Action<Future<TResult>> handler)
    {
      if (handler != null)
      {
        handler(Future);
      }
    }

    /// <summary> Sets the value associated with the future </summary>
    /// <exception cref="Exception"> If the result has already been set. </exception>
    /// <param name="result"> The result to associate with the future. </param>
    internal void SetResult(TResult result)
    {
      Action<Future<TResult>> handler;

      lock (this)
      {
        if (_actionResultOrException != null)
          throw new Exception("Result has already been set");

        handler = Complated;
        Complated = null;
        _actionResultOrException = new ResultOrException<TResult> {Value = result};
      }

      OnComplated(handler);
    }

    /// <summary> Sets the exception associated with the future </summary>
    /// <exception cref="Exception"> If the result has already been set. </exception>
    /// <param name="exception"> The exception to associate with the future. </param>
    internal void SetException(Exception exception)
    {
      if (exception == null)
        throw new ArgumentNullException("exception");

      Action<Future<TResult>> handler;

      lock (this)
      {
        if (_actionResultOrException != null)
          throw new Exception("Result has already been set");

        handler = Complated;
        Complated = null;
        _actionResultOrException = new ResultOrException<TResult> {Exception = exception};
      }

      OnComplated(handler);
    }

    private class ResultOrException<T>
    {
      public T Value;
      public Exception Exception;
    }
  }
}