using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BringBackSociety.Tasks;
using Xunit;

namespace Tests.Tasks
{
  public class CoroutineDispatcherTests
  {
    private readonly CoroutineDispatcher _dispatcher;
    private readonly Coroutine _okayCoroutine;
    private readonly Coroutine _waitCoroutine;
    private readonly Coroutine _exceptionCoroutine;

    public CoroutineDispatcherTests()
    {
      _dispatcher = new CoroutineDispatcher();
      _okayCoroutine = new Coroutine(OkayEnumerator());
      _waitCoroutine = new Coroutine(WaitForCoroutine());
      _exceptionCoroutine = new Coroutine(Exceptions());
    }

    public void Run(int iterations = -1)
    {
      if (iterations < 0)
      {
        while (_dispatcher.HasWork)
        {
          _dispatcher.Continue();
        }
      }
      else
      {
        for (int i = 0; i < iterations; i++)
        {
          _dispatcher.Continue();
        }
      }
    }

    [Fact]
    [Description("Dispatcher works")]
    public void Dispatcher_works()
    {
      _dispatcher.Start(_okayCoroutine);

      Run();

      Assert.True(_okayCoroutine.IsComplete);
      Assert.False(_dispatcher.HasWork);
    }

    [Fact]
    [Description("Waiting works")]
    public void Waiting_works()
    {
      _dispatcher.Start(_waitCoroutine);

      Assert.False(_waitCoroutine.IsComplete);
      Run(14);

      // should not be done
      Assert.False(_waitCoroutine.IsComplete);
      Assert.False(_okayCoroutine.IsComplete);

      // okay should be done, wait should not
      Run(1);
      Assert.True(_okayCoroutine.IsComplete);
      Assert.False(_waitCoroutine.IsComplete);

      // wait shouldn't yet, we're one step away
      Run(4);
      Assert.False(_waitCoroutine.IsComplete);

      // should be good now
      Run(1);
      Assert.True(_waitCoroutine.IsComplete);

      Assert.False(_dispatcher.HasWork);
    }

    [Fact]
    [Description("Exceptions are propegated")]
    public void OriginalExceptionIsPropegated()
    {
      _dispatcher.Start(_exceptionCoroutine);

      Run(4);

      try
      {
        Run(1);
      }
      catch (ArgumentException exception)
      {
        Console.WriteLine(exception);
        return;
      }
      catch (Exception exception)
      {
        Console.WriteLine(exception);
        Assert.False(true, "Incorrect type of exception was thrown");
      }

      Assert.False(true);
    }

    private IEnumerator OkayEnumerator()
    {
      for (int i = 0; i < 10; i++)
      {
        yield return i;
      }
    }

    private IEnumerator WaitForCoroutine()
    {
      for (int i = 0; i < 10; i++)
      {
        if (i == 5)
        {
          yield return _okayCoroutine;
        }
        else
        {
          yield return i;
        }
      }
    }

    private IEnumerator Exceptions()
    {
      for (int i = 0; i < 10; i++)
      {
        if (i == 5)
        {
          throw new ArgumentException("Argument not valid");
        }
        else
        {
          yield return i;
        }
      }
    }
  }
}