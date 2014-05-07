using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BringBackSociety.Tasks
{
  public abstract class Future : Waiter
  {
    internal Future()
    {
    }

    protected internal override void Enqueue(IFutureRunner futureRunner)
    {
      ContinueWith(futureRunner.Next);
    }

    internal abstract void ContinueWith(Action action);

    /// <summary> Create a value from a result. </summary>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    /// <param name="value"> The value of the future. </param>
    public static Future<T> FromResult<T>(T value)
    {
      return new FutureConstantCompletionSource<T>(value).Future;
    }
  }

  /// <summary> Holds a value that will complete at some point in the future. </summary>
  /// <typeparam name="T"> Generic type parameter. </typeparam>
  public sealed class Future<T> : Future
  {
    private readonly IFutureCompletionSource<T> _source;

    /// <summary> Constructor. </summary>
    /// <param name="source"> The value provider associated with the future. </param>
    internal Future(IFutureCompletionSource<T> source)
    {
      _source = source;
    }

    /// <summary> True if the future has completed. </summary>
    public bool IsCompleted
    {
      get { return _source.IsComplete; }
    }

    /// <summary> The value of the future.  Throws if IsComplete is false. </summary>
    public T Result
    {
      get { return _source.Result; }
    }

    /// <summary> The error associated with the future. Throws if IsComplete is false. </summary>
    public Exception Error
    {
      get { return _source.Error; }
    }

    internal override void ContinueWith(Action action)
    {
      ContinueWith(fake => action());
    }

    public void ContinueWith(Action<Future<T>> action)
    {
      _source.ContinueWith(action);
    }
  }

  internal static class FutureExtensions
  {
    public static Future<T> ToFuture<T>(this IEnumerable<Coop<T>> coop)
    {
      return new EnumeratorFutureRunner<T>(coop).Future;
    }
  }
}