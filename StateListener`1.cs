// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.StateListener`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Networking.Events;
using System;

#nullable disable
namespace CodeHatch.AI
{
  public abstract class StateListener<T> : EntityBehaviour where T : struct, IConvertible
  {
    public void Awake()
    {
      EventManager.Subscribe<StateChangedEvent>(new EventSubscriber<StateChangedEvent>(this.StateChangedEventHandler));
    }

    public void OnDestroy()
    {
      EventManager.Unsubscribe<StateChangedEvent>(new EventSubscriber<StateChangedEvent>(this.StateChangedEventHandler));
    }

    private void StateChangedEventHandler(StateChangedEvent e)
    {
      if ((UnityEngine.Object) e.Entity != (UnityEngine.Object) this.Entity)
        return;
      this.OnStateChanged(e.GetState<T>());
    }

    protected abstract void OnStateChanged(T newState);
  }
}
