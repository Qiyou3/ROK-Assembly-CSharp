// Decompiled with JetBrains decompiler
// Type: AIPath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Seeker))]
public class AIPath : MonoBehaviour
{
  public float repathRate = 0.5f;
  public Transform target;
  public bool canSearch = true;
  public bool canMove = true;
  public float speed = 3f;
  public float turningSpeed = 5f;
  public float slowdownDistance = 0.6f;
  public float pickNextWaypointDist = 2f;
  public float forwardLook = 1f;
  public float endReachedDistance = 0.2f;
  public bool closestOnPathCheck = true;
  protected float minMoveScale = 0.05f;
  protected Seeker seeker;
  protected Transform tr;
  private float lastRepath = -9999f;
  protected Path path;
  protected CharacterController controller;
  protected NavmeshController navController;
  protected Rigidbody rigid;
  protected int currentWaypointIndex;
  protected bool targetReached;
  protected bool canSearchAgain = true;
  private bool startHasRun;
  private bool waitingForRepath;
  protected Vector3 targetPoint;
  protected Vector3 targetDirection;

  public bool TargetReached => this.targetReached;

  public virtual void Awake()
  {
    this.seeker = this.GetComponent<Seeker>();
    this.tr = this.transform;
    this.seeker.pathCallback += new OnPathDelegate(this.OnPathComplete);
    this.controller = this.GetComponent<CharacterController>();
    this.navController = this.GetComponent<NavmeshController>();
    this.rigid = this.GetComponent<Rigidbody>();
  }

  public virtual void Start()
  {
    this.startHasRun = true;
    this.OnEnable();
  }

  public virtual void OnEnable()
  {
    if (!this.startHasRun)
      return;
    this.StartCoroutine(this.RepeatTrySearchPath());
  }

  [DebuggerHidden]
  public IEnumerator RepeatTrySearchPath()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AIPath.\u003CRepeatTrySearchPath\u003Ec__Iterator10C()
    {
      \u003C\u003Ef__this = this
    };
  }

  public void TrySearchPath()
  {
    if ((double) Time.time - (double) this.lastRepath >= (double) this.repathRate && this.canSearchAgain && this.canSearch)
      this.SearchPath();
    else
      this.StartCoroutine(this.WaitForRepath());
  }

  [DebuggerHidden]
  protected IEnumerator WaitForRepath()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AIPath.\u003CWaitForRepath\u003Ec__Iterator10D()
    {
      \u003C\u003Ef__this = this
    };
  }

  public virtual void SearchPath()
  {
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
    {
      this.LogError<AIPath>("Target is null, aborting all search");
      this.canSearch = false;
    }
    else
    {
      this.lastRepath = Time.time;
      Vector3 position = this.target.position;
      this.canSearchAgain = false;
      this.seeker.StartPath(this.GetFeetPosition(), position);
    }
  }

  public virtual void OnTargetReached()
  {
  }

  public void OnDestroy()
  {
    if (this.path == null)
      return;
    this.path.Release((object) this);
  }

  public virtual void OnPathComplete(Path _p)
  {
    if (!(_p is ABPath abPath))
      throw new Exception("This function only handles ABPaths, do not use special path types");
    if (this.path != null)
      this.path.Release((object) this);
    abPath.Claim((object) this);
    this.path = (Path) abPath;
    this.currentWaypointIndex = 0;
    this.targetReached = false;
    this.canSearchAgain = true;
    if (!this.closestOnPathCheck)
      return;
    Vector3 startPoint = abPath.startPoint;
    Vector3 feetPosition = this.GetFeetPosition();
    float num1 = Vector3.Distance(startPoint, feetPosition);
    Vector3 vector3 = (feetPosition - startPoint) / num1;
    int num2 = (int) ((double) num1 / (double) this.pickNextWaypointDist);
    for (int index = 0; index < num2; ++index)
    {
      this.CalculateVelocity(startPoint);
      startPoint += vector3;
    }
  }

  public virtual Vector3 GetFeetPosition()
  {
    return (UnityEngine.Object) this.controller != (UnityEngine.Object) null ? this.tr.position - Vector3.up * this.controller.height * 0.5f : this.tr.position;
  }

  public virtual void Update()
  {
    if (!this.canMove)
      return;
    Vector3 velocity = this.CalculateVelocity(this.GetFeetPosition());
    if (!velocity.IsValid())
      return;
    if (this.targetDirection != Vector3.zero)
      this.RotateTowards(this.targetDirection);
    if ((UnityEngine.Object) this.navController != (UnityEngine.Object) null)
      this.navController.SimpleMove(this.GetFeetPosition(), velocity);
    else if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
      this.controller.SimpleMove(velocity);
    else if ((UnityEngine.Object) this.rigid != (UnityEngine.Object) null)
      this.rigid.AddForce(velocity);
    else
      this.transform.Translate(velocity * Time.deltaTime, Space.World);
  }

  protected float XZSqrMagnitude(Vector3 a, Vector3 b)
  {
    float num1 = b.x - a.x;
    float num2 = b.z - a.z;
    return (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
  }

  protected Vector3 CalculateVelocity(Vector3 currentPosition)
  {
    if (this.path == null || this.path.vectorPath == null || this.path.vectorPath.Count == 0)
      return Vector3.zero;
    List<Vector3> vectorPath = this.path.vectorPath;
    if (vectorPath.Count == 1)
      vectorPath.Insert(0, currentPosition);
    if (this.currentWaypointIndex >= vectorPath.Count)
      this.currentWaypointIndex = vectorPath.Count - 1;
    if (this.currentWaypointIndex <= 1)
      this.currentWaypointIndex = 1;
    while (this.currentWaypointIndex < vectorPath.Count - 1 && (double) this.XZSqrMagnitude(vectorPath[this.currentWaypointIndex], currentPosition) < (double) this.pickNextWaypointDist * (double) this.pickNextWaypointDist)
      ++this.currentWaypointIndex;
    Vector3 vector3_1 = vectorPath[this.currentWaypointIndex] - vectorPath[this.currentWaypointIndex - 1];
    Vector3 targetPoint = this.CalculateTargetPoint(currentPosition, vectorPath[this.currentWaypointIndex - 1], vectorPath[this.currentWaypointIndex]);
    Vector3 vector3_2 = (targetPoint - currentPosition) with
    {
      y = 0.0f
    };
    float magnitude = vector3_2.magnitude;
    float num1 = Mathf.Clamp01(magnitude / this.slowdownDistance);
    this.targetDirection = vector3_2;
    this.targetPoint = targetPoint;
    if (this.currentWaypointIndex == vectorPath.Count - 1 && (double) magnitude <= (double) this.endReachedDistance)
    {
      if (!this.targetReached)
      {
        this.targetReached = true;
        this.OnTargetReached();
      }
      return Vector3.zero;
    }
    Vector3 forward = this.tr.forward;
    float num2 = this.speed * Mathf.Max(Vector3.Dot(vector3_2.normalized, forward), this.minMoveScale) * num1;
    if ((double) Time.deltaTime > 0.0)
      num2 = Mathf.Clamp(num2, 0.0f, magnitude / (Time.deltaTime * 2f));
    return forward * num2;
  }

  protected virtual void RotateTowards(Vector3 dir)
  {
    this.tr.rotation = Quaternion.Euler(Quaternion.Slerp(this.tr.rotation, Quaternion.LookRotation(dir), this.turningSpeed * Time.fixedDeltaTime).eulerAngles with
    {
      z = 0.0f,
      x = 0.0f
    });
  }

  protected Vector3 CalculateTargetPoint(Vector3 p, Vector3 a, Vector3 b)
  {
    a.y = p.y;
    b.y = p.y;
    float magnitude = (a - b).magnitude;
    if ((double) magnitude == 0.0)
      return a;
    float num1 = Mathfx.Clamp01(Mathfx.NearestPointFactor(a, b, p));
    float num2 = Mathf.Clamp(Mathf.Clamp(this.forwardLook - ((b - a) * num1 + a - p).magnitude, 0.0f, this.forwardLook) / magnitude + num1, 0.0f, 1f);
    return (b - a) * num2 + a;
  }
}
