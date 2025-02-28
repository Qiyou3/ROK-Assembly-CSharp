// Decompiled with JetBrains decompiler
// Type: CameraThirdPersonTrace
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Tracing;
using UnityEngine;

#nullable disable
public class CameraThirdPersonTrace : EntityBehaviour
{
  public Transform mount;
  public Transform lookDir;
  public Transform shakeLookDir;
  public Quaternion lookQuat;
  public Transform firstTarget;
  public Transform secondTarget;
  public Transform result;
  public CameraTracer mountTracer;
  public CameraTracer firstTracer;
  public CameraTracer secondTracer;
  public Lerper3 positionLerper;
  public Lerper3 relativePositionLerper;
  public RotationShake rotationShake;
  private TracerIgnoreParams _ignoreParams;

  private TracerIgnoreParams IgnoreParams
  {
    get
    {
      if (this._ignoreParams == null)
        this._ignoreParams = new TracerIgnoreParams("Camera", this.Entity);
      return this._ignoreParams;
    }
  }

  public void LateUpdate()
  {
    if ((Object) this.mount == (Object) null)
    {
      this.LogWarning<CameraThirdPersonTrace>("mount == null");
      this.enabled = false;
    }
    else
    {
      if ((Object) this.lookDir != (Object) null)
        this.lookQuat = this.lookDir.rotation;
      if ((bool) (Object) this.shakeLookDir)
        this.transform.rotation = this.rotationShake.Sample(this.shakeLookDir.rotation) * this.lookQuat;
      else
        this.transform.rotation = this.lookQuat;
      Orientation orientation = new Orientation()
      {
        Position = this.positionLerper.Sample(this.mount.position),
        Rotation = this.transform.rotation
      };
      Vector3 otherPosition = this.relativePositionLerper.Sample(orientation.InverseTransformPosition(this.mount.position));
      Vector3 vector3 = orientation.TransformPosition(otherPosition);
      Vector3 target = orientation.Position + (this.mount.position - vector3);
      this.mountTracer.traceMode = CameraTracer.TraceMode.Point;
      this.transform.position = this.mountTracer.CameraTrace(this.mount.position, target, this.IgnoreParams);
      if (!(bool) (Object) this.secondTarget || !(bool) (Object) this.firstTarget)
        return;
      this.firstTracer.traceMode = CameraTracer.TraceMode.Point;
      this.secondTracer.traceMode = CameraTracer.TraceMode.Point;
      this.firstTracer.CameraTrace(this.transform.position, this.firstTarget.position, this.IgnoreParams);
      this.secondTracer.CameraTrace(this.firstTracer.position, this.secondTarget.position, this.IgnoreParams);
      this.result.position = this.secondTracer.position;
      this.result.rotation = this.secondTarget.rotation;
    }
  }
}
