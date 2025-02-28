// Decompiled with JetBrains decompiler
// Type: UnityThreading.Dispatcher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Threading;

#nullable disable
namespace UnityThreading
{
  public class Dispatcher : DispatcherBase
  {
    [ThreadStatic]
    private static TaskBase currentTask;
    [ThreadStatic]
    internal static Dispatcher currentDispatcher;
    protected static Dispatcher mainDispatcher;

    public Dispatcher()
      : this(true)
    {
    }

    internal Dispatcher(bool setThreadDefaults)
    {
      if (!setThreadDefaults)
        return;
      Dispatcher.currentDispatcher = Dispatcher.currentDispatcher == null ? this : throw new InvalidOperationException("Only one Dispatcher instance allowed per thread.");
      if (Dispatcher.mainDispatcher != null)
        return;
      Dispatcher.mainDispatcher = this;
    }

    public static TaskBase CurrentTask
    {
      get
      {
        return Dispatcher.currentTask != null ? Dispatcher.currentTask : throw new InvalidOperationException("No task is currently running.");
      }
    }

    public static Dispatcher Current
    {
      get
      {
        return Dispatcher.currentDispatcher != null ? Dispatcher.currentDispatcher : throw new InvalidOperationException("No Dispatcher found for the current thread, please create a new Dispatcher instance before calling this property.");
      }
      set
      {
        if (Dispatcher.currentDispatcher != null)
          Dispatcher.currentDispatcher.Dispose();
        Dispatcher.currentDispatcher = value;
      }
    }

    public static Dispatcher Main
    {
      get
      {
        return Dispatcher.mainDispatcher != null ? Dispatcher.mainDispatcher : throw new InvalidOperationException("No Dispatcher found for the main thread, please create a new Dispatcher instance before calling this property.");
      }
    }

    public static Func<T> CreateSafeFunction<T>(Func<T> function)
    {
      return (Func<T>) (() =>
      {
        try
        {
          return function();
        }
        catch
        {
          Dispatcher.CurrentTask.Abort();
          return default (T);
        }
      });
    }

    public static Action CreateSafeAction<T>(Action action)
    {
      return (Action) (() =>
      {
        try
        {
          action();
        }
        catch
        {
          Dispatcher.CurrentTask.Abort();
        }
      });
    }

    public void ProcessTasks()
    {
      if (!this.dataEvent.WaitOne(0, false))
        return;
      this.ProcessTasksInternal();
    }

    public bool ProcessTasks(WaitHandle exitHandle)
    {
      if (WaitHandle.WaitAny(new WaitHandle[2]
      {
        exitHandle,
        (WaitHandle) this.dataEvent
      }) == 0)
        return false;
      this.ProcessTasksInternal();
      return true;
    }

    public bool ProcessNextTask()
    {
      lock (this.taskList)
      {
        if (this.taskList.Count == 0)
          return false;
        this.ProcessSingleTask();
        if (this.TaskCount == 0)
          this.dataEvent.Reset();
      }
      return true;
    }

    public bool ProcessNextTask(WaitHandle exitHandle)
    {
      if (WaitHandle.WaitAny(new WaitHandle[2]
      {
        exitHandle,
        (WaitHandle) this.dataEvent
      }) == 0)
        return false;
      lock (this.taskList)
      {
        this.ProcessSingleTask();
        if (this.TaskCount == 0)
          this.dataEvent.Reset();
      }
      return true;
    }

    private void ProcessTasksInternal()
    {
      lock (this.taskList)
      {
        while (this.taskList.Count != 0)
          this.ProcessSingleTask();
        if (this.TaskCount != 0)
          return;
        this.dataEvent.Reset();
      }
    }

    private void ProcessSingleTask()
    {
      if (this.taskList.Count == 0)
        return;
      TaskBase task = this.taskList[0];
      this.taskList.RemoveAt(0);
      this.RunTask(task);
      if (this.TaskSortingSystem != TaskSortingSystem.ReorderWhenExecuted)
        return;
      this.ReorderTasks();
    }

    internal void RunTask(TaskBase task)
    {
      TaskBase currentTask = Dispatcher.currentTask;
      Dispatcher.currentTask = task;
      Dispatcher.currentTask.DoInternal();
      Dispatcher.currentTask = currentTask;
    }

    protected override void CheckAccessLimitation()
    {
      if (Dispatcher.currentDispatcher == this)
        throw new InvalidOperationException("Dispatching a Task with the Dispatcher associated to the current thread is prohibited. You can run these Tasks without the need of a Dispatcher.");
    }

    public override void Dispose()
    {
      while (true)
      {
        lock (this.taskList)
        {
          if (this.taskList.Count != 0)
          {
            Dispatcher.currentTask = this.taskList[0];
            this.taskList.RemoveAt(0);
          }
          else
            break;
        }
        Dispatcher.currentTask.Dispose();
      }
      this.dataEvent.Close();
      this.dataEvent = (ManualResetEvent) null;
      if (Dispatcher.currentDispatcher == this)
        Dispatcher.currentDispatcher = (Dispatcher) null;
      if (Dispatcher.mainDispatcher != this)
        return;
      Dispatcher.mainDispatcher = (Dispatcher) null;
    }
  }
}
