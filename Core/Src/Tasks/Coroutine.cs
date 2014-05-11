using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Tasks
{
  /// <summary> Represents a resumable action. </summary>
  /// <typeparam name="T"> Generic type parameter. </typeparam>
  public sealed class Coroutine<T> : CoroutineBase<T>
  {
    /// <summary> Constructor. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="enumerator"> The enumerator to process as a coroutine. </param>
    public Coroutine(IEnumerator<T> enumerator)
      : base(enumerator)
    {
    }
  }

  /// <summary> Represents a resumable action. </summary>
  public sealed class Coroutine : CoroutineBase<object>
  {
    /// <summary> Constructor. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="enumerator"> The enumerator to process as a coroutine. </param>
    public Coroutine(IEnumerator enumerator)
      : base(new WrapperEnumeratorWeakToStrong(enumerator))
    {
    }

    /// <summary> Wraps an IEnumerator into an IEnumerator[object]. </summary>
    private class WrapperEnumeratorWeakToStrong : IEnumerator<object>
    {
      private readonly IEnumerator _mWrappedEnumerator;

      internal WrapperEnumeratorWeakToStrong(IEnumerator wrappedEnumerator)
      {
        if (wrappedEnumerator == null)
          throw new ArgumentNullException("wrappedEnumerator");

        _mWrappedEnumerator = wrappedEnumerator;
      }

      object IEnumerator.Current
      {
        get { return _mWrappedEnumerator.Current; }
      }

      object IEnumerator<object>.Current
      {
        get { return _mWrappedEnumerator.Current; }
      }

      void IDisposable.Dispose()
      {
        var disposable = _mWrappedEnumerator as IDisposable;
        if (disposable != null)
        {
          disposable.Dispose();
        }
      }

      bool IEnumerator.MoveNext()
      {
        return _mWrappedEnumerator.MoveNext();
      }

      void IEnumerator.Reset()
      {
        _mWrappedEnumerator.Reset();
      }
    }
  }
}