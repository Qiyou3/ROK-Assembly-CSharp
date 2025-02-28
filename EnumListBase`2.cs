// Decompiled with JetBrains decompiler
// Type: EnumListBase`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
public abstract class EnumListBase<TEnum, TValue>
{
  private static readonly int[] _enumValues = Enum.GetValues(typeof (TEnum)).Cast<int>().ToArray<int>();
  private static readonly int _arraySize = ((IEnumerable<int>) EnumListBase<TEnum, TValue>._enumValues).Max() + 1;

  protected static TValue[] InitialArray => new TValue[EnumListBase<TEnum, TValue>._arraySize];

  public abstract TValue[] Values { get; }

  public TValue this[int i]
  {
    get => this.Values[i];
    set => this.Values[i] = value;
  }
}
