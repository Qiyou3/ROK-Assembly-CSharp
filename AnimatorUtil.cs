// Decompiled with JetBrains decompiler
// Type: AnimatorUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using UnityEngine;

#nullable disable
public static class AnimatorUtil
{
  public static float GetNormalizedTime(this Animator animator, int layerIndex)
  {
    return animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime;
  }

  public static float GetNormalized01Time(this Animator animator, int layerIndex)
  {
    return Mathf.Repeat(animator.GetNormalizedTime(layerIndex), 1f);
  }

  public static int GetPhaseRepeatIndex(this Animator animator, int layerIndex, float phase)
  {
    return Mathf.FloorToInt(animator.GetNormalizedTime(layerIndex) - phase);
  }

  public static int GetPhaseRepeatIndex(float normalizedTime, float phase)
  {
    return Mathf.FloorToInt(normalizedTime - phase);
  }

  public static float Get01CyclicDiff(float first, float second)
  {
    float f = first - second;
    return f - Mathf.Round(f);
  }

  public static bool Get01PhaseChangedForward(float prevPhase, float curPhase, float changePhase)
  {
    float num1 = AnimatorUtil.Get01CyclicDiff(prevPhase, changePhase);
    float num2 = AnimatorUtil.Get01CyclicDiff(curPhase, changePhase);
    return (double) num1 < 0.0 && (double) num2 >= 0.0;
  }

  public static float GetForwardLerpBetweenTwoPhases(float firstPhase, float secondPhase, float t)
  {
    float from = secondPhase - firstPhase;
    if ((double) from < 0.0)
      ++from;
    float num = secondPhase - t;
    if ((double) num < 0.0)
      ++num;
    return Mathf.Clamp01(Mathf.InverseLerp(from, 0.0f, num));
  }

  public static float GetStateSelfWeight(
    this Animator animator,
    string stateFullPath,
    int layerIndex)
  {
    int hash = Animator.StringToHash(stateFullPath);
    if (!animator.IsInTransition(layerIndex))
      return animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash != hash ? 0.0f : 1f;
    AnimatorStateInfo animatorStateInfo1 = animator.GetCurrentAnimatorStateInfo(layerIndex);
    AnimatorStateInfo animatorStateInfo2 = animator.GetNextAnimatorStateInfo(layerIndex);
    if (animatorStateInfo1.fullPathHash == hash)
      return 1f - animator.GetAnimatorTransitionInfo(layerIndex).normalizedTime;
    return animatorStateInfo2.fullPathHash == hash ? animator.GetAnimatorTransitionInfo(layerIndex).normalizedTime : 0.0f;
  }

  public static float GetStateApparentWeight(
    this Animator animator,
    string stateFullPath,
    int layerIndex)
  {
    return animator.GetStateSelfWeight(stateFullPath, layerIndex) * animator.GetLayerApparentWeight(layerIndex);
  }

  public static float GetStateApparentWeight(
    this Animator animator,
    string stateFullPath,
    int layerIndex,
    bool[] layerMask)
  {
    return animator.GetStateSelfWeight(stateFullPath, layerIndex) * animator.GetLayerApparentWeight(layerIndex, layerMask);
  }

  public static float GetLayerApparentWeight(this Animator animator, int layerIndex)
  {
    float num1 = layerIndex != 0 ? animator.GetLayerWeight(layerIndex) : 1f;
    float num2 = 1f;
    for (int layerIndex1 = layerIndex + 1; layerIndex1 < animator.layerCount; ++layerIndex1)
      num2 *= 1f - animator.GetLayerWeight(layerIndex1);
    return num1 * num2;
  }

  public static float GetLayerApparentWeight(
    this Animator animator,
    int layerIndex,
    bool[] layerMask)
  {
    float num1 = layerIndex != 0 ? animator.GetLayerWeight(layerIndex) : 1f;
    float num2 = 1f;
    for (int layerIndex1 = layerIndex + 1; layerIndex1 < animator.layerCount; ++layerIndex1)
    {
      if (layerIndex1 >= layerMask.Length || layerMask[layerIndex1])
        num2 *= 1f - animator.GetLayerWeight(layerIndex1);
    }
    return num1 * num2;
  }

  public static Vector3 GetTorsoPosition(this Animator animator)
  {
    return (animator.GetBoneTransform(HumanBodyBones.Hips).position + animator.GetBoneTransform(HumanBodyBones.Head).position) / 2f;
  }

  public static Quaternion GetTorsoRotation(this Animator animator)
  {
    Transform boneTransform = animator.GetBoneTransform(HumanBodyBones.Hips);
    Vector3 upwards = animator.GetBoneTransform(HumanBodyBones.Head).position - boneTransform.position;
    return QuaternionUtil.LookRotation(boneTransform.forward, upwards, true);
  }

  public static Vector3 GetLocalTorsoPosition(this Animator animator)
  {
    return animator.transform.InverseTransformPoint(animator.GetTorsoPosition());
  }

  public static Quaternion GetLocalTorsoRotation(this Animator animator)
  {
    return Quaternion.Inverse(animator.transform.rotation) * animator.GetTorsoRotation();
  }

  public static Orientation GetLocalTorsoOrientation(this Animator animator)
  {
    return new Orientation()
    {
      Position = animator.GetLocalTorsoPosition(),
      Rotation = animator.GetLocalTorsoRotation()
    };
  }

  public static Orientation GetIKOrientation(this Animator animator, AvatarIKGoal goal)
  {
    return new Orientation()
    {
      Position = animator.GetIKPosition(goal),
      Rotation = animator.GetIKRotation(goal)
    };
  }

  public static bool IsInitialized(this Animator animator)
  {
    return !((Object) animator == (Object) null) && !((Object) animator.avatar == (Object) null) && !((Object) animator.runtimeAnimatorController == (Object) null) && animator.avatar.isValid;
  }
}
