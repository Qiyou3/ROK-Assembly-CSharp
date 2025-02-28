﻿// Decompiled with JetBrains decompiler
// Type: BMFont
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class BMFont
{
  [HideInInspector]
  [SerializeField]
  private int mSize = 16;
  [HideInInspector]
  [SerializeField]
  private int mBase;
  [HideInInspector]
  [SerializeField]
  private int mWidth;
  [HideInInspector]
  [SerializeField]
  private int mHeight;
  [SerializeField]
  [HideInInspector]
  private string mSpriteName;
  [HideInInspector]
  [SerializeField]
  private List<BMGlyph> mSaved = new List<BMGlyph>();
  private Dictionary<int, BMGlyph> mDict = new Dictionary<int, BMGlyph>();

  public bool isValid => this.mSaved.Count > 0;

  public int charSize
  {
    get => this.mSize;
    set => this.mSize = value;
  }

  public int baseOffset
  {
    get => this.mBase;
    set => this.mBase = value;
  }

  public int texWidth
  {
    get => this.mWidth;
    set => this.mWidth = value;
  }

  public int texHeight
  {
    get => this.mHeight;
    set => this.mHeight = value;
  }

  public int glyphCount => this.isValid ? this.mSaved.Count : 0;

  public string spriteName
  {
    get => this.mSpriteName;
    set => this.mSpriteName = value;
  }

  public List<BMGlyph> glyphs => this.mSaved;

  public BMGlyph GetGlyph(int index, bool createIfMissing)
  {
    BMGlyph glyph = (BMGlyph) null;
    if (this.mDict.Count == 0)
    {
      int index1 = 0;
      for (int count = this.mSaved.Count; index1 < count; ++index1)
      {
        BMGlyph bmGlyph = this.mSaved[index1];
        this.mDict.Add(bmGlyph.index, bmGlyph);
      }
    }
    if (!this.mDict.TryGetValue(index, out glyph) && createIfMissing)
    {
      glyph = new BMGlyph();
      glyph.index = index;
      this.mSaved.Add(glyph);
      this.mDict.Add(index, glyph);
    }
    return glyph;
  }

  public BMGlyph GetGlyph(int index) => this.GetGlyph(index, false);

  public void Clear()
  {
    this.mDict.Clear();
    this.mSaved.Clear();
  }

  public void Trim(int xMin, int yMin, int xMax, int yMax)
  {
    if (!this.isValid)
      return;
    int index = 0;
    for (int count = this.mSaved.Count; index < count; ++index)
      this.mSaved[index]?.Trim(xMin, yMin, xMax, yMax);
  }
}
