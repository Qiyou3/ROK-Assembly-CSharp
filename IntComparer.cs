// Decompiled with JetBrains decompiler
// Type: UnityTest.IntComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace UnityTest
{
  public class IntComparer : ComparerBaseGeneric<int>
  {
    public IntComparer.CompareType compareType;

    protected override bool Compare(int a, int b)
    {
      switch (this.compareType)
      {
        case IntComparer.CompareType.Equal:
          return a == b;
        case IntComparer.CompareType.NotEqual:
          return a != b;
        case IntComparer.CompareType.Greater:
          return a > b;
        case IntComparer.CompareType.GreaterOrEqual:
          return a >= b;
        case IntComparer.CompareType.Less:
          return a < b;
        case IntComparer.CompareType.LessOrEqual:
          return a <= b;
        default:
          throw new Exception();
      }
    }

    public enum CompareType
    {
      Equal,
      NotEqual,
      Greater,
      GreaterOrEqual,
      Less,
      LessOrEqual,
    }
  }
}
