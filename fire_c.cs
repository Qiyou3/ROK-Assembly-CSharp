// Decompiled with JetBrains decompiler
// Type: fire_c
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class fire_c : MonoBehaviour
{
  private float t;
  private float rnd;

  private void Start()
  {
  }

  private void Update()
  {
    this.t += Time.deltaTime * 10f;
    if ((double) this.t >= 1.0)
    {
      this.t = 0.0f;
      this.rnd = Random.Range(0.55f, 0.65f);
    }
    this.GetComponent<Light>().intensity += (float) (((double) this.rnd - (double) this.GetComponent<Light>().intensity) / 5.0);
  }
}
