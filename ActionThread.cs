// Decompiled with JetBrains decompiler
// Type: UnityThreading.ActionThread
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;

#nullable disable
namespace UnityThreading
{
  public class ActionThread : ThreadBase
  {
    private Action<ActionThread> action;

    public ActionThread(Action<ActionThread> action)
      : this(action, true)
    {
    }

    public ActionThread(Action<ActionThread> action, bool autoStartThread)
      : base(Dispatcher.Current, false)
    {
      this.action = action;
      if (!autoStartThread)
        return;
      this.Start();
    }

    protected override IEnumerator Do()
    {
      this.action(this);
      return (IEnumerator) null;
    }
  }
}
