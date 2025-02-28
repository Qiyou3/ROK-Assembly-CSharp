// Decompiled with JetBrains decompiler
// Type: ForceLocalPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ForceLocalPosition : MonoBehaviour
{
  public Transform forceLocalTo;

  public void Update()
  {
    if ((Object) this.forceLocalTo == (Object) null)
      return;
    this.transform.localPosition = this.forceLocalTo.localPosition;
    this.transform.localRotation = this.forceLocalTo.localRotation;
  }

  public void LateUpdate()
  {
    if ((Object) this.forceLocalTo == (Object) null)
      return;
    this.transform.localPosition = this.forceLocalTo.localPosition;
    this.transform.localRotation = this.forceLocalTo.localRotation;
  }

  public void FixedUpdate()
  {
    if ((Object) this.forceLocalTo == (Object) null)
      return;
    this.transform.localPosition = this.forceLocalTo.localPosition;
    this.transform.localRotation = this.forceLocalTo.localRotation;
  }
}
