using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BringBackSociety.Tasks
{
  /// <summary> Processes multiple coroutines in a single update and manages their lifecycle. </summary>
  public class CoroutineDispatcher : ICoroutineDispatcher
  {
    private readonly LinkedList<CoroutineData> _activeCoroutines;
    private readonly LinkedList<CoroutineData> _failedNodes;

    /// <summary> Default constructor. </summary>
    public CoroutineDispatcher()
    {
      _activeCoroutines = new LinkedList<CoroutineData>();
      _failedNodes = new LinkedList<CoroutineData>();
    }

    /// <summary> Start a new coroutine based on the given enumerator. </summary>
    public void Start(Coroutine coroutine)
    {
      Enqueue(coroutine);
    }

    /// <summary> Start a new coroutine based on the given enumerator. </summary>
    public void Start(IEnumerator enumerator)
    {
      Start(new Coroutine(enumerator));
    }

    /// <summary>
    ///  Add a new coroutine to be executed by this dispatcher when Continue() is invoked.
    /// </summary>
    /// <param name="coroutine"> The coroutine to execute. </param>
    private CoroutineData Enqueue(ICoroutine coroutine, bool executeFirstStep = true)
    {
      if (coroutine == null)
        throw new ArgumentNullException("coroutine");
      if (coroutine.Dispatcher != null)
        throw new ArgumentException("Coroutine is already registered to another dispatcher", "coroutine");

      var data = new CoroutineData(coroutine);
      _activeCoroutines.AddLast(data.Node);
      coroutine.Dispatcher = this;

      if (executeFirstStep)
      {
        Step(data);
      }

      return data;
    }

    /// <summary> True if the dispatcher has more coroutines to execute. </summary>
    public bool HasWork
    {
      get { return _activeCoroutines.Count > 0; }
    }

    /// <summary> Continue all coroutines currently registered to this dispatcher. </summary>
    public void Continue()
    {
      var node = _activeCoroutines.First;
      while (node != null)
      {
        var next = node.Next;

        var coroutineData = node.Value;
        Step(coroutineData);

        node = next;
      }

      ThrowErrorIfRequired();
    }

    private void Step(CoroutineData coroutineData)
    {
      var coroutine = coroutineData.Value;

      coroutine.Continue();
      ProcessCoroutineStep(coroutineData);
    }

    [DebuggerStepThrough]
    private void ThrowErrorIfRequired()
    {
      // throw the first one
      if (_failedNodes.Count > 0)
      {
        var first = _failedNodes.First.Value;
        var coroutine = first.Value;
        _failedNodes.RemoveFirst();

        throw new Exception("Exception thrown while executing co-routine", coroutine.Error);
      }
    }

    /// <summary> Handle the result of moving the coroutine forward one step. </summary>
    private void ProcessCoroutineStep(CoroutineData coroutineData)
    {
      var coroutine = coroutineData.Value;
      var node = coroutineData.Node;

      if (coroutine.IsComplete)
      {
        _activeCoroutines.Remove(node);

        // if there was something waiting on it before, re-add that one
        if (coroutineData.Waiter != null)
        {
          // make sure we can enqueue without problem
          coroutineData.Waiter.Dispatcher = null;
          Enqueue(coroutineData.Waiter, executeFirstStep: false);
        }

        if (coroutine.HasError)
        {
          _failedNodes.AddLast(coroutineData);
        }
      }
      else
      {
        // if the last value was another coroutine, that means that we want to wait for that coroutine
        // to finish
        var waitFor = coroutine.LastValue as ICoroutine;

        // wait for the new coroutine to finish.
        if (waitFor != null)
        {
          _activeCoroutines.Remove(node);
          var newWaiter = Enqueue(waitFor);
          newWaiter.Waiter = coroutine;
        }
      }
    }

    private class CoroutineData
    {
      /// <summary> The primary coroutine. </summary>
      public readonly ICoroutine Value;

      /// <summary> The node associated with the coroutine. </summary>
      public readonly LinkedListNode<CoroutineData> Node;

      /// <summary> Coroutine waiting for the primary Coroutine to complete. </summary>
      public ICoroutine Waiter;

      public CoroutineData(ICoroutine coroutine)
      {
        Value = coroutine;
        Node = new LinkedListNode<CoroutineData>(this);
      }
    }
  }
}