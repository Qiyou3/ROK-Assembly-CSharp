// Decompiled with JetBrains decompiler
// Type: UnityTest.ITestComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace UnityTest
{
  public interface ITestComponent : IComparable<ITestComponent>
  {
    void EnableTest(bool enable);

    bool IsTestGroup();

    GameObject gameObject { get; }

    string Name { get; }

    ITestComponent GetTestGroup();

    bool IsExceptionExpected(string exceptionType);

    bool ShouldSucceedOnException();

    double GetTimeout();

    bool IsIgnored();

    bool ShouldSucceedOnAssertions();

    bool IsExludedOnThisPlatform();
  }
}
