// Decompiled with JetBrains decompiler
// Type: CodeHatch.Blocks.Components.ColorableGrid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Colorable;
using CodeHatch.Common;
using UnityEngine;

#nullable disable
namespace CodeHatch.Blocks.Components
{
  public class ColorableGrid : MonoBehaviour, IColorable
  {
    private PageGrid _Grid;
    private Vector3 _LastPosition;

    public void Start() => this._Grid = this.GetComponent<PageGrid>();

    public int Cost() => 1;

    public void SetColor(Vector3 position, Color newColor, bool invoke)
    {
      Vector3Int localCoordinate = this._Grid.WorldToLocalCoordinate(position);
      CubeInfo cubeInfoAtLocal = this._Grid.GetCubeInfoAtLocal(localCoordinate);
      this._Grid.ColorCubeAtLocal(localCoordinate, (Color32) new Color(newColor.r, newColor.g, newColor.b, (float) cubeInfoAtLocal.CubeColor.a));
      if (!invoke)
        ;
    }

    public void SetAlpha(Vector3 position, float alpha, bool invoke)
    {
    }

    public Color CurrentColor(Vector3 position)
    {
      return (Color) this._Grid.GetCubeInfoAt(position).CubeColor;
    }
  }
}
