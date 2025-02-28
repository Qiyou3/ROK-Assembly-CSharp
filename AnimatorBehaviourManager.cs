// Decompiled with JetBrains decompiler
// Type: AnimatorBehaviourManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch;
using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Modules.ScriptLOD;
using JetBrains.Annotations;
using RootMotion.FinalIK;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class AnimatorBehaviourManager : EntityBehaviour
{
  private static readonly int[] _avatarIKGoalValues = Enum.GetValues(typeof (AvatarIKGoal)).Cast<int>().ToArray<int>();
  private AnimatorBehaviour[] _animatorBehaviours;
  private AnimatorBehaviourManager.AggregateInfo _aggregateAvatarTransform = new AnimatorBehaviourManager.AggregateInfo();
  private AnimatorBehaviourManager.AggregateLookInfo _aggregateLookInfo = new AnimatorBehaviourManager.AggregateLookInfo();
  [CanBeNull]
  private Animator _animator;
  [CanBeNull]
  private BipedIK _biped;
  [CanBeNull]
  private FullBodyBipedIK _fullBody;
  private CharacterDefinition _definition;
  [CanBeNull]
  private Transform _avatarTransform;
  [CanBeNull]
  private CodeHatch.Engine.Modules.ScriptLOD.ScriptLOD _scriptLOD;
  [CanBeNull]
  private AnimatorBehaviourManager.AggregateInfo[] _aggregateIKInfo;
  private float _tempWeight;
  private bool _aggAvatarSet;
  private bool _aggIKSet;
  private bool _aggLookSet;
  private readonly List<AnimatorPass> _preAnimatePasses = new List<AnimatorPass>();
  private readonly List<AnimatorPass> _postAnimatePasses = new List<AnimatorPass>();
  private readonly List<AnimatorPass> _postIKPasses = new List<AnimatorPass>();
  private bool _initialized;
  private AnimatorBehaviourPass _currentBehaviourPass;
  private Orientation _leftFootOriginalIKOrientation;
  private Orientation _rightFootOriginalIKOrientation;
  private Orientation _leftHandOriginalIKOrientation;
  private Orientation _rightHandOriginalIKOrientation;
  private Vector3 _leftLegSmoothLocalBendDirection;
  private Vector3 _rightLegSmoothLocalBendDirection;
  private Vector3 _leftArmSmoothLocalBendDirection;
  private Vector3 _rightArmSmoothLocalBendDirection;
  [CanBeNull]
  private Transform _hip;
  [CanBeNull]
  private Transform _chest;
  [Tooltip("The visibility (measured in Pixels Per World Unit) at which the animation will be updated 60 times a second.")]
  public float VisibilityFor60HzUpdate = 100f;
  [Tooltip("The minimum rate at which characters will be updated, in Hz.")]
  public float MinimumUpdateRateHz = 0.5f;
  [Range(0.0f, 100f)]
  public float DistanceAlwaysVisible = 30f;
  private float _lastAnimateTime;
  private float _animateTimer;
  private bool _forceThisFrameToAnimate = true;
  private bool _animateThisFrame;

  [CanBeNull]
  public Animator MyAnimator
  {
    get
    {
      if ((UnityEngine.Object) this._animator == (UnityEngine.Object) null)
      {
        Entity entity = this.TryGetEntity();
        if ((UnityEngine.Object) entity != (UnityEngine.Object) null)
          this._animator = entity.TryGet<Animator>();
      }
      return this._animator;
    }
  }

  [CanBeNull]
  private BipedIK Biped
  {
    get
    {
      if ((UnityEngine.Object) this._biped == (UnityEngine.Object) null)
      {
        Entity entity = this.TryGetEntity();
        if ((UnityEngine.Object) entity != (UnityEngine.Object) null)
          this._biped = entity.TryGet<BipedIK>();
      }
      return this._biped;
    }
  }

  [CanBeNull]
  private FullBodyBipedIK FullBody
  {
    get
    {
      if ((UnityEngine.Object) this._fullBody == (UnityEngine.Object) null)
      {
        Entity entity = this.TryGetEntity();
        if ((UnityEngine.Object) entity != (UnityEngine.Object) null)
          this._fullBody = entity.TryGet<FullBodyBipedIK>();
      }
      return this._fullBody;
    }
  }

  public CharacterDefinition Definition
  {
    get
    {
      if ((UnityEngine.Object) this._definition == (UnityEngine.Object) null)
      {
        Entity entity = this.TryGetEntity();
        if ((UnityEngine.Object) entity != (UnityEngine.Object) null)
          this._definition = entity.GetOrCreate<CharacterDefinition>();
      }
      return this._definition;
    }
  }

  [CanBeNull]
  public Transform AvatarTransform
  {
    get
    {
      if ((UnityEngine.Object) this._avatarTransform == (UnityEngine.Object) null && (UnityEngine.Object) this.MyAnimator != (UnityEngine.Object) null)
        this._avatarTransform = this.MyAnimator.transform;
      return this._avatarTransform;
    }
  }

  private CodeHatch.Engine.Modules.ScriptLOD.ScriptLOD ScriptLOD
  {
    get
    {
      if ((UnityEngine.Object) this._scriptLOD == (UnityEngine.Object) null)
        this._scriptLOD = this.Entity.GetScriptLOD();
      return this._scriptLOD;
    }
  }

  private AnimatorBehaviourManager.AggregateInfo[] AggregateIKInfo
  {
    get
    {
      if (this._aggregateIKInfo == null)
        this._aggregateIKInfo = new AnimatorBehaviourManager.AggregateInfo[((IEnumerable<int>) AnimatorBehaviourManager._avatarIKGoalValues).Max() + 1];
      return this._aggregateIKInfo;
    }
  }

  private List<AnimatorPass> GetPassList(AnimatorBehaviourPass behaviourPass)
  {
    switch (behaviourPass)
    {
      case AnimatorBehaviourPass.PreAnimate:
        return this._preAnimatePasses;
      case AnimatorBehaviourPass.PostAnimate:
        return this._postAnimatePasses;
      case AnimatorBehaviourPass.PostIK:
        return this._postIKPasses;
      default:
        throw new InvalidOperationException("Cannot return pass type " + (object) behaviourPass);
    }
  }

  public bool ApplyRootMotion
  {
    get
    {
      if (!((UnityEngine.Object) this.MyAnimator == (UnityEngine.Object) null))
        return this.MyAnimator.applyRootMotion;
      this.LogNull<AnimatorBehaviourManager>("MyAnimator");
      return false;
    }
    set
    {
      if ((UnityEngine.Object) this.MyAnimator == (UnityEngine.Object) null)
        this.LogNull<AnimatorBehaviourManager>("MyAnimator");
      else
        this.MyAnimator.applyRootMotion = value;
    }
  }

  public Orientation EyeOrientation
  {
    get
    {
      return new Orientation()
      {
        Rotation = this.Entity.GetOrCreate<LookBridge>().Rotation,
        Position = this.GetBoneTransform(CharacterDefinition.Part.EyesCenter).position
      };
    }
  }

  public Transform GetBoneTransform(HumanBodyBones bone) => this.Definition.GetTransform(bone);

  public Transform GetBoneTransform(CharacterDefinition.Part bone)
  {
    return this.Definition.GetTransform(bone);
  }

  public AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex)
  {
    if (!((UnityEngine.Object) this.MyAnimator == (UnityEngine.Object) null))
      return this.MyAnimator.GetCurrentAnimatorStateInfo(layerIndex);
    this.LogNull<AnimatorBehaviourManager>("MyAnimator");
    return new AnimatorStateInfo();
  }

  public void SetFloat(string floatName, float value)
  {
    if ((UnityEngine.Object) this.MyAnimator == (UnityEngine.Object) null)
      this.LogNull<AnimatorBehaviourManager>("MyAnimator");
    else
      this.MyAnimator.SetFloat(floatName, value);
  }

  public void SetLayerWeight(int layerIndex, float weight)
  {
    if ((UnityEngine.Object) this.MyAnimator == (UnityEngine.Object) null)
      this.LogNull<AnimatorBehaviourManager>("MyAnimator");
    else
      this.MyAnimator.SetLayerWeight(layerIndex, weight);
  }

  public float GetLayerWeight(int layerIndex)
  {
    if (!((UnityEngine.Object) this.MyAnimator == (UnityEngine.Object) null))
      return this.MyAnimator.GetLayerWeight(layerIndex);
    this.LogNull<AnimatorBehaviourManager>("MyAnimator");
    return 0.0f;
  }

  public void CrossFade(
    string stateName,
    float transitionDuration,
    int layer = -1,
    float normalizedTime = float.NegativeInfinity)
  {
    if ((UnityEngine.Object) this.MyAnimator == (UnityEngine.Object) null)
      this.LogNull<AnimatorBehaviourManager>("MyAnimator");
    else
      this.MyAnimator.CrossFade(stateName, transitionDuration, layer, normalizedTime);
  }

  public void SetTrigger(string triggerName)
  {
    if ((UnityEngine.Object) this.MyAnimator == (UnityEngine.Object) null)
      this.LogNull<AnimatorBehaviourManager>("MyAnimator");
    else
      this.MyAnimator.SetTrigger(triggerName);
  }

  public void Play(string stateName, int layerIndex, float normalizedTime)
  {
    if ((UnityEngine.Object) this.MyAnimator == (UnityEngine.Object) null)
      this.LogNull<AnimatorBehaviourManager>("MyAnimator");
    else
      this.MyAnimator.Play(stateName, layerIndex, normalizedTime);
  }

  public void Play(int stateID, int layerIndex, float normalizedTime)
  {
    if ((UnityEngine.Object) this.MyAnimator == (UnityEngine.Object) null)
      this.LogNull<AnimatorBehaviourManager>("MyAnimator");
    else
      this.MyAnimator.Play(stateID, layerIndex, normalizedTime);
  }

  public void Play(int stateID, int layerIndex)
  {
    if ((UnityEngine.Object) this.MyAnimator == (UnityEngine.Object) null)
    {
      this.LogNull<AnimatorBehaviourManager>("MyAnimator");
    }
    else
    {
      if (this.MyAnimator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash == stateID)
        return;
      this.MyAnimator.Play(stateID, layerIndex);
    }
  }

  public Transform GetIKTransform(AvatarIKGoal goal)
  {
    switch (goal)
    {
      case AvatarIKGoal.LeftFoot:
        return this.Definition.GetTransform(HumanBodyBones.LeftFoot);
      case AvatarIKGoal.RightFoot:
        return this.Definition.GetTransform(HumanBodyBones.RightFoot);
      case AvatarIKGoal.LeftHand:
        return this.Definition.GetTransform(HumanBodyBones.LeftHand);
      case AvatarIKGoal.RightHand:
        return this.Definition.GetTransform(HumanBodyBones.RightHand);
      default:
        this.LogError<AnimatorBehaviourManager>("{0} not valid goal.", (object) goal);
        return this.Definition.GetTransform(HumanBodyBones.RightHand);
    }
  }

  private void CaptureOriginalIKOrientations()
  {
    if ((UnityEngine.Object) this.MyAnimator == (UnityEngine.Object) null)
      return;
    this._leftFootOriginalIKOrientation.Position = this.MyAnimator.GetIKPosition(AvatarIKGoal.LeftFoot);
    this._rightFootOriginalIKOrientation.Position = this.MyAnimator.GetIKPosition(AvatarIKGoal.RightFoot);
    this._leftHandOriginalIKOrientation.Position = this.MyAnimator.GetIKPosition(AvatarIKGoal.LeftHand);
    this._rightHandOriginalIKOrientation.Position = this.MyAnimator.GetIKPosition(AvatarIKGoal.RightHand);
    this._leftFootOriginalIKOrientation.Rotation = this.GetIKTransform(AvatarIKGoal.LeftFoot).rotation;
    this._rightFootOriginalIKOrientation.Rotation = this.GetIKTransform(AvatarIKGoal.RightFoot).rotation;
    this._leftHandOriginalIKOrientation.Rotation = this.GetIKTransform(AvatarIKGoal.LeftHand).rotation;
    this._rightHandOriginalIKOrientation.Rotation = this.GetIKTransform(AvatarIKGoal.RightHand).rotation;
  }

  public Orientation GetOriginalIKOrientation(AvatarIKGoal goal)
  {
    if (this._currentBehaviourPass != AnimatorBehaviourPass.PostAnimate)
      this.LogError<AnimatorBehaviourManager>("You should only retrieve the position of an IK goal after in a PostAnimate pass.");
    switch (goal)
    {
      case AvatarIKGoal.LeftFoot:
        return this._leftFootOriginalIKOrientation;
      case AvatarIKGoal.RightFoot:
        return this._rightFootOriginalIKOrientation;
      case AvatarIKGoal.LeftHand:
        return this._leftHandOriginalIKOrientation;
      case AvatarIKGoal.RightHand:
        return this._rightHandOriginalIKOrientation;
      default:
        this.LogError<AnimatorBehaviourManager>("goal {0} invalid value.", (object) goal);
        return this._leftFootOriginalIKOrientation;
    }
  }

  public void SetAvatarPosition(Vector3 position)
  {
    this._aggAvatarSet = true;
    this._aggregateAvatarTransform.AddPosition(position, this._tempWeight);
  }

  public void SetAvatarRotation(Quaternion rotation)
  {
    this._aggAvatarSet = true;
    this._aggregateAvatarTransform.AddRotation(rotation, this._tempWeight);
  }

  public void SetIKOrientation(AvatarIKGoal goal, Orientation orientation)
  {
    if (this._currentBehaviourPass != AnimatorBehaviourPass.PostAnimate)
    {
      this.LogError<AnimatorBehaviourManager>("Position and rotation alterations must be applied in a PostAnimate pass.");
    }
    else
    {
      this._aggIKSet = true;
      this.AggregateIKInfo[(int) goal].AddPosition(orientation.Position, this._tempWeight);
      this.AggregateIKInfo[(int) goal].AddRotation(orientation.Rotation, this._tempWeight);
    }
  }

  public void SetIKPosition(AvatarIKGoal goal, Vector3 position)
  {
    if (this._currentBehaviourPass != AnimatorBehaviourPass.PostAnimate)
    {
      this.LogError<AnimatorBehaviourManager>("Position and rotation alterations must be applied in a PostAnimate pass.");
    }
    else
    {
      this._aggIKSet = true;
      this.AggregateIKInfo[(int) goal].AddPosition(position, this._tempWeight);
    }
  }

  public void SetIKRotation(AvatarIKGoal goal, Quaternion rotation)
  {
    if (this._currentBehaviourPass != AnimatorBehaviourPass.PostAnimate)
    {
      this.LogError<AnimatorBehaviourManager>("Position and rotation alterations must be applied in a PostAnimate pass.");
    }
    else
    {
      this._aggIKSet = true;
      this.AggregateIKInfo[(int) goal].AddRotation(rotation, this._tempWeight);
    }
  }

  public void SetLookAtPosition(
    Vector3 lookAtPoint,
    float torsoWeight,
    float headWeight,
    float eyesWeight)
  {
    if (this._currentBehaviourPass != AnimatorBehaviourPass.PreAnimate)
    {
      this.LogError<AnimatorBehaviourManager>("Position and rotation alterations must be applied in a PreAnimate pass.");
    }
    else
    {
      this._aggLookSet = true;
      this._aggregateLookInfo.AddLook(lookAtPoint, torsoWeight, headWeight, eyesWeight, this._tempWeight);
    }
  }

  private Transform Hip
  {
    get
    {
      if ((UnityEngine.Object) this._hip == (UnityEngine.Object) null)
        this._hip = this.Definition.GetTransform(HumanBodyBones.Hips);
      return this._hip;
    }
  }

  private Transform Chest
  {
    get
    {
      if ((UnityEngine.Object) this._chest == (UnityEngine.Object) null)
        this._chest = this.Definition.GetTransform(HumanBodyBones.Chest);
      return this._chest;
    }
  }

  private Transform GetBendSmoothingBone(AvatarIKGoal goal)
  {
    switch (goal)
    {
      case AvatarIKGoal.LeftFoot:
      case AvatarIKGoal.RightFoot:
        return this.Hip;
      case AvatarIKGoal.LeftHand:
      case AvatarIKGoal.RightHand:
        return this.Chest;
      default:
        this.LogError<AnimatorBehaviourManager>("goal invalid value of {0}", (object) goal);
        return this.Hip;
    }
  }

  private Vector3 GetSmoothLocalBendDirection(AvatarIKGoal goal)
  {
    switch (goal)
    {
      case AvatarIKGoal.LeftFoot:
        return this._leftLegSmoothLocalBendDirection;
      case AvatarIKGoal.RightFoot:
        return this._rightLegSmoothLocalBendDirection;
      case AvatarIKGoal.LeftHand:
        return this._leftArmSmoothLocalBendDirection;
      case AvatarIKGoal.RightHand:
        return this._rightArmSmoothLocalBendDirection;
      default:
        this.LogError<AnimatorBehaviourManager>("goal invalid value of {0}", (object) goal);
        return Vector3.forward;
    }
  }

  private void SetSmoothLocalBendDirection(AvatarIKGoal goal, Vector3 value)
  {
    switch (goal)
    {
      case AvatarIKGoal.LeftFoot:
        this._leftLegSmoothLocalBendDirection = value;
        break;
      case AvatarIKGoal.RightFoot:
        this._rightLegSmoothLocalBendDirection = value;
        break;
      case AvatarIKGoal.LeftHand:
        this._leftArmSmoothLocalBendDirection = value;
        break;
      case AvatarIKGoal.RightHand:
        this._rightArmSmoothLocalBendDirection = value;
        break;
      default:
        this.LogError<AnimatorBehaviourManager>("goal invalid value of {0}", (object) goal);
        break;
    }
  }

  private Vector3 GetSmoothBendDirection(AvatarIKGoal goal, Vector3 rawBendDirection)
  {
    Transform bendSmoothingBone = this.GetBendSmoothingBone(goal);
    Vector3 vector3 = bendSmoothingBone.InverseTransformDirection(rawBendDirection);
    Vector3 normalized = (this.GetSmoothLocalBendDirection(goal) / 4f + vector3).normalized;
    this.SetSmoothLocalBendDirection(goal, normalized);
    return bendSmoothingBone.TransformDirection(normalized);
  }

  private Vector3 GetBendGoal(
    AvatarIKGoal goal,
    Vector3 position,
    Vector3 hip,
    Vector3 knee,
    Vector3 ankle)
  {
    Vector3 vector3 = knee.ClosestOnLineSegment(hip, ankle);
    Vector3 rawBendDirection = (knee - vector3) / Mathf.Max(0.01f, Vector3.Distance(hip, ankle));
    Vector3 smoothBendDirection = this.GetSmoothBendDirection(goal, rawBendDirection);
    Quaternion rotation = Quaternion.FromToRotation(ankle - hip, position - hip);
    return (vector3 + smoothBendDirection * 20f).GetRotated(rotation, hip);
  }

  private void ClearAggregateAvatarInfo()
  {
    if (!this._aggAvatarSet)
      return;
    this._aggAvatarSet = false;
    this._aggregateAvatarTransform.Reset();
  }

  private void ClearIKAggregateInfo()
  {
    if (!this._aggIKSet)
      return;
    this._aggIKSet = false;
    AnimatorBehaviourManager.AggregateInfo[] aggregateIkInfo = this.AggregateIKInfo;
    for (int index = 0; index < AnimatorBehaviourManager._avatarIKGoalValues.Length; ++index)
      aggregateIkInfo[AnimatorBehaviourManager._avatarIKGoalValues[index]].Reset();
  }

  private void ClearAggregateLookInfo()
  {
    if (!this._aggLookSet)
      return;
    this._aggLookSet = false;
    this._aggregateLookInfo.Reset();
  }

  private void ApplyAggregateAvatarInfo()
  {
    if (!this._aggAvatarSet)
      return;
    if ((UnityEngine.Object) this.AvatarTransform == (UnityEngine.Object) null)
    {
      this.LogError<AnimatorBehaviourManager>("AvatarTransform");
    }
    else
    {
      if (this._aggregateAvatarTransform.PositionSet)
        this.AvatarTransform.position = Vector3.Lerp(this.AvatarTransform.position, this._aggregateAvatarTransform.AveragePosition, this._aggregateAvatarTransform.PositionWeight);
      if (!this._aggregateAvatarTransform.RotationSet)
        return;
      this.AvatarTransform.rotation = Quaternion.Slerp(this.AvatarTransform.rotation, this._aggregateAvatarTransform.AverageRotation, this._aggregateAvatarTransform.RotationWeight);
    }
  }

  private void ApplyAggregateIKInfo()
  {
    if (!this._aggIKSet)
    {
      this.PrivateSetIKPositionWeight(AvatarIKGoal.LeftFoot, 0.0f);
      this.PrivateSetIKPositionWeight(AvatarIKGoal.RightFoot, 0.0f);
      this.PrivateSetIKPositionWeight(AvatarIKGoal.LeftHand, 0.0f);
      this.PrivateSetIKPositionWeight(AvatarIKGoal.RightHand, 0.0f);
      this.PrivateSetIKRotationWeight(AvatarIKGoal.LeftFoot, 0.0f);
      this.PrivateSetIKRotationWeight(AvatarIKGoal.RightFoot, 0.0f);
      this.PrivateSetIKRotationWeight(AvatarIKGoal.LeftHand, 0.0f);
      this.PrivateSetIKRotationWeight(AvatarIKGoal.RightHand, 0.0f);
    }
    else
    {
      AnimatorBehaviourManager.AggregateInfo[] aggregateIkInfo = this.AggregateIKInfo;
      for (int index = 0; index < AnimatorBehaviourManager._avatarIKGoalValues.Length; ++index)
      {
        int avatarIkGoalValue = AnimatorBehaviourManager._avatarIKGoalValues[index];
        AvatarIKGoal goal = (AvatarIKGoal) avatarIkGoalValue;
        AnimatorBehaviourManager.AggregateInfo aggregateInfo = aggregateIkInfo[avatarIkGoalValue];
        if (aggregateInfo.PositionSet)
        {
          this.PrivateSetIKPosition(goal, aggregateInfo.AveragePosition);
          this.PrivateSetIKPositionWeight(goal, Mathf.Clamp01(aggregateInfo.PositionWeight));
        }
        else
          this.PrivateSetIKPositionWeight(goal, 0.0f);
        if (aggregateInfo.RotationSet)
        {
          this.PrivateSetIKRotation(goal, aggregateInfo.AverageRotation);
          this.PrivateSetIKRotationWeight(goal, Mathf.Clamp01(aggregateInfo.RotationWeight));
        }
        else
          this.PrivateSetIKRotationWeight(goal, 0.0f);
      }
    }
  }

  private void PrivateSetIKPosition(AvatarIKGoal goal, Vector3 position)
  {
    if ((UnityEngine.Object) this.FullBody != (UnityEngine.Object) null)
    {
      IKEffector ikEffector;
      FBIKChain fbikChain;
      switch (goal)
      {
        case AvatarIKGoal.LeftFoot:
          ikEffector = this.FullBody.solver.leftFootEffector;
          fbikChain = this.FullBody.solver.leftLegChain;
          break;
        case AvatarIKGoal.RightFoot:
          ikEffector = this.FullBody.solver.rightFootEffector;
          fbikChain = this.FullBody.solver.rightLegChain;
          break;
        case AvatarIKGoal.LeftHand:
          ikEffector = this.FullBody.solver.leftHandEffector;
          fbikChain = this.FullBody.solver.leftArmChain;
          break;
        case AvatarIKGoal.RightHand:
          ikEffector = this.FullBody.solver.rightHandEffector;
          fbikChain = this.FullBody.solver.rightArmChain;
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (goal));
      }
      ikEffector.position = position;
      Vector3 bendGoal = this.GetBendGoal(goal, position, fbikChain.bendConstraint.bone1.position, fbikChain.bendConstraint.bone2.position, fbikChain.bendConstraint.bone3.position);
      if ((UnityEngine.Object) fbikChain.bendConstraint.bendGoal == (UnityEngine.Object) null)
      {
        fbikChain.bendConstraint.bendGoal = new GameObject("[Bend Goal]").transform;
        fbikChain.bendConstraint.bendGoal.parent = fbikChain.bendConstraint.bone1;
      }
      fbikChain.bendConstraint.bendGoal.position = bendGoal;
    }
    else
    {
      if (!((UnityEngine.Object) this.Biped != (UnityEngine.Object) null))
        return;
      this.Biped.SetIKPosition(goal, position);
      IKSolverLimb goalIk = this.Biped.GetGoalIK(goal);
      Vector3 bendGoal = this.GetBendGoal(goal, position, goalIk.bone1.transform.position, goalIk.bone2.transform.position, goalIk.bone3.transform.position);
      if ((UnityEngine.Object) goalIk.bendGoal == (UnityEngine.Object) null)
      {
        goalIk.bendGoal = new GameObject("[Bend Goal]").transform;
        goalIk.bendGoal.parent = goalIk.bone1.transform;
      }
      goalIk.bendGoal.position = bendGoal;
      goalIk.bendModifier = IKSolverLimb.BendModifier.Goal;
      goalIk.bendModifierWeight = 1f;
    }
  }

  private void PrivateSetIKPositionWeight(AvatarIKGoal goal, float weight)
  {
    if ((UnityEngine.Object) this.FullBody != (UnityEngine.Object) null)
    {
      IKEffector ikEffector;
      FBIKChain fbikChain;
      switch (goal)
      {
        case AvatarIKGoal.LeftFoot:
          ikEffector = this.FullBody.solver.leftFootEffector;
          fbikChain = this.FullBody.solver.leftLegChain;
          break;
        case AvatarIKGoal.RightFoot:
          ikEffector = this.FullBody.solver.rightFootEffector;
          fbikChain = this.FullBody.solver.rightLegChain;
          break;
        case AvatarIKGoal.LeftHand:
          ikEffector = this.FullBody.solver.leftHandEffector;
          fbikChain = this.FullBody.solver.leftArmChain;
          break;
        case AvatarIKGoal.RightHand:
          ikEffector = this.FullBody.solver.rightHandEffector;
          fbikChain = this.FullBody.solver.rightArmChain;
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (goal));
      }
      ikEffector.positionWeight = weight;
      fbikChain.bendConstraint.weight = weight;
    }
    else
    {
      if (!((UnityEngine.Object) this.Biped != (UnityEngine.Object) null))
        return;
      this.Biped.SetIKPositionWeight(goal, weight);
    }
  }

  private void PrivateSetIKRotation(AvatarIKGoal goal, Quaternion rotation)
  {
    Quaternion IKRotation = rotation;
    if ((UnityEngine.Object) this.FullBody != (UnityEngine.Object) null)
    {
      switch (goal)
      {
        case AvatarIKGoal.LeftFoot:
          this.FullBody.solver.leftFootEffector.rotation = IKRotation;
          break;
        case AvatarIKGoal.RightFoot:
          this.FullBody.solver.rightFootEffector.rotation = IKRotation;
          break;
        case AvatarIKGoal.LeftHand:
          this.FullBody.solver.leftHandEffector.rotation = IKRotation;
          break;
        case AvatarIKGoal.RightHand:
          this.FullBody.solver.rightHandEffector.rotation = IKRotation;
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (goal));
      }
    }
    else
    {
      if (!((UnityEngine.Object) this.Biped != (UnityEngine.Object) null))
        return;
      this.Biped.SetIKRotation(goal, IKRotation);
    }
  }

  private void PrivateSetIKRotationWeight(AvatarIKGoal goal, float weight)
  {
    if ((UnityEngine.Object) this.FullBody != (UnityEngine.Object) null)
    {
      switch (goal)
      {
        case AvatarIKGoal.LeftFoot:
          this.FullBody.solver.leftFootEffector.rotationWeight = weight;
          break;
        case AvatarIKGoal.RightFoot:
          this.FullBody.solver.rightFootEffector.rotationWeight = weight;
          break;
        case AvatarIKGoal.LeftHand:
          this.FullBody.solver.leftHandEffector.rotationWeight = weight;
          break;
        case AvatarIKGoal.RightHand:
          this.FullBody.solver.rightHandEffector.rotationWeight = weight;
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (goal));
      }
    }
    else
    {
      if (!((UnityEngine.Object) this.Biped != (UnityEngine.Object) null))
        return;
      this.Biped.SetIKRotationWeight(goal, weight);
    }
  }

  private void ApplyAggregateLookInfo()
  {
    if (!this._aggLookSet || !this._aggregateLookInfo.LookSet)
      return;
    if ((UnityEngine.Object) this.MyAnimator == (UnityEngine.Object) null)
    {
      this.LogNull<AnimatorBehaviourManager>("MyAnimator");
    }
    else
    {
      this.MyAnimator.SetLookAtPosition(this._aggregateLookInfo.AverageLookAtPosition);
      this.MyAnimator.SetLookAtWeight(Mathf.Clamp01(this._aggregateLookInfo.LookAtWeight), this._aggregateLookInfo.AverageTorsoWeight, this._aggregateLookInfo.AverageHeadWeight, this._aggregateLookInfo.AverageEyesWeight);
    }
  }

  public void AddPass(System.Action action, AnimatorBehaviourPass behaviourPass, int queue)
  {
    List<AnimatorPass> passList = this.GetPassList(behaviourPass);
    for (int index = 0; index < passList.Count; ++index)
    {
      AnimatorPass animatorPass = passList[index];
      if (animatorPass.Action.Method == action.Method && animatorPass.Action.Target == action.Target)
      {
        this.LogError<AnimatorBehaviourManager>("Cannot add the same action twice for the same behaviour! This is an unexpected usage.");
        return;
      }
    }
    AnimatorPass animatorPass1 = new AnimatorPass()
    {
      Action = action,
      Queue = queue
    };
    passList.InsertSorted<AnimatorPass, int>(animatorPass1, (Func<AnimatorPass, int>) (p => p.Queue));
  }

  public void RemovePass(System.Action action, AnimatorBehaviourPass behaviourPass, int queue)
  {
    List<AnimatorPass> passList = this.GetPassList(behaviourPass);
    for (int index = 0; index < passList.Count; ++index)
    {
      AnimatorPass animatorPass = passList[index];
      if (animatorPass.Action.Method == action.Method && animatorPass.Action.Target == action.Target)
      {
        passList.RemoveAt(index);
        break;
      }
    }
  }

  public float DeltaTime { get; private set; }

  private void UpdateAnimateTimer(bool visible, float pixelsPerWorldUnit)
  {
    if ((double) this.VisibilityFor60HzUpdate <= 0.0)
      this._animateTimer = 1f;
    else if ((double) this.VisibilityFor60HzUpdate >= double.PositiveInfinity)
      this._animateTimer += Time.deltaTime * this.MinimumUpdateRateHz;
    else if (!visible)
      this._animateTimer += Time.deltaTime * this.MinimumUpdateRateHz;
    else
      this._animateTimer += Time.deltaTime * Mathf.Max(this.MinimumUpdateRateHz, (float) (60.0 * ((double) pixelsPerWorldUnit / (double) this.VisibilityFor60HzUpdate)));
    if (this._forceThisFrameToAnimate || (double) this._animateTimer >= 1.0)
    {
      float time = Time.time;
      this.DeltaTime = time - this._lastAnimateTime;
      this._lastAnimateTime = time;
      this._animateTimer = 0.0f;
      this._animateThisFrame = true;
      this._forceThisFrameToAnimate = false;
    }
    else
      this._animateThisFrame = false;
  }

  public void Start()
  {
    this._lastAnimateTime = Time.time;
    this._animatorBehaviours = this.Entity.TryGetArray<AnimatorBehaviour>();
    foreach (AnimatorBehaviour animatorBehaviour in this._animatorBehaviours)
    {
      try
      {
        animatorBehaviour.Initialize(this);
      }
      catch (Exception ex)
      {
        this.LogException<AnimatorBehaviourManager>(ex);
      }
    }
  }

  public void OnDestroy()
  {
    this._preAnimatePasses.Clear();
    this._postAnimatePasses.Clear();
    this._postIKPasses.Clear();
  }

  public void Update()
  {
    if ((UnityEngine.Object) this.MyAnimator != (UnityEngine.Object) null && this.MyAnimator.enabled)
      this.MyAnimator.enabled = false;
    if ((UnityEngine.Object) this.Biped != (UnityEngine.Object) null && this.Biped.enabled)
      this.Biped.enabled = false;
    if ((UnityEngine.Object) this.FullBody != (UnityEngine.Object) null && this.FullBody.enabled)
      this.FullBody.enabled = false;
    this._initialized = this.MyAnimator.IsInitialized();
    if (!this._initialized)
      return;
    this.UpdateAnimateTimer(this.ScriptLOD.Visible || (double) this.ScriptLOD.Distance < (double) this.DistanceAlwaysVisible, this.ScriptLOD.SizePixelsPerMeter);
    if (!this._animateThisFrame)
      return;
    this.ApplyPreAnimateAnimations();
    if (!((UnityEngine.Object) this.MyAnimator != (UnityEngine.Object) null))
      return;
    this.MyAnimator.Update(this.DeltaTime);
  }

  public void OnAnimatorIK(int layer)
  {
    this.CaptureOriginalIKOrientations();
    this.ApplyIKAnimations();
  }

  public void LateUpdate()
  {
    if (!this._initialized || !this._animateThisFrame)
      return;
    this.ApplyPostAnimateAnimations();
  }

  public void ForceUpdateThisFrame() => this._forceThisFrameToAnimate = true;

  public void ForceImmediateUpdate()
  {
    if ((UnityEngine.Object) this.MyAnimator == (UnityEngine.Object) null)
    {
      this.LogError<AnimatorBehaviourManager>("Failed to force immediate update,because MyAnimator is null.");
    }
    else
    {
      this._initialized = this.MyAnimator.IsInitialized();
      if (!this._initialized)
        this.LogError<AnimatorBehaviourManager>("Failed to force immediate update,because MyAnimator is not initialized.");
      else if ((UnityEngine.Object) this.MyAnimator.GetBoneTransform(HumanBodyBones.Hips) == (UnityEngine.Object) null)
      {
        this.LogError<AnimatorBehaviourManager>("MyAnimator.GetBoneTransform(HumanBodyBones.Hips) == null");
      }
      else
      {
        float deltaTime = this.DeltaTime;
        this.DeltaTime = 0.0f;
        try
        {
          this.ApplyPreAnimateAnimations();
          this.MyAnimator.Update(this.DeltaTime);
          this.ApplyPostAnimateAnimations();
        }
        catch (Exception ex)
        {
          this.LogException<AnimatorBehaviourManager>(ex);
        }
        this.DeltaTime = deltaTime;
      }
    }
  }

  private void PerformPasses(IList<AnimatorPass> passes)
  {
    for (int index = 0; index < passes.Count; ++index)
    {
      AnimatorPass pass = passes[index];
      if (pass == null)
      {
        this.LogError<AnimatorBehaviourManager>("passes[{0}] was null. Removing...");
        passes.RemoveAt(index);
        --index;
      }
      else if (pass.Action == null)
      {
        this.LogError<AnimatorBehaviourManager>("passes[{0}].action was null. Removing...");
        passes.RemoveAt(index);
        --index;
      }
      else if ((UnityEngine.Object) pass.Behaviour == (UnityEngine.Object) null)
      {
        this.LogError<AnimatorBehaviourManager>("passes[{0}].Behaviour was null. Removing...");
        passes.RemoveAt(index);
        --index;
      }
      else if (pass.Behaviour.enabled)
      {
        this._tempWeight = pass.Behaviour.Weight;
        if ((double) this._tempWeight > 0.0)
        {
          try
          {
            pass.Action();
          }
          catch (Exception ex)
          {
            this.LogException<AnimatorBehaviourManager>(ex);
          }
        }
      }
    }
  }

  private void CalculateBehaviourWeights()
  {
    for (int index = 0; index < this._animatorBehaviours.Length; ++index)
    {
      double weight = (double) this._animatorBehaviours[index].CalculateWeight();
    }
  }

  private void ApplyPreAnimateAnimations()
  {
    this.CalculateBehaviourWeights();
    this.ClearAggregateAvatarInfo();
    this.ClearAggregateLookInfo();
    this._currentBehaviourPass = AnimatorBehaviourPass.PreAnimate;
    this.PerformPasses((IList<AnimatorPass>) this._preAnimatePasses);
    this.ApplyAggregateAvatarInfo();
  }

  private void ApplyIKAnimations() => this.ApplyAggregateLookInfo();

  private void ApplyPostAnimateAnimations()
  {
    this.ClearIKAggregateInfo();
    this.ClearAggregateAvatarInfo();
    this._currentBehaviourPass = AnimatorBehaviourPass.PostAnimate;
    this.PerformPasses((IList<AnimatorPass>) this._postAnimatePasses);
    this.ApplyAggregateIKInfo();
    this.ApplyAggregateAvatarInfo();
    if ((UnityEngine.Object) this.Biped != (UnityEngine.Object) null)
      this.Biped.LateUpdate();
    if ((UnityEngine.Object) this.FullBody != (UnityEngine.Object) null)
      this.FullBody.LateUpdate();
    this.ClearAggregateAvatarInfo();
    this._currentBehaviourPass = AnimatorBehaviourPass.PostIK;
    this.PerformPasses((IList<AnimatorPass>) this._postIKPasses);
    this.ApplyAggregateAvatarInfo();
  }

  private struct AggregateInfo
  {
    private Vector3 _position;
    private Vector3 _forward;
    private Vector3 _up;

    public float PositionWeight { get; private set; }

    public float RotationWeight { get; private set; }

    public void Reset()
    {
      this.PositionWeight = 0.0f;
      this.RotationWeight = 0.0f;
      this._position = Vector3.zero;
      this._forward = Vector3.zero;
      this._up = Vector3.zero;
    }

    public void AddPosition(Vector3 position, float weight)
    {
      this._position += position * weight;
      this.PositionWeight += weight;
    }

    public void AddRotation(Quaternion rotation, float weight)
    {
      this._forward += rotation * Vector3.forward * weight;
      this._up += rotation * Vector3.up * weight;
      this.RotationWeight += weight;
    }

    public Vector3 AveragePosition
    {
      get => !this.PositionSet ? Vector3.zero : this._position / this.PositionWeight;
    }

    public Quaternion AverageRotation
    {
      get
      {
        return !this.RotationSet ? Quaternion.identity : Quaternion.LookRotation(this._forward / this.RotationWeight, this._up / this.RotationWeight);
      }
    }

    public bool PositionSet => (double) this.PositionWeight > 0.0;

    public bool RotationSet => (double) this.RotationWeight > 0.0;
  }

  private struct AggregateLookInfo
  {
    private Vector3 _lookAtPoint;
    private float _torsoWeight;
    private float _headWeight;
    private float _eyesWeight;

    public float LookAtWeight { get; private set; }

    public void AddLook(
      Vector3 lookAtPoint,
      float torsoWeight,
      float headWeight,
      float eyesWeight,
      float weight)
    {
      this.LookAtWeight += weight;
      this._lookAtPoint += lookAtPoint * weight;
      this._torsoWeight += torsoWeight * weight;
      this._headWeight += headWeight * weight;
      this._eyesWeight += eyesWeight * weight;
    }

    public void Reset()
    {
      this.LookAtWeight = 0.0f;
      this._torsoWeight = 0.0f;
      this._headWeight = 0.0f;
      this._eyesWeight = 0.0f;
      this._lookAtPoint = Vector3.zero;
    }

    public Vector3 AverageLookAtPosition => this._lookAtPoint / this.LookAtWeight;

    public float AverageTorsoWeight => this._torsoWeight / this.LookAtWeight;

    public float AverageHeadWeight => this._headWeight / this.LookAtWeight;

    public float AverageEyesWeight => this._eyesWeight / this.LookAtWeight;

    public bool LookSet => (double) this.LookAtWeight > 0.0;
  }
}
