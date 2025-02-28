// Decompiled with JetBrains decompiler
// Type: HingeController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HingeController : MonoBehaviour
{
  public HingeJoint theHingeJoint;
  public float ClosedAngle;
  public float ClosedAngleUpper = 1f;
  public bool IsGrabed;
  private JointMotor motor;

  public void Start()
  {
    if ((Object) this.theHingeJoint == (Object) null)
      this.theHingeJoint = this.GetComponent<HingeJoint>();
    this.theHingeJoint.useMotor = true;
    this.motor = this.theHingeJoint.motor;
    this.motor.freeSpin = true;
  }

  public void FixedUpdate()
  {
    if ((double) this.theHingeJoint.angle <= (double) this.ClosedAngleUpper && !this.IsGrabed)
      this.GetComponent<Rigidbody>().isKinematic = true;
    else
      this.GetComponent<Rigidbody>().isKinematic = false;
    if ((double) this.theHingeJoint.angle >= (double) this.ClosedAngle)
      return;
    this.SetToClosedAngle();
  }

  private void SetToClosedAngle()
  {
    this.transform.localRotation = Quaternion.Euler(this.transform.localRotation.eulerAngles with
    {
      y = this.ClosedAngle
    });
  }
}
