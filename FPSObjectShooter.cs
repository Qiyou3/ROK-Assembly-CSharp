// Decompiled with JetBrains decompiler
// Type: FPSObjectShooter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FPSObjectShooter : MonoBehaviour
{
  public GameObject Element;
  public float InitialSpeed = 1f;
  public float MouseSpeed = 0.3f;
  public float Scale = 1f;
  public float Mass = 1f;
  public float Life = 10f;
  private Vector3 m_v3MousePosition;

  private void Start() => this.m_v3MousePosition = Input.mousePosition;

  private void Update()
  {
    if ((Object) this.Element != (Object) null && Input.GetKeyDown(KeyCode.Space))
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.Element);
      gameObject.transform.position = this.transform.position;
      gameObject.transform.localScale = new Vector3(this.Scale, this.Scale, this.Scale);
      gameObject.GetComponent<Rigidbody>().mass = this.Mass;
      gameObject.GetComponent<Rigidbody>().solverIterationCount = (int) byte.MaxValue;
      gameObject.GetComponent<Rigidbody>().AddForce(this.transform.forward * this.InitialSpeed, ForceMode.VelocityChange);
      gameObject.AddComponent<DieTimer>().SecondsToDie = this.Life;
    }
    if (Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0))
    {
      this.transform.Rotate((float) -((double) Input.mousePosition.y - (double) this.m_v3MousePosition.y) * this.MouseSpeed, 0.0f, 0.0f);
      this.transform.RotateAround(this.transform.position, Vector3.up, (Input.mousePosition.x - this.m_v3MousePosition.x) * this.MouseSpeed);
    }
    this.m_v3MousePosition = Input.mousePosition;
  }
}
