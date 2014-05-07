using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Tasks
{
  public abstract class Waiter
  {
    protected internal abstract void Enqueue(IFutureRunner futureRunner);
  }

  public static class Coop
  {
    public static Coop<T> Value<T>(T value)
    {
      return new Coop<T>(value);
    }
  }

  public struct Coop<T>
  {
    private readonly bool _hasValue;
    private readonly T _returnedValue;
    private readonly Waiter _waiter;

    public Waiter Waiter
    {
      get { return _waiter; }
    }

    internal bool HasValue
    {
      get { return _hasValue; }
    }

    internal T ReturnedValue
    {
      get { return _returnedValue; }
    }

    public Coop(Waiter waiter)
    {
      _hasValue = false;
      _returnedValue = default(T);
      _waiter = waiter;
    }

    public Coop(T returnedValue)
    {
      _hasValue = true;
      _returnedValue = returnedValue;
      _waiter = null;
    }

    public static Coop<T> Value(T value)
    {
      return new Coop<T>(value);
    }

    public static implicit operator Coop<T>(T value)
    {
      return new Coop<T>(value);
    }

    public static implicit operator Coop<T>(Waiter waiter)
    {
      return new Coop<T>(waiter);
    }
  }
}