// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.RemoteTargeter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Networking.Events;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class RemoteTargeter : AIBehaviour
  {
    public void Awake()
    {
      EventManager.Subscribe<TargetChangedEvent>(new EventSubscriber<TargetChangedEvent>(this.OnTargetChanged));
      EventManager.Subscribe<TargetLostEvent>(new EventSubscriber<TargetLostEvent>(this.OnTargetLost));
    }

    public void OnDestroy()
    {
      EventManager.Unsubscribe<TargetChangedEvent>(new EventSubscriber<TargetChangedEvent>(this.OnTargetChanged));
      EventManager.Unsubscribe<TargetLostEvent>(new EventSubscriber<TargetLostEvent>(this.OnTargetLost));
    }

    private void OnTargetChanged(TargetChangedEvent e)
    {
      if ((Object) e.Entity != (Object) this.Entity)
        return;
      this.CurrentTarget = e.Targetable;
    }

    private void OnTargetLost(TargetLostEvent e)
    {
      if ((Object) e.Entity != (Object) this.Entity)
        return;
      this.CurrentTarget = (Targetable) null;
    }
  }
}
