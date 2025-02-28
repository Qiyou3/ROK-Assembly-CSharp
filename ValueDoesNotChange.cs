// Decompiled with JetBrains decompiler
// Type: UnityTest.ValueDoesNotChange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace UnityTest
{
  public class ValueDoesNotChange : ActionBase
  {
    private object m_Value;

    protected override bool Compare(object a)
    {
      if (this.m_Value == null)
        this.m_Value = a;
      return this.m_Value.Equals(a);
    }
  }
}
