// Decompiled with JetBrains decompiler
// Type: CrosshairTracer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch;
using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public class CrosshairTracer : BaseTracer
{
  public override Vector3 RayOrigin
  {
    get
    {
      Transform transform1 = Camera.main.transform;
      if ((Object) transform1 != (Object) null && this.Entity.IsLocallyOwned)
        return transform1.position;
      Transform transform2 = (Transform) (GameObjectAttribute<Transform>) this.Entity.Get<MainTransform>();
      return (Object) transform2 != (Object) null ? transform2.position : Vector3.zero;
    }
  }

  public override Vector3 RayDirection
  {
    get
    {
      LookBridge lookBridge = this.Entity.GetOrCreate<LookBridge>();
      if ((Object) lookBridge != (Object) null)
        return lookBridge.Forward;
      Transform transform = Camera.main.transform;
      return (Object) transform != (Object) null ? transform.forward : Vector3.forward;
    }
  }
}
