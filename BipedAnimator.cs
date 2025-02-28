// Decompiled with JetBrains decompiler
// Type: BipedAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch;
using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Core.Utility;
using CodeHatch.Engine.Core.Utility.Attributes;
using CodeHatch.Thrones.Waving;
using System;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public class BipedAnimator : AnimatorBehaviour
{
  public BipedLocomotionAnimation DefaultBipedLocomotionAnimation;
  [CanBeNull]
  public BipedLocomotionAnimation BipedLocomotionAnimationOverride;
  public bool LookUseCurvesOverride;
  public float LookCurvesTime;
  public float LookWeightsOverrideWeight;
  private readonly BipedAnimator.LookWeights _lookWeightsToUse = new BipedAnimator.LookWeights();
  public float LimitsOverrideWeight;
  public AnimationCurve RunPhaseAtJumpVelocity;
  private BipedAnimator.Limits _limitsToUse = new BipedAnimator.Limits();
  private BipedAnimator.LocomotionHelper _locomotion;
  private BipedAnimator.ViewTranslationHelper _viewOffsetHelper;
  private BipedAnimator.TorsoPositioningHelper _torsoPositioningHelper;
  public BipedAnimator.FeetAnchorHelper FeetAnchorHelp = new BipedAnimator.FeetAnchorHelper();
  public BipedAnimator.FeetIKHelper FeetIKHelp = new BipedAnimator.FeetIKHelper();
  public int FullBodyAnimationLayer = 9;
  public int TorsoAnimationLayer = 6;
  public Transform AvatarAnchorLegs;
  public Transform AvatarAnchorTorso;
  public Transform ViewAnchor;
  public AnimationCurve HipHeightForCrouchBend;
  public AnimationCurve HipHeightWeighting;
  [HideInInspector]
  public Transform FeetAnchor;
  [HideInInspector]
  public Transform AvatarAnchorLegsRotated;
  [HideInInspector]
  public Transform AvatarAnchorTorsoRotated;
  private LookBridge _lookBridge;
  private Transform _eyesCenter;
  private float _lastWeight;
  private bool _isClipAutomatic;
  private float _clipSpeedMultiplier;
  private float _clipWeightMultiplier;

  public BipedLocomotionAnimation BipedLocomotionToUse
  {
    get
    {
      return (UnityEngine.Object) this.BipedLocomotionAnimationOverride != (UnityEngine.Object) null ? this.BipedLocomotionAnimationOverride : this.DefaultBipedLocomotionAnimation;
    }
  }

  private BipedAnimator.LookWeights DefaultLookWeights
  {
    get
    {
      return (UnityEngine.Object) this.BipedLocomotionAnimationOverride != (UnityEngine.Object) null ? this.BipedLocomotionAnimationOverride.LookWeights : this.DefaultBipedLocomotionAnimation.LookWeights;
    }
  }

  private BipedAnimator.Limits DefaultLimits
  {
    get
    {
      return (UnityEngine.Object) this.BipedLocomotionAnimationOverride != (UnityEngine.Object) null ? this.BipedLocomotionAnimationOverride.HeadTranslationLimits : this.DefaultBipedLocomotionAnimation.HeadTranslationLimits;
    }
  }

  private float DefaultTorsoRotation
  {
    get
    {
      return (UnityEngine.Object) this.BipedLocomotionAnimationOverride != (UnityEngine.Object) null ? this.BipedLocomotionAnimationOverride.TorsoRotation : this.DefaultBipedLocomotionAnimation.TorsoRotation;
    }
  }

  public BipedAnimator.LookWeights LookWeightsOverride { get; set; }

  public BipedAnimator.LookCurves LookCurvesOverride { get; set; }

  private BipedAnimator.LookWeights LookWeightsToUse
  {
    get
    {
      if (this.LookWeightsOverride == null || (double) this.LookWeightsOverrideWeight <= 0.0)
        return this.DefaultLookWeights;
      BipedAnimator.LookWeights to;
      if (this.LookUseCurvesOverride)
      {
        this.LookCurvesOverride.ApplyCurve(this._lookWeightsToUse, this.LookCurvesTime);
        to = this._lookWeightsToUse;
      }
      else
        to = this.LookWeightsOverride;
      if ((double) this.LookWeightsOverrideWeight >= 1.0)
        return to;
      this._lookWeightsToUse.Lerp(this.DefaultLookWeights, to, this.LookWeightsOverrideWeight);
      return this._lookWeightsToUse;
    }
  }

  public BipedAnimator.Limits LimitsOverride { get; set; }

  private BipedAnimator.Limits LimitsToUse
  {
    get
    {
      if (this.LimitsOverride == null || (double) this.LimitsOverrideWeight <= 0.0)
        return this.DefaultLimits;
      if ((double) this.LimitsOverrideWeight >= 1.0)
        return this.LimitsOverride;
      this._limitsToUse.Lerp(this.DefaultLimits, this.LimitsOverride, this.LimitsOverrideWeight);
      return this._limitsToUse;
    }
  }

  public override void Initialize(AnimatorBehaviourManager manager)
  {
    this.FeetAnchor = new GameObject("Feet Anchor").transform;
    this.FeetAnchor.parent = this.AvatarAnchorLegs;
    this.FeetAnchor.localPosition = Vector3.zero;
    this.FeetAnchor.localRotation = Quaternion.identity;
    this.FeetAnchor.localScale = Vector3.one;
    this.AvatarAnchorLegsRotated = new GameObject("Rotated").transform;
    this.AvatarAnchorLegsRotated.parent = this.AvatarAnchorLegs;
    this.AvatarAnchorLegsRotated.localPosition = Vector3.zero;
    this.AvatarAnchorLegsRotated.localRotation = Quaternion.identity;
    this.AvatarAnchorLegsRotated.localScale = Vector3.one;
    this.AvatarAnchorTorsoRotated = new GameObject("Rotated").transform;
    this.AvatarAnchorTorsoRotated.parent = this.AvatarAnchorTorso;
    this.AvatarAnchorTorsoRotated.localPosition = Vector3.zero;
    this.AvatarAnchorTorsoRotated.localRotation = Quaternion.identity;
    this.AvatarAnchorTorsoRotated.localScale = Vector3.one;
    this._locomotion = new BipedAnimator.LocomotionHelper(manager, this.FeetAnchor, this.RunPhaseAtJumpVelocity);
    this._viewOffsetHelper = new BipedAnimator.ViewTranslationHelper(manager, this.ViewAnchor, this.AvatarAnchorTorsoRotated);
    this._torsoPositioningHelper = new BipedAnimator.TorsoPositioningHelper(manager, this.AvatarAnchorTorsoRotated);
    this.FeetAnchorHelp.Init(this.Entity.GetOrCreate<MotorBridge>(), this.AvatarAnchorLegsRotated, this.FeetAnchor);
    this.FeetIKHelp.Init(manager, this.AvatarAnchorLegsRotated, this.FeetAnchor);
    this._lookBridge = this.Entity.GetOrCreate<LookBridge>();
    this._eyesCenter = this.Manager.GetBoneTransform(CharacterDefinition.Part.EyesCenter);
    manager.ApplyRootMotion = false;
    manager.AddPass(new System.Action(this.PreAnimate), AnimatorBehaviourPass.PreAnimate, 1000);
    manager.AddPass(new System.Action(this.PostAnimate), AnimatorBehaviourPass.PostAnimate, 1000);
  }

  private void PreAnimate()
  {
    this.Manager.SetLookAtPosition(this._eyesCenter.position + this._lookBridge.Forward * 1000f, this.LookWeightsToUse.WeightTorso, this.LookWeightsToUse.WeightHead, this.LookWeightsToUse.WeightEyes);
    this.AvatarAnchorTorsoRotated.localPosition = Vector3.zero;
    this.AvatarAnchorTorsoRotated.rotation = this.GetDesiredLookRotation(this.AvatarAnchorTorso.up);
    this.AvatarAnchorLegsRotated.rotation = this.GetDesiredLookRotation(this.AvatarAnchorLegs.up);
    float num1 = Vector3.Dot(this.AvatarAnchorTorsoRotated.position - this.AvatarAnchorLegsRotated.position, this.AvatarAnchorLegsRotated.up);
    Quaternion rotation = this.ApplyTorsoRotation(this.BendRotation(this.AvatarAnchorTorsoRotated.rotation, num1));
    this.AvatarAnchorTorsoRotated.rotation = this.ApplyTorsoRotation(this.AvatarAnchorTorsoRotated.rotation);
    this.AvatarAnchorLegsRotated.rotation = this.ApplyTorsoRotation(this.AvatarAnchorLegsRotated.rotation);
    this.Manager.SetAvatarRotation(rotation);
    float num2 = 1f - this.HipHeightWeighting.Evaluate(num1);
    this.AvatarAnchorTorsoRotated.position += (this.AvatarAnchorLegsRotated.position - this.AvatarAnchorTorsoRotated.position).Parallel(this.AvatarAnchorLegsRotated.up) * num2;
    this.FeetAnchorHelp.UpdateFeetAnchor();
    this._locomotion.UpdateAnimator(this.BipedLocomotionToUse);
    this.UpdateAutoClip();
  }

  private Quaternion GetDesiredLookRotation(Vector3 desiredUp)
  {
    Vector3 normalized = this._lookBridge.Forward.Perpendicular(desiredUp).normalized;
    float num1 = Vector3.Angle(this._lookBridge.Forward, desiredUp);
    if ((double) num1 > 90.0)
      num1 = 180f - num1;
    float num2 = num1 / 90f;
    Vector3 from = Quaternion.FromToRotation(this._lookBridge.Up, desiredUp) * this._lookBridge.Forward;
    float num3 = Mathf.Clamp01((180f - Vector3.Angle(this._lookBridge.Up, desiredUp)) / 90f);
    float num4 = num2 * num2;
    float num5 = num3 * num3;
    float num6 = num4 + num5;
    float t = (double) num6 <= 0.0 ? 0.5f : num4 / num6;
    return QuaternionUtil.LookRotation(Vector3.Slerp(from, normalized, t), desiredUp, true);
  }

  public Quaternion BendRotation(Quaternion input, float hipHeight)
  {
    float multiplier = this.HipHeightForCrouchBend.Evaluate(hipHeight);
    if ((double) multiplier > 0.0)
      input = Quaternion.FromToRotation(input * Vector3.forward, -this.AvatarAnchorLegsRotated.up).GetScaled(multiplier) * input;
    float weightHips = this.LookWeightsToUse.WeightHips;
    if ((double) weightHips > 0.0)
      input = Quaternion.Slerp(Quaternion.identity, Quaternion.FromToRotation(input * Vector3.forward, this._lookBridge.Forward), weightHips) * input;
    return input;
  }

  public Quaternion ApplyTorsoRotation(Quaternion input)
  {
    return input * Quaternion.AngleAxis(this.DefaultTorsoRotation, Vector3.up);
  }

  private void PostAnimate()
  {
    this.FeetIKHelp.GetRelativeIKGoals();
    this._torsoPositioningHelper.AlignHips();
    this._viewOffsetHelper.ApplyViewOffsetLimiting(this.LimitsToUse, this.BipedLocomotionToUse.BaseOffset, this.Manager.DeltaTime);
    float layerWeight = this.Manager.GetLayerWeight(this.TorsoAnimationLayer);
    if ((double) layerWeight > 0.0099999997764825821)
    {
      Quaternion quaternion = Quaternion.AngleAxis(this.Manager.MyAnimator.GetFloat("BodyRotationY"), Vector3.down);
      this.FeetAnchor.rotation = Quaternion.Slerp(this.FeetAnchor.rotation, this.FeetAnchor.rotation * (((this.AvatarAnchorTorsoRotated.rotation.GetInverse() * (this.Manager.GetBoneTransform(HumanBodyBones.Hips).rotation * Quaternion.LookRotation(Vector3.forward, Vector3.right).GetInverse())).ToAngleAxis() with
      {
        x = 0.0f,
        z = 0.0f
      }).AngleAxisQuaternion() * quaternion.GetInverse()), layerWeight);
    }
    this.FeetIKHelp.SetIKGoals(this.BipedLocomotionToUse, this.Manager.MyAnimator.GetLayerApparentWeight(this.TorsoAnimationLayer), this.Manager.DeltaTime);
    this.FeetIKHelp.LiftHips();
    this._locomotion.UpdateVelocity(this.BipedLocomotionToUse, this.Manager.DeltaTime);
  }

  public AnimationClipEx CurrentClip { get; private set; }

  public float CurrentClipWeight
  {
    get => (UnityEngine.Object) this.CurrentClip == (UnityEngine.Object) null ? 0.0f : this._lastWeight;
  }

  public bool IsPlayingClip => (UnityEngine.Object) this.CurrentClip != (UnityEngine.Object) null;

  public float ElapsedProgress { get; private set; }

  public void PlayClipManual(AnimationClipEx clip, float progress = 0, float weightMultiplier = 1)
  {
    if ((UnityEngine.Object) clip == (UnityEngine.Object) null)
      return;
    this.StopClip();
    if (clip.Looping)
      progress = clip.GetLoopedProgress(progress);
    else if (!clip.IsProgressWithinRange(progress))
      return;
    this._isClipAutomatic = false;
    this.ElapsedProgress = progress;
    this._clipSpeedMultiplier = 1f;
    this._clipWeightMultiplier = weightMultiplier;
    this.PlayStateAtProgress(clip, progress, weightMultiplier);
    this.CurrentClip = clip;
  }

  public void PlayClip(
    AnimationClipEx clip,
    float startProgress = 0,
    float speedMultiplier = 1,
    float weightMultiplier = 1)
  {
    if ((UnityEngine.Object) clip == (UnityEngine.Object) null)
      return;
    this.StopClip();
    if (clip.Looping)
      startProgress = clip.GetLoopedProgress(startProgress);
    else if (!clip.IsProgressWithinRange(startProgress))
      return;
    this._isClipAutomatic = true;
    this.ElapsedProgress = startProgress;
    this._clipSpeedMultiplier = speedMultiplier;
    this._clipWeightMultiplier = weightMultiplier;
    this.PlayStateAtProgress(clip, startProgress, weightMultiplier);
    this.CurrentClip = clip;
  }

  private void PlayStateAtProgress(AnimationClipEx clip, float progress = 0, float weightMultiplier = 1)
  {
    clip.Play(this.Manager.MyAnimator, progress, weightMultiplier);
    float num = clip.GetWeight(progress) * weightMultiplier;
    this._lastWeight = num;
    this.LimitsOverride = clip.HeadTranslationLimits;
    this.LimitsOverrideWeight = num;
    this.LookWeightsOverride = clip.LookWeights;
    this.LookCurvesOverride = clip.LookCurves;
    this.LookUseCurvesOverride = clip.UseLookCurves;
    this.LookCurvesTime = progress;
    this.LookWeightsOverrideWeight = num;
  }

  private void UpdateAutoClip()
  {
    if ((UnityEngine.Object) this.CurrentClip == (UnityEngine.Object) null || !this._isClipAutomatic)
      return;
    if ((double) this.CurrentClip.Length <= 0.0)
    {
      this.CurrentClip = (AnimationClipEx) null;
    }
    else
    {
      this.ElapsedProgress += this.Manager.DeltaTime * this._clipSpeedMultiplier / this.CurrentClip.Length;
      if (this.CurrentClip.Looping)
        this.ElapsedProgress = this.CurrentClip.GetLoopedProgress(this.ElapsedProgress);
      else if (!this.CurrentClip.IsProgressWithinRange(this.ElapsedProgress))
      {
        this.StopClip();
        return;
      }
      this.PlayStateAtProgress(this.CurrentClip, this.ElapsedProgress, this._clipWeightMultiplier);
    }
  }

  public void StopClip()
  {
    if ((UnityEngine.Object) this.CurrentClip == (UnityEngine.Object) null)
      return;
    this.CurrentClip.Stop(this.Manager.MyAnimator);
    this.LimitsOverride = (BipedAnimator.Limits) null;
    this.LimitsOverrideWeight = 0.0f;
    this.LookWeightsOverride = (BipedAnimator.LookWeights) null;
    this.LookCurvesOverride = (BipedAnimator.LookCurves) null;
    this.LookUseCurvesOverride = false;
    this.LookCurvesTime = 0.0f;
    this.LookWeightsOverrideWeight = 0.0f;
    this.Manager.MyAnimator.Stop(this.CurrentClip.AnimationLayerIndex);
    this.CurrentClip = (AnimationClipEx) null;
    this._isClipAutomatic = false;
  }

  public void StopPlayingThisClip(AnimationClipEx clip)
  {
    if ((UnityEngine.Object) clip == (UnityEngine.Object) null || (UnityEngine.Object) this.CurrentClip != (UnityEngine.Object) clip)
      return;
    this.StopClip();
  }

  public void Reset()
  {
    this.StopClip();
    this.BipedLocomotionAnimationOverride = (BipedLocomotionAnimation) null;
  }

  [Conditional("UNITY_EDITOR")]
  public void DrawDebugInfo()
  {
  }

  private class LocomotionHelper
  {
    private AnimatorBehaviourManager _manager;
    private Transform _feetAnchor;
    private AnimationCurve _runPhaseAtJumpVelocity;
    private JumpAnimation _jumpAnimation;
    private Vector3 _previousPosition;
    private Vector3 _smoothRelativeVelocity;
    private Vector3 _velocity;
    private MotorBridge _motorBridge;
    private Rigidbody _rigidbody;
    public bool Enabled = true;

    public LocomotionHelper(
      AnimatorBehaviourManager manager,
      Transform feetAnchor,
      AnimationCurve runPhaseAtJumpVelocity)
    {
      this._manager = manager;
      this._feetAnchor = feetAnchor;
      this._previousPosition = this._feetAnchor.position;
      this._runPhaseAtJumpVelocity = runPhaseAtJumpVelocity;
      this._jumpAnimation = manager.Entity.TryGet<JumpAnimation>();
    }

    public MotorBridge MotorBridge
    {
      get
      {
        if ((UnityEngine.Object) this._motorBridge == (UnityEngine.Object) null)
          this._motorBridge = this._manager.Entity.GetOrCreate<MotorBridge>();
        return this._motorBridge;
      }
    }

    public Rigidbody Rigidbody
    {
      get
      {
        if ((UnityEngine.Object) this._rigidbody == (UnityEngine.Object) null)
          this._rigidbody = (Rigidbody) (GameObjectAttribute<Rigidbody>) this._manager.Entity.GetOrCreate<MainRigidbody>();
        return this._rigidbody;
      }
    }

    public void UpdateVelocity(BipedLocomotionAnimation bipedLocomotion, float deltaTime)
    {
      if ((double) deltaTime <= 0.0)
        return;
      Vector3 position = this._feetAnchor.position;
      this._velocity = (position - this._previousPosition) / deltaTime;
      this._previousPosition = position;
      float rate = HalfLife.GetRate(bipedLocomotion.AnimationBlendSmoothingTime, deltaTime);
      this._smoothRelativeVelocity = Vector3.Lerp(this._smoothRelativeVelocity, Quaternion.Inverse(this._feetAnchor.rotation) * this._velocity, rate);
    }

    public void UpdateAnimator(BipedLocomotionAnimation bipedLocomotion)
    {
      if (!this.Enabled)
        return;
      if (!this.MotorBridge.Grounded)
      {
        bool flag = (UnityEngine.Object) this._jumpAnimation != (UnityEngine.Object) null && this._jumpAnimation.Mirrored;
        this._manager.Play(bipedLocomotion.LocomotionStateID, bipedLocomotion.LocomotionStateLayerIndex, Mathf.Repeat(this._runPhaseAtJumpVelocity.Evaluate(this.Rigidbody.velocity.y) + (!flag ? 0.0f : 0.5f), 1f));
      }
      else
        this._manager.Play(bipedLocomotion.LocomotionStateID, bipedLocomotion.LocomotionStateLayerIndex);
      this._manager.SetFloat(bipedLocomotion.VelocityXParameterName, this._smoothRelativeVelocity.x);
      this._manager.SetFloat(bipedLocomotion.VelocityZParameterName, this._smoothRelativeVelocity.z);
    }
  }

  private class TorsoPositioningHelper
  {
    private readonly AnimatorBehaviourManager _manager;
    private readonly Transform _rotatedTorsoAvatarAnchor;
    private readonly Transform _hips;
    public bool Enabled = true;

    public TorsoPositioningHelper(
      AnimatorBehaviourManager manager,
      Transform rotatedTorsoAvatarAnchor)
    {
      this._manager = manager;
      this._rotatedTorsoAvatarAnchor = rotatedTorsoAvatarAnchor;
      this._hips = this._manager.GetBoneTransform(HumanBodyBones.Hips);
    }

    private Transform AvatarTransform => this._manager.AvatarTransform;

    public void AlignHips()
    {
      if (!this.Enabled)
        return;
      this._manager.AvatarTransform.position += this._rotatedTorsoAvatarAnchor.TransformPoint(this.AvatarTransform.InverseTransformPoint(this._hips.position)) - this._hips.position;
    }
  }

  [Serializable]
  public class FeetAnchorHelper
  {
    private MotorBridge _motorBridge;
    private Transform _rotatedLegsAvatarAnchor;
    private Transform _feetAnchor;
    private Vector3 _normal = Vector3.up;
    public float PlaneSmoothingTime = 0.2f;
    public SoftLimiter PlaneAngleLimit = new SoftLimiter(15f, 30f);
    public bool Enabled = true;

    public void Init(
      MotorBridge motorBridge,
      Transform rotatedLegsAvatarAnchor,
      Transform feetAnchor)
    {
      this._motorBridge = motorBridge;
      this._rotatedLegsAvatarAnchor = rotatedLegsAvatarAnchor;
      this._feetAnchor = feetAnchor;
      this._normal = -Physics.gravity.normalized;
    }

    public void UpdateFeetAnchor()
    {
      if (!this.Enabled)
        return;
      float t = this._motorBridge.Stability * this._motorBridge.Strength;
      this._normal = Vector3.Lerp(this._normal, Vector3.Lerp(this._rotatedLegsAvatarAnchor.up, this._motorBridge.LastGroundNormalTouched, t), t * HalfLife.GetRate(this.PlaneSmoothingTime));
      this._feetAnchor.rotation = Quaternion.FromToRotation(this._rotatedLegsAvatarAnchor.up, Vector3.RotateTowards(this._rotatedLegsAvatarAnchor.up, this._normal, this.PlaneAngleLimit.GetLimited(Vector3.Angle(this._rotatedLegsAvatarAnchor.up, this._normal)) * ((float) Math.PI / 180f), float.MaxValue)) * this._rotatedLegsAvatarAnchor.rotation;
    }
  }

  [Serializable]
  public class FeetIKHelper
  {
    private AnimatorBehaviourManager _manager;
    private Transform _rotatedLegsAvatarAnchor;
    private Transform _feetAnchor;
    private Transform _avatarTransform;
    private Transform _hips;
    private Vector3 _smoothLocalLegOffset;
    private BipedLocomotionAnimation _bipedLocomotion;
    private Orientation _relativeLeftIKOrientation;
    private Orientation _relativeRightIKOrientation;
    public float PlaneRadius = 0.27f;
    public bool OffsettingEnabled = true;
    public bool IKEnabled = true;
    public bool LiftEnabled = true;

    public void Init(
      AnimatorBehaviourManager manager,
      Transform rotatedLegsAvatarAnchor,
      Transform feetAnchor)
    {
      this._manager = manager;
      this._hips = manager.GetBoneTransform(HumanBodyBones.Hips);
      this._rotatedLegsAvatarAnchor = rotatedLegsAvatarAnchor;
      this._feetAnchor = feetAnchor;
      this._avatarTransform = manager.AvatarTransform;
    }

    public void GetRelativeIKGoals()
    {
      this._relativeLeftIKOrientation = this._avatarTransform.InverseTransformOrientation(this._manager.GetOriginalIKOrientation(AvatarIKGoal.LeftFoot));
      this._relativeRightIKOrientation = this._avatarTransform.InverseTransformOrientation(this._manager.GetOriginalIKOrientation(AvatarIKGoal.RightFoot));
    }

    public void SetIKGoals(
      BipedLocomotionAnimation bipedLocomotion,
      float torsoLayerWeight,
      float deltaTime)
    {
      this._bipedLocomotion = bipedLocomotion;
      this._feetAnchor.position = this._rotatedLegsAvatarAnchor.position + this.GetOffset(torsoLayerWeight, deltaTime);
      if (!this.IKEnabled)
        return;
      this._manager.SetIKOrientation(AvatarIKGoal.LeftFoot, this._feetAnchor.TransformOrientation(this._relativeLeftIKOrientation));
      this._manager.SetIKOrientation(AvatarIKGoal.RightFoot, this._feetAnchor.TransformOrientation(this._relativeRightIKOrientation));
    }

    public void LiftHips()
    {
      if (!this.LiftEnabled)
        return;
      Vector3 up = this._rotatedLegsAvatarAnchor.up;
      float num1 = Vector3.Dot(this._rotatedLegsAvatarAnchor.up.normalized, this._feetAnchor.up.normalized);
      double num2 = (double) Mathf.Clamp(num1, -1f, 1f);
      float num3 = Mathf.Sqrt(Mathf.Max(0.0f, (float) (1.0 - (double) num1 * (double) num1)));
      double num4 = (double) Mathf.Clamp(num3, -1f, 1f);
      float fl = num3 * -this.PlaneRadius;
      if (!fl.IsReal())
        fl = 0.0f;
      float num5 = Vector3.Dot(this._feetAnchor.position - this._rotatedLegsAvatarAnchor.position, up);
      Vector3 vector = up * (fl + num5);
      if (!vector.IsValid())
        return;
      this._avatarTransform.position += vector;
    }

    private Vector3 GetOffset(float torsoLayerWeight, float deltaTime)
    {
      if (!this.OffsettingEnabled)
        return Vector3.zero;
      Vector3 vector3 = this._rotatedLegsAvatarAnchor.InverseTransformDirection(Vector3.Lerp(this._hips.position - this._rotatedLegsAvatarAnchor.TransformPoint(this._avatarTransform.InverseTransformPoint(this._hips.position)), this._manager.MyAnimator.GetTorsoPosition() - this._rotatedLegsAvatarAnchor.position, torsoLayerWeight));
      if (float.IsNaN(this._smoothLocalLegOffset.x) || float.IsNaN(this._smoothLocalLegOffset.y) || float.IsNaN(this._smoothLocalLegOffset.z))
        this._smoothLocalLegOffset = Vector3.zero;
      this._smoothLocalLegOffset += (vector3 - this._smoothLocalLegOffset) * HalfLife.GetRate(this._bipedLocomotion.LegOffsetSmoothingTime, deltaTime);
      Vector3 normal = this._feetAnchor.rotation * Vector3.up;
      return this._rotatedLegsAvatarAnchor.TransformDirection(this._smoothLocalLegOffset).Perpendicular(normal);
    }
  }

  private class ViewTranslationHelper
  {
    private Transform _eyesCenter;
    private Transform _viewAnchor;
    private Transform _avatarTransform;
    private Transform _rotatedTorsoAvatarAnchor;
    private Vector3 _smoothOffset;
    public bool Enabled = true;

    public ViewTranslationHelper(
      AnimatorBehaviourManager manager,
      Transform viewAnchor,
      Transform rotatedTorsoAvatarAnchor)
    {
      this._rotatedTorsoAvatarAnchor = rotatedTorsoAvatarAnchor;
      this._viewAnchor = viewAnchor;
      this._eyesCenter = manager.GetBoneTransform(CharacterDefinition.Part.EyesCenter);
      this._avatarTransform = manager.AvatarTransform;
    }

    public void ApplyViewOffsetLimiting(
      BipedAnimator.Limits limitsToUse,
      Vector3 baseOffset,
      float deltaTime)
    {
      if (!this.Enabled)
        return;
      Vector3 position = this._viewAnchor.position;
      Vector3 vector3_1 = this._rotatedTorsoAvatarAnchor.rotation * baseOffset;
      Vector3 vector3_2 = this._eyesCenter.position + vector3_1;
      Vector3 to = Quaternion.Inverse(this._rotatedTorsoAvatarAnchor.rotation) * (vector3_2 - position);
      to.x = limitsToUse.XLimit.GetLimited(Mathf.Abs(to.x)) * Mathf.Sign(to.x);
      to.y = limitsToUse.YLimit.GetLimited(Mathf.Abs(to.y)) * Mathf.Sign(to.y);
      to.z = limitsToUse.ZLimit.GetLimited(Mathf.Abs(to.z)) * Mathf.Sign(to.z);
      this._smoothOffset = Vector3.Lerp(this._smoothOffset, to, HalfLife.GetRate(limitsToUse.SmoothingTime, deltaTime));
      to = this._smoothOffset;
      to = this._rotatedTorsoAvatarAnchor.rotation * to;
      this._avatarTransform.position += position + to - vector3_2 + vector3_1;
    }
  }

  [Serializable]
  public class LookWeights
  {
    public float WeightHips = 0.5f;
    public float WeightTorso = 0.1f;
    public float WeightHead = 1f;
    public float WeightEyes = 1f;

    public void Lerp(BipedAnimator.LookWeights from, BipedAnimator.LookWeights to, float t)
    {
      this.WeightHips = Mathf.Lerp(from.WeightHips, to.WeightHips, t);
      this.WeightTorso = Mathf.Lerp(from.WeightTorso, to.WeightTorso, t);
      this.WeightHead = Mathf.Lerp(from.WeightHead, to.WeightHead, t);
      this.WeightEyes = Mathf.Lerp(from.WeightEyes, to.WeightEyes, t);
    }
  }

  [Serializable]
  public class LookCurves
  {
    public AnimationCurve WeightHips = new AnimationCurve();
    public AnimationCurve WeightTorso = new AnimationCurve();
    public AnimationCurve WeightHead = new AnimationCurve();
    public AnimationCurve WeightEyes = new AnimationCurve();

    public void ApplyCurve(BipedAnimator.LookWeights target, float t)
    {
      target.WeightHips = this.WeightHips.Evaluate(t);
      target.WeightTorso = this.WeightTorso.Evaluate(t);
      target.WeightHead = this.WeightHead.Evaluate(t);
      target.WeightEyes = this.WeightEyes.Evaluate(t);
    }
  }

  [Serializable]
  public class Limits
  {
    public SoftLimiter XLimit = new SoftLimiter(float.MaxValue, float.MaxValue);
    public SoftLimiter YLimit = new SoftLimiter(float.MaxValue, float.MaxValue);
    public SoftLimiter ZLimit = new SoftLimiter(float.MaxValue, float.MaxValue);
    public float SmoothingTime = 0.1f;

    public void Lerp(BipedAnimator.Limits from, BipedAnimator.Limits to, float t)
    {
      this.XLimit.Lerp(from.XLimit, to.XLimit, t);
      this.YLimit.Lerp(from.YLimit, to.YLimit, t);
      this.ZLimit.Lerp(from.ZLimit, to.ZLimit, t);
      this.SmoothingTime = Mathf.Lerp(from.SmoothingTime, to.SmoothingTime, t);
    }
  }
}
