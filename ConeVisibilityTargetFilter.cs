// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.ConeVisibilityTargetFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class ConeVisibilityTargetFilter : TargetFilterBase
  {
    public float maxAngle;
    public float maxDistance;
    private Transform _eyesTransform;
    private LookBridge _lookBridge;
    private EntityColliderManager _colliderManager;

    protected Transform EyesTransform
    {
      get
      {
        if ((Object) this._eyesTransform == (Object) null)
          this._eyesTransform = this.Entity.GetOrCreate<CharacterDefinition>().GetTransform(CharacterDefinition.Part.EyesCenter);
        return this._eyesTransform;
      }
    }

    protected LookBridge MyLookBridge
    {
      get
      {
        if ((Object) this._lookBridge == (Object) null)
          this._lookBridge = this.Entity.GetOrCreate<LookBridge>();
        return this._lookBridge;
      }
    }

    protected EntityColliderManager ColliderManager
    {
      get
      {
        if ((Object) this._colliderManager == (Object) null)
          this._colliderManager = this.Entity.GetOrCreate<EntityColliderManager>();
        return this._colliderManager;
      }
    }

    protected override bool TestInternal(Targetable target)
    {
      Vector3 position1 = target.Entity.Position;
      Vector3 position2 = this.EyesTransform.position;
      Vector3 from = position1 - position2;
      return (double) from.magnitude <= (double) this.maxDistance && (double) Vector3.Angle(from, this.EyesTransform.forward) <= (double) this.maxAngle && this.ColliderManager.GetEntityVisibility(target.Entity, position2);
    }
  }
}
