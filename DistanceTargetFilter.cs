// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.DistanceTargetFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class DistanceTargetFilter : TargetFilterBase
  {
    public float maxDistance;
    private Transform _eyesTransform;

    protected Transform EyesTransform
    {
      get
      {
        if ((Object) this._eyesTransform == (Object) null)
          this._eyesTransform = this.Entity.GetOrCreate<CharacterDefinition>().GetTransform(CharacterDefinition.Part.EyesCenter);
        return this._eyesTransform;
      }
    }

    protected override bool TestInternal(Targetable target)
    {
      return (double) (target.Entity.Position - this.EyesTransform.position).magnitude <= (double) this.maxDistance;
    }
  }
}
