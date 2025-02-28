// Decompiled with JetBrains decompiler
// Type: CircularTargetTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CircularTargetTracker : TargetTracker
{
  public LineRenderer m_lineRenderer;
  private static int queueSize = 5;
  private Queue<Vector3> m_firstHalf;
  private Queue<Vector3> m_secondHalf;
  private Vector3 m_direction;
  private Vector3 m_lastPosition;
  private Vector3 m_start;
  private Vector3 m_middle;
  private Vector3 m_end;
  private Vector3 m_circleCentre;
  private Vector3 m_circleNormal;
  private bool m_isLinear;
  private float m_speed;
  private int m_validCount;

  public CircularTargetTracker()
  {
    Vector3 vector3 = Vector3.zero;
    this.m_firstHalf = new Queue<Vector3>();
    this.m_secondHalf = new Queue<Vector3>();
    if ((bool) (Object) this.m_target)
      vector3 = this.m_target.position;
    for (int index = 0; index < CircularTargetTracker.queueSize; ++index)
    {
      this.m_firstHalf.Enqueue(vector3);
      this.m_secondHalf.Enqueue(vector3);
    }
    this.m_validCount = -20;
  }

  public override Transform Target
  {
    get => this.m_target;
    set
    {
      if (!((Object) this.m_target != (Object) value))
        return;
      this.m_target = value;
      this.m_validCount = -20;
    }
  }

  public override bool IsValidAim() => this.m_validCount > 0;

  public override void UpdateTracker(float timeStep)
  {
    ++this.m_validCount;
    this.m_start = this.m_target.position;
    this.m_middle = this.m_firstHalf.Dequeue();
    this.m_end = this.m_secondHalf.Dequeue();
    Vector3 normalized1 = (this.m_start - this.m_middle).normalized;
    this.m_direction = this.m_start - this.m_lastPosition;
    this.m_speed = this.m_direction.magnitude / timeStep;
    Vector3 normalized2 = (this.m_middle - this.m_end).normalized;
    if ((double) Vector3.Dot(normalized1, normalized2) > 0.99989998340606689)
    {
      this.m_isLinear = true;
      this.m_lastPosition = this.m_target.position;
      this.m_firstHalf.Enqueue(this.m_start);
      this.m_secondHalf.Enqueue(this.m_middle);
    }
    else
    {
      this.m_isLinear = false;
      Plane plane1 = new Plane(this.m_start, this.m_middle, this.m_end);
      Ray ray = new Ray((this.m_start + this.m_middle) * 0.5f, Vector3.Cross(normalized1, plane1.normal));
      Vector3 inPoint = (this.m_middle + this.m_end) * 0.5f;
      Plane plane2 = new Plane(normalized2, inPoint);
      float enter = 0.0f;
      plane2.Raycast(ray, out enter);
      this.m_circleCentre = ray.GetPoint(enter);
      this.m_circleNormal = plane1.normal;
      Quaternion quaternion = Quaternion.AngleAxis(10f, this.m_circleNormal);
      if ((bool) (Object) this.m_lineRenderer)
      {
        this.m_lineRenderer.SetVertexCount(36);
        Vector3 vector3 = this.m_start - this.m_circleCentre;
        for (int index = 0; index < 36; ++index)
        {
          this.m_lineRenderer.SetPosition(index, this.m_circleCentre + vector3);
          vector3 = quaternion * vector3;
        }
      }
      this.m_lastPosition = this.m_target.position;
      this.m_firstHalf.Enqueue(this.m_start);
      this.m_secondHalf.Enqueue(this.m_middle);
    }
  }

  public override Vector3 PredictPosition(float secondsInFuture)
  {
    if (this.m_isLinear)
      return this.m_start + this.m_direction.normalized * this.m_speed * secondsInFuture;
    Vector3 vector3 = this.m_start - this.m_circleCentre;
    return this.m_circleCentre + Quaternion.AngleAxis((float) (-(double) (secondsInFuture * this.m_speed / vector3.magnitude) * 57.295780181884766), this.m_circleNormal) * vector3;
  }
}
