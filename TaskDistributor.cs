// Decompiled with JetBrains decompiler
// Type: UnityThreading.TaskDistributor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Threading;
using UnityEngine;

#nullable disable
namespace UnityThreading
{
  public class TaskDistributor : DispatcherBase
  {
    private TaskWorker[] workerThreads;
    private static TaskDistributor mainTaskDistributor;

    public TaskDistributor()
      : this(0)
    {
    }

    public TaskDistributor(int workerThreadCount)
      : this(workerThreadCount, true)
    {
    }

    public TaskDistributor(int workerThreadCount, bool autoStart)
    {
      if (workerThreadCount <= 0)
        workerThreadCount = SystemInfo.processorCount * 2;
      this.workerThreads = new TaskWorker[workerThreadCount];
      lock (this.workerThreads)
      {
        for (int index = 0; index < workerThreadCount; ++index)
          this.workerThreads[index] = new TaskWorker(this);
      }
      if (TaskDistributor.mainTaskDistributor == null)
        TaskDistributor.mainTaskDistributor = this;
      if (!autoStart)
        return;
      this.Start();
    }

    internal WaitHandle NewDataWaitHandle => (WaitHandle) this.dataEvent;

    public static TaskDistributor Main
    {
      get
      {
        return TaskDistributor.mainTaskDistributor != null ? TaskDistributor.mainTaskDistributor : throw new InvalidOperationException("No default TaskDistributor found, please create a new TaskDistributor instance before calling this property.");
      }
    }

    public void Start()
    {
      lock (this.workerThreads)
      {
        for (int index = 0; index < this.workerThreads.Length; ++index)
        {
          if (!this.workerThreads[index].IsAlive)
          {
            this.workerThreads[index].Dispatcher.AddTasks(this.SplitTasks(this.workerThreads.Length));
            this.workerThreads[index].Start();
          }
        }
      }
    }

    internal void FillTasks(Dispatcher target) => target.AddTasks(this.IsolateTasks(1));

    protected override void CheckAccessLimitation()
    {
      if (ThreadBase.CurrentThread != null && ThreadBase.CurrentThread is TaskWorker && ((TaskWorker) ThreadBase.CurrentThread).TaskDistributor == this)
        throw new InvalidOperationException("Access to TaskDistributor prohibited when called from inside a TaskDistributor thread. Dont dispatch new Tasks through the same TaskDistributor. If you want to distribute new tasks create a new TaskDistributor and use the new created instance. Remember to dispose the new instance to prevent thread spamming.");
    }

    public override void Dispose()
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
      lock (this.workerThreads)
      {
        for (int index = 0; index < this.workerThreads.Length; ++index)
          this.workerThreads[index].Dispose();
        this.workerThreads = new TaskWorker[0];
      }
      this.dataEvent.Close();
      this.dataEvent = (ManualResetEvent) null;
      if (TaskDistributor.mainTaskDistributor != this)
        return;
      TaskDistributor.mainTaskDistributor = (TaskDistributor) null;
    }
  }
}
