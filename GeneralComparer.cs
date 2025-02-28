// Decompiled with JetBrains decompiler
// Type: UnityTest.GeneralComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace UnityTest
{
  public class GeneralComparer : ComparerBase
  {
    public GeneralComparer.CompareType compareType;

    protected override bool Compare(object a, object b)
    {
      if (this.compareType == GeneralComparer.CompareType.AEqualsB)
        return a.Equals(b);
      if (this.compareType == GeneralComparer.CompareType.ANotEqualsB)
        return !a.Equals(b);
      throw new Exception();
    }

    public enum CompareType
    {
      AEqualsB,
      ANotEqualsB,
    }
  }
}
