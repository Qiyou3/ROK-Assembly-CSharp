// Decompiled with JetBrains decompiler
// Type: DrawArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DrawArea
{
  public Vector3 min;
  public Vector3 max;
  public Vector3 canvasMin = new Vector3(0.0f, 0.0f, 0.0f);
  public Vector3 canvasMax = new Vector3(1f, 1f, 1f);

  public DrawArea(Vector3 min, Vector3 max)
  {
    this.min = min;
    this.max = max;
  }

  public virtual Vector3 Point(Vector3 p)
  {
    return Camera.main.ScreenToWorldPoint(Vector3.Scale(new Vector3((float) (((double) p.x - (double) this.canvasMin.x) / ((double) this.canvasMax.x - (double) this.canvasMin.x)), (float) (((double) p.y - (double) this.canvasMin.y) / ((double) this.canvasMax.y - (double) this.canvasMin.y)), 0.0f), this.max - this.min) + this.min + Vector3.forward * Camera.main.nearClipPlane * 1.1f);
  }

  public void DrawLine(Vector3 a, Vector3 b, Color c)
  {
    GL.Color(c);
    GL.Vertex(this.Point(a));
    GL.Vertex(this.Point(b));
  }

  public void DrawRay(Vector3 start, Vector3 dir, Color c) => this.DrawLine(start, start + dir, c);

  public void DrawRect(Vector3 a, Vector3 b, Color c)
  {
    GL.Color(c);
    GL.Vertex(this.Point(new Vector3(a.x, a.y, 0.0f)));
    GL.Vertex(this.Point(new Vector3(a.x, b.y, 0.0f)));
    GL.Vertex(this.Point(new Vector3(b.x, b.y, 0.0f)));
    GL.Vertex(this.Point(new Vector3(b.x, a.y, 0.0f)));
  }

  public void DrawDiamond(Vector3 a, Vector3 b, Color c)
  {
    GL.Color(c);
    GL.Vertex(this.Point(new Vector3(a.x, (float) (((double) a.y + (double) b.y) / 2.0), 0.0f)));
    GL.Vertex(this.Point(new Vector3((float) (((double) a.x + (double) b.x) / 2.0), b.y, 0.0f)));
    GL.Vertex(this.Point(new Vector3(b.x, (float) (((double) a.y + (double) b.y) / 2.0), 0.0f)));
    GL.Vertex(this.Point(new Vector3((float) (((double) a.x + (double) b.x) / 2.0), a.y, 0.0f)));
  }

  public void DrawRect(Vector3 corner, Vector3 dirA, Vector3 dirB, Color c)
  {
    GL.Color(c);
    Vector3[] vector3Array = new Vector3[2]{ dirA, dirB };
    for (int index1 = 0; index1 < 2; ++index1)
    {
      for (int index2 = 0; index2 < 2; ++index2)
      {
        Vector3 p = corner + vector3Array[(index2 + 1) % 2] * (float) index1;
        GL.Vertex(this.Point(p));
        GL.Vertex(this.Point(p + vector3Array[index2]));
      }
    }
  }

  public void DrawCube(Vector3 corner, Vector3 dirA, Vector3 dirB, Vector3 dirC, Color c)
  {
    GL.Color(c);
    Vector3[] vector3Array = new Vector3[3]
    {
      dirA,
      dirB,
      dirC
    };
    for (int index1 = 0; index1 < 2; ++index1)
    {
      for (int index2 = 0; index2 < 2; ++index2)
      {
        for (int index3 = 0; index3 < 3; ++index3)
        {
          Vector3 p = corner + vector3Array[(index3 + 1) % 3] * (float) index1 + vector3Array[(index3 + 2) % 3] * (float) index2;
          GL.Vertex(this.Point(p));
          GL.Vertex(this.Point(p + vector3Array[index3]));
        }
      }
    }
  }
}
