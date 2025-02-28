// Decompiled with JetBrains decompiler
// Type: CarDebug
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CarDebug : MonoBehaviour
{
  private Drivetrain drivetrain;
  private Axles axles;
  private ForceFeedback forceFeedback;

  public void Start()
  {
    this.drivetrain = this.GetComponent<Drivetrain>();
    this.axles = this.GetComponent<Axles>();
    this.forceFeedback = this.GetComponent<ForceFeedback>();
  }

  public float RoundTo(float value, int precision)
  {
    int num = 1;
    for (int index = 1; index <= precision; ++index)
      num *= 10;
    return Mathf.Round(value * (float) num) / (float) num;
  }

  public void OnGUI()
  {
    GUILayout.Label("clutch.GetClutchPosition: " + (object) this.drivetrain.clutch.GetClutchPosition());
    GUILayout.Label("currentPower: " + (object) Mathf.Round(this.drivetrain.currentPower));
    GUILayout.Label("engFricTorque: " + (object) this.drivetrain.frictionTorque + " engineTorque: " + (object) this.drivetrain.torque);
    GUILayout.Label("clutchDrag: " + (object) (float) ((double) this.drivetrain.clutchDragImpulse / (double) Time.fixedDeltaTime) + "  torque - frictionTorque: " + (object) (float) ((double) this.drivetrain.torque - (double) this.drivetrain.frictionTorque));
    GUILayout.Label("CanShiftAgain: " + (object) this.drivetrain.CanShiftAgain + " changingGear: " + (object) this.drivetrain.changingGear);
    GUILayout.Label("clutch.speedDiff: " + (object) this.drivetrain.clutch.speedDiff);
    GUILayout.Label("gear: " + (object) this.drivetrain.gear + " clutchEngageSpeed: " + (object) this.drivetrain.clutchEngageSpeed);
    GUILayout.Label("maxPowerDriveShaft: " + (object) (float) ((double) this.drivetrain.maxPowerDriveShaft * (double) this.drivetrain.powerMultiplier) + " maxNetPower: " + (object) (float) ((double) this.drivetrain.maxNetPower * (double) this.drivetrain.powerMultiplier) + " maxTorque: " + (object) this.drivetrain.maxTorque + " maxNetTorque: " + (object) this.drivetrain.maxNetTorque);
    GUILayout.Label("startTorque: " + (object) this.drivetrain.startTorque + " throttle:" + (object) this.drivetrain.throttle);
    GUILayout.Label("% loss power: " + (object) (float) (((double) this.drivetrain.maxPowerDriveShaft - (double) this.drivetrain.maxNetPower) / (double) this.drivetrain.maxPowerDriveShaft * 100.0) + "%");
    if ((Object) this.forceFeedback != (Object) null)
      GUILayout.Label("force feedback: " + (object) this.forceFeedback.force);
    foreach (Wheel allWheel in this.axles.allWheels)
    {
      if (allWheel.wheelPos == WheelPos.FRONT_LEFT)
        GUI.Label(new Rect(300f, 280f, 600f, 200f), "hitDown.distance - radius : " + (object) allWheel.hitDown.distance);
      if (allWheel.wheelPos == WheelPos.FRONT_RIGHT)
        GUI.Label(new Rect(300f, 300f, 600f, 200f), "hitDown.normal .x         .z " + (object) allWheel.hitDown.normal.x + " " + (object) allWheel.hitDown.normal.z);
      if (allWheel.wheelPos == WheelPos.FRONT_RIGHT)
        GUI.Label(new Rect(300f, 320f, 600f, 200f), "groundNormal.x groundNormal.z " + (object) allWheel.groundNormal.x + " " + (object) allWheel.groundNormal.z);
      if (allWheel.wheelPos == WheelPos.FRONT_LEFT)
        GUI.Label(new Rect(300f, 340f, 600f, 200f), "wheelRoadVeloLatFL: " + (object) allWheel.wheelRoadVeloLat);
      if (allWheel.wheelPos == WheelPos.FRONT_RIGHT)
        GUI.Label(new Rect(300f, 360f, 600f, 200f), "wheelRoadVeloLatFR: " + (object) allWheel.wheelRoadVeloLat);
      if (allWheel.wheelPos == WheelPos.FRONT_LEFT)
        GUI.Label(new Rect(300f, 380f, 600f, 200f), "absRoadVelo : " + (object) Mathf.Abs(allWheel.wheelRoadVelo));
      if (allWheel.wheelPos == WheelPos.FRONT_LEFT)
        GUI.Label(new Rect(300f, 400f, 600f, 200f), "idealSlipRatioFL :" + (object) allWheel.idealSlipRatio + " idealSlipAngleFL " + (object) allWheel.idealSlipAngle);
      if (allWheel.wheelPos == WheelPos.FRONT_RIGHT)
        GUI.Label(new Rect(300f, 420f, 600f, 200f), "idealSlipRatioFR :" + (object) allWheel.idealSlipRatio + " idealSlipAngleFR " + (object) allWheel.idealSlipAngle);
      if (allWheel.wheelPos == WheelPos.REAR_LEFT)
        GUI.Label(new Rect(300f, 440f, 600f, 200f), "idealSlipRatioRL :" + (object) allWheel.idealSlipRatio + " idealSlipAngleRL " + (object) allWheel.idealSlipAngle);
      if (allWheel.wheelPos == WheelPos.REAR_RIGHT)
        GUI.Label(new Rect(300f, 460f, 600f, 200f), "idealSlipRatioRR :" + (object) allWheel.idealSlipRatio + " idealSlipAngleRR " + (object) allWheel.idealSlipAngle);
      if (allWheel.wheelPos == WheelPos.FRONT_LEFT)
        GUI.Label(new Rect(300f, 480f, 600f, 200f), "slipRatioFL: " + (object) allWheel.slipRatio + "    Fx: " + (object) allWheel.Fx + " longitudinalSlip:" + (object) this.RoundTo(allWheel.longitudinalSlip, 3) + " normalForce:" + (object) allWheel.normalForce);
      if (allWheel.wheelPos == WheelPos.FRONT_RIGHT)
        GUI.Label(new Rect(300f, 500f, 600f, 200f), "slipRatioFR: " + (object) allWheel.slipRatio + "    Fx: " + (object) allWheel.Fx + " longitudinalSlip:" + (object) this.RoundTo(allWheel.longitudinalSlip, 3) + " normalForce:" + (object) allWheel.normalForce);
      if (allWheel.wheelPos == WheelPos.REAR_LEFT)
        GUI.Label(new Rect(300f, 520f, 600f, 200f), "slipRatioRL: " + (object) allWheel.slipRatio + "    Fx: " + (object) allWheel.Fx + " longitudinalSlip:" + (object) this.RoundTo(allWheel.longitudinalSlip, 3) + " normalForce:" + (object) allWheel.normalForce);
      if (allWheel.wheelPos == WheelPos.REAR_RIGHT)
        GUI.Label(new Rect(300f, 540f, 600f, 200f), "slipRatioRR: " + (object) allWheel.slipRatio + "    Fx: " + (object) allWheel.Fx + " longitudinalSlip:" + (object) this.RoundTo(allWheel.longitudinalSlip, 3) + " normalForce:" + (object) allWheel.normalForce);
      if (allWheel.wheelPos == WheelPos.FRONT_LEFT)
        GUI.Label(new Rect(300f, 560f, 600f, 200f), "slipAngleFL: " + (object) allWheel.slipAngle + "    Fy: " + (object) allWheel.Fy + " lateralSlip:" + (object) this.RoundTo(allWheel.lateralSlip, 3) + " rho:" + (object) Mathf.Sqrt((float) ((double) allWheel.lateralSlip * (double) allWheel.lateralSlip + (double) allWheel.longitudinalSlip * (double) allWheel.longitudinalSlip)));
      if (allWheel.wheelPos == WheelPos.FRONT_RIGHT)
        GUI.Label(new Rect(300f, 580f, 600f, 200f), "slipAngleFR: " + (object) allWheel.slipAngle + "    Fy: " + (object) allWheel.Fy + " lateralSlip:" + (object) this.RoundTo(allWheel.lateralSlip, 3) + " rho:" + (object) Mathf.Sqrt((float) ((double) allWheel.lateralSlip * (double) allWheel.lateralSlip + (double) allWheel.longitudinalSlip * (double) allWheel.longitudinalSlip)));
      if (allWheel.wheelPos == WheelPos.REAR_LEFT)
        GUI.Label(new Rect(300f, 600f, 600f, 200f), "slipAngleRL: " + (object) allWheel.slipAngle + "    Fy: " + (object) allWheel.Fy + " lateralSlip:" + (object) this.RoundTo(allWheel.lateralSlip, 3) + " rho:" + (object) Mathf.Sqrt((float) ((double) allWheel.lateralSlip * (double) allWheel.lateralSlip + (double) allWheel.longitudinalSlip * (double) allWheel.longitudinalSlip)));
      if (allWheel.wheelPos == WheelPos.REAR_RIGHT)
        GUI.Label(new Rect(300f, 620f, 600f, 200f), "slipAngleRR: " + (object) allWheel.slipAngle + "    Fy: " + (object) allWheel.Fy + " lateralSlip:" + (object) this.RoundTo(allWheel.lateralSlip, 3) + " rho:" + (object) Mathf.Sqrt((float) ((double) allWheel.lateralSlip * (double) allWheel.lateralSlip + (double) allWheel.longitudinalSlip * (double) allWheel.longitudinalSlip)));
    }
  }
}
