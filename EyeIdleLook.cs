// Decompiled with JetBrains decompiler
// Type: EyeIdleLook
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Entities.Definitions;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EyeIdleLook : EntityBehaviour
{
  public AnimationCurve RetargetTimeContinuum = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.5f),
    new Keyframe(1f, 1.3f)
  });
  public AnimationCurve RetargetTimeScaleAtTargetVelocity = AnimationCurve.Linear(0.0f, 1f, 0.0f, 1f);
  public Vector3 forward = Vector3.left;
  public float MaxLookAngle = 30f;
  public float MaxDetectAngle = 30f;
  public float DefaultLookDistance = 10f;
  public float LODDistance = 50f;
  public Vector3 RandomLookOffset = new Vector3(0.4f, 1.2f, 0.4f);
  public AnimationCurve FromToGoalAnimation;
  public EyeIdleLook.Target CurrentTarget;
  public EyeIdleLook.Target LastTarget;
  private float RetargetTime;
  public Transform[] EyeTransforms;

  public Transform ValidTarget
  {
    get
    {
      List<Transform> transformList = new List<Transform>();
      if (this.Entity.IsLocallyControlled && (UnityEngine.Object) Camera.main != (UnityEngine.Object) null)
        return Camera.main.transform;
      return !this.Entity.IsLocallyControlled ? Entity.TryGetLocalPlayer().GetOrCreate<CharacterDefinition>().GetTransform(CharacterDefinition.Part.EyesCenter) : (Transform) null;
    }
  }

  public void GetNewTarget()
  {
    this.LastTarget = this.CurrentTarget;
    bool flag = false;
    Transform validTarget = this.ValidTarget;
    if ((UnityEngine.Object) validTarget != (UnityEngine.Object) null)
    {
      float angle;
      Quaternion.FromToRotation(this.transform.forward, validTarget.position - this.transform.position).ToAngleAxis(out angle, out Vector3 _);
      if ((double) angle < (double) this.MaxDetectAngle)
      {
        this.CurrentTarget = new EyeIdleLook.Target()
        {
          relativePosition = Vector3.zero,
          transform = validTarget,
          time = Time.time
        };
        flag = true;
      }
    }
    if (!flag)
      this.CurrentTarget = new EyeIdleLook.Target()
      {
        relativePosition = this.transform.position + this.transform.forward * this.DefaultLookDistance,
        time = Time.time
      };
    Vector3 planeNormal = this.CurrentTarget.CurrentPosition - this.transform.position;
    this.CurrentTarget.relativePosition += Vector3.Scale(Vector3.ProjectOnPlane(UnityEngine.Random.insideUnitSphere, planeNormal), this.RandomLookOffset) * planeNormal.magnitude;
    if (this.RetargetTimeContinuum.length <= 0)
      return;
    this.RetargetTime = Time.time + this.RetargetTimeContinuum.Evaluate(Mathf.Lerp(this.RetargetTimeContinuum[0].time, this.RetargetTimeContinuum[this.RetargetTimeContinuum.length - 1].time, UnityEngine.Random.value));
  }

  public Vector3 CurrentLookPosition
  {
    get
    {
      return Vector3.Lerp(this.LastTarget.CurrentPosition, this.CurrentTarget.CurrentPosition, this.FromToGoalAnimation.Evaluate(Time.time - this.CurrentTarget.time));
    }
  }

  public void Update()
  {
    Entity entity = this.TryGetEntity();
    if ((UnityEngine.Object) entity == (UnityEngine.Object) null)
      return;
    PlayerHealth playerHealth = entity.TryGet<PlayerHealth>();
    if ((UnityEngine.Object) playerHealth == (UnityEngine.Object) null || playerHealth.IsDead || (double) entity.GetOrCreate<CodeHatch.Engine.Modules.ScriptLOD.ScriptLOD>().Distance > (double) this.LODDistance)
      return;
    if ((double) Time.time > (double) this.RetargetTime)
      this.GetNewTarget();
    Vector3 currentLookPosition = this.CurrentLookPosition;
    foreach (Transform eyeTransform in this.EyeTransforms)
    {
      eyeTransform.rotation = Quaternion.FromToRotation(this.transform.forward, currentLookPosition - eyeTransform.position) * eyeTransform.parent.rotation;
      float angle;
      Vector3 axis;
      eyeTransform.localRotation.ToAngleAxis(out angle, out axis);
      if ((double) angle > (double) this.MaxDetectAngle)
        this.GetNewTarget();
      angle = Mathf.Clamp(angle, -this.MaxLookAngle, this.MaxLookAngle);
      eyeTransform.localRotation = Quaternion.AngleAxis(angle, axis);
    }
  }

  [Serializable]
  public struct Target
  {
    public Vector3 relativePosition;
    public Transform transform;
    public float time;

    public Vector3 CurrentPosition
    {
      get
      {
        return (UnityEngine.Object) this.transform == (UnityEngine.Object) null ? this.relativePosition : this.relativePosition + this.transform.position;
      }
    }
  }
}
