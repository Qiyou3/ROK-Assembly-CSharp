// Decompiled with JetBrains decompiler
// Type: CaptureExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Networking;
using CodeHatch.StarForge.Sleeping;
using CodeHatch.Thrones.Capture;
using UnityEngine;

#nullable disable
public static class CaptureExtensions
{
  public static bool CanCapture(this Entity entity)
  {
    return entity.Has<PlayerCaptureManager>() && entity.Get<PlayerCaptureManager>().CanCapture;
  }

  public static bool CanBeCaptured(this Entity entity)
  {
    return entity.Has<PlayerCaptureManager>() && entity.Get<PlayerCaptureManager>().CanBeCaptured;
  }

  public static bool CanBeTied(this Entity entity)
  {
    return entity.Has<PlayerCaptureManager>() && entity.Get<PlayerCaptureManager>().CanBeTied;
  }

  public static bool Captured(this Player player)
  {
    return (Object) player.Entity != (Object) null && player.Entity.Has<PlayerCaptureManager>() && player.Entity.Get<PlayerCaptureManager>().Captured;
  }

  public static bool Captured(this Entity entity)
  {
    return (Object) entity != (Object) null && entity.Has<PlayerCaptureManager>() && entity.Get<PlayerCaptureManager>().Captured;
  }

  public static bool HoldingCaptive(this Player player)
  {
    return (Object) player.Entity != (Object) null && player.Entity.Has<PlayerCaptureManager>() && player.Entity.Get<PlayerCaptureManager>().HoldingCaptive;
  }

  public static bool ExistsForCapture(this ulong playerID)
  {
    return (long) playerID != (long) CodeHatch.Engine.Networking.Server.ServerPlayer.Id && CodeHatch.Engine.Networking.Server.GetPlayerById(playerID) != null || PlayerSleeperObject.AllSleeperObjects.ContainsKey(playerID);
  }
}
