// Decompiled with JetBrains decompiler
// Type: BaseTracer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Tracing;
using JetBrains.Annotations;
using System;
using UnityEngine;

#nullable disable
public abstract class BaseTracer : EntityBehaviour
{
  private const float _longTraceLength = 300f;

  public abstract Vector3 RayOrigin { get; }

  public abstract Vector3 RayDirection { get; }

  public Ray Ray => new Ray(this.RayOrigin, this.RayDirection);

  public RaycastHit Trace() => this.Trace(0.0f);

  public RaycastHit Trace(float radius) => this.Trace(300f, radius);

  public RaycastHit Trace(float distance, float radius)
  {
    try
    {
      return this.Ray.Raycast(radius, distance, this.Entity);
    }
    catch (Exception ex)
    {
      this.LogException<BaseTracer>(ex);
      return new RaycastHit();
    }
  }

  public virtual RaycastHit Trace([NotNull] TracerIgnoreParams ignore) => this.Trace(0.0f, ignore);

  public virtual RaycastHit Trace(float radius, [NotNull] TracerIgnoreParams ignore)
  {
    return this.Trace(300f, radius, ignore);
  }

  public virtual RaycastHit Trace(float distance, float radius, [NotNull] TracerIgnoreParams ignore)
  {
    if (ignore == null)
    {
      Logger.Error<object>("ignore == null");
      return new RaycastHit();
    }
    try
    {
      return this.Ray.Raycast(radius, distance, ignore);
    }
    catch (Exception ex)
    {
      this.LogException<BaseTracer>(ex);
      return new RaycastHit();
    }
  }
}
