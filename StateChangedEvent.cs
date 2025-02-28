// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.StateChangedEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Networking;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Entities;
using System;

#nullable disable
namespace CodeHatch.AI
{
  public class StateChangedEvent : EntityEvent
  {
    private int _stateAsInt;

    public StateChangedEvent()
      : this((Entity) null, (IConvertible) 0)
    {
    }

    public StateChangedEvent(Entity entity, IConvertible state, Action<BaseEvent> cancelCallback = null)
      : base(entity, cancelCallback)
    {
      this._stateAsInt = Convert.ToInt32((object) state);
    }

    public TEnum GetState<TEnum>() where TEnum : struct, IConvertible
    {
      return (TEnum) (ValueType) this._stateAsInt;
    }

    public override void Write(IStream stream)
    {
      base.Write(stream);
      stream.Write<int>(this._stateAsInt);
    }

    public override void Read(IStream stream)
    {
      base.Read(stream);
      this._stateAsInt = stream.Read<int>();
    }
  }
}
