// Decompiled with JetBrains decompiler
// Type: UnityTest.FloatComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace UnityTest
{
  public class FloatComparer : ComparerBaseGeneric<float>
  {
    public FloatComparer.CompareTypes compareTypes;
    public double floatingPointError = 9.9999997473787516E-05;

    protected override bool Compare(float a, float b)
    {
      switch (this.compareTypes)
      {
        case FloatComparer.CompareTypes.Equal:
          return (double) Math.Abs(a - b) < this.floatingPointError;
        case FloatComparer.CompareTypes.NotEqual:
          return (double) Math.Abs(a - b) > this.floatingPointError;
        case FloatComparer.CompareTypes.Greater:
          return (double) a > (double) b;
        case FloatComparer.CompareTypes.Less:
          return (double) a < (double) b;
        default:
          throw new Exception();
      }
    }

    public override int GetDepthOfSearch() => 3;

    public enum CompareTypes
    {
      Equal,
      NotEqual,
      Greater,
      Less,
    }
  }
}
