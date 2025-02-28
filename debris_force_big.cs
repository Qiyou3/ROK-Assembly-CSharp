// Decompiled with JetBrains decompiler
// Type: debris_force_big
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class debris_force_big : MonoBehaviour
{
  private float t;
  public float delay = 0.7f;
  private bool expl;

  private void Start() => this.GetComponent<Collider>().enabled = false;

  private void explo()
  {
    this.expl = true;
    this.GetComponent<Collider>().enabled = true;
    this.transform.eulerAngles = new Vector3(Random.Range(-0.0f, -45f), Random.Range(-180f, 180f), 0.0f);
    this.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 30f * Random.Range(1f, 1.5f));
    this.transform.localScale = new Vector3(Random.Range(0.9f, 1.4f), Random.Range(0.8f, 1.2f), Random.Range(0.9f, 1.4f));
  }

  private void Update()
  {
    this.t += Time.deltaTime;
    if ((double) this.t < (double) this.delay)
      return;
    this.GetComponent<Renderer>().material.color += (new Color(1f, 1f, 1f, 1f) - this.GetComponent<Renderer>().material.color) / 30f;
  }

  private void FixedUpdate()
  {
    if ((double) this.t < (double) this.delay)
      return;
    if (!this.expl)
      this.explo();
    this.GetComponent<Rigidbody>().AddForce(-Vector3.up / 2f);
  }
}
