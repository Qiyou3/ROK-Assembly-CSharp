// Decompiled with JetBrains decompiler
// Type: InputLayersDebug
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Input;
using System;
using UnityEngine;

#nullable disable
public class InputLayersDebug : MonoBehaviour
{
  private string[] _names;
  private InputLayers[] _values;

  private string[] Names
  {
    get
    {
      if (this._names == null)
        this._names = Enum.GetNames(typeof (InputLayers));
      return this._names;
    }
  }

  private InputLayers[] Values
  {
    get
    {
      if (this._values == null)
      {
        Array values = Enum.GetValues(typeof (InputLayers));
        this._values = new InputLayers[values.Length];
        for (int index = 0; index < this._values.Length; ++index)
          this._values[index] = (InputLayers) values.GetValue(index);
      }
      return this._values;
    }
  }
}
