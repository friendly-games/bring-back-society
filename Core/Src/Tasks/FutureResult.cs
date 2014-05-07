using System;
using System.Collections.Generic;
using System.Linq;

namespace Grid.Tasks
{
  /// <summary> The result of a future completing. </summary>
  /// <typeparam name="T"> Generic type parameter. </typeparam>
  public struct FutureResult<T>
  {
    private readonly T _value;
    private readonly Exception _exception;
    private readonly bool _isDone;

    public FutureResult(T value)
    {
      _value = value;
      _exception = null;
      _isDone = true;
    }

    public FutureResult(Exception exception)
    {
      _exception = exception;
      _value = default(T);
      _isDone = true;
    }

    /// <summary> True if the future has completed. </summary>
    public bool IsDone
    {
      get { return _isDone; }
    }

    /// <summary> The value of the future.  Only valid if IsDone is true. </summary>
    public T Value
    {
      get { return _value; }
    }

    /// <summary> The exception of the future.  Only valid if IsDone is true. </summary>
    public Exception Exception
    {
      get { return _exception; }
    }

    /// <summary> True if there is an exception associated with the future. </summary>
    public bool HasError
    {
      get { return Exception == null; }
    }

    public T ValueOrException
    {
      get
      {
        if (HasError)
          throw new Exception("Future resulted in exception", Exception);

        return Value;
      }
    }
  }
}