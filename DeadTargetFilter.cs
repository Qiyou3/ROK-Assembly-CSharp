// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.DeadTargetFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace CodeHatch.AI
{
  public class DeadTargetFilter : TargetFilterBase
  {
    protected override bool TestInternal(Targetable target)
    {
      IHealth health = target.Entity.TryGet<IHealth>();
      return health == null || !health.IsDead;
    }
  }
}
