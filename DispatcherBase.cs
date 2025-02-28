// Decompiled with JetBrains decompiler
// Type: UnityThreading.DispatcherBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#nullable disable
namespace UnityThreading
{
  public abstract class DispatcherBase : IDisposable
  {
    protected List<TaskBase> taskList = new List<TaskBase>();
    protected ManualResetEvent dataEvent = new ManualResetEvent(false);
    public TaskSortingSystem TaskSortingSystem;

    public int TaskCount
    {
      get
      {
        lock (this.taskList)
          return this.taskList.Count;
      }
    }

    public Task<T> Dispatch<T>(Func<T> function)
    {
      this.CheckAccessLimitation();
      Task<T> task = new Task<T>(function);
      this.AddTask((TaskBase) task);
      return task;
    }

    public Task Dispatch(Action action)
    {
      this.CheckAccessLimitation();
      Task task = new Task(action);
      this.AddTask((TaskBase) task);
      return task;
    }

    public TaskBase Dispatch(TaskBase task)
    {
      this.CheckAccessLimitation();
      this.AddTask(task);
      return task;
    }

    internal void AddTask(TaskBase task)
    {
      lock (this.taskList)
      {
        this.taskList.Add(task);
        if (this.TaskSortingSystem == TaskSortingSystem.ReorderWhenAdded || this.TaskSortingSystem == TaskSortingSystem.ReorderWhenExecuted)
          this.ReorderTasks();
        this.dataEvent.Set();
      }
    }

    internal void AddTasks(IEnumerable<TaskBase> tasks)
    {
      lock (this.taskList)
      {
        foreach (TaskBase task in tasks)
          this.taskList.Add(task);
        if (this.TaskSortingSystem == TaskSortingSystem.ReorderWhenAdded || this.TaskSortingSystem == TaskSortingSystem.ReorderWhenExecuted)
          this.ReorderTasks();
        this.dataEvent.Set();
      }
    }

    protected void ReorderTasks()
    {
      this.taskList.Sort((Comparison<TaskBase>) ((a, b) => -a.Priority.CompareTo(b.Priority)));
    }

    internal IEnumerable<TaskBase> SplitTasks(int divisor)
    {
      if (divisor == 0)
        divisor = 2;
      return this.IsolateTasks(this.TaskCount / divisor);
    }

    internal IEnumerable<TaskBase> IsolateTasks(int count)
    {
      List<TaskBase> taskBaseList = new List<TaskBase>();
      if (count == 0)
        count = this.taskList.Count;
      lock (this.taskList)
      {
        taskBaseList.AddRange(this.taskList.Take<TaskBase>(count));
        this.taskList.RemoveRange(0, Math.Min(count, this.taskList.Count));
        if (this.TaskSortingSystem == TaskSortingSystem.ReorderWhenExecuted)
          this.ReorderTasks();
        if (this.TaskCount == 0)
          this.dataEvent.Reset();
      }
      return (IEnumerable<TaskBase>) taskBaseList;
    }

    protected abstract void CheckAccessLimitation();

    public virtual void Dispose()
    {
      while (true)
      {
        TaskBase task;
        lock (this.taskList)
        {
          if (this.taskList.Count != 0)
          {
            task = this.taskList[0];
            this.taskList.RemoveAt(0);
          }
          else
            break;
        }
        task.Dispose();
      }
      this.dataEvent.Close();
      this.dataEvent = (ManualResetEvent) null;
    }
  }
}
