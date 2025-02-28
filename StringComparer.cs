// Decompiled with JetBrains decompiler
// Type: UnityTest.StringComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace UnityTest
{
  public class StringComparer : ComparerBaseGeneric<string>
  {
    public StringComparer.CompareType compareType;
    public StringComparison comparisonType = StringComparison.Ordinal;
    public bool ignoreCase;

    protected override bool Compare(string a, string b)
    {
      if (this.ignoreCase)
      {
        a = a.ToLower();
        b = b.ToLower();
      }
      switch (this.compareType)
      {
        case StringComparer.CompareType.Equal:
          return string.Compare(a, b, this.comparisonType) == 0;
        case StringComparer.CompareType.NotEqual:
          return string.Compare(a, b, this.comparisonType) != 0;
        case StringComparer.CompareType.Shorter:
          return string.Compare(a, b, this.comparisonType) < 0;
        case StringComparer.CompareType.Longer:
          return string.Compare(a, b, this.comparisonType) > 0;
        default:
          throw new Exception();
      }
    }

    public enum CompareType
    {
      Equal,
      NotEqual,
      Shorter,
      Longer,
    }
  }
}
