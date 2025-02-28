// Decompiled with JetBrains decompiler
// Type: ExplosionAftermath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ExplosionAftermath : MonoBehaviour
{
  public float EmissionTime = 120f;
  public float TimeTillDestroy = 186f;
  public ParticleSystem[] Emiters;
  public GameObject DestroyAtEndOfAftermath;

  public void Awake() => this.Emiters = this.GetComponentsInChildren<ParticleSystem>();

  public void Start() => this.transform.rotation = Quaternion.Euler(Vector3.zero);

  public void Update()
  {
    this.EmissionTime -= Time.deltaTime;
    this.TimeTillDestroy -= Time.deltaTime;
    if ((double) this.EmissionTime < 0.0)
    {
      for (int index = 0; index < this.Emiters.Length; ++index)
        this.Emiters[index].enableEmission = false;
      if ((Object) this.GetComponent<Collider>() != (Object) null)
        this.GetComponent<Collider>().enabled = false;
    }
    if ((double) this.TimeTillDestroy >= 0.0)
      return;
    if ((Object) this.DestroyAtEndOfAftermath != (Object) null)
      Object.Destroy((Object) this.DestroyAtEndOfAftermath);
    Object.Destroy((Object) this.gameObject);
  }
}
