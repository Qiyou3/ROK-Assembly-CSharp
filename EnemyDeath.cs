// Decompiled with JetBrains decompiler
// Type: EnemyDeath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Networking;
using UnityEngine;

#nullable disable
public class EnemyDeath : UnityEngine.MonoBehaviour
{
  public Animation m_animation;
  public AnimationClip animationClip;
  private AnimationState m_animationState;
  private bool dead;
  public Object[] disableObjects;
  private float deathTimer = 5f;

  public Animation animation
  {
    get
    {
      if ((Object) this.m_animation == (Object) null)
        this.m_animation = this.GetComponent<Animation>();
      if ((Object) this.m_animation == (Object) null)
        this.m_animation = this.GetComponentInChildren<Animation>();
      return this.m_animation;
    }
  }

  public AnimationState animationState
  {
    get
    {
      if ((TrackedReference) this.m_animationState != (TrackedReference) null && (Object) this.m_animationState.clip != (Object) this.animationClip)
        this.m_animationState = (AnimationState) null;
      if ((TrackedReference) this.m_animationState == (TrackedReference) null)
      {
        if ((Object) this.animation != (Object) null)
          this.m_animationState = this.animation[this.animationClip.name];
        if ((TrackedReference) this.m_animationState == (TrackedReference) null)
          this.LogError<EnemyDeath>("m_animationState == null");
      }
      return this.m_animationState;
    }
  }

  public void Update()
  {
    if (!this.GetInterface<IHealth>().IsDead)
      return;
    if (!this.dead)
    {
      this.dead = true;
      this.animation.clip = this.animationClip;
      this.animation.Play();
      foreach (Object disableObject in this.disableObjects)
        Object.Destroy(disableObject);
    }
    this.deathTimer -= Time.deltaTime;
    if (!Player.IsLocalServer || (double) this.deathTimer >= 0.0)
      return;
    uLink.Network.Destroy(this.gameObject);
  }
}
