// Decompiled with JetBrains decompiler
// Type: AlwaysForward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AlwaysForward : MonoBehaviour
{
  public float Speed;
  public float yRotation;

  private void Update()
  {
    this.transform.position = this.transform.position + this.transform.forward * this.Speed;
    this.transform.Rotate(Vector3.up, this.yRotation);
  }
}
