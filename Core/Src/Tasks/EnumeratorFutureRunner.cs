using System;
using System.Collections.Generic;
using System.Linq;

namespace Grid.Tasks
{
  /// <summary> Executes an enumerator to use yield like await. </summary>
  /// <typeparam name="T"> The return value type of the future. </typeparam>
  public sealed class EnumeratorFutureRunner<T> : IFutureRunner
  {
    private readonly IEnumerator<Coop<T>> _coroutine;

    private readonly FutureCompletionSource<T> _futureCompletionSource;
    private Coop<T> _lastValue;

    public EnumeratorFutureRunner(IEnumerable<Coop<T>> coroutine)
    {
      _coroutine = coroutine.GetEnumerator();
      _futureCompletionSource = new FutureCompletionSource<T>();

      HandleIteration();
    }

    /// <summary> The future associated with this runner. </summary>
    public Future<T> Future
    {
      get { return _futureCompletionSource.Future; }
    }

    void IFutureRunner.Next()
    {
      HandleIteration();
    }

    private void HandleIteration()
    {
      bool again;
      do
      {
        again = Step();
      } while (again);
    }

    /// <summary> Move through the iterator once. </summary>
    /// <exception cref="Exception"> Thrown when an exception error condition occurs. </exception>
    /// <returns> true if it succeeds, false if it fails. </returns>
    private bool Step()
    {
      bool isComplete;
      bool shouldContinue;

      try
      {
        isComplete = !_coroutine.MoveNext();
        var result = _coroutine.Current;

        if (isComplete)
        {
          // stop we're all done
          shouldContinue = false;
        }
        else
        {
          // were we returning a value from the co-routine?
          if (result.HasValue)
          {
            // store the value and go onto the next iteration
            _lastValue = result;
            shouldContinue = true;
          }
          else if (result.Waiter != null)
          {
            // if we're waiting for something, then enqueue it to be notified of that thing
            result.Waiter.Enqueue(this);
            shouldContinue = false;
          }
          else
          {
            throw new Exception("Waiters cannot be null");
          }
        }
      }
      catch (Exception exception)
      {
        OnCompleted(exception, default(T));
        return false;
      }

      if (isComplete)
      {
        OnCompleted(null, _lastValue.ReturnedValue);
        return false;
      }

      return shouldContinue;
    }

    /// <summary> Executes the completed action. </summary>
    /// <param name="exception"> The exception. </param>
    /// <param name="value"> The value. </param>
    private void OnCompleted(Exception exception, T value)
    {
      try
      {
        Dispose();
      }
      finally
      {
        if (exception != null)
        {
          _futureCompletionSource.SetException(exception);
        }
        else
        {
          _futureCompletionSource.SetResult(value);
        }
      }
    }

    private void Dispose()
    {
      var disposable = _coroutine as IDisposable;
      if (disposable != null)
      {
        disposable.Dispose();
      }
    }
  }
}