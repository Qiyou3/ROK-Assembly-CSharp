// Decompiled with JetBrains decompiler
// Type: Explosion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Damaging;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Core.Utility.Attributes;
using CodeHatch.Engine.Networking;
using uLink;
using UnityEngine;

#nullable disable
public class Explosion : UnityEngine.MonoBehaviour, IDamager
{
  public float EndRadius = 10f;
  public float ExplosionTime = 0.5f;
  public float BlastStrength = 9000f;
  public float BlastDamage = 4f;
  public float radius = 12.5f;
  public float timeLapse;
  public GameObject Aftermath;
  public AudioClip ExplosionSound;
  public GameObject DestroyAtEndOfAftermath;
  [FlagEnum(typeof (DamageType))]
  private DamageType _DamageType = DamageType.Explosion;

  public void Start()
  {
    GameObject gameObject = new GameObject("Frag Grenade SFX");
    gameObject.transform.position = this.transform.position;
    gameObject.transform.rotation = this.transform.rotation;
    AudioSource audioSource = gameObject.AddComponent<AudioSource>();
    audioSource.clip = this.ExplosionSound;
    audioSource.maxDistance = 120f;
    audioSource.rolloffMode = AudioRolloffMode.Linear;
    audioSource.Play();
    Object.Destroy((Object) gameObject, 5f);
  }

  public void FixedUpdate()
  {
    this.timeLapse += Time.fixedDeltaTime;
    if ((double) this.timeLapse < (double) this.ExplosionTime)
    {
      float num = this.timeLapse / this.ExplosionTime * this.EndRadius;
      this.transform.localScale = new Vector3(num, num, num);
      this.radius = this.transform.localScale.x / 2f;
      foreach (Collider col in Physics.OverlapSphere(this.transform.position, this.radius))
        this.ImpactObject(col);
    }
    else
    {
      if (!Player.IsLocalServer)
        return;
      if ((Object) this.Aftermath != (Object) null)
      {
        GameObject gameObject = uLink.Network.Instantiate(this.Aftermath, this.transform.position, Quaternion.Euler(Vector3.zero), (NetworkGroup) 0);
        if ((Object) this.DestroyAtEndOfAftermath != (Object) null)
          gameObject.GetComponent<ExplosionAftermath>().DestroyAtEndOfAftermath = this.DestroyAtEndOfAftermath;
      }
      uLink.Network.Destroy(this.gameObject);
    }
  }

  private void ImpactObject(Collider col)
  {
    float explosionForce = this.BlastStrength * (this.radius / this.EndRadius);
    float num = this.BlastDamage * (this.radius / this.EndRadius);
    IDamageReceiver damageReceiver = col.gameObject.GetImplementor<IDamageReceiver>() ?? col.gameObject.FirstImplementingAncestor<IDamageReceiver>();
    if (damageReceiver != null)
    {
      Damage damage = new Damage((Object) this, this.DamageSource)
      {
        DamageTypes = this.DamageTypes,
        Amount = num,
        point = col.ClosestPointOnBounds(this.transform.position)
      };
      damageReceiver.OnDamage(damage);
    }
    Rigidbody rigidbody = col.attachedRigidbody ?? col.GetComponent<Rigidbody>();
    if (!((Object) rigidbody != (Object) null))
      return;
    rigidbody.AddExplosionForce(explosionForce, this.transform.position, this.radius);
  }

  public Entity DamageSource { get; set; }

  public DamageType DamageTypes
  {
    get => this._DamageType;
    set => this._DamageType = value;
  }
}
