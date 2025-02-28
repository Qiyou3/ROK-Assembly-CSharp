// Decompiled with JetBrains decompiler
// Type: FlipCode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (Rigidbody))]
public class FlipCode : MonoBehaviour
{
  public void Start()
  {
  }

  public void Update()
  {
    if (Input.GetKeyDown(KeyCode.F) && (double) this.GetComponent<Rigidbody>().velocity.sqrMagnitude <= 1.0 && !this.gameObject.GetComponent<CarDynamics>().AllWheelsOnGround())
    {
      this.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, this.GetComponent<Rigidbody>().mass * 300f, 0.0f));
      this.LogInfo<FlipCode>("{0}", (object) this.transform.localEulerAngles.z);
      int num = 150;
      if ((double) this.transform.localEulerAngles.z < 180.0)
        num *= -1;
      this.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.forward * this.GetComponent<Rigidbody>().mass * (float) num);
    }
    if (!Input.GetKeyDown(KeyCode.B))
      return;
    this.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * this.GetComponent<Rigidbody>().mass * 300f);
  }
}
