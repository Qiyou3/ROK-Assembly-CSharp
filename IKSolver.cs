// Decompiled with JetBrains decompiler
// Type: IKSolver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public abstract class IKSolver
{
  public float positionAccuracy = 1f / 1000f;

  public abstract void Solve(Transform[] bones, Vector3 target);
}
