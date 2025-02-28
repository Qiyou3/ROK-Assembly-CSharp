// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.JellyPuffExplode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Networking;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Entities.Enemy;
using uLink;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class JellyPuffExplode : EntityBehaviour
  {
    public GameObject explosionPrefab;

    public void Start()
    {
      EventManager.Subscribe<EnemyExplodeEvent>(new EventSubscriber<EnemyExplodeEvent>(this.OnExplode));
    }

    public void OnDestroy()
    {
      EventManager.Unsubscribe<EnemyExplodeEvent>(new EventSubscriber<EnemyExplodeEvent>(this.OnExplode));
    }

    private void OnExplode(EnemyExplodeEvent e)
    {
      if (!Player.IsLocalServer || !((Object) e.Entity == (Object) this.Entity))
        return;
      uLink.Network.Instantiate<ulong>(this.explosionPrefab, this.Entity.MainTransform.position, this.Entity.MainTransform.rotation, (NetworkGroup) 0, e.Entity.NetViewID);
    }
  }
}
