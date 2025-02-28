// Decompiled with JetBrains decompiler
// Type: UnityTest.Assertions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace UnityTest
{
  public static class Assertions
  {
    public static void CheckAssertions()
    {
      Assertions.CheckAssertions(Object.FindObjectsOfType(typeof (AssertionComponent)) as AssertionComponent[]);
    }

    public static void CheckAssertions(AssertionComponent assertion)
    {
      Assertions.CheckAssertions(new AssertionComponent[1]
      {
        assertion
      });
    }

    public static void CheckAssertions(GameObject gameObject)
    {
      Assertions.CheckAssertions(gameObject.GetComponents<AssertionComponent>());
    }

    public static void CheckAssertions(AssertionComponent[] assertions)
    {
      if (!Debug.isDebugBuild)
        return;
      foreach (AssertionComponent assertion in assertions)
      {
        ++assertion.checksPerformed;
        if (!assertion.Action.Compare())
        {
          assertion.hasFailed = true;
          assertion.Action.Fail(assertion);
        }
      }
    }
  }
}
