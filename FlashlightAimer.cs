// Decompiled with JetBrains decompiler
// Type: FlashlightAimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public class FlashlightAimer : EntityBehaviour
{
  private CrosshairTracer _tracer;

  public void LateUpdate()
  {
    if ((Object) this._tracer == (Object) null)
      this._tracer = this.Entity.TryGet<CrosshairTracer>();
    if (!((Object) this._tracer != (Object) null))
      return;
    this.transform.LookAt(this._tracer.Trace().point);
  }
}
