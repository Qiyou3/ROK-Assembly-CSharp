// Decompiled with JetBrains decompiler
// Type: EyeLookAnimate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch;
using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public class EyeLookAnimate : EntityBehaviour
{
  public Transform eyeForward;
  public Transform headForward;
  public Transform upperEyelidRight;
  public Transform lowerEyelidRight;
  public Transform eyeball;
  public Transform upperEyelid;
  public Transform lowerEyelid;
  private Quaternion randomOffset = Quaternion.identity;
  public float randomEyeMoveAngle = 1f;
  private float randomTimer;
  public float eyeMoveMinTime = 0.2f;
  public float eyeMoveMaxTime = 2f;
  private float blinkTimer;
  private float blink;
  private float blinkTime;
  private float lastBlinkTimer;
  public float blinkMinTime = 1f;
  public float blinkMaxTime = 3f;
  public float blinkTimeMultiplier = 0.5f;
  private Transform _target;
  private float _lookStrength;
  private Quaternion _lookRotation;

  public void Start()
  {
    this.randomTimer = Random.Range(this.eyeMoveMinTime, this.eyeMoveMaxTime);
    this.blinkTimer = Random.Range(this.blinkMinTime, this.blinkMaxTime);
    this.lastBlinkTimer = this.blinkTimer;
  }

  public void LateUpdate()
  {
    this.randomTimer -= Time.deltaTime;
    this.blinkTimer -= Time.deltaTime;
    if ((double) this.randomTimer < 0.0)
    {
      this.randomOffset = Quaternion.Slerp(Quaternion.identity, Random.rotation, this.randomEyeMoveAngle / 360f);
      this.randomTimer += Random.Range(this.eyeMoveMinTime, this.eyeMoveMaxTime);
    }
    if ((double) this.blinkTimer < 0.0)
    {
      this.blink = 1f;
      this.blinkTime = this.lastBlinkTimer * this.blinkTimeMultiplier;
      this.blinkTimer += Random.Range(this.blinkMinTime, this.blinkMaxTime);
      this.lastBlinkTimer = this.blinkTimer;
    }
    this.blink += (float) ((0.0 - (double) this.blink) * (1.0 - (double) Mathf.Pow(0.5f, Time.deltaTime / this.blinkTime)));
    LookBridge lookBridge = this.Entity.Get<LookBridge>();
    Quaternion quaternion = Quaternion.Slerp(this.eyeForward.rotation, this.randomOffset * lookBridge.Rotation, lookBridge.Strength);
    this.eyeball.rotation = Quaternion.RotateTowards(Quaternion.identity, Quaternion.FromToRotation(this.eyeForward.forward, quaternion * Vector3.forward), 45f) * this.eyeball.rotation;
    this.upperEyelid.rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.RotateTowards(Quaternion.identity, Quaternion.LookRotation(this.headForward.right, quaternion * Vector3.forward) * Quaternion.Inverse(this.upperEyelidRight.rotation), 22.5f), 0.5f) * this.upperEyelid.rotation;
    this.lowerEyelid.rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.RotateTowards(Quaternion.identity, Quaternion.LookRotation(this.headForward.right, quaternion * Vector3.forward) * Quaternion.Inverse(this.lowerEyelidRight.rotation), 22.5f), 0.5f) * this.lowerEyelid.rotation;
    Quaternion to = Quaternion.Slerp(this.lowerEyelid.rotation, this.upperEyelid.rotation, 0.5f);
    this.upperEyelid.rotation = Quaternion.Slerp(this.upperEyelid.rotation, to, this.blink);
    this.lowerEyelid.rotation = Quaternion.Slerp(this.lowerEyelid.rotation, to, this.blink);
  }
}
