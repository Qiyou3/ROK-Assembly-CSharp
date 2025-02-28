// Decompiled with JetBrains decompiler
// Type: AstarSerializer3_01
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using Pathfinding;
using System.IO;
using UnityEngine;

#nullable disable
public class AstarSerializer3_01(AstarPath script) : AstarSerializer3_04(script)
{
  public override void SerializeUserConnections(UserConnection[] userConnections)
  {
    this.LogInfo<AstarSerializer3_01>("Loading from 3.0.1");
    BinaryWriter writerStream = this.writerStream;
    this.AddAnchor("UserConnections");
    if (userConnections != null)
    {
      writerStream.Write(userConnections.Length);
      for (int index = 0; index < userConnections.Length; ++index)
      {
        UserConnection userConnection = userConnections[index];
        writerStream.Write(userConnection.p1.x);
        writerStream.Write(userConnection.p1.y);
        writerStream.Write(userConnection.p1.z);
        writerStream.Write(userConnection.p2.x);
        writerStream.Write(userConnection.p2.y);
        writerStream.Write(userConnection.p2.z);
        writerStream.Write(userConnection.overrideCost);
        writerStream.Write(userConnection.oneWay);
        writerStream.Write(userConnection.width);
        this.LogInfo<AstarSerializer3_01>("End - {0}", (object) writerStream.BaseStream.Position);
      }
    }
    else
      writerStream.Write(0);
  }

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
        overrideCost = readerStream.ReadInt32(),
        oneWay = readerStream.ReadBoolean(),
        width = readerStream.ReadSingle()
      };
    return userConnectionArray;
  }
}
