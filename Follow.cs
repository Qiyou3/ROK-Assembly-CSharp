// Decompiled with JetBrains decompiler
// Type: Follow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (Target))]
public class Follow : MonoBehaviour
{
  public float lerp = 0.1f;
  public float distanceMin = 5f;
  public float distanceMax = 10f;

  public void Update()
  {
    Transform target = this.GetComponent<Target>().target;
    Vector3 a = target.position - this.transform.position;
    float num = Vector3.Magnitude(a);
    if ((double) num < (double) this.distanceMin)
    {
      this.transform.position = Vector3.Lerp(this.transform.position, target.position - a / num * this.distanceMin, this.lerp);
    }
    else
    {
      if ((double) num <= (double) this.distanceMax)
        return;
      this.transform.position = Vector3.Lerp(this.transform.position, target.position - a / num * this.distanceMax, this.lerp);
    }
  }
}
