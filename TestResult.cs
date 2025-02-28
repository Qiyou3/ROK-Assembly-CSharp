// Decompiled with JetBrains decompiler
// Type: UnityTest.TestResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace UnityTest
{
  [Serializable]
  public class TestResult : IComparable<TestResult>, ITestResult
  {
    private readonly GameObject m_Go;
    private string m_Name;
    public TestResult.ResultType resultType = TestResult.ResultType.NotRun;
    public double duration;
    public string messages;
    public string stacktrace;
    public string id;
    public bool dynamicTest;
    public TestComponent TestComponent;

    public TestResult(TestComponent testComponent)
    {
      this.TestComponent = testComponent;
      this.m_Go = testComponent.gameObject;
      this.id = testComponent.gameObject.GetInstanceID().ToString();
      this.dynamicTest = testComponent.dynamic;
      if ((UnityEngine.Object) this.m_Go != (UnityEngine.Object) null)
        this.m_Name = this.m_Go.name;
      if (!this.dynamicTest)
        return;
      this.id = testComponent.dynamicTypeName;
    }

    public GameObject GameObject => this.m_Go;

    public void Update(TestResult oldResult)
    {
      this.resultType = oldResult.resultType;
      this.duration = oldResult.duration;
      this.messages = oldResult.messages;
      this.stacktrace = oldResult.stacktrace;
    }

    public void Reset()
    {
      this.resultType = TestResult.ResultType.NotRun;
      this.duration = 0.0;
      this.messages = string.Empty;
      this.stacktrace = string.Empty;
    }

    public TestResultState ResultState
    {
      get
      {
        switch (this.resultType)
        {
          case TestResult.ResultType.Success:
            return TestResultState.Success;
          case TestResult.ResultType.Failed:
            return TestResultState.Failure;
          case TestResult.ResultType.Timeout:
            return TestResultState.Cancelled;
          case TestResult.ResultType.NotRun:
            return TestResultState.Skipped;
          case TestResult.ResultType.FailedException:
            return TestResultState.Error;
          case TestResult.ResultType.Ignored:
            return TestResultState.Ignored;
          default:
            throw new Exception();
        }
      }
    }

    public string Message => this.messages;

    public string Logs => (string) null;

    public bool Executed => this.resultType != TestResult.ResultType.NotRun;

    public string Name
    {
      get
      {
        if ((UnityEngine.Object) this.m_Go != (UnityEngine.Object) null)
          this.m_Name = this.m_Go.name;
        return this.m_Name;
      }
    }

    public string Id => this.id;

    public bool IsSuccess => this.resultType == TestResult.ResultType.Success;

    public bool IsTimeout => this.resultType == TestResult.ResultType.Timeout;

    public double Duration => this.duration;

    public string StackTrace => this.stacktrace;

    public string FullName
    {
      get
      {
        string fullName = this.Name;
        if ((UnityEngine.Object) this.m_Go != (UnityEngine.Object) null)
        {
          for (Transform parent = this.m_Go.transform.parent; (UnityEngine.Object) parent != (UnityEngine.Object) null; parent = parent.transform.parent)
            fullName = parent.name + "." + fullName;
        }
        return fullName;
      }
    }

    public bool IsIgnored => this.resultType == TestResult.ResultType.Ignored;

    public bool IsFailure
    {
      get
      {
        return this.resultType == TestResult.ResultType.Failed || this.resultType == TestResult.ResultType.FailedException || this.resultType == TestResult.ResultType.Timeout;
      }
    }

    public override int GetHashCode() => this.id.GetHashCode();

    public int CompareTo(TestResult other)
    {
      int num = this.Name.CompareTo(other.Name);
      if (num == 0)
        num = this.m_Go.GetInstanceID().CompareTo(other.m_Go.GetInstanceID());
      return num;
    }

    public override bool Equals(object obj)
    {
      return obj is TestResult ? this.GetHashCode() == obj.GetHashCode() : base.Equals(obj);
    }

    public enum ResultType
    {
      Success,
      Failed,
      Timeout,
      NotRun,
      FailedException,
      Ignored,
    }
  }
}
