// Decompiled with JetBrains decompiler
// Type: debris_force
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class debris_force : MonoBehaviour
{
  private float t;

  private void Start()
  {
    this.transform.eulerAngles = new Vector3(Random.Range(-0.0f, -45f), Random.Range(-180f, 180f), 0.0f);
    this.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 20f * Random.Range(1f, 1.5f));
    this.transform.localScale = new Vector3(Random.Range(0.6f, 1.2f), Random.Range(0.3f, 1f), Random.Range(0.3f, 1f));
  }

  private void Update()
  {
    this.GetComponent<Renderer>().material.color += (new Color(1f, 1f, 1f, 1f) - this.GetComponent<Renderer>().material.color) / 30f;
  }

  private void FixedUpdate() => this.GetComponent<Rigidbody>().AddForce(-Vector3.up / 2f);
}
