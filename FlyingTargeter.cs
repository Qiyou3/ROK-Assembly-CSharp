// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.FlyingTargeter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class FlyingTargeter : MonoBehaviour
  {
    public void Awake() => this.LogError<FlyingTargeter>("Not Implemented");

    private bool IsTargetsScoreBetter(ref TargetPath otherTargetPath, HiveNode node)
    {
      if ((Object) otherTargetPath.target == (Object) null)
        return false;
      if ((Object) node.Target == (Object) null)
        return true;
      float targetPathScore = this.GetTargetPathScore(ref node.CurrentTargetPath, node);
      return this.FirstScoreIsBetterThanSecond(this.GetTargetPathScore(ref otherTargetPath, node), targetPathScore);
    }

    private float GetTargetPathScore(ref TargetPath targetPath, HiveNode node)
    {
      float visibilityScore = this.GetVisibilityScore(ref targetPath);
      if ((double) visibilityScore <= 0.0)
        return 0.0f;
      float num1 = visibilityScore * this.GetIndirectionScore(targetPath.target, node);
      if ((double) num1 <= 0.0)
        return 0.0f;
      float num2 = num1 * this.GetPriorityScore(targetPath.target, node);
      return (double) num2 <= 0.0 ? 0.0f : num2 * this.GetDistanceScore(targetPath.Waypoint, node);
    }

    private float GetVisibilityScore(ref TargetPath targetPath)
    {
      if (targetPath.targetDirectlyVisible)
        return 5f;
      return targetPath.unobstructedPathExists ? 1f : 0.0f;
    }

    private float GetIndirectionScore(Targetable target, HiveNode node)
    {
      if (!target.IsFriendOf((Team) node))
        return 10f;
      HiveNode hiveNode = target as HiveNode;
      return (Object) hiveNode == (Object) null || (Object) hiveNode.TargetAtEndOfChain == (Object) null || hiveNode.TargetAtEndOfChain.IsFriendOf((Team) node) || hiveNode.TargetChainIndirectionLevels == 0 ? 0.0f : 1f / (float) (1 + hiveNode.TargetChainIndirectionLevels);
    }

    private float GetPriorityScore(Targetable target, HiveNode node)
    {
      if ((Object) target == (Object) null)
        return 0.0f;
      if (!target.IsFriendOf((Team) node))
        return target.priority;
      HiveNode hiveNode = target as HiveNode;
      return (Object) hiveNode == (Object) null || (Object) hiveNode.TargetAtEndOfChain == (Object) null ? 0.0f : hiveNode.TargetAtEndOfChain.priority;
    }

    private float GetDistanceScore(Vector3 otherWaypoint, HiveNode node)
    {
      return 5f / Mathf.Min(5f, Vector3.Distance(node.Position, otherWaypoint));
    }

    private bool FirstScoreIsBetterThanSecond(float firstScore, float secondScore)
    {
      return (double) firstScore > (double) secondScore;
    }
  }
}
