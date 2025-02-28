// Decompiled with JetBrains decompiler
// Type: UnityThreading.Task`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;

#nullable disable
namespace UnityThreading
{
  public class Task<T> : TaskBase
  {
    private Func<T> function;
    private T result;

    public Task(Func<T> function) => this.function = function;

    protected override IEnumerator Do()
    {
      this.result = this.function();
      return (IEnumerator) null;
    }

    public override TResult Wait<TResult>() => (TResult) (object) this.Result;

    public override TResult WaitForSeconds<TResult>(float seconds)
    {
      return this.WaitForSeconds<TResult>(seconds, default (TResult));
    }

    public override TResult WaitForSeconds<TResult>(float seconds, TResult defaultReturnValue)
    {
      if (!this.HasEnded)
        this.WaitForSeconds(seconds);
      return this.IsSucceeded ? (TResult) (object) this.result : defaultReturnValue;
    }

    public T Result
    {
      get
      {
        if (!this.HasEnded)
          this.Wait();
        return this.result;
      }
    }
  }
}
