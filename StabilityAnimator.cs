// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.StabilityAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class StabilityAnimator : EntityBehaviour
  {
    public string stabilityParamName;
    public float groundedPower = -2f;
    public float airPower = 1f;
    private float _stability = 1f;
    public float smoothingHalfLife = 0.2f;

    public Animator MyAnimator => this.Entity.Get<Animator>();

    public MotorBridge MyMotorBridge => this.Entity.GetOrCreate<MotorBridge>();

    public void Update()
    {
      if (!this.MyAnimator.IsInitialized())
        return;
      this._stability += (float) (((double) Mathf.Pow(Mathf.Clamp01(this.MyMotorBridge.Stability), !this.MyMotorBridge.Grounded ? this.airPower : this.groundedPower) - (double) this._stability) * 0.10000000149011612);
      this.MyAnimator.SetFloat(this.stabilityParamName, this._stability);
    }
  }
}
