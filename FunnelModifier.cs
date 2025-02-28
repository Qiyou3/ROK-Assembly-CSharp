// Decompiled with JetBrains decompiler
// Type: FunnelModifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using Pathfinding;
using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("Pathfinding/Modifiers/Funnel")]
[Serializable]
public class FunnelModifier : MonoModifier
{
  private static List<Vector3> tmpList;
  private static Int3[] leftFunnel;
  private static Int3[] rightFunnel;

  public override ModifierData input => ModifierData.StrictVectorPath;

  public override ModifierData output => ModifierData.VectorPath;

  public override void Apply(Path p, ModifierData source)
  {
    List<Pathfinding.Node> path = p.path;
    List<Vector3> vectorPath = p.vectorPath;
    if (path == null || path.Count == 0 || vectorPath == null || vectorPath.Count != path.Count)
      return;
    int graphIndex = path[0].graphIndex;
    int num = 0;
    List<Vector3> funnelPath = ListPool<Vector3>.Claim();
    List<Vector3> vector3List1 = ListPool<Vector3>.Claim();
    List<Vector3> vector3List2 = ListPool<Vector3>.Claim();
    for (int index1 = 0; index1 < path.Count; ++index1)
    {
      if (path[index1].graphIndex != graphIndex)
      {
        if (!(AstarData.GetGraph(path[num]) is IFunnelGraph graph))
        {
          for (int index2 = num; index2 <= index1; ++index2)
            funnelPath.Add((Vector3) path[index2].position);
        }
        else
          this.ConstructFunnel(graph, vectorPath, path, num, index1 - 1, funnelPath, vector3List1, vector3List2);
        graphIndex = path[index1].graphIndex;
        num = index1;
      }
    }
    if (!(AstarData.GetGraph(path[num]) is IFunnelGraph graph1))
    {
      for (int index = num; index < path.Count - 1; ++index)
        funnelPath.Add((Vector3) path[index].position);
    }
    else
      this.ConstructFunnel(graph1, vectorPath, path, num, path.Count - 1, funnelPath, vector3List1, vector3List2);
    ListPool<Vector3>.Release(p.vectorPath);
    p.vectorPath = funnelPath;
    ListPool<Vector3>.Release(vector3List1);
    ListPool<Vector3>.Release(vector3List2);
  }

  public void ConstructFunnel(
    IFunnelGraph funnelGraph,
    List<Vector3> vectorPath,
    List<Pathfinding.Node> path,
    int sIndex,
    int eIndex,
    List<Vector3> funnelPath,
    List<Vector3> left,
    List<Vector3> right)
  {
    left.Clear();
    right.Clear();
    left.Add(vectorPath[sIndex]);
    right.Add(vectorPath[sIndex]);
    funnelGraph.BuildFunnelCorridor(path, sIndex, eIndex, left, right);
    left.Add(vectorPath[eIndex]);
    right.Add(vectorPath[eIndex]);
    if (this.RunFunnel(left, right, funnelPath))
      return;
    funnelPath.Add(vectorPath[sIndex]);
    funnelPath.Add(vectorPath[eIndex]);
  }

  public bool RunFunnel(List<Vector3> left, List<Vector3> right, List<Vector3> funnelPath)
  {
    if (left.Count <= 3)
      return false;
    while (left[1] == left[2] && right[1] == right[2])
    {
      left.RemoveAt(1);
      right.RemoveAt(1);
      if (left.Count <= 3)
        return false;
    }
    Vector3 p = left[2];
    if (p == left[1])
      p = right[2];
    while (Polygon.IsColinear(left[0], left[1], right[1]) || Polygon.Left(left[1], right[1], p) == Polygon.Left(left[1], right[1], left[0]))
    {
      left.RemoveAt(1);
      right.RemoveAt(1);
      if (left.Count <= 3)
        return false;
      p = left[2];
      if (p == left[1])
        p = right[2];
    }
    if (!Polygon.IsClockwise(left[0], left[1], right[1]) && !Polygon.IsColinear(left[0], left[1], right[1]))
    {
      List<Vector3> vector3List = left;
      left = right;
      right = vector3List;
    }
    funnelPath.Add(left[0]);
    Vector3 a = left[0];
    Vector3 b1 = left[1];
    Vector3 b2 = right[1];
    int num1 = 1;
    int num2 = 1;
    for (int index = 2; index < left.Count; ++index)
    {
      if (funnelPath.Count > 200)
      {
        this.LogWarning<FunnelModifier>("Avoiding infinite loop");
        break;
      }
      Vector3 c1 = left[index];
      Vector3 c2 = right[index];
      if ((double) Polygon.TriangleArea2(a, b2, c2) >= 0.0)
      {
        if (a == b2 || (double) Polygon.TriangleArea2(a, b1, c2) <= 0.0)
        {
          b2 = c2;
          num1 = index;
        }
        else
        {
          funnelPath.Add(b1);
          a = b1;
          int num3 = num2;
          b1 = a;
          b2 = a;
          num2 = num3;
          num1 = num3;
          index = num3;
          continue;
        }
      }
      if ((double) Polygon.TriangleArea2(a, b1, c1) <= 0.0)
      {
        if (a == b1 || (double) Polygon.TriangleArea2(a, b2, c1) >= 0.0)
        {
          b1 = c1;
          num2 = index;
        }
        else
        {
          funnelPath.Add(b2);
          a = b2;
          int num4 = num1;
          b1 = a;
          b2 = a;
          num2 = num4;
          num1 = num4;
          index = num4;
        }
      }
    }
    funnelPath.Add(left[left.Count - 1]);
    return true;
  }
}
