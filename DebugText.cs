// Decompiled with JetBrains decompiler
// Type: DebugText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DebugText : MonoBehaviour
{
  public float DesiredWorldSize = 1f;
  public float MinAngularSize = 1f / 1000f;
  public float MaxAngularSize = 0.1f;

  public void OnWillRenderObject()
  {
    Transform transform = Camera.current.transform;
    this.transform.rotation = transform.rotation;
    float num1 = Vector3.Dot(transform.forward, this.transform.position - transform.position);
    float num2 = Mathf.Clamp(this.DesiredWorldSize / num1, this.MinAngularSize, this.MaxAngularSize) * num1;
    this.transform.localScale = new Vector3(num2, num2, num2);
  }
}
