// Decompiled with JetBrains decompiler
// Type: AutoDestroyParticleSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (ParticleSystem))]
public class AutoDestroyParticleSystem : MonoBehaviour
{
  private ParticleSystem _particleSystem;

  private bool GetParticleSystem()
  {
    if ((Object) this._particleSystem == (Object) null)
      this._particleSystem = this.GetComponent<ParticleSystem>();
    return (Object) this._particleSystem != (Object) null;
  }

  public void PlayUntilEnd()
  {
    if (!this.GetParticleSystem())
      return;
    this._particleSystem.Play(true);
    Object.Destroy((Object) this._particleSystem.gameObject, this._particleSystem.duration + this._particleSystem.startLifetime);
  }
}
