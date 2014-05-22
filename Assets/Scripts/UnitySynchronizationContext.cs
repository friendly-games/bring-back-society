using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using log4net;

/// <summary>
///  Provides a SynchronizationContext that's single-threaded and queues items to be manually
///  processed.
/// </summary>
internal sealed class UnitySynchronizationContext : SynchronizationContext
{
  /// <summary> Provides logging for the class. </summary>
  private static readonly ILog Log = LogManager.GetLogger(typeof(UnitySynchronizationContext));

  /// <summary>The queue of work items.</summary>
  private readonly BlockingCollection<KeyValuePair<SendOrPostCallback, object>> _queue =
    new BlockingCollection<KeyValuePair<SendOrPostCallback, object>>();

  /// <summary>Dispatches an asynchronous message to the synchronization context.</summary>
  /// <param name="callback">The System.Threading.SendOrPostCallback delegate to call.</param>
  /// <param name="state">The object passed to the delegate.</param>
  public override void Post(SendOrPostCallback callback, object state)
  {
    if (callback == null)
      throw new ArgumentNullException("callback");

    _queue.Add(new KeyValuePair<SendOrPostCallback, object>(callback, state));
  }

  /// <summary>
  ///  When overridden in a derived class, dispatches a synchronous message to a synchronization
  ///  context.
  /// </summary>
  /// <param name="callback"> The System.Threading.SendOrPostCallback delegate to call. </param>
  /// <param name="state"> The object passed to the delegate. </param>
  public override void Send(SendOrPostCallback callback, object state)
  {
    using (var isDone = new ManualResetEvent(false))
    {
      Post(theState =>
      {
        callback(theState);
        isDone.Set();
      },
           state);

      // wait for it to be completed
      isDone.WaitOne();
    }
  }

  /// <summary> Runs an loop to process all queued work items. </summary>
  /// <remarks> Should be called from the unity thread. </remarks>
  public void Process()
  {
    KeyValuePair<SendOrPostCallback, object> item;
    while (_queue.TryTake(out item))
    {
      item.Key(item.Value);
    }
  }
}