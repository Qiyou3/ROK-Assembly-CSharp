// Decompiled with JetBrains decompiler
// Type: UnityTest.IntegrationTestRunner.TestRunnerCallbackList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace UnityTest.IntegrationTestRunner
{
  public class TestRunnerCallbackList : ITestRunnerCallback
  {
    private readonly List<ITestRunnerCallback> m_CallbackList = new List<ITestRunnerCallback>();

    public void Add(ITestRunnerCallback callback) => this.m_CallbackList.Add(callback);

    public void Remove(ITestRunnerCallback callback) => this.m_CallbackList.Remove(callback);

    public void RunStarted(string platform, List<TestComponent> testsToRun)
    {
      foreach (ITestRunnerCallback callback in this.m_CallbackList)
        callback.RunStarted(platform, testsToRun);
    }

    public void RunFinished(List<TestResult> testResults)
    {
      foreach (ITestRunnerCallback callback in this.m_CallbackList)
        callback.RunFinished(testResults);
    }

    public void TestStarted(TestResult test)
    {
      foreach (ITestRunnerCallback callback in this.m_CallbackList)
        callback.TestStarted(test);
    }

    public void TestFinished(TestResult test)
    {
      foreach (ITestRunnerCallback callback in this.m_CallbackList)
        callback.TestFinished(test);
    }

    public void TestRunInterrupted(List<ITestComponent> testsNotRun)
    {
      foreach (ITestRunnerCallback callback in this.m_CallbackList)
        callback.TestRunInterrupted(testsNotRun);
    }
  }
}
