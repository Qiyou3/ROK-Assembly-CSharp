// Decompiled with JetBrains decompiler
// Type: DrawArea3D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DrawArea3D : DrawArea
{
  public Matrix4x4 matrix;

  public DrawArea3D(Vector3 min, Vector3 max, Matrix4x4 matrix)
    : base(min, max)
  {
    this.matrix = matrix;
  }

  public override Vector3 Point(Vector3 p)
  {
    return this.matrix.MultiplyPoint3x4(Vector3.Scale(new Vector3((float) (((double) p.x - (double) this.canvasMin.x) / ((double) this.canvasMax.x - (double) this.canvasMin.x)), (float) (((double) p.y - (double) this.canvasMin.y) / ((double) this.canvasMax.y - (double) this.canvasMin.y)), p.z), this.max - this.min) + this.min);
  }
}
