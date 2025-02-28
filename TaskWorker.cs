// Decompiled with JetBrains decompiler
// Type: UnityThreading.TaskWorker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Threading;

#nullable disable
namespace UnityThreading
{
  internal class TaskWorker : ThreadBase
  {
    public Dispatcher Dispatcher;
    public TaskDistributor TaskDistributor;

    public TaskWorker(TaskDistributor taskDistributor)
      : base(false)
    {
      this.TaskDistributor = taskDistributor;
      this.Dispatcher = new Dispatcher(false);
    }

    protected override IEnumerator Do()
    {
      while (!this.exitEvent.WaitOne(0, false))
      {
        if (!this.Dispatcher.ProcessNextTask())
        {
          this.TaskDistributor.FillTasks(this.Dispatcher);
          if (this.Dispatcher.TaskCount == 0)
          {
            if (WaitHandle.WaitAny(new WaitHandle[2]
            {
              (WaitHandle) this.exitEvent,
              this.TaskDistributor.NewDataWaitHandle
            }) == 0)
              return (IEnumerator) null;
            this.TaskDistributor.FillTasks(this.Dispatcher);
          }
        }
      }
      return (IEnumerator) null;
    }

    public override void Dispose()
    {
      base.Dispose();
      if (this.Dispatcher != null)
        this.Dispatcher.Dispose();
      this.Dispatcher = (Dispatcher) null;
    }
  }
}
