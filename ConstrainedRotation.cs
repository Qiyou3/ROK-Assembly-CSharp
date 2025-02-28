// Decompiled with JetBrains decompiler
// Type: ConstrainedRotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ConstrainedRotation : MonoBehaviour
{
  public Transform goalRotation;
  public Transform constraintBase;
  public float slerpLimitDegrees;
  public bool constrainGoal = true;
  public float goalLimitDegrees = 150f;
  private Quaternion _prevGoalRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
  private Quaternion _prevBaseRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
  private Quaternion _constrainedRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

  public Quaternion Rotation
  {
    get
    {
      this.UpdateConstrainedRotation();
      return this._constrainedRotation;
    }
  }

  private void UpdateConstrainedRotation()
  {
    Quaternion rotation1 = this.goalRotation.rotation;
    Quaternion rotation2 = this.constraintBase.rotation;
    if (rotation1 == this._prevGoalRotation && rotation2 == this._prevBaseRotation)
      return;
    this._constrainedRotation = Quaternion.RotateTowards(rotation2, rotation1, this.slerpLimitDegrees);
    this.transform.rotation = this._constrainedRotation;
    if (this.constrainGoal)
    {
      this.goalRotation.rotation = Quaternion.RotateTowards(rotation2, rotation1, this.goalLimitDegrees);
      this.goalRotation.localRotation = Quaternion.Euler(this.goalRotation.localRotation.eulerAngles with
      {
        z = 0.0f
      });
      this._prevGoalRotation = this.goalRotation.rotation;
      this._prevBaseRotation = rotation2;
    }
    else
    {
      this._prevGoalRotation = rotation1;
      this._prevBaseRotation = rotation2;
    }
  }

  public void Update() => this.UpdateConstrainedRotation();

  public void FixedUpdate() => this.UpdateConstrainedRotation();

  public void LateUpdate() => this.UpdateConstrainedRotation();
}
