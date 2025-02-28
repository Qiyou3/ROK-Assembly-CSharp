// Decompiled with JetBrains decompiler
// Type: BezierTrack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class BezierTrack : MonoBehaviour
{
  public List<Transform> points;

  public int Count
  {
    get
    {
      if (this.points == null)
        this.InitPointsList();
      return this.points.Count;
    }
  }

  public static Vector3 GetTangent(Transform point, BezierTrack.Tangent tangent)
  {
    Vector3 vector3 = Vector3.zero;
    switch (tangent)
    {
      case BezierTrack.Tangent.Right:
        vector3 = Vector3.right * point.localScale.x;
        break;
      case BezierTrack.Tangent.Left:
        vector3 = Vector3.left * point.localScale.x;
        break;
      case BezierTrack.Tangent.Up:
        vector3 = Vector3.up * point.localScale.y;
        break;
      case BezierTrack.Tangent.Down:
        vector3 = Vector3.down * point.localScale.y;
        break;
      case BezierTrack.Tangent.Forward:
        vector3 = Vector3.forward * point.localScale.z;
        break;
      case BezierTrack.Tangent.Back:
        vector3 = Vector3.back * point.localScale.z;
        break;
    }
    return point.localRotation * vector3;
  }

  public int GetSafeT(ref float t)
  {
    int safeT = Mathf.Min(this.Count - 2, Mathf.FloorToInt(t));
    t -= (float) safeT;
    return safeT;
  }

  public Vector3 Sample(float t)
  {
    int safeT = this.GetSafeT(ref t);
    return this.Sample(this.points[safeT], this.points[safeT + 1], t);
  }

  public Vector3 SampleFwdTan(float t)
  {
    int safeT = this.GetSafeT(ref t);
    return this.SampleFwdTan(this.points[safeT], this.points[safeT + 1], t);
  }

  public Vector3 SampleUpTan(float t)
  {
    int safeT = this.GetSafeT(ref t);
    return this.SampleUpTan(this.points[safeT], this.points[safeT + 1], t);
  }

  public Vector3 SampleScale(float t)
  {
    int safeT = this.GetSafeT(ref t);
    return this.SampleScale(this.points[safeT], this.points[safeT + 1], t);
  }

  public Quaternion SampleRotation(float t)
  {
    int safeT = this.GetSafeT(ref t);
    return this.SampleRotation(this.points[safeT], this.points[safeT + 1], t);
  }

  public void SampleTransform(Transform result, float t)
  {
    int safeT = this.GetSafeT(ref t);
    this.SampleTransform(result, this.points[safeT], this.points[safeT + 1], t);
  }

  public Vector3 Sample(Transform first, Transform second, float t)
  {
    Vector3 localPosition1 = first.localPosition;
    Vector3 localPosition2 = second.localPosition;
    Vector3 vector3_1 = localPosition1 + BezierTrack.GetTangent(first, BezierTrack.Tangent.Forward);
    Vector3 vector3_2 = localPosition2 + BezierTrack.GetTangent(second, BezierTrack.Tangent.Back);
    Vector3 from = Vector3.Lerp(localPosition1, vector3_1, t);
    Vector3 vector3_3 = Vector3.Lerp(vector3_1, vector3_2, t);
    Vector3 to = Vector3.Lerp(vector3_2, localPosition2, t);
    return Vector3.Lerp(Vector3.Lerp(from, vector3_3, t), Vector3.Lerp(vector3_3, to, t), t);
  }

  public Vector3 SampleFwdTan(Transform first, Transform second, float t)
  {
    Vector3 localPosition1 = first.localPosition;
    Vector3 localPosition2 = second.localPosition;
    Vector3 vector3_1 = localPosition1 + BezierTrack.GetTangent(first, BezierTrack.Tangent.Forward);
    Vector3 vector3_2 = localPosition2 + BezierTrack.GetTangent(second, BezierTrack.Tangent.Back);
    Vector3 from = vector3_1 - localPosition1;
    Vector3 vector3_3 = vector3_2 - vector3_1;
    Vector3 to = localPosition2 - vector3_2;
    return Vector3.Lerp(Vector3.Lerp(from, vector3_3, t), Vector3.Lerp(vector3_3, to, t), t);
  }

  public Vector3 SampleUpTan(Transform first, Transform second, float t)
  {
    Vector3 normalized1 = BezierTrack.GetTangent(first, BezierTrack.Tangent.Up).normalized;
    Vector3 normalized2 = BezierTrack.GetTangent(second, BezierTrack.Tangent.Up).normalized;
    Vector3 from1 = normalized1;
    Vector3 to1 = normalized2;
    Vector3 from2 = normalized1;
    Vector3 normalized3 = Vector3.Slerp(from1, to1, t).normalized;
    Vector3 to2 = normalized2;
    return Vector3.Slerp(Vector3.Slerp(from2, normalized3, t).normalized, Vector3.Slerp(normalized3, to2, t).normalized, t).normalized;
  }

  public Quaternion SampleRotation(Transform first, Transform second, float t)
  {
    Quaternion localRotation1 = first.localRotation;
    Quaternion localRotation2 = second.localRotation;
    Quaternion from1 = localRotation1;
    Quaternion to1 = localRotation2;
    Quaternion from2 = localRotation1;
    Quaternion quaternion = Quaternion.Lerp(from1, to1, t);
    Quaternion to2 = localRotation2;
    return Quaternion.Lerp(Quaternion.Lerp(from2, quaternion, t), Quaternion.Lerp(quaternion, to2, t), t);
  }

  public Vector3 SampleScale(Transform first, Transform second, float t)
  {
    Vector3 localScale1 = first.localScale;
    Vector3 localScale2 = second.localScale;
    Vector3 from1 = localScale1;
    Vector3 to1 = localScale2;
    Vector3 from2 = localScale1;
    Vector3 vector3 = Vector3.Lerp(from1, to1, t);
    Vector3 to2 = localScale2;
    return Vector3.Lerp(Vector3.Lerp(from2, vector3, t), Vector3.Lerp(vector3, to2, t), t);
  }

  public void SampleTransform(Transform result, Transform first, Transform second, float t)
  {
    Vector3 vector3_1 = this.Sample(first, second, t);
    Vector3 normalized = this.SampleFwdTan(first, second, t).normalized;
    Vector3 vector3_2 = this.SampleScale(first, second, t);
    Quaternion quaternion1 = Quaternion.LookRotation(normalized, this.SampleUpTan(first, second, t));
    Quaternion quaternion2 = Quaternion.FromToRotation(quaternion1 * Vector3.forward, normalized) * quaternion1;
    result.localPosition = vector3_1;
    result.localRotation = quaternion2;
    result.localScale = vector3_2;
  }

  public void Start() => this.InitPointsList();

  private void InitPointsList()
  {
    this.points = ((IEnumerable<Transform>) this.GetComponentsInChildren<Transform>()).Where<Transform>((Func<Transform, bool>) (t => (UnityEngine.Object) t != (UnityEngine.Object) this.transform)).ToList<Transform>();
  }

  public void Update()
  {
    if (Application.isPlaying)
      return;
    this.InitPointsList();
  }

  public enum Tangent
  {
    None,
    Right,
    Left,
    Up,
    Down,
    Forward,
    Back,
  }
}
