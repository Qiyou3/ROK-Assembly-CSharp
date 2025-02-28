// Decompiled with JetBrains decompiler
// Type: Echo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using UnityEngine;

#nullable disable
public class Echo : MonoBehaviour
{
  public Transform audioSource;
  public Transform plane;

  public void FixedUpdate()
  {
    this.transform.position = this.audioSource.position + Vector3UtilEx.Parallel(this.plane.position - this.audioSource.position, this.plane.up) * 2f;
  }
}
