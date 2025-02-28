// Decompiled with JetBrains decompiler
// Type: AnalyticalAimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AnalyticalAimer
{
  public GameObject m_target;
  public Transform m_transform;
  public float m_bulletSpeed;
  private Vector3 m_lastPosition;
  private int m_validAim;

  public void SetTarget(GameObject target)
  {
    if (!((Object) target != (Object) this.m_target))
      return;
    this.m_target = target;
    if ((bool) (Object) target)
      this.m_lastPosition = target.transform.position;
    this.m_validAim = -1;
  }

  public static bool SolveQuadratic(out float first, out float second, float a, float b, float c)
  {
    float f = (float) ((double) b * (double) b - 4.0 * (double) a * (double) c);
    if ((double) f < 0.0)
    {
      first = 0.0f;
      second = 0.0f;
      return false;
    }
    float num1 = Mathf.Sqrt(f);
    float num2 = 2f * a;
    first = (-b + num1) / num2;
    second = (-b - num1) / num2;
    return true;
  }

  public Vector3 Predict(out Vector3 predictedPosition, out float time, out bool valid)
  {
    if (this.m_validAim < 0)
    {
      time = 0.0f;
      valid = false;
      this.m_lastPosition = this.m_target.transform.position;
    }
    valid = true;
    ++this.m_validAim;
    Vector3 vector3_1;
    if ((bool) (Object) this.m_target.GetComponent<Rigidbody>() && !this.m_target.GetComponent<Rigidbody>().isKinematic)
    {
      vector3_1 = this.m_target.GetComponent<Rigidbody>().velocity;
    }
    else
    {
      vector3_1 = (this.m_target.transform.position - this.m_lastPosition) / Time.fixedDeltaTime;
      this.m_lastPosition = this.m_target.transform.position;
    }
    Vector3 vector3_2 = this.m_target.transform.position - this.m_transform.position;
    float a = vector3_1.sqrMagnitude - this.m_bulletSpeed * this.m_bulletSpeed;
    float b = (float) (2.0 * ((double) vector3_2.x + (double) vector3_2.y + (double) vector3_2.z));
    float sqrMagnitude = vector3_2.sqrMagnitude;
    float first;
    float second;
    if (AnalyticalAimer.SolveQuadratic(out first, out second, a, b, sqrMagnitude))
    {
      time = first;
      if ((double) time < 0.0 || (double) second < (double) time && (double) second > 0.0)
        time = second;
      predictedPosition = this.m_target.transform.position + vector3_1 * time;
      return (predictedPosition - this.m_transform.position).normalized;
    }
    time = 0.0f;
    predictedPosition = this.m_target.transform.position;
    return this.m_transform.forward;
  }
}
