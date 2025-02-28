// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.EnemyDeathSync
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Damaging;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Networking;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Entities.Enemy;
using CodeHatch.TerrainAPI;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class EnemyDeathSync : EntityBehaviour
  {
    private Health health;
    private bool notSent = true;

    public void OnEnable()
    {
      this.notSent = true;
      this.StartCoroutine(this.InitialEvent());
    }

    [DebuggerHidden]
    private IEnumerator InitialEvent()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new EnemyDeathSync.\u003CInitialEvent\u003Ec__IteratorC7()
      {
        \u003C\u003Ef__this = this
      };
    }

    public void OnDisable()
    {
      EventManager.Unsubscribe<EnemyDeathSyncEvent>(new EventSubscriber<EnemyDeathSyncEvent>(this.OnEvent));
    }

    private void CallEvent(ulong id, bool dead)
    {
      Vector3 terrainHeight = this.transform.localPosition.AdjustToTerrainHeight();
      EventManager.CallEvent((BaseEvent) new EnemyDeathSyncEvent(this.Entity, id, dead, terrainHeight));
    }

    public void OnEvent(EnemyDeathSyncEvent e)
    {
      if (e.IsSender)
        return;
      if (Player.IsLocalServer)
      {
        if ((Object) this.Entity == (Object) null || (Object) e.Entity != (Object) this.Entity)
          return;
        if ((Object) this.health != (Object) null)
        {
          this.CallEvent(e.PlayerID, this.health.IsDead);
        }
        else
        {
          this.health = this.Entity.TryGet<Health>();
          if (!((Object) this.health != (Object) null))
            return;
          this.CallEvent(e.PlayerID, this.health.IsDead);
        }
      }
      else
      {
        if ((Object) e.Entity != (Object) this.Entity || (long) e.PlayerID != (long) Player.Local.Id || !e.IsDead)
          return;
        this.transform.localPosition = e.Position;
        EventManager.CallEvent((BaseEvent) new EnemyDeathEvent(this.Entity, new Damage()));
        if ((Object) this.health == (Object) null)
          this.Entity.TryGet<Health>();
        if ((bool) (Object) this.health)
          this.health.SetHealth(0.0f);
        this.enabled = false;
      }
    }
  }
}
