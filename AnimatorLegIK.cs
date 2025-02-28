// Decompiled with JetBrains decompiler
// Type: AnimatorLegIK
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Cache;
using CodeHatch.Common.Attributes;
using System;
using UnityEngine;

#nullable disable
public class AnimatorLegIK : AnimatorBehaviour
{
  private const float _apparentSizeRequired = 100f;
  private const int _traceLength = 2;
  public AnimatorLegIK.Leg _leftLeg;
  public AnimatorLegIK.Leg _rightLeg;
  public int locomotionLayer;
  public float rotationSmoothing = 0.1f;
  public AnimationCurve rotationWeightCurve;
  private Dependency<Transform> _hipTransform;

  public void Awake()
  {
    this._hipTransform = new Dependency<Transform>((Func<Transform>) (() => this.Manager.GetBoneTransform(HumanBodyBones.Hips)), (UnityEngine.Object) this, staticErrorMessage: "Please ensure that the Hip transform is exposed.");
  }

  private Transform HipTransform => this._hipTransform.Get();

  private float GaitDuration
  {
    get => this.Manager.GetCurrentAnimatorStateInfo(this.locomotionLayer).length;
  }

  public override void Initialize(AnimatorBehaviourManager manager)
  {
    throw new NotImplementedException();
  }

  private void PostAnimate(float currentLOD)
  {
    this.ApplyIK(this._leftLeg);
    this.ApplyIK(this._rightLeg);
  }

  private void ApplyIK(AnimatorLegIK.Leg leg) => throw new NotImplementedException();

  [Serializable]
  public class Leg
  {
    public AvatarIKGoal avatarIKGoal;
    [NoEdit]
    public Quaternion plantRotation;

    public Leg(AvatarIKGoal avatarIKGoal)
    {
      this.avatarIKGoal = avatarIKGoal;
      this.plantRotation = Quaternion.identity;
    }
  }
}
