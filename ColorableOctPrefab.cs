// Decompiled with JetBrains decompiler
// Type: CodeHatch.Blocks.Components.ColorableOctPrefab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Colorable;
using CodeHatch.Common;
using CodeHatch.Common.Attributes;
using UnityEngine;

#nullable disable
namespace CodeHatch.Blocks.Components
{
  public class ColorableOctPrefab : MonoBehaviour, IColorable
  {
    private ColorableGrid _Parent;
    [MinValue(0.0f)]
    [SerializeField]
    private int _PaintCost;

    public ColorableGrid Parent
    {
      get
      {
        if ((Object) this._Parent == (Object) null)
        {
          this._Parent = this.gameObject.FirstComponentAncestor<ColorableGrid>();
          if ((Object) this._Parent == (Object) null)
            this.LogError<ColorableOctPrefab>("Failed to find ColorableOctTree in parent heirarchy.");
        }
        return this._Parent;
      }
    }

    public int PaintCost => this._PaintCost;

    public void SetColor(Vector3 position, Color newColor, bool invoke)
    {
      this.Parent.SetColor(this.transform.position, newColor, invoke);
    }

    public void SetAlpha(Vector3 position, float alpha, bool invoke)
    {
      this.Parent.SetAlpha(this.transform.position, alpha, invoke);
    }

    public int Cost() => this.PaintCost;

    public Color CurrentColor(Vector3 position)
    {
      return this.Parent.CurrentColor(this.transform.position);
    }

    public void ReapplyColor()
    {
      Vector3 position = this.transform.position;
      this.Parent.SetColor(position, this.CurrentColor(position), false);
    }
  }
}
