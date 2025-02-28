// Decompiled with JetBrains decompiler
// Type: CameraTrack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CameraTrack : MonoBehaviour
{
  public Transform target;

  private void LateUpdate()
  {
    if (!(bool) (Object) this.target)
      return;
    Vector3 vector3 = this.transform.InverseTransformPoint(this.target.position);
    this.transform.Rotate(0.0f, Mathf.Atan2(vector3.x, vector3.z) * 57.29578f, 0.0f, Space.World);
  }
}
