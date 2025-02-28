// Decompiled with JetBrains decompiler
// Type: AstarSerializer3_05
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using Pathfinding;
using System.IO;
using UnityEngine;

#nullable disable
public class AstarSerializer3_05(AstarPath script) : AstarSerializer3_07(script)
{
  public override UserConnection[] DeserializeUserConnections()
  {
    BinaryReader readerStream = this.readerStream;
    if (!this.MoveToAnchor("UserConnections"))
      return new UserConnection[0];
    int length = readerStream.ReadInt32();
    UserConnection[] userConnectionArray = new UserConnection[length];
    for (int index = 0; index < length; ++index)
      userConnectionArray[index] = new UserConnection()
      {
        p1 = new Vector3(readerStream.ReadSingle(), readerStream.ReadSingle(), readerStream.ReadSingle()),
        p2 = new Vector3(readerStream.ReadSingle(), readerStream.ReadSingle(), readerStream.ReadSingle()),
        doOverrideCost = readerStream.ReadBoolean(),
        overrideCost = readerStream.ReadInt32(),
        oneWay = readerStream.ReadBoolean(),
        width = readerStream.ReadSingle(),
        type = (ConnectionType) readerStream.ReadInt32()
      };
    return userConnectionArray;
  }
}
