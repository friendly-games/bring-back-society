using System;
using System.Collections.Generic;
using System.Linq;

namespace Grid.Tasks
{
  /// <summary> The value of a future. </summary>
  public interface IFutureCompletionSource<T>
  {
    /// <summary> True if the future as completed. </summary>
    bool IsComplete { get; }

    /// <summary> The value of future.  Only valid if IsComplete is true. </summary>
    T Result { get; }

    /// <summary> The error associated with the value.  Only valid if IsComplete is true. </summary>
    Exception Error { get; }

    void ContinueWith(Action<Future<T>> action);

    /// <summary> Gets the future associated with the value. </summary>
    Future<T> Future { get; }
  }

  internal class FutureConstantCompletionSource<T> : IFutureCompletionSource<T>
  {
    public FutureConstantCompletionSource(T result)
    {
      Result = result;
      Future = new Future<T>(this);
    }

    public bool IsComplete
    {
      get { return true; }
    }

    public T Result { get; private set; }

    public Exception Error
    {
      get { return null; }
    }

    public void ContinueWith(Action<Future<T>> action)
    {
      if (action == null)
        throw new ArgumentNullException("action");

      action(Future);
    }

    public Future<T> Future { get; private set; }
  }
}