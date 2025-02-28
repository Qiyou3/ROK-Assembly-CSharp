// Decompiled with JetBrains decompiler
// Type: GunAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

#nullable disable
public class GunAnimation : EntityBehaviour
{
  public string animationName = "Firing";
  public GunAnimation.GunAnimationType animationType;
  public WrapMode WrapAnimation;
  private float _speed;

  public void Animate(float length = -1, float delay = 0, GunAnimation.GunAnimationSnap snap = GunAnimation.GunAnimationSnap.Start)
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    this.StartCoroutine(this.DelayedAnimate(length, delay, snap));
  }

  [DebuggerHidden]
  public IEnumerator DelayedAnimate(float length = -1, float delay = 0, GunAnimation.GunAnimationSnap snap = GunAnimation.GunAnimationSnap.Start)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new GunAnimation.\u003CDelayedAnimate\u003Ec__IteratorFE()
    {
      length = length,
      snap = snap,
      delay = delay,
      \u003C\u0024\u003Elength = length,
      \u003C\u0024\u003Esnap = snap,
      \u003C\u0024\u003Edelay = delay,
      \u003C\u003Ef__this = this
    };
  }

  public void ResetAll()
  {
    ((IEnumerable<GunAnimation>) this.Entity.TryGetArray<GunAnimation>()).ToList<GunAnimation>().ForEach((Action<GunAnimation>) (ga => ga.Reset()));
  }

  public void Pause() => this.GetComponent<Animation>()[this.animationName].speed = 0.0f;

  public void Resume() => this.GetComponent<Animation>()[this.animationName].speed = this._speed;

  public void Reset()
  {
    this.GetComponent<Animation>().enabled = true;
    this.GetComponent<Animation>().Rewind(this.animationName);
    this.GetComponent<Animation>().Sample();
  }

  public void Finish(float time)
  {
    this.GetComponent<Animation>()[this.animationName].speed = (this.GetComponent<Animation>()[this.animationName].length - this.GetComponent<Animation>()[this.animationName].time) / time;
  }

  public void OnDisable() => this.Reset();

  public enum GunAnimationType
  {
    Fire,
    Chamber,
    Reload,
    Equip,
    LoadShell,
  }

  public enum GunAnimationSnap
  {
    Start,
    End,
  }
}
