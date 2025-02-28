// Decompiled with JetBrains decompiler
// Type: BlobShadowController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BlobShadowController : MonoBehaviour
{
  private Transform mTranform;

  public void Start() => this.mTranform = this.transform;

  public void Update()
  {
    this.mTranform.position = this.mTranform.parent.position + Vector3.up * 8.246965f;
    this.mTranform.rotation = Quaternion.LookRotation(-Vector3.up, this.mTranform.parent.forward);
  }
}
