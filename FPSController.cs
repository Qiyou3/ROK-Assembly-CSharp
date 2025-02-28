// Decompiled with JetBrains decompiler
// Type: FPSController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FPSController : MonoBehaviour
{
  public Animator CamAnimator;
  public Animator WeaponAnimator;
  public float moveSpeed;

  private void Start()
  {
  }

  private void Update()
  {
    this.CamAnimator.SetBool("Running", Input.GetKey(KeyCode.W));
    this.WeaponAnimator.SetBool("Fire", Input.GetKey(KeyCode.Space));
    if (!Input.GetKey(KeyCode.W))
      return;
    this.transform.position = this.transform.position + this.transform.forward * this.moveSpeed * Time.deltaTime;
  }
}
