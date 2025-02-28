// Decompiled with JetBrains decompiler
// Type: UnityThreading.TaskBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Threading;

#nullable disable
namespace UnityThreading
{
  public abstract class TaskBase
  {
    public volatile int Priority;
    private ManualResetEvent abortEvent = new ManualResetEvent(false);
    private ManualResetEvent endedEvent = new ManualResetEvent(false);
    private bool hasStarted;

    protected abstract IEnumerator Do();

    public bool ShouldAbort => this.abortEvent.WaitOne(0, false);

    public bool HasEnded => this.endedEvent.WaitOne(0, false);

    public bool IsSucceeded
    {
      get => this.endedEvent.WaitOne(0, false) && !this.abortEvent.WaitOne(0, false);
    }

    public bool IsFailed => this.endedEvent.WaitOne(0, false) && this.abortEvent.WaitOne(0, false);

    public void Abort() => this.abortEvent.Set();

    public void AbortWait()
    {
      this.Abort();
      this.Wait();
    }

    public void AbortWaitForSeconds(float seconds)
    {
      this.Abort();
      this.WaitForSeconds(seconds);
    }

    public void Wait() => this.endedEvent.WaitOne();

    public void WaitForSeconds(float seconds)
    {
      this.endedEvent.WaitOne(TimeSpan.FromSeconds((double) seconds));
    }

    public virtual TResult Wait<TResult>()
    {
      throw new InvalidOperationException("This task type does not support return values.");
    }

    public virtual TResult WaitForSeconds<TResult>(float seconds)
    {
      throw new InvalidOperationException("This task type does not support return values.");
    }

    public virtual TResult WaitForSeconds<TResult>(float seconds, TResult defaultReturnValue)
    {
      throw new InvalidOperationException("This task type does not support return values.");
    }

    internal void DoInternal()
    {
      this.hasStarted = true;
      if (!this.ShouldAbort)
      {
        IEnumerator enumerator = this.Do();
        if (enumerator == null)
        {
          this.endedEvent.Set();
          return;
        }
        ThreadBase currentThread = ThreadBase.CurrentThread;
        do
        {
          TaskBase current = (TaskBase) enumerator.Current;
          if (current != null && currentThread != null)
            currentThread.DispatchAndWait(current);
        }
        while (enumerator.MoveNext());
      }
      this.endedEvent.Set();
    }

    public void Dispose()
    {
      if (this.hasStarted)
        this.Wait();
      this.endedEvent.Close();
      this.abortEvent.Close();
    }
  }
}
