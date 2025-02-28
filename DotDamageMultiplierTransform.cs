// Decompiled with JetBrains decompiler
// Type: DotDamageMultiplierTransform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Damaging;
using UnityEngine;

#nullable disable
public class DotDamageMultiplierTransform : MonoBehaviour, IDamageTypeModifier
{
  public Vector3 WorldVector = Vector3.up;
  public Vector3 LocalVector = Vector3.up;
  public float DamageMultiplier = 1.5f;

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
    this.GetComponent<Rigidbody>().drag = this.DamageMultiplier * amount;
  }

  public DamageVulnerability[] ModifyTypes => (DamageVulnerability[]) null;

  public void ModifyDamage(Damage damage)
  {
    damage.Amount *= Mathf.Lerp(1f, this.DamageMultiplier, this.Amount);
  }
}
