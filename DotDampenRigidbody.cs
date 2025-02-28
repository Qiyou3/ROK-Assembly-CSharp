// Decompiled with JetBrains decompiler
// Type: DotDampenRigidbody
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DotDampenRigidbody : MonoBehaviour
{
  public Vector3 WorldVector = Vector3.up;
  public Vector3 LocalVector = Vector3.up;
  public float Drag = 1f;
  public float AngularDrag = 1f;

  public float Amount
  {
    get
    {
      return Mathf.Clamp01(1f - Mathf.Abs(Vector3.Dot(this.transform.TransformDirection(this.LocalVector), this.WorldVector)));
    }
  }

  public void Start()
  {
    this.WorldVector.Normalize();
    this.LocalVector.Normalize();
  }

  public void Update()
  {
    if ((Object) this.GetComponent<Rigidbody>() == (Object) null)
      return;
    float amount = this.Amount;
    this.GetComponent<Rigidbody>().drag = this.Drag * amount;
    this.GetComponent<Rigidbody>().angularDrag = this.AngularDrag * amount;
  }
}
