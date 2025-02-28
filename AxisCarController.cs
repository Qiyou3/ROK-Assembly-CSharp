// Decompiled with JetBrains decompiler
// Type: AxisCarController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Input;

#nullable disable
public class AxisCarController : CarController
{
  public bool normalizeThrottleInput;
  public bool exponentialThrottleInput;
  public bool normalizeBrakesInput;
  public bool exponentialBrakesInput;
  public bool normalizeClutchInput;
  public bool exponentialClutchInput;
  public CodeHatch.Engine.Core.Input.Axis throttleAxis = new CodeHatch.Engine.Core.Input.Axis("Throttle");
  public CodeHatch.Engine.Core.Input.Axis brakeAxis = new CodeHatch.Engine.Core.Input.Axis("Brake");
  public CodeHatch.Engine.Core.Input.Axis steerAxis = new CodeHatch.Engine.Core.Input.Axis("Horizontal");
  public CodeHatch.Engine.Core.Input.Axis handbrakeAxis = new CodeHatch.Engine.Core.Input.Axis("Handbrake");
  public CodeHatch.Engine.Core.Input.Axis clutchAxis = new CodeHatch.Engine.Core.Input.Axis("Clutch");
  public KeyButton shiftUpButton = new KeyButton("ShiftUp");
  public KeyButton shiftDownButton = new KeyButton("ShiftDown");
  public KeyButton startEngineButton = new KeyButton("StartEngine");

  public void Awake()
  {
    this.throttleAxis.raw = true;
    this.brakeAxis.raw = true;
    this.steerAxis.raw = true;
    this.handbrakeAxis.raw = true;
    this.clutchAxis.raw = true;
  }

  protected override void GetInput(
    out float throttleInput,
    out float brakeInput,
    out float steerInput,
    out float handbrakeInput,
    out float clutchInput,
    out bool startEngineInput,
    out int targetGear)
  {
    throttleInput = this.throttleAxis.Get();
    brakeInput = this.brakeAxis.Get();
    steerInput = this.steerAxis.Get();
    handbrakeInput = this.handbrakeAxis.Get();
    clutchInput = this.clutchAxis.Get();
    if (this.normalizeThrottleInput)
      throttleInput = (float) (((double) throttleInput + 1.0) / 2.0);
    if (this.exponentialThrottleInput)
      throttleInput *= throttleInput;
    if (this.normalizeBrakesInput)
      brakeInput = (float) (((double) brakeInput + 1.0) / 2.0);
    if (this.exponentialBrakesInput)
      brakeInput *= brakeInput;
    if (this.normalizeClutchInput)
      clutchInput = (float) (((double) clutchInput + 1.0) / 2.0);
    if (this.exponentialClutchInput)
      clutchInput *= clutchInput;
    startEngineInput = this.startEngineButton.Get();
    targetGear = this.drivetrain.gear;
    if (this.shiftUpButton.GetDown())
      ++targetGear;
    if (this.shiftDownButton.GetDown())
      --targetGear;
    if (!this.drivetrain.shifter)
      return;
    if (UnityEngine.Input.GetButton("reverse"))
      targetGear = 0;
    else if (UnityEngine.Input.GetButton("neutral"))
      targetGear = 1;
    else if (UnityEngine.Input.GetButton("first"))
      targetGear = 2;
    else if (UnityEngine.Input.GetButton("second"))
      targetGear = 3;
    else if (UnityEngine.Input.GetButton("third"))
      targetGear = 4;
    else if (UnityEngine.Input.GetButton("fourth"))
      targetGear = 5;
    else if (UnityEngine.Input.GetButton("fifth"))
      targetGear = 6;
    else if (UnityEngine.Input.GetButton("sixth"))
      targetGear = 7;
    else
      targetGear = 1;
  }
}
