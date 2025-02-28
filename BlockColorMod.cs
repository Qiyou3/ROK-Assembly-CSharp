// Decompiled with JetBrains decompiler
// Type: BlockColorMod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Blocks;
using CodeHatch.Common;
using CodeHatch.Engine.Modding.Abstract;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BlockColorMod : FullInspector.BaseBehavior
{
  public Dictionary<string, BlockColor> blocks;
  private TilesetLibrary _tilesets;

  private TilesetLibrary Tilesets
  {
    get
    {
      if ((Object) this._tilesets == (Object) null)
        this._tilesets = Object.FindObjectOfType<TilesetLibrary>();
      return this._tilesets;
    }
  }

  public void Defaults(IList<ModEntry> defaults)
  {
    foreach (KeyValuePair<string, BlockColor> block in this.blocks)
      defaults.Add(new ModEntry(block.Key, (object) block.Value.Color.GetHexCodeRGB(string.Empty, string.Empty), block.Value.Comment));
  }

  public void Apply(string key, object value)
  {
    if (!this.blocks.ContainsKey(key))
      return;
    BlockColor block = this.blocks[key];
    if (block == null)
      return;
    block.Color = ColorUtil.ParseHexString((string) value);
    block.ApplyColor(this.Tilesets);
  }
}
