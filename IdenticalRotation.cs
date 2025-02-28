// Decompiled with JetBrains decompiler
// Type: IdenticalRotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class IdenticalRotation : MonoBehaviour
{
  public Transform target;

  public void Update() => this.LateUpdate();

  public void LateUpdate()
  {
    if ((bool) (Object) this.target)
      this.transform.rotation = this.target.rotation;
    else
      this.transform.rotation = Quaternion.identity;
  }
}
