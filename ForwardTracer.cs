// Decompiled with JetBrains decompiler
// Type: ForwardTracer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch;
using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public class ForwardTracer : BaseTracer
{
  public override Vector3 RayOrigin
  {
    get
    {
      CharacterDefinition characterDefinition = this.Entity.GetOrCreate<CharacterDefinition>();
      if ((Object) characterDefinition != (Object) null)
      {
        Transform transform = characterDefinition.GetTransform(CharacterDefinition.Part.EyesCenter);
        if ((Object) transform != (Object) null)
          return transform.position;
      }
      Transform transform1 = (Transform) (GameObjectAttribute<Transform>) this.Entity.Get<MainTransform>();
      return (Object) transform1 != (Object) null ? transform1.position : Vector3.zero;
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
