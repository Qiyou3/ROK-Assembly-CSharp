// Decompiled with JetBrains decompiler
// Type: Ghost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Ghost : MonoBehaviour
{
  public GameObject[] proxies;
  public GameObject[] reals;
  public Rigidbody realRigidBody;
  public float springStrength = 10f;
  public bool syncChildPositions;
  private Transform[] parents;
  private Transform[] transforms;
  private Vector3[] positions;
  private Quaternion[] rotations;
  private bool started;

  public void Start()
  {
    if (this.reals.Length != this.proxies.Length)
    {
      this.LogError<Ghost>("Mismatch between real and proxy length for ghosting");
      this.enabled = false;
    }
    else
    {
      this.parents = new Transform[this.reals.Length];
      this.transforms = new Transform[this.reals.Length];
      this.positions = new Vector3[this.reals.Length];
      this.rotations = new Quaternion[this.reals.Length];
      for (int index = 0; index < this.reals.Length; ++index)
      {
        this.transforms[index] = this.proxies[index].transform;
        this.parents[index] = this.reals[index].transform;
        this.positions[index] = this.reals[index].transform.position;
        this.rotations[index] = this.reals[index].transform.rotation;
      }
      this.started = true;
    }
  }

  public void LateUpdate()
  {
    if (!this.started)
      return;
    float t = Mathf.Clamp01(Time.deltaTime * this.springStrength);
    Vector3 vector3 = !((Object) this.realRigidBody != (Object) null) ? Vector3.zero : this.realRigidBody.velocity * Time.deltaTime;
    for (int index = 0; index < this.reals.Length; ++index)
    {
      if (index == 0 || this.syncChildPositions)
      {
        this.positions[index] = Vector3.Lerp(this.positions[index] + vector3, this.reals[index].transform.position, t);
        this.proxies[index].transform.position = this.positions[index];
      }
      this.rotations[index] = Quaternion.Slerp(this.rotations[index], this.reals[index].transform.rotation, t);
      this.proxies[index].transform.rotation = this.rotations[index];
    }
  }
}
