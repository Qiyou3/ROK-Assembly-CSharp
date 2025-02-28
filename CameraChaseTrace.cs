// Decompiled with JetBrains decompiler
// Type: CameraChaseTrace
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using System;
using UnityEngine;

#nullable disable
public class CameraChaseTrace : MonoBehaviour
{
  public Transform mount;
  public Transform lookAt;
  public Transform firstTarget;
  public Transform secondTarget;
  public Transform result;
  public CameraTracer firstTracer;
  public CameraTracer secondTracer;
  public Lerper3 positionLerper;
  public Lerper3 lookLerper;
  public Lerper3 rotationLerper;
  public float maxAngle = 20f;

  public void LateUpdate()
  {
    Transform transform = this.transform;
    if ((bool) (UnityEngine.Object) this.mount)
    {
      transform.position = this.mount.position;
      transform.rotation = this.mount.rotation;
    }
    if ((UnityEngine.Object) this.lookAt == (UnityEngine.Object) null)
      this.lookAt = this.mount;
    if ((UnityEngine.Object) this.lookAt == (UnityEngine.Object) null)
      this.lookAt = transform;
    if ((UnityEngine.Object) this.firstTarget == (UnityEngine.Object) null && (UnityEngine.Object) this.secondTarget == (UnityEngine.Object) null)
    {
      this.LogWarning<CameraChaseTrace>("firstTarget == null && secondTarget == null");
      this.enabled = false;
    }
    else
    {
      if ((UnityEngine.Object) this.secondTarget != (UnityEngine.Object) null && (UnityEngine.Object) this.firstTarget == (UnityEngine.Object) null)
      {
        this.firstTarget = this.secondTarget;
        this.secondTarget = (Transform) null;
      }
      if ((UnityEngine.Object) this.secondTarget != (UnityEngine.Object) null && (UnityEngine.Object) this.firstTarget != (UnityEngine.Object) null)
      {
        this.firstTracer.traceMode = CameraTracer.TraceMode.Point;
        this.secondTracer.traceMode = CameraTracer.TraceMode.Point;
        Vector3 position = this.firstTarget.position;
        throw new NotImplementedException("Need to update to new usage of Tracer.");
      }
      this.firstTracer.traceMode = CameraTracer.TraceMode.Point;
      Vector3 position1 = transform.position;
      throw new NotImplementedException("Need to update to new usage of Tracer.");
    }
  }
}
