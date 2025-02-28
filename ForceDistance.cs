// Decompiled with JetBrains decompiler
// Type: ForceDistance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ForceDistance : MonoBehaviour
{
  public Transform target;
  public float maxDistance = 1.5f;
  public float minDistance;

  public void Start()
  {
    if ((bool) (Object) this.target)
      return;
    this.enabled = false;
  }

  public void FixedUpdate()
  {
    if (!(bool) (Object) this.target)
    {
      this.enabled = false;
    }
    else
    {
      Vector3 vector3 = this.GetComponent<Rigidbody>().position - this.target.position;
      float magnitude = vector3.magnitude;
      if ((double) magnitude > (double) this.maxDistance)
      {
        vector3 *= this.maxDistance / magnitude;
        this.GetComponent<Rigidbody>().position = this.target.position + vector3;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
      }
      else
      {
        if ((double) magnitude >= (double) this.minDistance)
          return;
        vector3 *= this.minDistance / magnitude;
        this.GetComponent<Rigidbody>().position = this.target.position + vector3;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
      }
    }
  }
}
