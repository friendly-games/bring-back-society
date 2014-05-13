using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using BringBackSociety.Tasks;
using NUnit.Framework;

namespace Tests.Tasks
{
  internal class CoroutineDispatcherTests
  {
    private CoroutineDispatcher _dispatcher;
    private Coroutine _okayCoroutine;
    private Coroutine _waitCoroutine;
    private Coroutine _exceptionCoroutine;

    [SetUp]
    public void Setup()
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

    [Test]
    [Description("Dispatcher works")]
    public void Dispatcher_works()
    {
      _dispatcher.Start(_okayCoroutine);

      Run();

      Assert.IsTrue(_okayCoroutine.IsComplete);
      Assert.IsFalse(_dispatcher.HasWork);
    }

    [Test]
    [Description("Waiting works")]
    public void Waiting_works()
    {
      _dispatcher.Start(_waitCoroutine);

      Assert.IsFalse(_waitCoroutine.IsComplete);
      Run(14);

      // should not be done
      Assert.IsFalse(_waitCoroutine.IsComplete);
      Assert.IsFalse(_okayCoroutine.IsComplete);

      // okay should be done, wait should not
      Run(1);
      Assert.IsTrue(_okayCoroutine.IsComplete);
      Assert.IsFalse(_waitCoroutine.IsComplete);

      // wait shouldn't yet, we're one step away
      Run(4);
      Assert.IsFalse(_waitCoroutine.IsComplete);

      // should be good now
      Run(1);
      Assert.IsTrue(_waitCoroutine.IsComplete);

      Assert.IsFalse(_dispatcher.HasWork);
    }

    [Test]
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
        Assert.Pass();
      }
      catch (Exception exception)
      {
        Console.WriteLine(exception);
        Assert.Fail("Incorrect type of exception was thrown");
      }

      Assert.Fail();
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