// Decompiled with JetBrains decompiler
// Type: ConsoleAddMessageEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Networking.Events;
using System;

#nullable disable
public class ConsoleAddMessageEvent : BaseEvent
{
  public ConsoleAddMessageEvent()
    : this((string) null, (Action<BaseEvent>) null)
  {
  }

  public ConsoleAddMessageEvent(string message)
    : this(message, (Action<BaseEvent>) null)
  {
  }

  public ConsoleAddMessageEvent(string message, Action<BaseEvent> callback)
    : base(callback)
  {
    this.Message = message;
  }

  public string Message { get; protected set; }
}
