// Decompiled with JetBrains decompiler
// Type: Drivetrain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch;
using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Axles))]
public class Drivetrain : EntityBehaviour
{
  [HideInInspector]
  public bool engineTorqueFromFile;
  [HideInInspector]
  public int torqueRPMValuesLen;
  [HideInInspector]
  public float[,] torqueRPMValues = new float[0, 0];
  [HideInInspector]
  public Clutch clutch;
  [HideInInspector]
  public CarController carController;
  private Axles axles;
  [HideInInspector]
  public FuelTank[] fuelTanks;
  private Rigidbody body;
  private Transform myTransform;
  [HideInInspector]
  public Wheel[] poweredWheels;
  [Designer]
  public float maxPower = 210f;
  [Designer]
  public float maxPowerRPM = 5000f;
  [Designer]
  public float maxTorque = 360f;
  [Designer]
  public float maxTorqueRPM = 2500f;
  [HideInInspector]
  public float originalMaxPower = 210f;
  [HideInInspector]
  public float maxNetPower;
  [HideInInspector]
  public float maxNetPowerRPM;
  [HideInInspector]
  public float maxNetTorque;
  [HideInInspector]
  public float maxNetTorqueRPM;
  [HideInInspector]
  public float torque;
  private float netTorque;
  private float netTorqueImpulse;
  [HideInInspector]
  public float wheelTireVelo;
  public float minRPM = 1000f;
  public float maxRPM = 6000f;
  public bool canStall;
  [HideInInspector]
  public bool startEngine;
  public bool revLimiter;
  public float revLimiterTime = 0.1f;
  [HideInInspector]
  public bool revLimiterTriggered;
  [HideInInspector]
  public bool revLimiterReleased;
  private float timer;
  public float engineInertia = 0.3f;
  public float drivetrainInertia = 0.02f;
  private float rotationalInertia;
  public float engineFrictionFactor = 0.25f;
  public Vector3 engineOrientation = Vector3.forward;
  public Drivetrain.Transmissions transmission;
  public float[] gearRatios = new float[7]
  {
    -2.66f,
    0.0f,
    2.66f,
    1.91f,
    1.39f,
    1f,
    0.71f
  };
  [HideInInspector]
  public int neutral = 1;
  [HideInInspector]
  public int first;
  [HideInInspector]
  public int firstReverse;
  public float finalDriveRatio = 6.09f;
  public float differentialLockCoefficient = 80f;
  public bool shifter;
  public bool automatic = true;
  public bool autoReverse = true;
  public float shiftDownRPM = 2000f;
  public float shiftUpRPM;
  public float shiftTime = 0.5f;
  [HideInInspector]
  public float clutchMaxTorque;
  public bool autoClutch = true;
  public float engageRPM = 1500f;
  public float disengageRPM = 1000f;
  public float _fuelConsumptionAtCostantSpeed = 4.3f;
  public float _fuelConsumptionSpeed = 130f;
  public float currentConsumption;
  [HideInInspector]
  public float istantConsumption;
  [HideInInspector]
  public float RPMAtSpeedInLastGear;
  private float secondsToCover100Km;
  [HideInInspector]
  public float clutchEngageSpeed;
  private float clutchPosition;
  [HideInInspector]
  public float throttle;
  [HideInInspector]
  public float idlethrottle;
  private float idlethrottleMinRPMDown;
  private float idleNetTorque;
  [HideInInspector]
  public float startTorque;
  private float startThrottle;
  private float nextStartImpulse;
  private float duration;
  [HideInInspector]
  public bool shiftTriggered;
  [HideInInspector]
  public bool soundPlayed;
  [HideInInspector]
  public float clutchDragImpulse;
  private float wheelImpulse;
  private float TransferredTorque;
  [HideInInspector]
  public float differentialSpeed;
  private float clutchSpeed;
  [HideInInspector]
  public bool engaging;
  private float shiftTimer;
  private float TimeToShiftAgain;
  [HideInInspector]
  public bool CanShiftAgain = true;
  private float ShiftDelay = -1f;
  private float lastShiftTime = -1f;
  public int gear = 1;
  public float rpm;
  private float slipRatio;
  private float idealSlipRatio;
  private float engineAngularVelo;
  [HideInInspector]
  public float angularVelo2RPM = 9.549296f;
  [HideInInspector]
  public float RPM2angularVelo = (float) Math.PI / 30f;
  [HideInInspector]
  public float KW2CV = 1.359f;
  [HideInInspector]
  public float CV2KW = 0.7358f;
  [HideInInspector]
  public float maxPowerDriveShaft;
  [HideInInspector]
  public float currentPower;
  private float maxPowerKW;
  private float maxPowerAngVel;
  private float maxPowerEngineTorque;
  private float P1;
  private float P2;
  private float P3;
  [HideInInspector]
  public float curveFactor;
  [HideInInspector]
  public float frictionTorque;
  [HideInInspector]
  public float powerMultiplier = 1f;
  [HideInInspector]
  public float externalMultiplier = 1f;
  [HideInInspector]
  public float ratio;
  [HideInInspector]
  public float lastGearRatio;
  [HideInInspector]
  public bool changingGear;
  private bool shiftImmediately;
  private int nextGear;
  private float lockingTorqueImpulse;
  private float max_power;
  [HideInInspector]
  public float drivetrainFraction;
  [HideInInspector]
  public float velo;
  [HideInInspector]
  private bool fuel = true;
  [HideInInspector]
  public float RPMAt130Kmh;
  public bool started = true;
  public float charge = 1f;
  public float chargeTime = 5f;
  private float _sleepTimer;

  private float Sqr(float x) => x * x;

  public float fuelConsumptionAtCostantSpeed
  {
    get => this._fuelConsumptionAtCostantSpeed;
    set
    {
      if ((double) value < 0.0)
        this._fuelConsumptionAtCostantSpeed = 0.0f;
      else
        this._fuelConsumptionAtCostantSpeed = value;
    }
  }

  public float fuelConsumptionSpeed
  {
    get => this._fuelConsumptionSpeed;
    set
    {
      if ((double) value < 1.0)
        this._fuelConsumptionSpeed = 1f;
      else
        this._fuelConsumptionSpeed = value;
    }
  }

  private bool EnforceSleep()
  {
    DrivableCar drivableCar = this.Entity.TryGet<DrivableCar>();
    return (!((UnityEngine.Object) drivableCar != (UnityEngine.Object) null) || !((UnityEngine.Object) drivableCar.driver != (UnityEngine.Object) null)) && this.body.EnforceSleep(ref this._sleepTimer, 0.25f);
  }

  public void Awake()
  {
    this.engineTorqueFromFile = false;
    this.torqueRPMValuesLen = 0;
    this.clutch = new Clutch();
  }

  public void Start()
  {
    this.body = (Rigidbody) (GameObjectAttribute<Rigidbody>) this.Entity.Get<MainRigidbody>();
    this.myTransform = (Transform) (GameObjectAttribute<Transform>) this.Entity.Get<MainTransform>();
    this.carController = this.Entity.Get<CarController>();
    this.axles = this.Entity.Get<Axles>();
    this.fuelTanks = this.Entity.GetArray<FuelTank>();
    this.poweredWheels = this.axles.rearAxle.wheels;
    this.CalcValues(1f, this.engineTorqueFromFile);
    if ((double) this.shiftUpRPM == 0.0)
      this.shiftUpRPM = this.maxPowerRPM;
    bool flag = false;
    for (int index = 0; index < this.gearRatios.Length; ++index)
    {
      if ((double) this.gearRatios[index] == 0.0)
      {
        this.neutral = index;
        this.first = this.neutral + 1;
        this.firstReverse = this.neutral - 1;
        flag = true;
      }
    }
    if (!flag)
      this.LogError<Drivetrain>("UnityCar: Neutral gear (a gear with value 0) is missing in gearRatios array. Neutral gear is mandatory (" + this.myTransform.name + ")");
    this.SetTransmission(this.transmission);
    if ((double) this.clutch.maxTorque == 0.0)
      this.CalcClutchTorque();
    if ((double) this.shiftTime == 0.0)
      this.shiftTime = 0.5f;
    this.lastGearRatio = this.gearRatios[this.gearRatios.Length - 1] * this.finalDriveRatio;
    this.fuelConsumptionAtCostantSpeed = this.fuelConsumptionAtCostantSpeed;
    this.fuelConsumptionSpeed = this.fuelConsumptionSpeed;
    this.RPMAtSpeedInLastGear = this.CalcRPMAtSpeedInLastGear(this.fuelConsumptionSpeed);
    this.secondsToCover100Km = (float) (100.0 / (double) this.fuelConsumptionSpeed * 3600.0);
    this.CalcIdleThrottle();
    this.DisengageClutch();
    this.StartEngine();
  }

  public float CalcRPMAtSpeedInLastGear(float speed)
  {
    return (double) speed > 0.0 ? speed / (float) ((double) this.axles.frontAxle.leftWheel.radius * 2.0 * 0.18850000202655792 / ((double) Mathf.Abs(this.gearRatios[this.gearRatios.Length - 1]) * (double) this.finalDriveRatio)) : 0.0f;
  }

  public void CalcClutchTorque()
  {
    this.clutchMaxTorque = Mathf.Round(this.maxNetTorque * 1.6f) * this.powerMultiplier;
    this.clutch.maxTorque = this.clutchMaxTorque;
  }

  public void SetTransmission(Drivetrain.Transmissions transmission)
  {
    foreach (Wheel allWheel in this.axles.allWheels)
    {
      allWheel.lockingTorqueImpulse = 0.0f;
      allWheel.drivetrainInertia = 0.0f;
      allWheel.isPowered = false;
    }
    switch (transmission)
    {
      case Drivetrain.Transmissions.RWD:
        foreach (Wheel wheel in this.axles.rearAxle.wheels)
          wheel.isPowered = true;
        this.poweredWheels = this.axles.rearAxle.wheels;
        this.axles.frontAxle.powered = false;
        this.axles.rearAxle.powered = true;
        foreach (Axle otherAxle in this.axles.otherAxles)
          otherAxle.powered = false;
        break;
      case Drivetrain.Transmissions.FWD:
        foreach (Wheel wheel in this.axles.frontAxle.wheels)
          wheel.isPowered = true;
        this.poweredWheels = this.axles.frontAxle.wheels;
        this.axles.frontAxle.powered = true;
        this.axles.rearAxle.powered = false;
        foreach (Axle otherAxle in this.axles.otherAxles)
          otherAxle.powered = false;
        break;
      case Drivetrain.Transmissions.AWD:
        foreach (Wheel allWheel in this.axles.allWheels)
          allWheel.isPowered = true;
        this.poweredWheels = this.axles.allWheels;
        this.axles.frontAxle.powered = true;
        this.axles.rearAxle.powered = true;
        foreach (Axle otherAxle in this.axles.otherAxles)
          otherAxle.powered = true;
        break;
      case Drivetrain.Transmissions.XWD:
        List<Wheel> wheelList = new List<Wheel>();
        if (this.axles.frontAxle.powered)
        {
          foreach (Wheel wheel in this.axles.frontAxle.wheels)
          {
            wheel.isPowered = true;
            wheelList.Add(wheel);
          }
        }
        if (this.axles.rearAxle.powered)
        {
          foreach (Wheel wheel in this.axles.rearAxle.wheels)
          {
            wheel.isPowered = true;
            wheelList.Add(wheel);
          }
        }
        foreach (Axle otherAxle in this.axles.otherAxles)
        {
          if (otherAxle.powered)
          {
            foreach (Wheel wheel in otherAxle.wheels)
            {
              wheel.isPowered = true;
              wheelList.Add(wheel);
            }
          }
        }
        this.poweredWheels = wheelList.ToArray();
        break;
    }
    this.drivetrainFraction = 1f / (float) this.poweredWheels.Length;
  }

  public float CalcEngineTorque(float factor, float rpm)
  {
    return this.engineTorqueFromFile ? this.CalcEngineTorqueExt(factor, rpm) : this.CalcEngineTorqueInt(factor, rpm);
  }

  private float CalcEngineTorqueExt(float factor, float RPM)
  {
    if (this.torqueRPMValuesLen == 0)
      return 0.0f;
    int rightPoint = this.FindRightPoint(RPM);
    return rightPoint == 0 || rightPoint == this.torqueRPMValuesLen ? 0.0f : (float) (((double) RPM - (double) this.torqueRPMValues[rightPoint, 0]) / ((double) this.torqueRPMValues[rightPoint - 1, 0] - (double) this.torqueRPMValues[rightPoint, 0]) * (double) this.torqueRPMValues[rightPoint - 1, 1] - ((double) RPM - (double) this.torqueRPMValues[rightPoint - 1, 0]) / ((double) this.torqueRPMValues[rightPoint - 1, 0] - (double) this.torqueRPMValues[rightPoint, 0]) * (double) this.torqueRPMValues[rightPoint, 1]) * factor;
  }

  private int FindRightPoint(float RPM)
  {
    int rightPoint = 0;
    while (rightPoint <= this.torqueRPMValuesLen - 1 && (double) this.torqueRPMValues[rightPoint, 0] <= (double) RPM)
      ++rightPoint;
    return rightPoint;
  }

  private float CalcEngineTorqueInt(float factor, float rpm)
  {
    float num1;
    if ((double) rpm < (double) this.maxTorqueRPM)
    {
      num1 = this.maxTorque * (float) (-(double) this.Sqr((float) ((double) rpm / (double) this.maxTorqueRPM - 1.0)) + 1.0);
    }
    else
    {
      float num2 = (float) (((double) this.maxTorque - (double) ((float) ((double) this.maxPower * (double) this.CV2KW * 1000.0) / this.maxPowerAngVel)) / (2.0 * (double) this.maxTorqueRPM * (double) this.maxPowerRPM - (double) this.Sqr(this.maxPowerRPM) - (double) this.Sqr(this.maxTorqueRPM))) * this.Sqr(rpm - this.maxTorqueRPM) + this.maxTorque;
      num1 = (double) num2 <= 0.0 ? 0.0f : num2;
    }
    if ((double) rpm < 0.0 || (double) num1 < 0.0)
      num1 = 0.0f;
    return num1 * factor;
  }

  public float CalcEngineTorqueInt_reference(float factor, float RPM)
  {
    float num1 = RPM * this.RPM2angularVelo;
    float num2 = (float) ((double) this.P1 + (double) this.P2 * (double) num1 + (double) this.P3 * ((double) num1 * (double) num1));
    if ((double) RPM < (double) this.maxTorqueRPM)
      num2 *= 1f - this.Sqr((float) ((double) RPM / (double) this.maxTorqueRPM - 1.0));
    return num2 * 1000f * factor;
  }

  public float CalcEngineFrictionTorque(float factor, float rpm)
  {
    float num = 0.1f;
    if ((double) rpm < (double) this.minRPM)
      num = (float) (1.0 - 0.89999997615814209 * ((double) rpm / (double) this.minRPM));
    return (float) ((double) this.maxPowerEngineTorque * (double) factor * (double) this.engineFrictionFactor * ((double) num + (1.0 - (double) num) * (double) rpm / (double) this.maxRPM));
  }

  private float CalcEnginePower(float rpm, bool total, float factor)
  {
    return total ? (float) (((double) this.CalcEngineTorque(factor, rpm) - (double) this.CalcEngineFrictionTorque(factor, rpm)) * (double) rpm * (double) this.RPM2angularVelo * (1.0 / 1000.0)) * this.KW2CV : (float) ((double) this.CalcEngineTorque(factor, rpm) * (double) rpm * (double) this.RPM2angularVelo * (1.0 / 1000.0)) * this.KW2CV;
  }

  public void StartEngine()
  {
    this.engineAngularVelo = (float) ((double) this.minRPM * (double) this.RPM2angularVelo * 1.5);
  }

  private void CalcEngineMaxPower(float powerMultiplier, bool setMaxPower)
  {
    for (float minRpm = this.minRPM; (double) minRpm < (double) this.maxRPM; ++minRpm)
    {
      float num1 = this.CalcEnginePower(minRpm, true, powerMultiplier);
      float num2 = this.CalcEnginePower(minRpm + 1f, true, powerMultiplier);
      if ((double) num2 > (double) num1)
      {
        this.maxNetPowerRPM = minRpm + 1f;
        this.maxNetPower = num2;
      }
      if (setMaxPower)
      {
        float num3 = this.CalcEnginePower(minRpm, false, powerMultiplier);
        float num4 = this.CalcEnginePower(minRpm + 1f, false, powerMultiplier);
        if ((double) num4 > (double) num3)
        {
          this.maxPowerRPM = minRpm + 1f;
          this.maxPower = num4;
        }
      }
    }
  }

  private void CalcengineMaxTorque(float powerMultiplier, bool setMaxTorque)
  {
    for (float minRpm = this.minRPM; (double) minRpm < (double) this.maxRPM; ++minRpm)
    {
      float num1 = this.CalcEngineTorque(powerMultiplier, minRpm) - this.CalcEngineFrictionTorque(powerMultiplier, minRpm);
      float num2 = this.CalcEngineTorque(powerMultiplier, minRpm + 1f) - this.CalcEngineFrictionTorque(powerMultiplier, minRpm + 1f);
      if ((double) num2 > (double) num1)
      {
        this.maxNetTorqueRPM = minRpm + 1f;
        this.maxNetTorque = num2;
      }
      if (setMaxTorque)
      {
        float num3 = this.CalcEngineTorque(powerMultiplier, minRpm);
        float num4 = this.CalcEngineTorque(powerMultiplier, minRpm + 1f);
        if ((double) num4 > (double) num3)
        {
          this.maxTorqueRPM = minRpm + 1f;
          this.maxTorque = num4;
        }
      }
    }
  }

  public void CalcIdleThrottle()
  {
    float num1 = this.CalcEngineFrictionTorque(this.powerMultiplier, this.minRPM);
    float num2 = this.CalcEngineTorque(this.powerMultiplier, this.minRPM);
    this.idleNetTorque = num2 - num1;
    this.idlethrottle = 0.0f;
    while ((double) this.idlethrottle < 1.0 && (double) num2 * (double) this.idlethrottle < (double) num1)
      this.idlethrottle += 0.0001f;
    this.idlethrottleMinRPMDown = this.idlethrottle;
  }

  public void CalcValues(float externalFactor, bool setMaxPower)
  {
    this.maxPowerAngVel = this.maxPowerRPM * this.RPM2angularVelo;
    this.maxPowerKW = this.maxPower * this.CV2KW * externalFactor;
    this.maxPowerDriveShaft = this.maxPower * externalFactor;
    this.P1 = this.maxPowerKW / this.maxPowerAngVel;
    this.P2 = this.maxPowerKW / (this.maxPowerAngVel * this.maxPowerAngVel);
    this.P3 = (float) (-(double) this.maxPowerKW / ((double) this.maxPowerAngVel * (double) this.maxPowerAngVel * (double) this.maxPowerAngVel));
    this.maxPowerEngineTorque = this.CalcEngineTorque(1f, this.maxPowerRPM);
    this.CalcengineMaxTorque(1f, setMaxPower);
    this.CalcEngineMaxPower(1f, setMaxPower);
    this.originalMaxPower = this.maxPower;
    this.curveFactor = externalFactor;
  }

  public void FixedUpdate()
  {
    if (this.clutch == null)
    {
      this.clutch = new Clutch();
      this.CalcClutchTorque();
    }
    if (this.shifter)
      this.automatic = false;
    this.ratio = this.gearRatios[this.gear] * this.finalDriveRatio;
    this.idlethrottle = (double) this.rpm > (double) this.minRPM + (double) this.maxRPM * 0.05000000074505806 ? 0.0f : this.idlethrottleMinRPMDown * (float) (((double) this.minRPM + 500.0 - (double) Mathf.Clamp(this.rpm, this.minRPM, this.rpm)) * (1.0 / 500.0));
    this.currentPower = this.CalcEnginePower(this.rpm, true, this.powerMultiplier);
    float engineInertia = this.engineInertia * this.powerMultiplier * this.externalMultiplier;
    float num1 = this.drivetrainInertia * this.powerMultiplier * this.externalMultiplier;
    this.velo = Mathf.Abs(this.myTransform.InverseTransformDirection(this.body.velocity).z);
    if (((double) this.rpm >= (double) this.engageRPM || this.engaging) && this.autoClutch && (double) this.carController.clutchInput == 0.0 && (double) this.clutch.GetClutchPosition() != 1.0 && (double) this.carController.handbrakeInput == 0.0 && (double) this.ratio != 0.0)
      this.EngageClutch();
    if ((double) this.rpm <= (double) this.disengageRPM && !this.engaging && this.autoClutch)
      this.DisengageClutch();
    if (this.changingGear)
      this.DoGearShifting();
    else
      this.lastShiftTime = 0.0f;
    if (this.automatic)
    {
      this.autoClutch = true;
      if (!this.CanShiftAgain)
      {
        this.TimeToShiftAgain = Mathf.Clamp01((float) (((double) Time.time - (double) this.ShiftDelay) / ((double) this.shiftTime + (double) this.shiftTime / 2.0)));
        if ((double) this.TimeToShiftAgain >= 1.0)
          this.CanShiftAgain = true;
      }
      if (!this.changingGear)
      {
        if ((double) this.rpm >= (double) this.shiftUpRPM)
        {
          if (this.gear >= 0 && this.gear < this.gearRatios.Length - 1 && (double) Mathf.Abs(this.slipRatio / this.idealSlipRatio) <= 1.0 && (double) this.clutch.GetClutchPosition() != 0.0 && (double) this.clutch.speedDiff < 50.0 && !this.engaging)
          {
            if (this.CanShiftAgain && this.OnGround())
            {
              if ((double) this.gearRatios[this.gear] > 0.0)
                this.Shift(this.gear + 1);
              else
                this.Shift(this.gear - 1);
            }
            this.CanShiftAgain = false;
            this.ShiftDelay = Time.time;
          }
        }
        else if ((double) this.rpm <= (double) this.shiftDownRPM && this.gear != this.first && this.gear != this.firstReverse && this.gear != this.neutral && this.gear > 0 && this.gear < this.gearRatios.Length && (double) this.clutch.GetClutchPosition() != 0.0 && (double) Mathf.Abs(this.clutch.speedDiff) < 50.0 && !this.engaging && this.CanShiftAgain && this.OnGround())
        {
          if ((double) this.velo < 3.0)
            this.Shift(this.first);
          else if ((double) this.gearRatios[this.gear] > 0.0)
            this.Shift(this.gear - 1);
          else
            this.Shift(this.gear + 1);
          this.CanShiftAgain = false;
          this.ShiftDelay = Time.time;
        }
      }
    }
    float num2 = 0.0f;
    this.wheelImpulse = 0.0f;
    this.rotationalInertia = 0.0f;
    this.wheelTireVelo = 0.0f;
    foreach (Wheel poweredWheel in this.poweredWheels)
    {
      num2 += poweredWheel.angularVelocity;
      this.wheelImpulse += poweredWheel.wheelImpulse;
      this.rotationalInertia += poweredWheel.rotationalInertia;
      this.wheelTireVelo += poweredWheel.wheelTireVelo;
    }
    float num3 = num2 * this.drivetrainFraction;
    this.wheelTireVelo *= this.drivetrainFraction;
    float totalRotationalInertia = num1 + this.rotationalInertia;
    if ((double) this.rpm < 20.0 && (double) this.startTorque == 0.0)
    {
      this.differentialSpeed = 0.0f;
      this.wheelImpulse = 0.0f;
    }
    this.clutchSpeed = this.differentialSpeed * this.ratio;
    this.fuel = true;
    if (this.fuelTanks.Length != 0)
    {
      float max = this.velo * 3.6f;
      float num4 = Mathf.Clamp(max, 50f, max) / this.fuelConsumptionSpeed;
      float num5 = num4 * num4;
      if ((double) this.RPMAtSpeedInLastGear != 0.0)
        this.istantConsumption = (float) ((double) this.rpm * (double) this.throttle * (double) this.fuelConsumptionAtCostantSpeed / ((double) this.RPMAtSpeedInLastGear * (double) this.secondsToCover100Km)) * num5;
      this.currentConsumption = (double) this.velo <= 1.0 ? 0.0f : (float) ((double) this.istantConsumption / (double) this.velo * 100000.0);
      this.fuel = false;
      foreach (FuelTank fuelTank in this.fuelTanks)
      {
        if ((double) fuelTank.currentFuel != 0.0)
          this.fuel = true;
      }
    }
    this.torque = (float) ((double) this.CalcEngineTorque(this.powerMultiplier, this.rpm) * ((double) this.throttle + (double) this.startThrottle) * (!this.fuel ? 0.0 : 1.0));
    this.frictionTorque = this.CalcEngineFrictionTorque(this.powerMultiplier, this.rpm);
    this.startThrottle = 0.0f;
    this.startTorque = 0.0f;
    this.netTorque = this.torque - this.frictionTorque;
    if ((double) this.rpm < 20.0 && (double) this.startTorque == 0.0)
      this.netTorque = 0.0f;
    if ((double) this.rpm < (double) this.minRPM)
      this.startThrottle = (float) (1.0 - (double) this.rpm / (double) this.minRPM);
    if (this.startEngine && (double) this.rpm < (double) this.minRPM && (double) Time.time > (double) this.nextStartImpulse)
    {
      if ((double) this.duration == 0.0)
        this.duration = Time.time + 0.1f;
      if ((double) Time.time > (double) this.duration)
        this.nextStartImpulse = Time.time + 0.2f;
      this.startThrottle = 1f;
      this.startTorque = this.idleNetTorque;
    }
    else
      this.duration = 0.0f;
    this.netTorqueImpulse = (this.netTorque + this.startTorque) * Time.deltaTime;
    if ((double) this.engineAngularVelo >= (double) this.maxRPM * (double) this.RPM2angularVelo)
    {
      if ((double) this.revLimiterTime == 0.0 && this.revLimiter)
        this.engineAngularVelo = this.maxRPM * this.RPM2angularVelo;
      else
        this.revLimiterTriggered = true;
    }
    else if ((double) this.engineAngularVelo <= (double) this.minRPM * (double) this.RPM2angularVelo && !this.canStall)
      this.engineAngularVelo = this.minRPM * this.RPM2angularVelo;
    else if ((double) this.engineAngularVelo < 0.0)
      this.engineAngularVelo = 0.0f;
    this.rpm = this.engineAngularVelo * this.angularVelo2RPM;
    if ((double) this.ratio == 0.0 || (double) this.clutch.GetClutchPosition() == 0.0)
    {
      this.clutchDragImpulse = 0.0f;
      this.differentialSpeed = num3;
      if (this.autoClutch)
        this.DisengageClutch();
      if (this.engineOrientation.IsValid())
        this.body.AddTorque(-this.engineOrientation * Mathf.Min(Mathf.Abs(this.netTorque), 2000f) * Mathf.Sign(this.netTorque));
    }
    else
      this.clutchDragImpulse = this.clutch.GetDragImpulse(this.engineAngularVelo, this.clutchSpeed, engineInertia, totalRotationalInertia, this.ratio, this.wheelImpulse, this.netTorqueImpulse);
    this.engineAngularVelo += (this.netTorqueImpulse - this.clutchDragImpulse) / engineInertia;
    this.differentialSpeed += (this.wheelImpulse + this.clutchDragImpulse * this.ratio) / totalRotationalInertia;
    if (float.IsNaN(this.differentialSpeed))
      this.differentialSpeed = 0.0f;
    float num6 = this.differentialSpeed - num3;
    this.slipRatio = 0.0f;
    this.idealSlipRatio = 0.0f;
    float num7 = this.maxRPM / (Mathf.Abs(this.ratio) * this.angularVelo2RPM);
    foreach (Wheel poweredWheel in this.poweredWheels)
    {
      if (this.revLimiter && (double) poweredWheel.angularVelocity > (double) num7)
        poweredWheel.angularVelocity = num7;
      this.lockingTorqueImpulse = (num3 - poweredWheel.angularVelocity) * this.differentialLockCoefficient * Time.deltaTime;
      poweredWheel.drivetrainInertia = num1 * this.ratio * this.ratio * this.drivetrainFraction * this.clutch.GetClutchPosition();
      poweredWheel.angularVelocity += num6;
      poweredWheel.lockingTorqueImpulse = this.lockingTorqueImpulse;
      this.slipRatio += poweredWheel.slipRatio * this.drivetrainFraction;
      this.idealSlipRatio += poweredWheel.idealSlipRatio * this.drivetrainFraction;
    }
    if (this.revLimiter)
    {
      if (this.revLimiterTriggered)
      {
        this.revLimiterReleased = false;
        this.timer += Time.deltaTime;
        if ((double) this.timer < (double) this.revLimiterTime)
          return;
        this.timer = 0.0f;
        this.revLimiterTriggered = false;
        this.revLimiterReleased = true;
      }
      else
        this.revLimiterReleased = false;
    }
    else
    {
      this.revLimiterTriggered = false;
      this.revLimiterReleased = false;
    }
  }

  private void DoGearShifting()
  {
    if (this.shiftImmediately)
    {
      this.gear = this.nextGear;
      if (this.nextGear != this.neutral)
        this.shiftTriggered = true;
      this.changingGear = false;
    }
    else
    {
      if ((double) this.throttle > (double) this.idlethrottle * 1.1000000238418579 && this.automatic)
        return;
      if ((double) this.lastShiftTime == 0.0)
        this.lastShiftTime = Time.time;
      this.shiftTimer = (Time.time - this.lastShiftTime) / this.shiftTime;
      if ((double) this.shiftTimer < 1.0)
        this.DisengageClutch();
      if ((double) this.shiftTimer >= 0.33000001311302185)
        this.gear = this.neutral;
      if ((double) this.shiftTimer >= 0.6600000262260437)
      {
        this.gear = this.nextGear;
        if (!this.soundPlayed)
          this.shiftTriggered = true;
      }
      if ((double) this.shiftTimer < 1.0 || (double) this.rpm >= (double) this.engageRPM && (double) this.carController.clutchInput == 0.0)
        return;
      this.changingGear = false;
    }
  }

  private void EngageClutch()
  {
    this.engaging = true;
    int num = 1;
    if ((double) this.rpm < (double) this.maxPowerRPM / 2.0)
    {
      this.clutchEngageSpeed = Mathf.Clamp(this.clutchDragImpulse / this.netTorqueImpulse, Time.fixedDeltaTime * 2f, 1f);
      num = (double) this.clutchDragImpulse < (double) this.netTorqueImpulse || (double) this.netTorque < 1.0 ? 1 : -1;
    }
    else
      this.clutchEngageSpeed = 0.1f;
    if ((double) this.clutchEngageSpeed != 0.0)
      this.clutchPosition += (float) ((double) Time.deltaTime / (double) this.clutchEngageSpeed * (double) num * ((double) this.throttle <= (double) this.idlethrottle ? 1.0 : (double) this.throttle));
    this.clutchPosition = Mathf.Clamp01(this.clutchPosition);
    this.clutch.SetClutchPosition(this.clutchPosition);
    if ((double) this.clutchPosition != 1.0)
      return;
    this.engaging = false;
    this.changingGear = false;
  }

  private void DisengageClutch()
  {
    this.clutchPosition = 0.0f;
    this.clutch.SetClutchPosition(this.clutchPosition);
  }

  public bool OnGround()
  {
    bool flag = false;
    if (this.poweredWheels != null)
    {
      foreach (Wheel poweredWheel in this.poweredWheels)
      {
        flag = poweredWheel.onGroundDown;
        if (flag)
          break;
      }
    }
    return flag;
  }

  public void Shift(int m_gear)
  {
    if (m_gear > this.gearRatios.Length - 1 || m_gear < 0 || this.changingGear || !this.autoClutch && (double) this.clutch.GetClutchPosition() != 0.0)
      return;
    this.soundPlayed = false;
    this.changingGear = true;
    this.nextGear = m_gear;
    if (this.nextGear == this.neutral || this.gear == this.neutral && this.nextGear == this.first || this.gear == this.neutral && this.nextGear == this.firstReverse || this.shifter)
      this.shiftImmediately = true;
    else
      this.shiftImmediately = false;
  }

  public enum Transmissions
  {
    RWD,
    FWD,
    AWD,
    XWD,
  }
}
