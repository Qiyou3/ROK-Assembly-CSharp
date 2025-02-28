// Decompiled with JetBrains decompiler
// Type: CustomCarController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class CustomCarController : CarController
{
  public float lastThrottleInput;
  public float lastBrakeInput;
  public float lastSteerInput;
  public float lastHandbrakeInput;

  protected override void GetInput(
    out float throttleInput,
    out float brakeInput,
    out float steerInput,
    out float handbrakeInput,
    out float clutchInput,
    out bool startEngineInput,
    out int targetGear)
  {
    throttleInput = this.lastThrottleInput;
    brakeInput = this.lastBrakeInput;
    steerInput = this.lastSteerInput;
    handbrakeInput = this.lastHandbrakeInput;
    clutchInput = this.clutchInput;
    startEngineInput = this.startEngineInput;
    targetGear = this.drivetrain.gear;
  }
}
