// Decompiled with JetBrains decompiler
// Type: UnityTest.IntegrationTestRunner.ITestRunnerCallback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace UnityTest.IntegrationTestRunner
{
  public interface ITestRunnerCallback
  {
    void RunStarted(string platform, List<TestComponent> testsToRun);

    void RunFinished(List<TestResult> testResults);

    void TestStarted(TestResult test);

    void TestFinished(TestResult test);

    void TestRunInterrupted(List<ITestComponent> testsNotRun);
  }
}
