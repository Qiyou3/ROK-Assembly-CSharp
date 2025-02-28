// Decompiled with JetBrains decompiler
// Type: EnvironmentReverb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Tracing;
using UnityEngine;

#nullable disable
public class EnvironmentReverb : EntityBehaviour
{
  public float RangeSmoothing = 0.3f;
  public float SmoothRange;
  public float MaxDistance = 1000f;
  private AudioReverbZone _AudioReverbZone;
  public float roomRolloff;
  public AnimationCurve reverbAmountAtDistance;
  private Animation _Animation;
  private AnimationState _AnimationState;

  public AudioReverbZone AudioReverbZone
  {
    get
    {
      this._AudioReverbZone = this.GetComponent<AudioReverbZone>();
      return this._AudioReverbZone;
    }
  }

  public Animation Animation
  {
    get
    {
      if ((Object) this._Animation == (Object) null)
        this._Animation = this.GetComponent<Animation>();
      return this._Animation;
    }
  }

  public AnimationState AnimationState
  {
    get
    {
      if ((TrackedReference) this._AnimationState == (TrackedReference) null)
        this._AnimationState = this.Animation["ReverbAtDistance"];
      return this._AnimationState;
    }
  }

  public void Start() => this.transform.parent = this.Entity.Get<MainTransform>().transform;

  public void Update()
  {
    this.AnimationState.enabled = true;
    this.AnimationState.normalizedTime = this.SmoothRange / this.MaxDistance;
    this.AnimationState.blendMode = AnimationBlendMode.Blend;
    this.AnimationState.speed = 0.0f;
    this.AnimationState.weight = 1f;
    this.AudioReverbZone.room = (int) (-10000.0 * (1.0 - (double) this.reverbAmountAtDistance.Evaluate(this.SmoothRange / this.MaxDistance)));
    this.AudioReverbZone.roomRolloffFactor = this.roomRolloff;
    if ((Object) this.Entity == (Object) null)
      return;
    this.SmoothRange = HalfLife.GainLoss(this.SmoothRange, Mathf.Min(this.MaxDistance, new Ray(this.transform.position, Random.insideUnitSphere).Raycast(float.PositiveInfinity, this.Entity).distance), this.RangeSmoothing, this.RangeSmoothing);
  }
}
