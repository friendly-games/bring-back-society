using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grid.Tasks;
using NUnit.Framework;

namespace Tests.Tasks
{
  internal class TestFuture
  {
    private IEnumerable<Coop<int>> GetIntegerCoop()
    {
      yield return 8;
      yield return 9;
      yield return Coop.Value(10);
    }

    [Test]
    [Description("It works")]
    public void It_works()
    {
      var future = GetIntegerCoop().ToFuture();

      Assert.IsTrue(future.IsCompleted);
      Assert.AreEqual(10, future.Result);
    }

    private IEnumerable<Coop<string>> NeedToWaitForFuture(Future<string> toWaitFor)
    {
      yield return "what";
      yield return "ok";
      yield return toWaitFor;
      yield return "What" + toWaitFor.Result;
    }

    [Test]
    [Description("Waiting for futures works")]
    public void Waiting_for_futures_works()
    {
      var waitFor = new FutureCompletionSource<string>();

      var future = NeedToWaitForFuture(waitFor.Future).ToFuture();
      Assert.IsFalse(future.IsCompleted);

      waitFor.SetResult(" are you waiting for");
      Assert.IsTrue(future.IsCompleted);
      Assert.AreEqual("What are you waiting for", future.Result);
    }

    private IEnumerable<Coop<int>> ExceptionsPropegate()
    {
      yield return 18;
      throw new Exception("It broke");
    }

    [Test]
    [Description("Exceptions do propegate")]
    public void Exceptions_do_propegate()
    {
      var future = ExceptionsPropegate().ToFuture();

      Assert.IsTrue(future.IsCompleted);
      Assert.IsNotNull(future.Error);
      Assert.AreEqual(future.Error.Message, "It broke");
    }

    [Test]
    [Description("Exceptions propegate from other futures")]
    public void Exceptions_propegate_from_other_futures()
    {
      var waitFor = new FutureCompletionSource<string>();
      var exception = new Exception("This is what happens");

      var future = NeedToWaitForFuture(waitFor.Future).ToFuture();
      waitFor.SetException(exception);

      Assert.IsTrue(future.IsCompleted);
      Assert.AreEqual(exception, future.Error.InnerException);
    }

    private IEnumerable<Coop<int>> DisposalsOccur(DoIDispose doIDispose)
    {
      using (doIDispose)
      {
        throw new Exception();
        yield return 18;
      }
    }

    [Test]
    [Description("Disposals occur")]
    public void Disposals_occur()
    {
      var doIDispose = new DoIDispose();

      Assert.IsFalse(doIDispose.Disposed);
      var future = DisposalsOccur(doIDispose).ToFuture();

      Assert.IsTrue(future.IsCompleted);
      Assert.IsNotNull(future.Error);
      Assert.IsTrue(doIDispose.Disposed);
    }

    private class DoIDispose : IDisposable
    {
      public void Dispose()
      {
        Disposed = true;
      }

      public bool Disposed { get; set; }
    }
  }
}