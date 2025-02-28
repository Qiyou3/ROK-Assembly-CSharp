// Decompiled with JetBrains decompiler
// Type: ConstrainAbove
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ConstrainAbove : MonoBehaviour
{
  public float Y;
  public float lerp = 0.5f;

  public void Update()
  {
    if ((double) this.transform.position.y >= (double) this.Y)
      return;
    this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, this.Y, this.transform.position.z), this.lerp);
  }
}
