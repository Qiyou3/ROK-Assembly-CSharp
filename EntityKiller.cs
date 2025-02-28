// Decompiled with JetBrains decompiler
// Type: EntityKiller
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Networking;
using UnityEngine;

#nullable disable
public static class EntityKiller
{
  public static bool Kill(Entity entity)
  {
    IHealth health = entity.TryGet<IHealth>();
    return health != null && health.Kill();
  }

  public static bool SilentKill(Entity entity)
  {
    IHealth health = entity.TryGet<IHealth>();
    if (health == null || health.PreventDeath)
      return false;
    if (Player.IsLocalServer && entity.IsNetViewAssigned)
      uLink.Network.Destroy(entity.gameObject);
    else
      Object.Destroy((Object) entity.gameObject);
    return true;
  }
}
