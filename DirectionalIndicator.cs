// Decompiled with JetBrains decompiler
// Type: DirectionalIndicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Utility;
using System;
using UnityEngine;

#nullable disable
public class DirectionalIndicator : MonoBehaviour
{
  private static DirectionalIndicator _instance;
  public Transform Object;
  public Transform Camera;
  public Transform Arrow;
  public SoftLimiter frontAngleLimit;
  public AnimationCurve backCurve;
  public float backSphereRadius = 0.3f;

  public void Awake()
  {
    if (!((UnityEngine.Object) DirectionalIndicator._instance == (UnityEngine.Object) null))
      return;
    DirectionalIndicator._instance = this;
  }

  public void Start()
  {
    if (!((UnityEngine.Object) this.Camera == (UnityEngine.Object) null))
      return;
    this.Camera = this.transform.parent;
  }

  public void Update()
  {
    if ((UnityEngine.Object) this.Object == (UnityEngine.Object) null)
    {
      DirectionalIndicator.Deactivate();
    }
    else
    {
      Vector3 vector3_1 = this.Object.position - this.Camera.position;
      Vector3 vector3_2 = this.transform.position - this.Camera.position;
      Vector3 vector3_3 = vector3_1.LimitMagnitude(this.transform.localPosition.z);
      float time = Vector3.Angle(vector3_2, vector3_3);
      float limited = this.frontAngleLimit.GetLimited(Vector3.Angle(vector3_2, vector3_3));
      Vector3 vector3_4 = Vector3.RotateTowards(vector3_2, vector3_3, limited * ((float) Math.PI / 180f), float.PositiveInfinity);
      Vector3 from1 = vector3_4;
      Quaternion from2 = QuaternionUtil.LookRotation(vector3_1.Perpendicular(this.Camera.forward), -vector3_4, true);
      Quaternion to1 = Quaternion.LookRotation(vector3_1, -this.Camera.forward);
      Vector3 to2 = this.transform.position - this.Camera.position + to1 * Vector3.forward * this.backSphereRadius;
      float t = this.backCurve.Evaluate(time);
      this.Arrow.position = Vector3.Slerp(from1, to2, t) + this.Camera.position;
      this.Arrow.rotation = Quaternion.Slerp(from2, to1, t);
    }
  }

  private void SetColor()
  {
    float num = Vector3.Dot(this.Camera.forward, (this.Arrow.position - this.Camera.position).normalized);
    Color color = this.Arrow.GetComponent<Renderer>().material.color with
    {
      a = Mathf.Clamp01((float) (((double) num - 0.99250000715255737) / -0.042500019073486328))
    };
    this.Arrow.GetComponent<Renderer>().material.color = color;
  }

  public static void Target(Transform targetTransform)
  {
    if (!((UnityEngine.Object) DirectionalIndicator._instance != (UnityEngine.Object) null))
      return;
    DirectionalIndicator._instance.Object = targetTransform;
  }

  public static void Activate()
  {
    if (!((UnityEngine.Object) DirectionalIndicator._instance != (UnityEngine.Object) null))
      return;
    DirectionalIndicator._instance.gameObject.SetActive(true);
  }

  public static void Deactivate()
  {
    if (!((UnityEngine.Object) DirectionalIndicator._instance != (UnityEngine.Object) null))
      return;
    DirectionalIndicator._instance.Object = DirectionalIndicator._instance.transform;
    DirectionalIndicator._instance.gameObject.SetActive(false);
  }
}
