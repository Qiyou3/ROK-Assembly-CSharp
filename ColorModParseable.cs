// Decompiled with JetBrains decompiler
// Type: ColorModParseable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Interfaces;
using System;
using UnityEngine;

#nullable disable
internal class ColorModParseable : IModParseable
{
  public Color Value;

  public object ParseFromString(string s)
  {
    string[] strArray = s.Trim('(', ')', 'r', 'g', 'b', 'a').Split(',');
    this.Value = new Color(Convert.ToSingle(strArray[0]), Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]));
    return (object) this;
  }

  public string MakeString()
  {
    return string.Format("rgba({0},{1},{2},{3})", (object) this.Value.r, (object) this.Value.g, (object) this.Value.b, (object) this.Value.a);
  }
}
