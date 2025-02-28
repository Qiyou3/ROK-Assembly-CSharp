// Decompiled with JetBrains decompiler
// Type: CodeHatch.Audio.PlayerHealthSounds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Entities.Definitions;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Players;
using UnityEngine;

#nullable disable
namespace CodeHatch.Audio
{
  public class PlayerHealthSounds : EntityBehaviour
  {
    public string[] takeDamageClips;
    public string[] lowHealthClips;
    public string[] deathClips;
    [Range(0.0f, 100f)]
    public float lowHealthPercentage;
    public GameObject character;
    private int rand;

    public void Start()
    {
      EventManager.Subscribe<PlayerDamageEvent>(new EventSubscriber<PlayerDamageEvent>(this.OnDamage));
      EventManager.Subscribe<PlayerDeathEvent>(new EventSubscriber<PlayerDeathEvent>(this.OnDeath));
    }

    public void OnDestroy()
    {
      EventManager.Unsubscribe<PlayerDamageEvent>(new EventSubscriber<PlayerDamageEvent>(this.OnDamage));
      EventManager.Unsubscribe<PlayerDeathEvent>(new EventSubscriber<PlayerDeathEvent>(this.OnDeath));
    }

    public void OnDamage(PlayerDamageEvent e)
    {
      if (!this.IsMe((PlayerEvent) e) || (double) e.Damage.Amount <= 0.0)
        return;
      this.rand = Random.Range(0, this.takeDamageClips.Length);
      AudioController.Play(this.takeDamageClips[this.rand], this.transform.position, this.transform.parent);
      if ((double) this.Entity.Get<PlayerHealth>().CurrentHealthPercent > (double) this.lowHealthPercentage / 100.0)
        return;
      this.rand = Random.Range(0, this.lowHealthClips.Length);
      AudioController.Play(this.lowHealthClips[this.rand], this.transform.position, this.transform.parent);
    }

    public void OnDeath(PlayerDeathEvent e)
    {
      if (!this.IsMe((PlayerEvent) e))
        return;
      this.rand = Random.Range(0, this.deathClips.Length);
      AudioController.Play(this.deathClips[this.rand], this.transform.position);
    }

    private bool IsMe(PlayerEvent e)
    {
      return this.Entity.IsLocallyOwned && !((Object) Entity.LocalPlayer != (Object) this.Entity) && e.Player.IsLocal;
    }
  }
}
