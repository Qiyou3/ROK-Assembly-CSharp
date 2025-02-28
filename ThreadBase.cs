// Decompiled with JetBrains decompiler
// Type: UnityThreading.ThreadBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Threading;

#nullable disable
namespace UnityThreading
{
  public abstract class ThreadBase : IDisposable
  {
    protected Dispatcher targetDispatcher;
    protected Thread thread;
    protected ManualResetEvent exitEvent = new ManualResetEvent(false);
    [ThreadStatic]
    private static ThreadBase currentThread;

    public ThreadBase()
      : this(true)
    {
    }

    public ThreadBase(bool autoStartThread)
      : this(Dispatcher.Current, autoStartThread)
    {
    }

    public ThreadBase(Dispatcher targetDispatcher)
      : this(targetDispatcher, true)
    {
      this.targetDispatcher = targetDispatcher;
    }

    public ThreadBase(Dispatcher targetDispatcher, bool autoStartThread)
    {
      this.targetDispatcher = targetDispatcher;
      if (!autoStartThread)
        return;
      this.Start();
    }

    public static ThreadBase CurrentThread => ThreadBase.currentThread;

    public bool IsAlive => this.thread != null && this.thread.IsAlive;

    public bool ShouldStop => this.exitEvent.WaitOne(0, false);

    public void Start()
    {
      if (this.thread != null)
        this.Abort();
      this.exitEvent.Reset();
      this.thread = new Thread(new ThreadStart(this.DoInternal));
      this.thread.Start();
    }

    public void Exit()
    {
      if (this.thread == null)
        return;
      this.exitEvent.Set();
    }

    public void Abort()
    {
      this.Exit();
      if (this.thread == null)
        return;
      this.thread.Join();
    }

    public void AbortWaitForSeconds(float seconds)
    {
      this.Exit();
      if (this.thread == null)
        return;
      this.thread.Join((int) ((double) seconds * 1000.0));
      if (!this.thread.IsAlive)
        return;
      this.thread.Abort();
    }

    public Task<T> Dispatch<T>(Func<T> function) => this.targetDispatcher.Dispatch<T>(function);

    public T DispatchAndWait<T>(Func<T> function)
    {
      Task<T> task = this.Dispatch<T>(function);
      task.Wait();
      return task.Result;
    }

    public T DispatchAndWait<T>(Func<T> function, float timeOutSeconds)
    {
      Task<T> task = this.Dispatch<T>(function);
      task.WaitForSeconds(timeOutSeconds);
      return task.Result;
    }

    public Task Dispatch(Action action) => this.targetDispatcher.Dispatch(action);

    public void DispatchAndWait(Action action) => this.Dispatch(action).Wait();

    public void DispatchAndWait(Action action, float timeOutSeconds)
    {
      this.Dispatch(action).WaitForSeconds(timeOutSeconds);
    }

    public TaskBase Dispatch(TaskBase taskBase) => this.targetDispatcher.Dispatch(taskBase);

    public void DispatchAndWait(TaskBase taskBase) => this.Dispatch(taskBase).Wait();

    public void DispatchAndWait(TaskBase taskBase, float timeOutSeconds)
    {
      this.Dispatch(taskBase).WaitForSeconds(timeOutSeconds);
    }

    protected void DoInternal()
    {
      ThreadBase.currentThread = this;
      IEnumerator enumerator = this.Do();
      if (enumerator == null)
        return;
      do
      {
        TaskBase current = (TaskBase) enumerator.Current;
        if (current != null)
          this.DispatchAndWait(current);
      }
      while (enumerator.MoveNext());
    }

    protected abstract IEnumerator Do();

    public virtual void Dispose() => this.AbortWaitForSeconds(1f);
  }
}
