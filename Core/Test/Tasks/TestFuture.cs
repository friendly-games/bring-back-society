using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BringBackSociety.Tasks;
using Xunit;

namespace Tests.Tasks
{
  public class TestFuture
  {
    private IEnumerator<Coop<int>> GetIntegerCoop()
    {
      yield return 8;
      yield return 9;
      yield return Coop.Value(10);
    }

    [Fact]
    [Description("It works")]
    public void It_works()
    {
      var future = GetIntegerCoop().ToFuture();

      Assert.True(future.IsCompleted);
      Assert.Equal(10, future.Result);
    }

    private IEnumerator<Coop<string>> NeedToWaitForFuture(Future<string> toWaitFor)
    {
      yield return "what";
      yield return "ok";
      yield return toWaitFor;
      yield return "What" + toWaitFor.Result;
    }

    [Fact]
    [Description("Waiting for futures works")]
    public void Waiting_for_futures_works()
    {
      var waitFor = new FutureCompletionSource<string>();

      var future = NeedToWaitForFuture(waitFor.Future).ToFuture();
      Assert.False(future.IsCompleted);

      waitFor.SetResult(" are you waiting for");
      Assert.True(future.IsCompleted);
      Assert.Equal("What are you waiting for", future.Result);
    }

    private IEnumerator<Coop<int>> ExceptionsPropegate()
    {
      yield return 18;
      throw new Exception("It broke");
    }

    [Fact]
    [Description("Exceptions do propegate")]
    public void Exceptions_do_propegate()
    {
      var future = ExceptionsPropegate().ToFuture();

      Assert.True(future.IsCompleted);
      Assert.NotNull(future.Error);
      Assert.Equal(future.Error.Message, "It broke");
    }

    [Fact]
    [Description("Exceptions propegate from other futures")]
    public void Exceptions_propegate_from_other_futures()
    {
      var waitFor = new FutureCompletionSource<string>();
      var exception = new Exception("This is what happens");

      var future = NeedToWaitForFuture(waitFor.Future).ToFuture();
      waitFor.SetException(exception);

      Assert.True(future.IsCompleted);
      Assert.Equal(exception, future.Error.InnerException);
    }

    private IEnumerator<Coop<int>> DisposalsOccur(DoIDispose doIDispose)
    {
      using (doIDispose)
      {
        throw new Exception();
        yield return 18;
      }
    }

    [Fact]
    [Description("Disposals occur")]
    public void Disposals_occur()
    {
      var doIDispose = new DoIDispose();

      Assert.False(doIDispose.Disposed);
      var future = DisposalsOccur(doIDispose).ToFuture();

      Assert.True(future.IsCompleted);
      Assert.NotNull(future.Error);
      Assert.True(doIDispose.Disposed);
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