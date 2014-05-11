using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Tasks
{
  /// <summary> Executes an enumerator to use yield like await. </summary>
  /// <typeparam name="T"> The return value type of the future. </typeparam>
  public sealed class CoroutineFutereRunner<T> : IFutureRunner
  {
    private readonly ICoroutine<Coop<T>> _coroutine;

    private readonly FutureCompletionSource<T> _futureCompletionSource;
    private T _lastValue;

    public CoroutineFutereRunner(Coroutine<Coop<T>> coroutine)
    {
      if (coroutine == null)
        throw new ArgumentNullException("coroutine");

      _coroutine = coroutine;
      _futureCompletionSource = new FutureCompletionSource<T>();

      _coroutine.Completed += CoroutineOnCompleted;

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
    /// <returns>
    ///  True if it should continue for another step, false if should not be called again.
    /// </returns>
    private bool Step()
    {
      _coroutine.Continue();

      // if it completed, the callback was automatically called.
      if (_coroutine.IsComplete)
        return false;

      bool shouldContinue;
      var result = _coroutine.LastValue;

      // were we returning a value from the co-routine?
      if (result.HasValue)
      {
        // store the value and go onto the next iteration
        shouldContinue = true;
        // store the last actual value
        _lastValue = result.ReturnedValue;
      }
      else if (result.Waiter != null)
      {
        // if we're waiting for something, then enqueue it to be notified of that thing
        result.Waiter.Enqueue(this);
        shouldContinue = false;
      }
      else
      {
        throw new Exception("Coroutine must either return a value or a waiter");
      }

      return shouldContinue;
    }

    private void CoroutineOnCompleted(object sender, EventArgs eventArgs)
    {
      if (_coroutine.HasError)
      {
        _futureCompletionSource.SetException(_coroutine.Error ?? _coroutine.DisposalException);
      }
      else
      {
        _futureCompletionSource.SetResult(_lastValue);
      }
    }
  }
}