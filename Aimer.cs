// Decompiled with JetBrains decompiler
// Type: Aimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.AI;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Starforge.Turret;
using UnityEngine;

#nullable disable
[AddComponentMenu("AutoAim/Aimer")]
public class Aimer : EntityBehaviour
{
  public Aimer.TargetPredictionType m_targetType;
  public Aimer.ProjectilePredictionType m_projectileType;
  public float m_projectileSpeed = 100f;
  public GameObject m_gunObject;
  public GameObject m_optionalSwivel;
  public GameObject m_upReference;
  public bool m_applyAimToBarrel = true;
  public float m_maximumInaccuracy = 0.1f;
  public int m_maximumIterations = 10;
  public int m_iterationsUsed;
  public float m_artificalStupidity;
  public bool m_hasAimed;
  private Vector3 m_aimDirection;
  private Quaternion m_aimRotation;
  private IterativeAimer m_iterativeAimer;
  private AnalyticalAimer m_analyticalAimer;

  public GameObject TargetObject
  {
    get
    {
      Targetable targetable = (Targetable) null;
      AIBridge aiBridge = this.Entity.TryGet<AIBridge>();
      if ((Object) aiBridge != (Object) null)
        targetable = aiBridge.CurrentTarget;
      return (Object) targetable != (Object) null ? targetable.Entity.MainTransform.gameObject : (GameObject) null;
    }
  }

  public Vector3 AimDirection => this.m_aimDirection;

  public Quaternion AimRotation => this.m_aimRotation;

  public void Start()
  {
    if ((Object) this.m_gunObject == (Object) null)
    {
      VerticalPivot verticalPivot = this.Entity.TryGet<VerticalPivot>();
      if ((Object) verticalPivot != (Object) null)
        this.m_gunObject = verticalPivot.gameObject;
    }
    if ((Object) this.m_optionalSwivel == (Object) null)
    {
      HorizontalPivot horizontalPivot = this.Entity.TryGet<HorizontalPivot>();
      if ((Object) horizontalPivot != (Object) null)
        this.m_optionalSwivel = horizontalPivot.gameObject;
    }
    if (!(bool) (Object) this.m_gunObject)
      this.m_gunObject = this.gameObject;
    if (!(bool) (Object) this.m_upReference)
      this.m_upReference = this.gameObject;
    if (this.m_targetType == Aimer.TargetPredictionType.Linear && this.m_projectileType == Aimer.ProjectilePredictionType.Linear)
    {
      this.m_analyticalAimer = new AnalyticalAimer();
      this.m_analyticalAimer.m_target = this.TargetObject;
      this.m_analyticalAimer.m_bulletSpeed = this.m_projectileSpeed;
      this.m_analyticalAimer.m_transform = this.m_gunObject.transform;
    }
    else
    {
      this.m_iterativeAimer = new IterativeAimer();
      switch (this.m_targetType)
      {
        case Aimer.TargetPredictionType.Linear:
          this.m_iterativeAimer.m_targetTracker = (TargetTracker) new LinearTargetTracker();
          break;
        case Aimer.TargetPredictionType.Parabolic:
          this.m_iterativeAimer.m_targetTracker = (TargetTracker) new QuadraticTargetTracker();
          break;
        case Aimer.TargetPredictionType.Circular:
          this.m_iterativeAimer.m_targetTracker = (TargetTracker) new CircularTargetTracker()
          {
            m_lineRenderer = this.GetComponent<LineRenderer>()
          };
          break;
      }
      switch (this.m_projectileType)
      {
        case Aimer.ProjectilePredictionType.Instant:
          this.m_iterativeAimer.m_projectilePredictor = (ProjectilePredictor) new InstantProjectilePredictor();
          break;
        case Aimer.ProjectilePredictionType.Linear:
          this.m_iterativeAimer.m_projectilePredictor = (ProjectilePredictor) new LinearProjectilePredictor();
          break;
        case Aimer.ProjectilePredictionType.Parabolic:
          this.m_iterativeAimer.m_projectilePredictor = (ProjectilePredictor) new QuadraticProjectilePredictor();
          break;
      }
      this.m_iterativeAimer.m_projectilePredictor.transform = this.m_gunObject.transform;
      this.m_iterativeAimer.m_projectilePredictor.m_projectileSpeed = this.m_projectileSpeed;
      this.m_iterativeAimer.m_maximumInaccuracy = this.m_maximumInaccuracy;
      this.m_iterativeAimer.Start();
    }
  }

  public void FixedUpdate()
  {
    this.m_hasAimed = false;
    if ((bool) (Object) this.TargetObject)
    {
      Vector3 predictedPosition;
      float time;
      if (this.m_iterativeAimer != null)
      {
        this.m_iterativeAimer.m_targetTracker.Target = this.TargetObject.transform;
        Vector3 vector3 = this.m_iterativeAimer.IterativePredict(this.m_maximumIterations, out predictedPosition, out time);
        if (!this.m_iterativeAimer.m_targetTracker.IsValidAim())
          return;
        this.m_aimDirection = vector3;
        this.m_iterationsUsed = this.m_iterativeAimer.m_iterationsTaken;
      }
      else
      {
        this.m_analyticalAimer.m_target = this.TargetObject;
        bool valid = true;
        this.m_aimDirection = this.m_analyticalAimer.Predict(out predictedPosition, out time, out valid);
        if (!valid)
          return;
        this.m_iterationsUsed = 1;
      }
      if ((double) this.m_artificalStupidity <= 0.0)
        this.m_aimDirection = Vector3.Slerp(this.m_aimDirection, (this.TargetObject.transform.position - this.m_gunObject.transform.position).normalized, this.m_artificalStupidity);
      Quaternion rotation = Quaternion.LookRotation(this.m_aimDirection, this.m_upReference.transform.up);
      if (this.m_applyAimToBarrel)
        this.ApplyRotation(rotation);
      else
        this.m_aimRotation = rotation;
      this.m_hasAimed = true;
    }
    else if (this.m_iterativeAimer != null)
      this.m_iterativeAimer.m_targetTracker.Target = (Transform) null;
    else
      this.m_analyticalAimer.SetTarget((GameObject) null);
  }

  public void ApplyRotation(Quaternion rotation)
  {
    this.m_aimRotation = rotation;
    if ((bool) (Object) this.m_optionalSwivel)
    {
      Quaternion quaternion = Quaternion.Inverse(this.m_upReference.transform.rotation) * this.m_aimRotation;
      this.m_optionalSwivel.transform.localRotation = Quaternion.Euler(0.0f, quaternion.eulerAngles.y, 0.0f);
      this.m_gunObject.transform.localRotation = Quaternion.Euler(quaternion.eulerAngles.x, 0.0f, 0.0f);
    }
    else
    {
      Quaternion quaternion = Quaternion.Inverse(this.m_upReference.transform.rotation) * this.m_aimRotation;
      this.m_gunObject.transform.rotation = this.m_upReference.transform.rotation * Quaternion.Euler(quaternion.eulerAngles.x, quaternion.eulerAngles.y, 0.0f);
    }
  }

  public enum TargetPredictionType
  {
    Linear,
    Parabolic,
    Circular,
  }

  public enum ProjectilePredictionType
  {
    Instant,
    Linear,
    Parabolic,
  }
}
