// Decompiled with JetBrains decompiler
// Type: UnityTest.TestRunner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityTest.IntegrationTestRunner;

#nullable disable
namespace UnityTest
{
  [Serializable]
  public class TestRunner : MonoBehaviour
  {
    private const string k_Prefix = "IntegrationTest";
    private const string k_StartedMessage = "IntegrationTest Started";
    private const string k_FinishedMessage = "IntegrationTest Finished";
    private const string k_TimeoutMessage = "IntegrationTest Timeout";
    private const string k_FailedMessage = "IntegrationTest Failed";
    private const string k_FailedExceptionMessage = "IntegrationTest Failed with exception";
    private const string k_IgnoredMessage = "IntegrationTest Ignored";
    private const string k_InterruptedMessage = "IntegrationTest Run interrupted";
    private static readonly TestResultRenderer k_ResultRenderer = new TestResultRenderer();
    public TestComponent currentTest;
    private List<TestResult> m_ResultList = new List<TestResult>();
    private List<TestComponent> m_TestComponents;
    private double m_StartTime;
    private bool m_ReadyToRun;
    private string m_TestMessages;
    private string m_Stacktrace;
    private TestRunner.TestState m_TestState;
    private TestRunnerConfigurator m_Configurator;
    public TestRunnerCallbackList TestRunnerCallback = new TestRunnerCallbackList();
    private IntegrationTestsProvider m_TestsProvider;

    public bool isInitializedByRunner => Application.isEditor && !TestRunner.IsBatchMode();

    public void Awake()
    {
      this.m_Configurator = new TestRunnerConfigurator();
      if (this.isInitializedByRunner)
        return;
      TestComponent.DisableAllTests();
    }

    public void Start()
    {
      if (this.isInitializedByRunner)
        return;
      if (this.m_Configurator.sendResultsOverNetwork)
      {
        ITestRunnerCallback callback = this.m_Configurator.ResolveNetworkConnection();
        if (callback != null)
          this.TestRunnerCallback.Add(callback);
      }
      TestComponent.DestroyAllDynamicTests();
      IEnumerable<System.Type> withHelpAttribute = TestComponent.GetTypesWithHelpAttribute(Application.loadedLevelName);
      foreach (System.Type type in withHelpAttribute)
        TestComponent.CreateDynamicTest(type);
      this.InitRunner(TestComponent.FindAllTestsOnScene(), withHelpAttribute.Select<System.Type, string>((Func<System.Type, string>) (type => type.AssemblyQualifiedName)).ToList<string>());
    }

    public void InitRunner(List<TestComponent> tests, List<string> dynamicTestsToRun)
    {
      Application.logMessageReceived += new Application.LogCallback(this.LogHandler);
      foreach (string typeName in dynamicTestsToRun)
      {
        System.Type type = System.Type.GetType(typeName);
        if (type != null)
        {
          MonoBehaviour[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(type) as MonoBehaviour[];
          if (objectsOfTypeAll.Length == 0)
          {
            UnityEngine.Debug.LogWarning((object) (type.ToString() + " not found. Skipping."));
          }
          else
          {
            if (objectsOfTypeAll.Length > 1)
              UnityEngine.Debug.LogWarning((object) ("Multiple GameObjects refer to " + typeName));
            tests.Add(((IEnumerable<MonoBehaviour>) objectsOfTypeAll).First<MonoBehaviour>().GetComponent<TestComponent>());
          }
        }
      }
      this.m_TestComponents = TestRunner.ParseListForGroups((IEnumerable<TestComponent>) tests).ToList<TestComponent>();
      this.m_ResultList = this.m_TestComponents.Select<TestComponent, TestResult>((Func<TestComponent, TestResult>) (component => new TestResult(component))).ToList<TestResult>();
      this.m_TestsProvider = new IntegrationTestsProvider(this.m_ResultList.Select<TestResult, ITestComponent>((Func<TestResult, ITestComponent>) (result => (ITestComponent) result.TestComponent)));
      this.m_ReadyToRun = true;
    }

    private static IEnumerable<TestComponent> ParseListForGroups(IEnumerable<TestComponent> tests)
    {
      HashSet<TestComponent> listForGroups = new HashSet<TestComponent>();
      foreach (TestComponent test in tests)
      {
        TestComponent testResult = test;
        if (testResult.IsTestGroup())
        {
          foreach (TestComponent testComponent in ((IEnumerable<Component>) testResult.gameObject.GetComponentsInChildren(typeof (TestComponent), true)).Where<Component>((Func<Component, bool>) (t => (UnityEngine.Object) t != (UnityEngine.Object) testResult)).Cast<TestComponent>().ToArray<TestComponent>())
          {
            if (!testComponent.IsTestGroup())
              listForGroups.Add(testComponent);
          }
        }
        else
          listForGroups.Add(testResult);
      }
      return (IEnumerable<TestComponent>) listForGroups;
    }

    public void Update()
    {
      if (!this.m_ReadyToRun || Time.frameCount <= 1)
        return;
      this.m_ReadyToRun = false;
      this.StartCoroutine("StateMachine");
    }

    public void OnDestroy()
    {
      if (this.currentTest != (TestComponent) null)
      {
        this.m_ResultList.Single<TestResult>((Func<TestResult, bool>) (result => result.TestComponent == this.currentTest)).messages += "Test run interrupted (crash?)";
        this.LogMessage("IntegrationTest Run interrupted");
        this.FinishTest(TestResult.ResultType.Failed);
      }
      if (this.currentTest != (TestComponent) null || this.m_TestsProvider != null && this.m_TestsProvider.AnyTestsLeft())
        this.TestRunnerCallback.TestRunInterrupted(this.m_TestsProvider.GetRemainingTests().ToList<ITestComponent>());
      Application.logMessageReceived -= new Application.LogCallback(this.LogHandler);
    }

    private void LogHandler(string condition, string stacktrace, UnityEngine.LogType type)
    {
      if (!condition.StartsWith("IntegrationTest Started") && !condition.StartsWith("IntegrationTest Finished"))
      {
        string str = condition;
        if (str.StartsWith("IntegrationTest"))
          str = str.Substring("IntegrationTest".Length + 1);
        if (this.currentTest != (TestComponent) null && str.EndsWith("(" + this.currentTest.name + (object) ')'))
          str = str.Substring(0, str.LastIndexOf('('));
        TestRunner testRunner = this;
        testRunner.m_TestMessages = testRunner.m_TestMessages + str + "\n";
      }
      switch (type)
      {
        case UnityEngine.LogType.Error:
        case UnityEngine.LogType.Assert:
          this.m_TestState = TestRunner.TestState.Failure;
          this.m_Stacktrace = stacktrace;
          break;
        case UnityEngine.LogType.Log:
          if (this.m_TestState == TestRunner.TestState.Running && condition.StartsWith("IntegrationTest Pass"))
            this.m_TestState = TestRunner.TestState.Success;
          if (!condition.StartsWith("IntegrationTest Fail"))
            break;
          this.m_TestState = TestRunner.TestState.Failure;
          break;
        case UnityEngine.LogType.Exception:
          string exception = condition.Substring(0, condition.IndexOf(':'));
          if (this.currentTest != (TestComponent) null && this.currentTest.IsExceptionExpected(exception))
          {
            TestRunner testRunner = this;
            testRunner.m_TestMessages = testRunner.m_TestMessages + exception + " was expected\n";
            if (!this.currentTest.ShouldSucceedOnException())
              break;
            this.m_TestState = TestRunner.TestState.Success;
            break;
          }
          this.m_TestState = TestRunner.TestState.Exception;
          this.m_Stacktrace = stacktrace;
          break;
      }
    }

    [DebuggerHidden]
    public IEnumerator StateMachine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new TestRunner.\u003CStateMachine\u003Ec__Iterator203()
      {
        \u003C\u003Ef__this = this
      };
    }

    private void LogMessage(string message)
    {
      if (this.currentTest != (TestComponent) null)
        UnityEngine.Debug.Log((object) (message + " (" + this.currentTest.Name + ")"), (UnityEngine.Object) this.currentTest.gameObject);
      else
        UnityEngine.Debug.Log((object) message);
    }

    private void FinishTestRun()
    {
      this.PrintResultToLog();
      this.TestRunnerCallback.RunFinished(this.m_ResultList);
      this.LoadNextLevelOrQuit();
    }

    private void PrintResultToLog()
    {
      string message = string.Empty + "Passed: " + (object) this.m_ResultList.Count<TestResult>((Func<TestResult, bool>) (t => t.IsSuccess));
      if (this.m_ResultList.Any<TestResult>((Func<TestResult, bool>) (result => result.IsFailure)))
      {
        message = message + " Failed: " + (object) this.m_ResultList.Count<TestResult>((Func<TestResult, bool>) (t => t.IsFailure));
        UnityEngine.Debug.Log((object) ("Failed tests: " + string.Join(", ", this.m_ResultList.Where<TestResult>((Func<TestResult, bool>) (t => t.IsFailure)).Select<TestResult, string>((Func<TestResult, string>) (result => result.Name)).ToArray<string>())));
      }
      if (this.m_ResultList.Any<TestResult>((Func<TestResult, bool>) (result => result.IsIgnored)))
      {
        message = message + " Ignored: " + (object) this.m_ResultList.Count<TestResult>((Func<TestResult, bool>) (t => t.IsIgnored));
        UnityEngine.Debug.Log((object) ("Ignored tests: " + string.Join(", ", this.m_ResultList.Where<TestResult>((Func<TestResult, bool>) (t => t.IsIgnored)).Select<TestResult, string>((Func<TestResult, string>) (result => result.Name)).ToArray<string>())));
      }
      UnityEngine.Debug.Log((object) message);
    }

    private void LoadNextLevelOrQuit()
    {
      if (this.isInitializedByRunner)
        return;
      if (Application.loadedLevel < Application.levelCount - 1)
      {
        Application.LoadLevel(Application.loadedLevel + 1);
      }
      else
      {
        TestRunner.k_ResultRenderer.ShowResults();
        if (!this.m_Configurator.isBatchRun || !this.m_Configurator.sendResultsOverNetwork)
          return;
        Application.Quit();
      }
    }

    public void OnGUI() => TestRunner.k_ResultRenderer.Draw();

    private void StartNewTest()
    {
      this.m_TestMessages = string.Empty;
      this.m_Stacktrace = string.Empty;
      this.m_TestState = TestRunner.TestState.Running;
      this.m_StartTime = (double) Time.time;
      this.currentTest = this.m_TestsProvider.GetNextTest() as TestComponent;
      TestResult test = this.m_ResultList.Single<TestResult>((Func<TestResult, bool>) (result => result.TestComponent == this.currentTest));
      if (this.currentTest != (TestComponent) null && this.currentTest.IsExludedOnThisPlatform())
      {
        this.m_TestState = TestRunner.TestState.Ignored;
        UnityEngine.Debug.Log((object) (this.currentTest.gameObject.name + " is excluded on this platform"));
      }
      if (this.currentTest != (TestComponent) null && this.currentTest.IsIgnored() && (!this.isInitializedByRunner || this.m_ResultList.Count != 1))
        this.m_TestState = TestRunner.TestState.Ignored;
      this.LogMessage("IntegrationTest Started");
      this.TestRunnerCallback.TestStarted(test);
    }

    private void FinishTest(TestResult.ResultType result)
    {
      this.m_TestsProvider.FinishTest((ITestComponent) this.currentTest);
      TestResult testResult = this.m_ResultList.Single<TestResult>((Func<TestResult, bool>) (t => (UnityEngine.Object) t.GameObject == (UnityEngine.Object) this.currentTest.gameObject));
      testResult.resultType = result;
      testResult.duration = (double) Time.time - this.m_StartTime;
      testResult.messages = this.m_TestMessages;
      testResult.stacktrace = this.m_Stacktrace;
      this.TestRunnerCallback.TestFinished(testResult);
      this.currentTest = (TestComponent) null;
      if (testResult.IsSuccess || !testResult.Executed || testResult.IsIgnored)
        return;
      TestRunner.k_ResultRenderer.AddResults(Application.loadedLevelName, (ITestResult) testResult);
    }

    public static TestRunner GetTestRunner()
    {
      TestRunner testRunner = (TestRunner) null;
      UnityEngine.Object[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (TestRunner));
      if (((IEnumerable<UnityEngine.Object>) objectsOfTypeAll).Count<UnityEngine.Object>() > 1)
      {
        foreach (Component component in objectsOfTypeAll)
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) component.gameObject);
      }
      else
        testRunner = ((IEnumerable<UnityEngine.Object>) objectsOfTypeAll).Any<UnityEngine.Object>() ? ((IEnumerable<UnityEngine.Object>) objectsOfTypeAll).Single<UnityEngine.Object>() as TestRunner : TestRunner.Create().GetComponent<TestRunner>();
      return testRunner;
    }

    private static GameObject Create()
    {
      GameObject gameObject = new GameObject(nameof (TestRunner));
      gameObject.AddComponent<TestRunner>();
      UnityEngine.Debug.Log((object) "Created Test Runner");
      return gameObject;
    }

    private static bool IsBatchMode()
    {
      System.Type type = System.Type.GetType("UnityEditorInternal.InternalEditorUtility, UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", false);
      return type != null && (bool) type.GetProperty("inBatchMode").GetValue((object) null, (object[]) null);
    }

    private enum TestState
    {
      Running,
      Success,
      Failure,
      Exception,
      Timeout,
      Ignored,
    }
  }
}
