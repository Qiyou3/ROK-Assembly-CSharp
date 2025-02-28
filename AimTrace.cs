// Decompiled with JetBrains decompiler
// Type: AimTrace
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using UnityEngine;

#nullable disable
public class AimTrace : MonoBehaviour
{
  public bool useMainCameraAsTracePosition;
  public Transform tracePosition;
  public Transform traceDirection;
  public Transform resultTransform;
  public Transform crosshairPivot;
  public float normalOffset = 0.5f;
  public float maxDist = 5000f;
  public bool traceTo;
  public GameObject[] ignoreCollision;
  public float distanceGrowTime = 0.5f;
  public float smoothDistance;
  public bool runInEditor;
  public bool ignoreNonKinematicRigidbodies;
  public bool ignoreKinematicRigidbodies;
  public RaycastHit raycastHit;

  public Transform TracePosition
  {
    get
    {
      if (this.useMainCameraAsTracePosition && ((Object) this.tracePosition == (Object) null || (Object) this.tracePosition.GetComponent<Camera>() == (Object) null))
      {
        this.tracePosition = !((Object) Camera.main == (Object) null) ? Camera.main.transform : (Transform) null;
        if ((Object) this.tracePosition == (Object) null)
        {
          Camera objectOfType = Object.FindObjectOfType(typeof (Camera)) as Camera;
          if ((Object) objectOfType != (Object) null)
            this.tracePosition = objectOfType.transform;
        }
      }
      if ((Object) this.tracePosition == (Object) null)
      {
        if ((Object) this.tracePosition == (Object) null)
        {
          this.LogWarning<AimTrace>(ErrorUtil.NullRefWarning(this.gameObject.GetFullName() + ".tracePosition"));
          this.tracePosition = this.transform.parent;
        }
        if ((Object) this.tracePosition == (Object) null)
          this.LogError<AimTrace>("tracePosition == null");
      }
      return this.tracePosition;
    }
  }

  public Transform TraceDirection
  {
    get
    {
      if ((Object) this.traceDirection == (Object) null)
      {
        this.LogWarning<AimTrace>(ErrorUtil.NullRefWarning(this.gameObject.GetFullName() + ".traceDirection"));
        this.traceDirection = this.transform.parent;
      }
      if ((Object) this.traceDirection == (Object) null)
      {
        this.LogWarning<AimTrace>(ErrorUtil.NullRefWarning(this.gameObject.GetFullName() + ".traceDirection"));
        this.traceDirection = this.transform;
      }
      if ((Object) this.traceDirection == (Object) null)
        this.LogError<AimTrace>("traceDirection == null");
      return this.traceDirection;
    }
  }

  public Transform ResultTransform
  {
    get
    {
      if ((Object) this.resultTransform == (Object) null)
      {
        this.LogWarning<AimTrace>(ErrorUtil.NullRefWarning(this.gameObject.GetFullName() + ".traceDirection"));
        this.resultTransform = this.transform;
      }
      return this.resultTransform;
    }
  }

  public void Start()
  {
    if (!((Object) this.crosshairPivot == (Object) null))
      return;
    this.LogWarning<AimTrace>(ErrorUtil.NullRefWarning(this.gameObject.GetFullName() + ".crosshairPivot"));
  }

  public void Update()
  {
    float num1;
    Ray ray;
    if (this.traceTo)
    {
      Vector3 direction = this.TraceDirection.position - this.TracePosition.position;
      num1 = direction.magnitude;
      if ((double) num1 > 0.0)
        direction /= num1;
      ray = new Ray(this.TracePosition.position, direction);
    }
    else
    {
      num1 = this.maxDist;
      ray = new Ray(this.TracePosition.position, this.TraceDirection.forward);
    }
    float num2 = 1f - Mathf.Pow(0.5f, Time.deltaTime / this.distanceGrowTime);
    if ((double) num1 < (double) this.smoothDistance)
      this.smoothDistance = num1;
    else
      this.smoothDistance += (num1 - this.smoothDistance) * num2;
    this.ResultTransform.position = ray.GetPoint(this.smoothDistance);
    if (!(bool) (Object) this.crosshairPivot)
      return;
    this.crosshairPivot.rotation = this.TraceDirection.rotation;
  }
}
