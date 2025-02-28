// Decompiled with JetBrains decompiler
// Type: HolderPose
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using System;
using UnityEngine;

#nullable disable
public class HolderPose : EntityBehaviour
{
  public HolderPose.Pose[] Poses;
  private HolderPose.PoseType _setPoseType;
  private Animator _animator;

  public HolderPose.PoseType CurrentPose { get; set; }

  private Animator Animator
  {
    get
    {
      if ((UnityEngine.Object) this._animator == (UnityEngine.Object) null)
        this._animator = this.GetComponent<Animator>();
      if ((UnityEngine.Object) this._animator == (UnityEngine.Object) null)
        this.LogError<HolderPose>("No Animator for HolderPose");
      return this._animator;
    }
  }

  public void Update()
  {
    foreach (HolderPose.Pose pose in this.Poses)
    {
      pose.Active = pose.PoseType == this.CurrentPose;
      pose.Apply(this.Animator);
    }
  }

  public enum PoseType
  {
    Empty,
    TwoHandedTool,
    OneHandedTool,
  }

  [Serializable]
  public class Pose
  {
    public HolderPose.PoseType PoseType;
    public int AnimatorLayer;
    public AnimationCurve WeightOverTime;
    private float _weightTime;

    public bool Active { get; set; }

    public void Apply(Animator animator)
    {
      float minTime = this.WeightOverTime.GetMinTime();
      float maxTime = this.WeightOverTime.GetMaxTime();
      this._weightTime = Mathf.Clamp(this._weightTime + Time.deltaTime * (!this.Active ? -1f : 1f), minTime, maxTime);
      if (animator.layerCount <= this.AnimatorLayer)
        return;
      animator.SetLayerWeight(this.AnimatorLayer, this.WeightOverTime.Evaluate(this._weightTime));
    }
  }
}
