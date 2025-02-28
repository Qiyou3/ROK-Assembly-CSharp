// Decompiled with JetBrains decompiler
// Type: UnityTest.AssertionException
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace UnityTest
{
  public class AssertionException : Exception
  {
    private readonly AssertionComponent m_Assertion;

    public AssertionException(AssertionComponent assertion)
      : base(assertion.Action.GetFailureMessage())
    {
      this.m_Assertion = assertion;
    }

    public override string StackTrace => "Created in " + this.m_Assertion.GetCreationLocation();
  }
}
