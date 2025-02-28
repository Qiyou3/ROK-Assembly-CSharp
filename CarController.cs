// Decompiled with JetBrains decompiler
// Type: CarController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public abstract class CarController : EntityBehaviour
{
  private bool counterSteering;
  private float steerTimer;
  private Wheel[] allWheels;
  private Transform myTransform;
  public float currentBrake;
  public float currentThrottle;
  public float currentSteering;
  private float oldSteering;
  private float deltaSteering;
  private float steeringVelo;
  private float maxSteer = 1f;
  private float maxThrottle = 1f;
  [HideInInspector]
  public bool brakeKey;
  [HideInInspector]
  public bool accelKey;
  [HideInInspector]
  public float steerInput;
  [HideInInspector]
  public float brakeInput;
  [HideInInspector]
  public float throttleInput;
  [HideInInspector]
  public float handbrakeInput;
  [HideInInspector]
  public float clutchInput;
  [HideInInspector]
  public bool startEngineInput;
  private Rigidbody body;
  protected Drivetrain drivetrain;
  private CarDynamics cardynamics;
  private Axles axles;
  private float velo;
  private float veloKmh;
  public bool smoothInput = true;
  public float throttleTime = 0.1f;
  public float throttleReleaseTime = 0.1f;
  public float maxThrottleInReverse = 1f;
  public float brakesTime = 0.1f;
  public float brakesReleaseTime = 0.1f;
  public float steerTime = 0.1f;
  public float steerReleaseTime = 0.1f;
  public float veloSteerTime = 0.05f;
  public float veloSteerReleaseTime = 0.05f;
  public float steerCorrectionFactor;
  public bool steerAssistance = true;
  public float SteerAssistanceMinVelocity = 20f;
  public bool TCS = true;
  [HideInInspector]
  public bool TCSTriggered;
  public float TCSAllowedSlip;
  public float TCSMinVelocity = 20f;
  [HideInInspector]
  public float externalTCSThreshold;
  public bool ABS = true;
  [HideInInspector]
  public bool ABSTriggered;
  public float ABSAllowedSlip;
  public float ABSMinVelocity = 20f;
  public bool ESP = true;
  [HideInInspector]
  public bool ESPTriggered;
  public float ESPStrength = 2f;
  public float ESPMinVelocity = 35f;
  [HideInInspector]
  public bool reverse;

  protected abstract void GetInput(
    out float throttleInput,
    out float brakeInput,
    out float steerInput,
    out float handbrakeInput,
    out float clutchInput,
    out bool startEngineInput,
    out int targetGear);

  public void Start()
  {
    this.body = (Rigidbody) (GameObjectAttribute<Rigidbody>) this.Entity.Get<MainRigidbody>();
    this.cardynamics = this.Entity.Get<CarDynamics>();
    this.drivetrain = this.Entity.Get<Drivetrain>();
    this.axles = this.Entity.Get<Axles>();
    if ((Object) this.axles != (Object) null)
      this.allWheels = this.axles.allWheels;
    this.myTransform = this.transform;
  }

  public void Update()
  {
    int targetGear;
    this.GetInput(out this.throttleInput, out this.brakeInput, out this.steerInput, out this.handbrakeInput, out this.clutchInput, out this.startEngineInput, out targetGear);
    if (!this.drivetrain.changingGear && targetGear != this.drivetrain.gear)
      this.drivetrain.Shift(targetGear);
    if (this.drivetrain.automatic && this.drivetrain.autoReverse)
    {
      if ((double) this.brakeInput > 0.0 && (double) this.velo <= 0.5)
      {
        this.reverse = true;
        if (this.drivetrain.gear != this.drivetrain.firstReverse)
          this.drivetrain.Shift(this.drivetrain.firstReverse);
      }
      if ((double) this.throttleInput > 0.0 && (double) this.velo <= 0.5)
      {
        this.reverse = false;
        if (this.drivetrain.gear != this.drivetrain.first)
          this.drivetrain.Shift(this.drivetrain.first);
      }
      if (this.reverse)
      {
        float throttleInput = this.throttleInput;
        this.throttleInput = this.brakeInput;
        this.brakeInput = throttleInput;
      }
    }
    else
      this.reverse = false;
    this.brakeKey = (double) this.brakeInput > 0.0;
    this.accelKey = (double) this.throttleInput > 0.0;
  }

  public void FixedUpdate()
  {
    this.maxThrottle = 1f;
    this.oldSteering = this.currentSteering;
    this.velo = this.cardynamics.velo;
    this.veloKmh = this.cardynamics.velo * 3.6f;
    bool flag = this.drivetrain.OnGround();
    float f1 = this.cardynamics.LateralSlipVeloRearWheels();
    float f2 = (double) Mathf.Abs(f1) >= 0.10000000149011612 ? f1 : 0.0f;
    this.counterSteering = (double) Mathf.Sign(f2) == (double) Mathf.Sign(this.currentSteering) && (double) f2 != 0.0 && (double) this.currentSteering != 0.0;
    if (this.smoothInput)
    {
      this.SmoothSteer();
      this.smoothThrottle();
      this.smoothBrakes();
    }
    else
    {
      this.currentSteering = this.steerInput;
      this.currentBrake = this.brakeInput;
      this.currentThrottle = this.throttleInput;
      if (this.drivetrain.changingGear && this.drivetrain.automatic)
        this.currentThrottle = 0.0f;
    }
    if (this.steerAssistance && (double) this.drivetrain.ratio > 0.0 && (double) this.veloKmh > (double) this.SteerAssistanceMinVelocity && (!this.counterSteering || (double) this.steerInput == 0.0))
    {
      this.SteerAssistance();
    }
    else
    {
      this.steerTimer = 0.0f;
      this.maxSteer = 1f;
    }
    this.TCSTriggered = false;
    if (this.TCS && (double) this.drivetrain.ratio > 0.0 && (double) this.drivetrain.clutch.GetClutchPosition() >= 0.89999997615814209 && flag && (double) this.currentThrottle > (double) this.drivetrain.idlethrottle && (double) this.veloKmh > (double) this.TCSMinVelocity)
      this.DoTCS();
    this.ESPTriggered = false;
    if (this.ESP && (double) this.drivetrain.ratio > 0.0 && flag && (double) this.veloKmh > (double) this.ESPMinVelocity)
      this.DoESP();
    this.ABSTriggered = false;
    if (this.ABS && (double) this.currentBrake > 0.0 && (double) this.veloKmh > (double) this.ABSMinVelocity && flag)
      this.DoABS();
    float max = (double) this.drivetrain.gearRatios[this.drivetrain.gear] <= 0.0 ? this.maxThrottleInReverse : this.maxThrottle;
    this.currentThrottle = !this.drivetrain.revLimiterTriggered ? (!this.drivetrain.revLimiterReleased ? Mathf.Clamp(this.currentThrottle, this.drivetrain.idlethrottle, max) : this.throttleInput) : 0.0f;
    this.currentBrake = Mathf.Clamp01(this.currentBrake);
    this.currentSteering = Mathf.Clamp(this.currentSteering, -1f, 1f);
    this.deltaSteering = this.currentSteering - this.oldSteering;
    foreach (Wheel allWheel in this.allWheels)
    {
      if (!this.ABS || (double) this.veloKmh <= (double) this.ABSMinVelocity || (double) this.brakeInput <= 0.0)
        allWheel.brake = this.currentBrake;
      allWheel.handbrake = this.handbrakeInput;
      allWheel.steering = this.currentSteering;
      allWheel.deltaSteering = this.deltaSteering;
    }
    this.drivetrain.throttle = this.currentThrottle;
    if (this.drivetrain.clutch != null && ((double) this.clutchInput != 0.0 || !this.drivetrain.autoClutch))
      this.drivetrain.clutch.SetClutchPosition(1f - this.clutchInput);
    this.drivetrain.startEngine = this.startEngineInput;
  }

  private void SteerAssistance()
  {
    float num1 = 0.0f;
    foreach (Wheel allWheel in this.allWheels)
      num1 += allWheel.lateralSlip;
    float f = num1 / (float) this.allWheels.Length;
    this.maxSteer = Mathf.Clamp(1f - Mathf.Abs(f), -1f, 1f);
    if ((double) this.steerTimer <= 1.0)
    {
      this.steerTimer += Time.deltaTime;
      this.maxSteer = (float) (1.0 - (double) this.steerTimer + (double) this.steerTimer * (double) this.maxSteer);
    }
    this.oldSteering = this.currentSteering;
    float num2 = Mathf.Sign(f);
    this.currentSteering = (double) this.maxSteer <= 0.0 ? Mathf.Clamp(this.currentSteering, (float) ((double) num2 * (double) this.maxSteer / 2.0), (float) ((double) num2 * (double) this.maxSteer / 2.0)) : Mathf.Clamp(this.currentSteering, -this.maxSteer, this.maxSteer);
    this.steeringVelo = this.currentSteering - this.oldSteering;
    this.currentSteering -= this.steeringVelo * (40f * this.cardynamics.fixedTimeStepScalar) * Time.deltaTime;
  }

  private void SmoothSteer()
  {
    if ((double) this.steerInput < (double) this.currentSteering)
    {
      float num = (double) this.currentSteering <= 0.0 || (double) this.steerInput != 0.0 ? (float) (1.0 / ((double) this.steerTime + (double) this.veloSteerTime * (double) this.velo)) : (float) (1.0 / ((double) this.steerReleaseTime + (double) this.veloSteerReleaseTime * (double) this.velo));
      if (this.counterSteering)
        num *= (float) (1.0 + (double) Mathf.Abs(Mathf.Abs(this.currentSteering) - Mathf.Abs(this.steerInput)) * (double) this.steerCorrectionFactor);
      this.currentSteering -= num * Time.deltaTime;
      if ((double) this.steerInput <= (double) this.currentSteering)
        return;
      this.currentSteering = this.steerInput;
    }
    else
    {
      if ((double) this.steerInput <= (double) this.currentSteering)
        return;
      float num = (double) this.currentSteering >= 0.0 || (double) this.steerInput != 0.0 ? (float) (1.0 / ((double) this.steerTime + (double) this.veloSteerTime * (double) this.velo)) : (float) (1.0 / ((double) this.steerReleaseTime + (double) this.veloSteerReleaseTime * (double) this.velo));
      if (this.counterSteering)
        num *= (float) (1.0 + (double) Mathf.Abs(Mathf.Abs(this.currentSteering) - Mathf.Abs(this.steerInput)) * (double) this.steerCorrectionFactor);
      this.currentSteering += num * Time.deltaTime;
      if ((double) this.steerInput >= (double) this.currentSteering)
        return;
      this.currentSteering = this.steerInput;
    }
  }

  private void smoothThrottle()
  {
    if ((double) this.throttleInput > 0.0 && (!this.drivetrain.changingGear || !this.drivetrain.automatic))
    {
      if ((double) this.throttleInput < (double) this.currentThrottle)
      {
        this.currentThrottle -= Time.deltaTime / this.throttleReleaseTime;
        if ((double) this.throttleInput <= (double) this.currentThrottle)
          return;
        this.currentThrottle = this.throttleInput;
      }
      else
      {
        if ((double) this.throttleInput <= (double) this.currentThrottle)
          return;
        this.currentThrottle += Time.deltaTime / this.throttleTime;
        if ((double) this.throttleInput >= (double) this.currentThrottle)
          return;
        this.currentThrottle = this.throttleInput;
      }
    }
    else
      this.currentThrottle -= Time.deltaTime / this.throttleReleaseTime;
  }

  private void smoothBrakes()
  {
    if ((double) this.brakeInput > 0.0)
    {
      if ((double) this.brakeInput < (double) this.currentBrake)
      {
        this.currentBrake -= Time.deltaTime / this.brakesReleaseTime;
        if ((double) this.brakeInput <= (double) this.currentBrake)
          return;
        this.currentBrake = this.brakeInput;
      }
      else
      {
        if ((double) this.brakeInput <= (double) this.currentBrake)
          return;
        this.currentBrake += Time.deltaTime / this.brakesTime;
        if ((double) this.brakeInput >= (double) this.currentBrake)
          return;
        this.currentBrake = this.brakeInput;
      }
    }
    else
      this.currentBrake -= Time.deltaTime / this.brakesReleaseTime;
  }

  private void DoABS()
  {
    foreach (Wheel allWheel in this.allWheels)
    {
      if (-(double) allWheel.longitudinalSlip * (1.0 - (double) this.ABSAllowedSlip) >= 1.0)
      {
        allWheel.brake = 0.0f;
        this.ABSTriggered = true;
      }
      else
      {
        allWheel.brake = this.currentBrake;
        this.ABSTriggered = false;
      }
    }
  }

  private void DoTCS()
  {
    float num1 = 0.0f;
    foreach (Wheel poweredWheel in this.drivetrain.poweredWheels)
    {
      float num2 = Mathf.Max(poweredWheel.longitudinalSlip, Mathf.Abs(poweredWheel.lateralSlip) * 1.5f);
      if ((double) num2 > (double) num1)
        num1 = num2;
    }
    this.TCSTriggered = false;
    float num3 = num1 * (1f - this.TCSAllowedSlip) + this.externalTCSThreshold;
    if ((double) num3 < 1.0)
      return;
    this.maxThrottle = Mathf.Clamp(2f - num3, 0.0f, 1f);
    if ((double) this.maxThrottle > 0.89999997615814209)
      this.maxThrottle = 1f;
    else
      this.TCSTriggered = true;
  }

  private void DoESP()
  {
    Vector3 forward = this.myTransform.forward;
    Vector3 velocity = this.body.velocity;
    Vector3 rhs = velocity - this.myTransform.up * Vector3.Dot(velocity, this.myTransform.up);
    rhs.Normalize();
    float f = 0.0f;
    if ((double) this.velo > 1.0)
      f = -Mathf.Asin(Vector3.Dot(Vector3.Cross(forward, rhs), this.myTransform.up));
    this.ESPTriggered = false;
    if ((double) f > 0.10000000149011612)
    {
      if ((bool) (Object) this.axles.frontAxle.leftWheel)
        this.axles.frontAxle.leftWheel.brake = Mathf.Clamp01(this.axles.frontAxle.leftWheel.brake + Mathf.Abs(f) * this.ESPStrength);
      this.maxThrottle = Mathf.Max(this.maxThrottle - f * this.ESPStrength, this.drivetrain.idlethrottle);
      this.ESPTriggered = true;
    }
    else
    {
      if ((double) f >= -0.10000000149011612)
        return;
      if ((bool) (Object) this.axles.frontAxle.rightWheel)
        this.axles.frontAxle.rightWheel.brake = Mathf.Clamp01(this.axles.frontAxle.rightWheel.brake + Mathf.Abs(f) * this.ESPStrength);
      this.maxThrottle = Mathf.Max(this.maxThrottle + f * this.ESPStrength, this.drivetrain.idlethrottle);
      this.ESPTriggered = true;
    }
  }
}
