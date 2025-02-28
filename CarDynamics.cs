// Decompiled with JetBrains decompiler
// Type: CarDynamics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using System;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Axles))]
[RequireComponent(typeof (Rigidbody))]
public class CarDynamics : EntityBehaviour
{
  private float frontnforce;
  private float backnforce;
  private float factor1;
  private float factor2;
  private float factor3;
  private float deltaFactor;
  [HideInInspector]
  public float factor;
  [HideInInspector]
  public float velo;
  private Drivetrain drivetrain;
  private BrakeLights brakeLights;
  private DashBoard dashBoard;
  private SteeringWheel steeringWheel;
  private SoundController soundController;
  private Axles axles;
  [HideInInspector]
  public float transitionDamperVelo = 0.3f;
  public Transform centerOfMass;
  private GameObject centerOfMassObject;
  private Vector3 deltaCenterOfMass;
  private Vector3 originalCenterOfMass;
  private Transform myTransform;
  public float dampAbsRoadVelo = 8f;
  public float inertiaFactor = 1f;
  public float frontRearWeightRepartition = 0.5f;
  public float frontRearBrakeBalance = 0.65f;
  public float frontRearHandBrakeBalance;
  public bool enableForceFeedback;
  [HideInInspector]
  public float forceFeedback;
  [HideInInspector]
  public bool tridimensionalTire;
  [HideInInspector]
  public bool tirePressureEnabled = true;
  [HideInInspector]
  public float airDensity = 1.2041f;
  public Skidmarks skidmarks;
  public MyPhysicMaterial[] physicMaterials = new MyPhysicMaterial[4];
  [HideInInspector]
  public float xlocalPosition;
  [HideInInspector]
  public float xlocalPosition_orig;
  [HideInInspector]
  public float zlocalPosition;
  [HideInInspector]
  public float zlocalPosition_orig;
  [HideInInspector]
  public float ylocalPosition;
  [HideInInspector]
  public float ylocalPosition_orig;
  private float normalForceR;
  private float normalForceF;
  private float antiRollBarForce;
  [HideInInspector]
  public float fixedTimeStepScalar;
  [HideInInspector]
  public float invFixedTimeStepScalar;
  private float invAllWheelsLength;
  private Rigidbody body;
  private bool tiresFound;
  public string wheelLayer = "Unitycar - Wheels";
  private float _sleepTimer;

  [HideInInspector]
  public CarController carController => this.GetComponent<CarController>();

  private float RoundTo(float value, int precision)
  {
    int num = 1;
    for (int index = 1; index <= precision; ++index)
      num *= 10;
    return Mathf.Round(value * (float) num) / (float) num;
  }

  public float GetCentrifugalAccel()
  {
    float num = 0.0f;
    if ((UnityEngine.Object) this.axles != (UnityEngine.Object) null)
    {
      foreach (Wheel allWheel in this.axles.allWheels)
        num += allWheel.Fy;
    }
    return num * this.invAllWheelsLength / this.GetComponent<Rigidbody>().mass;
  }

  public void FixPhysX()
  {
    this.body.collisionDetectionMode = CollisionDetectionMode.Continuous;
    this.body.collisionDetectionMode = CollisionDetectionMode.Discrete;
    this.body.centerOfMass -= this.deltaCenterOfMass;
  }

  public void Awake()
  {
    this.body = this.GetComponent<Rigidbody>();
    this.originalCenterOfMass = this.body.centerOfMass;
    this.myTransform = this.transform;
    this.drivetrain = this.GetComponent<Drivetrain>();
    this.brakeLights = this.GetComponent<BrakeLights>();
    this.dashBoard = this.myTransform.GetComponentInChildren<DashBoard>();
    this.steeringWheel = this.transform.GetComponentInChildren<SteeringWheel>();
    this.soundController = this.GetComponent<SoundController>();
    this.axles = this.GetComponent<Axles>();
    this.invAllWheelsLength = 1f / (float) this.axles.allWheels.Length;
    this.fixedTimeStepScalar = 0.02f / Time.fixedDeltaTime;
    this.invFixedTimeStepScalar = 1f / this.fixedTimeStepScalar;
  }

  public void Start()
  {
    this.body.inertiaTensor *= this.inertiaFactor;
    this.axles.frontAxle.oldCamber = this.axles.frontAxle.camber;
    this.axles.rearAxle.oldCamber = this.axles.rearAxle.camber;
    foreach (Axle otherAxle in this.axles.otherAxles)
      otherAxle.oldCamber = otherAxle.camber;
    this.SetCenterOfMass();
    this.SetWheelsParams();
    this.SetBrakes();
    this.SetTiresType();
    this.xlocalPosition_orig = this.xlocalPosition;
    this.ylocalPosition_orig = this.ylocalPosition;
    this.zlocalPosition_orig = this.zlocalPosition;
  }

  public Vector3 BoundingSize(Collider[] colliders)
  {
    float x = 0.0f;
    float y = 0.0f;
    float z = 0.0f;
    if (colliders.Length != 0)
    {
      foreach (Collider collider in colliders)
      {
        if (collider.gameObject.layer != LayerMask.NameToLayer(this.wheelLayer) && (UnityEngine.Object) collider.transform.GetComponent<FuelTank>() == (UnityEngine.Object) null)
        {
          x += collider.bounds.size.x;
          y += collider.bounds.size.y;
          z += collider.bounds.size.z;
        }
      }
    }
    return new Vector3(x, y, z);
  }

  public void SetCenterOfMass(Vector3? COGPosition = null)
  {
    if ((UnityEngine.Object) this.centerOfMass == (UnityEngine.Object) null)
    {
      this.centerOfMassObject = new GameObject("COG");
      this.centerOfMassObject.transform.parent = this.transform;
      this.centerOfMass = this.centerOfMassObject.transform;
      if (COGPosition.HasValue)
      {
        Rigidbody component = this.GetComponent<Rigidbody>();
        Vector3 vector3_1 = COGPosition.Value;
        this.centerOfMass.localPosition = vector3_1;
        Vector3 vector3_2 = vector3_1;
        component.centerOfMass = vector3_2;
      }
      else
        this.centerOfMass.localPosition = this.GetComponent<Rigidbody>().centerOfMass;
    }
    else
    {
      if (COGPosition.HasValue)
      {
        Rigidbody component = this.GetComponent<Rigidbody>();
        Vector3 vector3_3 = COGPosition.Value;
        this.centerOfMass.localPosition = vector3_3;
        Vector3 vector3_4 = vector3_3;
        component.centerOfMass = vector3_4;
      }
      else
        this.GetComponent<Rigidbody>().centerOfMass = this.centerOfMass.localPosition;
      this.deltaCenterOfMass = this.originalCenterOfMass - this.centerOfMass.localPosition;
    }
    this.xlocalPosition = this.centerOfMass.localPosition.x;
    this.ylocalPosition = this.centerOfMass.localPosition.y;
    this.zlocalPosition = this.centerOfMass.localPosition.z;
  }

  public void SetBrakes()
  {
    this.frontRearBrakeBalance = Mathf.Clamp01(this.frontRearBrakeBalance);
    this.frontRearHandBrakeBalance = Mathf.Clamp01(this.frontRearHandBrakeBalance);
    foreach (Wheel wheel in this.axles.frontAxle.wheels)
    {
      wheel.brakeFrictionTorque = (float) ((double) this.axles.frontAxle.brakeFrictionTorque * (double) Mathf.Min(this.frontRearBrakeBalance, 0.5f) * 2.0);
      wheel.handbrakeFrictionTorque = (float) ((double) this.axles.frontAxle.handbrakeFrictionTorque * (double) Mathf.Min(this.frontRearHandBrakeBalance, 0.5f) * 2.0);
    }
    foreach (Wheel wheel in this.axles.rearAxle.wheels)
    {
      wheel.brakeFrictionTorque = (float) ((double) this.axles.rearAxle.brakeFrictionTorque * (double) Mathf.Min(1f - this.frontRearBrakeBalance, 0.5f) * 2.0);
      wheel.handbrakeFrictionTorque = (float) ((double) this.axles.rearAxle.handbrakeFrictionTorque * (double) Mathf.Min(1f - this.frontRearHandBrakeBalance, 0.5f) * 2.0);
    }
    foreach (Axle otherAxle in this.axles.otherAxles)
    {
      foreach (Wheel wheel in otherAxle.wheels)
      {
        wheel.brakeFrictionTorque = otherAxle.brakeFrictionTorque;
        wheel.handbrakeFrictionTorque = otherAxle.handbrakeFrictionTorque;
      }
    }
  }

  public void SetWheelsParams()
  {
    foreach (Wheel wheel in this.axles.frontAxle.wheels)
    {
      if ((UnityEngine.Object) wheel != (UnityEngine.Object) null)
      {
        wheel.forwardGripFactor = this.axles.frontAxle.forwardGripFactor;
        wheel.sidewaysGripFactor = this.axles.frontAxle.sidewaysGripFactor;
        wheel.suspensionTravel = this.axles.frontAxle.suspensionTravel;
        wheel.suspensionRate = this.axles.frontAxle.suspensionRate;
        wheel.bumpRate = this.axles.frontAxle.bumpRate;
        wheel.reboundRate = this.axles.frontAxle.reboundRate;
        wheel.fastBumpFactor = this.axles.frontAxle.fastBumpFactor;
        wheel.fastReboundFactor = this.axles.frontAxle.fastReboundFactor;
        wheel.pressure = this.axles.frontAxle.tiresPressure;
        wheel.optimalPressure = this.axles.frontAxle.optimalTiresPressure;
        wheel.SetTireStiffness();
        wheel.maxSteeringAngle = this.axles.frontAxle.maxSteeringAngle;
        wheel.axleWheelsLength = this.axles.frontAxle.wheels.Length;
        wheel.axlesNumber = this.axles.otherAxles.Length + 2;
        wheel.caster = this.axles.frontAxle.caster;
        if (wheel.wheelPos == WheelPos.FRONT_RIGHT)
        {
          wheel.deltaCamber = -this.axles.frontAxle.deltaCamber;
          wheel.camber = -this.axles.frontAxle.camber;
        }
        else
        {
          wheel.deltaCamber = this.axles.frontAxle.deltaCamber;
          wheel.camber = this.axles.frontAxle.camber;
        }
      }
    }
    foreach (Wheel wheel in this.axles.rearAxle.wheels)
    {
      if ((UnityEngine.Object) wheel != (UnityEngine.Object) null)
      {
        wheel.forwardGripFactor = this.axles.rearAxle.forwardGripFactor;
        wheel.sidewaysGripFactor = this.axles.rearAxle.sidewaysGripFactor;
        wheel.suspensionTravel = this.axles.rearAxle.suspensionTravel;
        wheel.suspensionRate = this.axles.rearAxle.suspensionRate;
        wheel.bumpRate = this.axles.rearAxle.bumpRate;
        wheel.reboundRate = this.axles.rearAxle.reboundRate;
        wheel.fastBumpFactor = this.axles.rearAxle.fastBumpFactor;
        wheel.fastReboundFactor = this.axles.rearAxle.fastReboundFactor;
        wheel.pressure = this.axles.rearAxle.tiresPressure;
        wheel.optimalPressure = this.axles.rearAxle.optimalTiresPressure;
        wheel.SetTireStiffness();
        wheel.maxSteeringAngle = -this.axles.rearAxle.maxSteeringAngle;
        wheel.axleWheelsLength = this.axles.rearAxle.wheels.Length;
        wheel.axlesNumber = this.axles.otherAxles.Length + 2;
        wheel.caster = this.axles.rearAxle.caster;
        if (wheel.wheelPos == WheelPos.REAR_RIGHT)
        {
          wheel.deltaCamber = -this.axles.rearAxle.deltaCamber;
          wheel.camber = -this.axles.rearAxle.camber;
        }
        else
        {
          wheel.deltaCamber = this.axles.rearAxle.deltaCamber;
          wheel.camber = this.axles.rearAxle.camber;
        }
      }
    }
    foreach (Axle otherAxle in this.axles.otherAxles)
    {
      foreach (Wheel wheel in otherAxle.wheels)
      {
        if ((UnityEngine.Object) wheel != (UnityEngine.Object) null)
        {
          wheel.forwardGripFactor = otherAxle.forwardGripFactor;
          wheel.sidewaysGripFactor = otherAxle.sidewaysGripFactor;
          wheel.suspensionTravel = otherAxle.suspensionTravel;
          wheel.suspensionRate = otherAxle.suspensionRate;
          wheel.bumpRate = otherAxle.bumpRate;
          wheel.reboundRate = otherAxle.reboundRate;
          wheel.fastBumpFactor = otherAxle.fastBumpFactor;
          wheel.fastReboundFactor = otherAxle.fastReboundFactor;
          wheel.pressure = otherAxle.tiresPressure;
          wheel.optimalPressure = otherAxle.optimalTiresPressure;
          wheel.SetTireStiffness();
          wheel.maxSteeringAngle = otherAxle.maxSteeringAngle;
          wheel.axleWheelsLength = otherAxle.wheels.Length;
          wheel.axlesNumber = this.axles.otherAxles.Length + 2;
          wheel.caster = otherAxle.caster;
          if ((UnityEngine.Object) wheel == (UnityEngine.Object) otherAxle.rightWheel)
          {
            wheel.deltaCamber = -otherAxle.deltaCamber;
            wheel.camber = -otherAxle.camber;
          }
          else
          {
            wheel.deltaCamber = otherAxle.deltaCamber;
            wheel.camber = otherAxle.camber;
          }
        }
      }
    }
  }

  public void SetTiresType()
  {
    for (int index = 0; index < this.axles.frontAxle.wheels.Length; ++index)
      this.LoadTiresData(this.axles.frontAxle.wheels[index], this.axles.frontAxle.tires.ToString());
    if (!this.tiresFound)
      this.LogWarning<CarDynamics>("UnityCar: Tires \"{0}\" not found. Using standard tires data on front axle ({1})", (object) this.axles.frontAxle.tires.ToString(), (object) this.myTransform.name);
    for (int index = 0; index < this.axles.rearAxle.wheels.Length; ++index)
      this.LoadTiresData(this.axles.rearAxle.wheels[index], this.axles.rearAxle.tires.ToString());
    if (!this.tiresFound)
      this.LogWarning<CarDynamics>("UnityCar: Tires \"{0}\" not found. Using standard tires data on rear axle ({1})", (object) this.axles.rearAxle.tires.ToString(), (object) this.myTransform.name);
    int num = 1;
    for (int index1 = 0; index1 < this.axles.otherAxles.Length; ++index1)
    {
      Axle otherAxle = this.axles.otherAxles[index1];
      for (int index2 = 0; index2 < otherAxle.wheels.Length; ++index2)
        this.LoadTiresData(otherAxle.wheels[index2], otherAxle.tires.ToString());
      if (!this.tiresFound)
        this.LogWarning<CarDynamics>("UnityCar: Tires  \"{0}\" not found. Using standard tires data on other axle{1} ({2})", (object) otherAxle.tires.ToString(), (object) num, (object) this.myTransform.name);
      ++num;
    }
  }

  public void LoadTiresData(Wheel w, string tires)
  {
    this.tiresFound = true;
    switch (tires)
    {
      case "competition_front":
        Array.Copy((Array) TireParameters.aCompetitionFront, (Array) w.a, w.a.Length);
        Array.Copy((Array) TireParameters.bCompetitionFront, (Array) w.b, w.b.Length);
        if (TireParameters.cCompetitionFront.Length != 0)
        {
          Array.Copy((Array) TireParameters.cCompetitionFront, (Array) w.c, w.c.Length);
          break;
        }
        break;
      case "competition_rear":
        Array.Copy((Array) TireParameters.aCompetitionRear, (Array) w.a, w.a.Length);
        Array.Copy((Array) TireParameters.bCompetitionRear, (Array) w.b, w.b.Length);
        if (TireParameters.cCompetitionRear.Length != 0)
        {
          Array.Copy((Array) TireParameters.cCompetitionRear, (Array) w.c, w.c.Length);
          break;
        }
        break;
      case "supersport_front":
        Array.Copy((Array) TireParameters.aSuperSportFront, (Array) w.a, w.a.Length);
        Array.Copy((Array) TireParameters.bSuperSportFront, (Array) w.b, w.b.Length);
        if (TireParameters.cSuperSportFront.Length != 0)
        {
          Array.Copy((Array) TireParameters.cSuperSportFront, (Array) w.c, w.c.Length);
          break;
        }
        break;
      case "supersport_rear":
        Array.Copy((Array) TireParameters.aSuperSportRear, (Array) w.a, w.a.Length);
        Array.Copy((Array) TireParameters.bSuperSportRear, (Array) w.b, w.b.Length);
        if (TireParameters.cSuperSportRear.Length != 0)
        {
          Array.Copy((Array) TireParameters.cSuperSportRear, (Array) w.c, w.c.Length);
          break;
        }
        break;
      case "sport_front":
        Array.Copy((Array) TireParameters.aSportFront, (Array) w.a, w.a.Length);
        Array.Copy((Array) TireParameters.bSportFront, (Array) w.b, w.b.Length);
        if (TireParameters.cSportFront.Length != 0)
        {
          Array.Copy((Array) TireParameters.cSportFront, (Array) w.c, w.c.Length);
          break;
        }
        break;
      case "sport_rear":
        Array.Copy((Array) TireParameters.aSportRear, (Array) w.a, w.a.Length);
        Array.Copy((Array) TireParameters.bSportRear, (Array) w.b, w.b.Length);
        if (TireParameters.cSportRear.Length != 0)
        {
          Array.Copy((Array) TireParameters.cSportRear, (Array) w.c, w.c.Length);
          break;
        }
        break;
      case "touring_front":
        Array.Copy((Array) TireParameters.aTouringFront, (Array) w.a, w.a.Length);
        Array.Copy((Array) TireParameters.bTouringFront, (Array) w.b, w.b.Length);
        if (TireParameters.cTouringFront.Length != 0)
        {
          Array.Copy((Array) TireParameters.cTouringFront, (Array) w.c, w.c.Length);
          break;
        }
        break;
      case "touring_rear":
        Array.Copy((Array) TireParameters.aTouringRear, (Array) w.a, w.a.Length);
        Array.Copy((Array) TireParameters.bTouringRear, (Array) w.b, w.b.Length);
        if (TireParameters.cTouringRear.Length != 0)
        {
          Array.Copy((Array) TireParameters.cTouringRear, (Array) w.c, w.c.Length);
          break;
        }
        break;
      case "offroad_front":
        Array.Copy((Array) TireParameters.aOffRoadFront, (Array) w.a, w.a.Length);
        Array.Copy((Array) TireParameters.bOffRoadFront, (Array) w.b, w.b.Length);
        if (TireParameters.cOffRoadFront.Length != 0)
        {
          Array.Copy((Array) TireParameters.cOffRoadFront, (Array) w.c, w.c.Length);
          break;
        }
        break;
      case "offroad_rear":
        Array.Copy((Array) TireParameters.aOffRoadRear, (Array) w.a, w.a.Length);
        Array.Copy((Array) TireParameters.bOffRoadRear, (Array) w.b, w.b.Length);
        if (TireParameters.cOffRoadRear.Length != 0)
        {
          Array.Copy((Array) TireParameters.cOffRoadRear, (Array) w.c, w.c.Length);
          break;
        }
        break;
      case "truck_front":
        Array.Copy((Array) TireParameters.aTruckFront, (Array) w.a, w.a.Length);
        Array.Copy((Array) TireParameters.bTruckFront, (Array) w.b, w.b.Length);
        if (TireParameters.cTruckFront.Length != 0)
        {
          Array.Copy((Array) TireParameters.cTruckFront, (Array) w.c, w.c.Length);
          break;
        }
        break;
      case "truck_rear":
        Array.Copy((Array) TireParameters.aTruckRear, (Array) w.a, w.a.Length);
        Array.Copy((Array) TireParameters.bTruckRear, (Array) w.b, w.b.Length);
        if (TireParameters.cTruckRear.Length != 0)
        {
          Array.Copy((Array) TireParameters.cTruckRear, (Array) w.c, w.c.Length);
          break;
        }
        break;
      default:
        this.tiresFound = false;
        break;
    }
    w.CalculateIdealSlipRatioIdealSlipAngle(20);
  }

  public void FixedUpdate()
  {
    if (this.body.centerOfMass != this.centerOfMass.localPosition)
      this.body.centerOfMass = this.centerOfMass.localPosition;
    this.fixedTimeStepScalar = 0.02f / Time.fixedDeltaTime;
    this.invFixedTimeStepScalar = 1f / this.fixedTimeStepScalar;
    this.velo = Mathf.Abs(this.myTransform.InverseTransformDirection(this.body.velocity).z);
    if ((UnityEngine.Object) this.axles.frontAxle.leftWheel != (UnityEngine.Object) null && (UnityEngine.Object) this.axles.frontAxle.rightWheel != (UnityEngine.Object) null)
    {
      this.antiRollBarForce = (this.axles.frontAxle.leftWheel.compression - this.axles.frontAxle.rightWheel.compression) * this.axles.frontAxle.antiRollBarRate;
      this.axles.frontAxle.leftWheel.antiRollBarForce = this.antiRollBarForce;
      this.axles.frontAxle.rightWheel.antiRollBarForce = -this.antiRollBarForce;
    }
    if ((UnityEngine.Object) this.axles.rearAxle.leftWheel != (UnityEngine.Object) null && (UnityEngine.Object) this.axles.rearAxle.rightWheel != (UnityEngine.Object) null)
    {
      this.antiRollBarForce = (this.axles.rearAxle.leftWheel.compression - this.axles.rearAxle.rightWheel.compression) * this.axles.rearAxle.antiRollBarRate;
      this.axles.rearAxle.leftWheel.antiRollBarForce = this.antiRollBarForce;
      this.axles.rearAxle.rightWheel.antiRollBarForce = -this.antiRollBarForce;
    }
    for (int index = 0; index < this.axles.otherAxles.Length; ++index)
    {
      Axle otherAxle = this.axles.otherAxles[index];
      if ((UnityEngine.Object) otherAxle.leftWheel != (UnityEngine.Object) null && (UnityEngine.Object) otherAxle.rightWheel != (UnityEngine.Object) null)
      {
        this.antiRollBarForce = (otherAxle.leftWheel.compression - otherAxle.rightWheel.compression) * otherAxle.antiRollBarRate;
        otherAxle.leftWheel.antiRollBarForce = this.antiRollBarForce;
        otherAxle.rightWheel.antiRollBarForce = -this.antiRollBarForce;
      }
    }
    this.normalForceF = this.normalForceR = 0.0f;
    for (int index = 0; index < this.axles.frontAxle.wheels.Length; ++index)
      this.normalForceF += this.axles.frontAxle.wheels[index].normalForce;
    if (this.axles.frontAxle.wheels.Length != 0)
      this.normalForceF /= (float) this.axles.frontAxle.wheels.Length;
    for (int index = 0; index < this.axles.rearAxle.wheels.Length; ++index)
      this.normalForceR += this.axles.rearAxle.wheels[index].normalForce;
    if (this.axles.rearAxle.wheels.Length != 0)
      this.normalForceR /= (float) this.axles.rearAxle.wheels.Length;
    if ((double) this.normalForceF + (double) this.normalForceR != 0.0)
      this.frontRearWeightRepartition = this.RoundTo(this.normalForceF / (this.normalForceF + this.normalForceR), 2);
    float num1 = (float) ((double) this.body.mass * -(double) Physics.gravity.y * 0.25);
    if ((double) this.zlocalPosition > 0.0)
    {
      this.frontnforce = num1 * (1f + this.zlocalPosition);
      this.backnforce = num1 * (1f - this.zlocalPosition);
    }
    else
    {
      this.frontnforce = num1 * (1f + this.zlocalPosition);
      this.backnforce = num1 * (1f - this.zlocalPosition);
    }
    if ((double) this.normalForceR != 0.0)
      this.factor1 = this.normalForceF / this.normalForceR;
    if ((double) this.backnforce != 0.0)
      this.factor2 = this.frontnforce / this.backnforce;
    this.factor3 = this.factor2 + (float) (0.25 * (1.0 - (double) this.factor2));
    this.deltaFactor = (double) this.factor1 >= (double) this.factor3 ? this.factor1 - this.factor3 : this.factor3 - this.factor1;
    this.factor = this.factor3 - this.deltaFactor;
    this.factor = Mathf.Clamp(this.factor, 0.25f, 1f);
    this.forceFeedback = 0.0f;
    if (!this.enableForceFeedback)
      return;
    float num2 = 0.0f;
    for (int index = 0; index < this.axles.allWheels.Length; ++index)
    {
      Wheel allWheel = this.axles.allWheels[index];
      if ((double) allWheel.maxSteeringAngle != 0.0)
      {
        this.forceFeedback += allWheel.Mz;
        ++num2;
      }
    }
    this.forceFeedback /= num2;
  }

  public float LateralSlipVeloRearWheels()
  {
    float num = 0.0f;
    for (int index = 0; index < this.axles.rearAxle.wheels.Length; ++index)
      num += this.axles.rearAxle.wheels[index].lateralSlipVelo;
    return num / (float) this.axles.rearAxle.wheels.Length;
  }

  public float SlipVelo()
  {
    float num = 0.0f;
    for (int index = 0; index < this.axles.allWheels.Length; ++index)
      num += this.axles.allWheels[index].slipVelo;
    return num * this.invAllWheelsLength;
  }

  public bool AllWheelsOnGround()
  {
    bool flag = false;
    for (int index = 0; index < this.axles.allWheels.Length; ++index)
    {
      flag = this.axles.allWheels[index].onGroundDown;
      if (flag)
        break;
    }
    return flag;
  }

  public enum SurfaceType
  {
    track,
    grass,
    sand,
    offroad,
    oil,
  }

  public enum Tires
  {
    competition_front,
    competition_rear,
    supersport_front,
    supersport_rear,
    sport_front,
    sport_rear,
    touring_front,
    touring_rear,
    offroad_front,
    offroad_rear,
    truck_front,
    truck_rear,
  }
}
