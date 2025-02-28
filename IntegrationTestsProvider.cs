// Decompiled with JetBrains decompiler
// Type: UnityTest.IntegrationTestRunner.IntegrationTestsProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace UnityTest.IntegrationTestRunner
{
  internal class IntegrationTestsProvider
  {
    internal Dictionary<ITestComponent, HashSet<ITestComponent>> testCollection = new Dictionary<ITestComponent, HashSet<ITestComponent>>();
    internal ITestComponent currentTestGroup;
    internal IEnumerable<ITestComponent> testToRun;

    public IntegrationTestsProvider(IEnumerable<ITestComponent> tests)
    {
      this.testToRun = tests;
      foreach (ITestComponent test in (IEnumerable<ITestComponent>) tests.OrderBy<ITestComponent, ITestComponent>((Func<ITestComponent, ITestComponent>) (component => component)))
      {
        if (test.IsTestGroup())
          throw new Exception(test.Name + " is test a group");
        this.AddTestToList(test);
      }
      if (this.currentTestGroup != null)
        return;
      this.currentTestGroup = this.FindInnerTestGroup(TestComponent.NullTestComponent);
    }

    private void AddTestToList(ITestComponent test)
    {
      ITestComponent testGroup = test.GetTestGroup();
      if (!this.testCollection.ContainsKey(testGroup))
        this.testCollection.Add(testGroup, new HashSet<ITestComponent>());
      this.testCollection[testGroup].Add(test);
      if (testGroup == TestComponent.NullTestComponent)
        return;
      this.AddTestToList(testGroup);
    }

    public ITestComponent GetNextTest()
    {
      ITestComponent nextTest = this.testCollection[this.currentTestGroup].First<ITestComponent>();
      this.testCollection[this.currentTestGroup].Remove(nextTest);
      nextTest.EnableTest(true);
      return nextTest;
    }

    public void FinishTest(ITestComponent test)
    {
      try
      {
        test.EnableTest(false);
        this.currentTestGroup = this.FindNextTestGroup(this.currentTestGroup);
      }
      catch (MissingReferenceException ex)
      {
        Debug.LogException((Exception) ex);
      }
    }

    private ITestComponent FindNextTestGroup(ITestComponent testGroup)
    {
      if (testGroup == null)
        throw new Exception("No test left");
      if (this.testCollection[testGroup].Any<ITestComponent>())
      {
        testGroup.EnableTest(true);
        return this.FindInnerTestGroup(testGroup);
      }
      this.testCollection.Remove(testGroup);
      testGroup.EnableTest(false);
      ITestComponent testGroup1 = testGroup.GetTestGroup();
      if (testGroup1 == null)
        return (ITestComponent) null;
      this.testCollection[testGroup1].Remove(testGroup);
      return this.FindNextTestGroup(testGroup1);
    }

    private ITestComponent FindInnerTestGroup(ITestComponent group)
    {
      foreach (ITestComponent group1 in this.testCollection[group])
      {
        if (group1.IsTestGroup())
        {
          group1.EnableTest(true);
          return this.FindInnerTestGroup(group1);
        }
      }
      return group;
    }

    public bool AnyTestsLeft() => this.testCollection.Count != 0;

    public List<ITestComponent> GetRemainingTests()
    {
      List<ITestComponent> remainingTests = new List<ITestComponent>();
      foreach (KeyValuePair<ITestComponent, HashSet<ITestComponent>> test in this.testCollection)
        remainingTests.AddRange((IEnumerable<ITestComponent>) test.Value);
      return remainingTests;
    }
  }
}
