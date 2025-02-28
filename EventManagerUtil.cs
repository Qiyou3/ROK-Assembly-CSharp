// Decompiled with JetBrains decompiler
// Type: EventManagerUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Networking;
using CodeHatch.Networking.Events;
using System;
using System.Collections.Generic;

#nullable disable
public static class EventManagerUtil
{
  private static readonly object[] _handlerArgList = new object[1];
  private static readonly List<Player> _singlePlayerList = new List<Player>(1);
  private static readonly System.Type _objectType = typeof (object);

  public static object[] GetHandlerArgList(BaseEvent theEvent)
  {
    EventManagerUtil._handlerArgList[0] = (object) theEvent;
    return EventManagerUtil._handlerArgList;
  }

  public static List<Player> GetSinglePlayerList(Player player)
  {
    int count = EventManagerUtil._singlePlayerList.Count;
    switch (EventManagerUtil._singlePlayerList.Count)
    {
      case 0:
        EventManagerUtil._singlePlayerList.Add(player);
        break;
      case 1:
        EventManagerUtil._singlePlayerList[0] = player;
        break;
      case 2:
        Logger.Error("Unexpected: _singlePlayerList was modified outside of GetSinglePlayerList!");
        EventManagerUtil._singlePlayerList.Clear();
        EventManagerUtil._singlePlayerList.Add(player);
        break;
    }
    return EventManagerUtil._singlePlayerList;
  }

  public static void ProfileEventSend(NetworkEvent theEvent, int bytesSent)
  {
  }

  public static void ProfileEventReceive(Player sender, NetworkEvent theEvent, int bytesReceived)
  {
  }

  public static void ProfileSimpleEventSend(string id, SimpleEventArgs args, int bytesSent)
  {
  }

  public static void ProfileSimpleEventReceive(Player sender, string id, int bytesReceived)
  {
  }

  public static bool MoveNextType(ref System.Type type)
  {
    if (type == null)
      throw new ArgumentNullException(nameof (type), "When using this in an iterator,be sure to break when this method returns false,and ensure you've provided a valid event type!");
    if (type == EventManagerUtil._objectType)
      throw new ArgumentException(nameof (type), "When using this in an iterator,be sure to break when this method returns false,and ensure you've provided a valid event type!");
    type = type.BaseType;
    if (type == EventManagerUtil._objectType)
      return false;
    if (type != null)
      return true;
    Logger.Error("Unexpected: Reached null before typeof(object)!");
    return false;
  }
}
