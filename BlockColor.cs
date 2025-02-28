// Decompiled with JetBrains decompiler
// Type: BlockColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Blocks;
using System;
using UnityEngine;

#nullable disable
[Serializable]
public class BlockColor
{
  public Color Color = new Color(0.5882353f, 0.5882353f, 0.5882353f);
  public OctTileset Tileset;
  public int TilesetsArrayIndex;
  public Material[] Materials;
  public string Comment = "The color of this block set (includes ramps/stairs/corners etc.)";

  public void ApplyColor(TilesetLibrary Tilesets)
  {
    this.Tileset.DefaultColor = new Color(this.Color.r, this.Color.g, this.Color.b, this.Tileset.DefaultColor.a);
    if ((UnityEngine.Object) Tilesets != (UnityEngine.Object) null)
      Tilesets.Tilesets[this.TilesetsArrayIndex].DefaultColor = this.Tileset.DefaultColor;
    for (int index = 0; index < this.Materials.Length; ++index)
      this.Materials[index].color = new Color(this.Color.r, this.Color.g, this.Color.b, this.Materials[index].color.a);
  }
}
