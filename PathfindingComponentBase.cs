// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.PathfindingComponentBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;

#nullable disable
namespace CodeHatch.AI
{
  public abstract class PathfindingComponentBase : EntityBehaviour
  {
    public abstract bool TryFindPathToTarget(Targetable target, out TargetPath path, HiveNode node);
  }
}
