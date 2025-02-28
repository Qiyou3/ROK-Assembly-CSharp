// Decompiled with JetBrains decompiler
// Type: FPSWeaponTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using PigeonCoopToolkit.Effects.Trails;
using UnityEngine;

#nullable disable
public class FPSWeaponTrigger : MonoBehaviour
{
  public Transform ShellEjectionTransform;
  public float EjectionForce;
  public Rigidbody Shell;
  public Transform Muzzle;
  public GameObject Bullet;
  public float SmokeAfter;
  public float SmokeMax;
  public float SmokeIncrement;
  public SmokePlume MuzzlePlume;
  public GameObject MuzzleFlashObject;
  private float _smoke;

  private void Update()
  {
    this.MuzzlePlume.Emit = (double) this._smoke > (double) this.SmokeAfter;
    this._smoke -= Time.deltaTime;
    if ((double) this._smoke > (double) this.SmokeMax)
      this._smoke = this.SmokeMax;
    if ((double) this._smoke >= 0.0)
      return;
    this._smoke = 0.0f;
  }

  public void Fire()
  {
    this.MuzzleFlashObject.SetActive(true);
    this.Invoke("LightsOff", 0.05f);
    this._smoke += this.SmokeIncrement;
    Rigidbody component = (Object.Instantiate((Object) this.Shell.gameObject, this.ShellEjectionTransform.position, this.ShellEjectionTransform.rotation) as GameObject).GetComponent<Rigidbody>();
    component.velocity = this.ShellEjectionTransform.right * this.EjectionForce + Random.onUnitSphere * 0.25f;
    component.angularVelocity = Random.onUnitSphere * this.EjectionForce;
    Object.Instantiate((Object) this.Bullet, this.Muzzle.transform.position, this.Muzzle.rotation);
  }

  private void LightsOff() => this.MuzzleFlashObject.SetActive(false);
}
