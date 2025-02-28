// Decompiled with JetBrains decompiler
// Type: BillboardRotate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BillboardRotate : MonoBehaviour
{
  public void OnWillRenderObject()
  {
    Camera current = Camera.current;
    Vector3 up = this.transform.up;
    this.transform.LookAt(current.transform);
    this.transform.rotation = Quaternion.FromToRotation(this.transform.up, up) * this.transform.rotation;
  }
}
