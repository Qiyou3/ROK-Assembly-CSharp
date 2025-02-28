// Decompiled with JetBrains decompiler
// Type: AlignmentTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AlignmentTracker : MonoBehaviour, IAlignmentTracker
{
  public bool fixedUpdate;
  public float velocitySmoothingTime = 0.05f;
  public float accelerationSmoothingTime = 0.02f;
  public float angularVelocitySmoothingTime = 0.02f;
  private float m_CurrentFixedTime;
  private float m_CurrentLateTime;
  private Vector3 m_Position = Vector3.zero;
  private Vector3 m_PositionPrev = Vector3.zero;
  private Vector3 m_Velocity = Vector3.zero;
  private Vector3 m_VelocityPrev = Vector3.zero;
  private Vector3 m_VelocitySmoothed = Vector3.zero;
  private Vector3 m_Acceleration = Vector3.zero;
  private Vector3 m_AccelerationSmoothed = Vector3.zero;
  private Quaternion m_Rotation = Quaternion.identity;
  private Quaternion m_RotationPrev = Quaternion.identity;
  private Vector3 m_AngularVelocity = Vector3.zero;
  private Vector3 m_AngularVelocitySmoothed = Vector3.zero;
  private Vector3 m_Up = Vector3.up;
  private Vector3 m_Forward = Vector3.forward;
  private Vector3 m_Right = Vector3.right;
  private float m_Speed;
  private float m_SpeedSmooth;
  private Rigidbody m_Rigidbody;
  private Transform m_Transform;

  public Vector3 position => this.m_Position;

  public Vector3 velocity => this.m_Velocity;

  public Vector3 velocitySmoothed => this.m_VelocitySmoothed;

  public Vector3 acceleration => this.m_Acceleration;

  public Vector3 accelerationSmoothed => this.m_AccelerationSmoothed;

  public Quaternion rotation => this.m_Rotation;

  public Vector3 angularVelocity => this.m_AngularVelocity;

  public Vector3 angularVelocitySmoothed => this.m_AngularVelocitySmoothed;

  public Vector3 up => this.m_Up;

  public Vector3 forward => this.m_Forward;

  public Vector3 right => this.m_Right;

  public float speed => this.m_Speed;

  public float speedSmooth => this.m_SpeedSmooth;

  public void Awake() => this.Reset();

  public void OnEnable() => this.Reset();

  public void Reset()
  {
    this.m_Rigidbody = this.GetComponent<Rigidbody>();
    this.m_Transform = this.transform;
    this.m_CurrentLateTime = -1f;
    this.m_CurrentFixedTime = -1f;
    this.m_Position = this.m_PositionPrev = this.m_Transform.position;
    this.m_Rotation = this.m_RotationPrev = this.m_Transform.rotation;
    this.m_Velocity = Vector3.zero;
    this.m_VelocityPrev = Vector3.zero;
    this.m_VelocitySmoothed = Vector3.zero;
    this.m_Acceleration = Vector3.zero;
    this.m_AccelerationSmoothed = Vector3.zero;
    this.m_AngularVelocity = Vector3.zero;
    this.m_AngularVelocitySmoothed = Vector3.zero;
    this.m_Up = this.m_Transform.up;
    this.m_Forward = this.m_Transform.forward;
    this.m_Right = this.m_Transform.right;
    this.m_Speed = 0.0f;
    this.m_SpeedSmooth = 0.0f;
  }

  private Vector3 CalculateAngularVelocity(Quaternion prev, Quaternion current)
  {
    Quaternion quaternion = Quaternion.Inverse(prev) * current;
    float angle = 0.0f;
    Vector3 axis = Vector3.zero;
    quaternion.ToAngleAxis(out angle, out axis);
    if (axis == Vector3.zero || (double) axis.x == double.PositiveInfinity || (double) axis.x == double.NegativeInfinity)
      return Vector3.zero;
    if ((double) angle > 180.0)
      angle -= 360f;
    float num = angle / Time.deltaTime;
    return axis.normalized * num;
  }

  private void UpdateTracking()
  {
    this.m_Position = this.m_Transform.position;
    this.m_Rotation = this.m_Transform.rotation;
    if ((Object) this.m_Rigidbody != (Object) null)
    {
      this.m_Velocity = (this.m_Position - this.m_PositionPrev) / Time.deltaTime;
      this.m_AngularVelocity = this.CalculateAngularVelocity(this.m_RotationPrev, this.m_Rotation);
    }
    else
    {
      this.m_Velocity = (this.m_Position - this.m_PositionPrev) / Time.deltaTime;
      this.m_AngularVelocity = this.CalculateAngularVelocity(this.m_RotationPrev, this.m_Rotation);
    }
    this.m_Acceleration = (this.m_Velocity - this.m_VelocityPrev) / Time.deltaTime;
    this.m_PositionPrev = this.m_Position;
    this.m_RotationPrev = this.m_Rotation;
    this.m_VelocityPrev = this.m_Velocity;
    this.m_Up = this.m_Transform.up;
    this.m_Forward = this.m_Transform.forward;
    this.m_Right = this.m_Transform.right;
    Vector3 vector3 = Util.ProjectOntoPlane(this.m_Velocity, this.m_Up);
    this.m_Speed = vector3.magnitude;
    this.m_Speed = (vector3 + this.m_Up * Mathf.Clamp(Vector3.Dot(this.m_Velocity, this.m_Up), -this.m_Speed, this.m_Speed)).magnitude;
    this.m_SpeedSmooth = Util.ProjectOntoPlane(this.m_VelocitySmoothed, this.m_Up).magnitude;
  }

  public void ControlledFixedUpdate()
  {
    if ((double) Time.deltaTime == 0.0 || (double) Time.timeScale == 0.0 || (double) this.m_CurrentFixedTime == (double) Time.time)
      return;
    this.m_CurrentFixedTime = Time.time;
    if (!this.fixedUpdate)
      return;
    this.UpdateTracking();
  }

  public void ControlledLateUpdate()
  {
    if ((double) Time.deltaTime == 0.0 || (double) Time.timeScale == 0.0 || (double) this.m_CurrentLateTime == (double) Time.time)
      return;
    this.m_CurrentLateTime = Time.time;
    if (!this.fixedUpdate)
      this.UpdateTracking();
    this.m_VelocitySmoothed = Vector3.Lerp(this.m_VelocitySmoothed, this.m_Velocity, 1f - Mathf.Pow(0.5f, Time.smoothDeltaTime / this.velocitySmoothingTime));
    this.m_AccelerationSmoothed = Vector3.Lerp(this.m_AccelerationSmoothed, this.m_Acceleration, 1f - Mathf.Pow(0.5f, Time.smoothDeltaTime / this.accelerationSmoothingTime));
    this.m_AngularVelocitySmoothed = Vector3.Lerp(this.m_AngularVelocitySmoothed, this.m_AngularVelocity, 1f - Mathf.Pow(0.5f, Time.smoothDeltaTime / this.angularVelocitySmoothingTime));
    if (!this.fixedUpdate)
      return;
    this.m_Position += this.m_Velocity * Time.deltaTime;
  }
}
