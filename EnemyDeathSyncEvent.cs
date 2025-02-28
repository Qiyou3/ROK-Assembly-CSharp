// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.EnemyDeathSyncEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Networking;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Entities.Enemy;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class EnemyDeathSyncEvent : EnemyEvent
  {
    public EnemyDeathSyncEvent()
    {
    }

    public EnemyDeathSyncEvent(Entity entity, ulong playerID, bool isDead = false, [Optional] Vector3 position)
      : this(entity, playerID, (Action<BaseEvent>) null, isDead, position)
    {
    }

    public EnemyDeathSyncEvent(
      Entity entity,
      ulong playerID,
      Action<BaseEvent> cancelCallback,
      bool isDead = false,
      [Optional] Vector3 position)
      : base(entity, cancelCallback)
    {
      this.PlayerID = playerID;
      this.IsDead = isDead;
      this.Position = position;
    }

    public override string PermissionString => "rok.enemy.death";

    public ulong PlayerID { get; set; }

    public bool IsDead { get; set; }

    public Vector3 Position { get; set; }

    public override void Write(IStream stream)
    {
      base.Write(stream);
      stream.Write<bool>(this.IsDead);
      stream.Write<Vector3>(this.Position);
      stream.Write<ulong>(this.PlayerID);
    }

    public override void Read(IStream stream)
    {
      base.Read(stream);
      this.IsDead = stream.Read<bool>();
      this.Position = stream.Read<Vector3>();
      this.PlayerID = stream.Read<ulong>();
    }
  }
}
