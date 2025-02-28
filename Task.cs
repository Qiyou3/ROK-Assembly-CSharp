// Decompiled with JetBrains decompiler
// Type: UnityThreading.Task
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;

#nullable disable
namespace UnityThreading
{
  public class Task : TaskBase
  {
    private Action action;

    public Task(Action action) => this.action = action;

    protected override IEnumerator Do()
    {
      this.action();
      return (IEnumerator) null;
    }
  }
}
