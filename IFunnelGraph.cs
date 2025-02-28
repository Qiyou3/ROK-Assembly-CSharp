// Decompiled with JetBrains decompiler
// Type: IFunnelGraph
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public interface IFunnelGraph
{
  void BuildFunnelCorridor(
    List<Pathfinding.Node> path,
    int sIndex,
    int eIndex,
    List<Vector3> left,
    List<Vector3> right);

  void AddPortal(Pathfinding.Node n1, Pathfinding.Node n2, List<Vector3> left, List<Vector3> right);
}
