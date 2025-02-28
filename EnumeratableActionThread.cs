// Decompiled with JetBrains decompiler
// Type: UnityThreading.EnumeratableActionThread
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;

#nullable disable
namespace UnityThreading
{
  public class EnumeratableActionThread : ThreadBase
  {
    private Func<ThreadBase, IEnumerator> enumeratableAction;

    public EnumeratableActionThread(Func<ThreadBase, IEnumerator> enumeratableAction)
      : this(enumeratableAction, true)
    {
    }

    public EnumeratableActionThread(
      Func<ThreadBase, IEnumerator> enumeratableAction,
      bool autoStartThread)
      : base(Dispatcher.Current, false)
    {
      this.enumeratableAction = enumeratableAction;
      if (!autoStartThread)
        return;
      this.Start();
    }

    protected override IEnumerator Do() => this.enumeratableAction((ThreadBase) this);
  }
}
