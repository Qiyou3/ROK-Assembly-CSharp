// Decompiled with JetBrains decompiler
// Type: UnityTest.ResultDTO
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace UnityTest
{
  [Serializable]
  public class ResultDTO
  {
    public ResultDTO.MessageType messageType;
    public int levelCount;
    public int loadedLevel;
    public string loadedLevelName;
    public string testName;
    public float testTimeout;
    public ITestResult testResult;

    private ResultDTO(ResultDTO.MessageType messageType)
    {
      this.messageType = messageType;
      this.levelCount = Application.levelCount;
      this.loadedLevel = Application.loadedLevel;
      this.loadedLevelName = Application.loadedLevelName;
    }

    public static ResultDTO CreatePing() => new ResultDTO(ResultDTO.MessageType.Ping);

    public static ResultDTO CreateRunStarted() => new ResultDTO(ResultDTO.MessageType.RunStarted);

    public static ResultDTO CreateRunFinished(List<TestResult> testResults)
    {
      return new ResultDTO(ResultDTO.MessageType.RunFinished);
    }

    public static ResultDTO CreateTestStarted(TestResult test)
    {
      return new ResultDTO(ResultDTO.MessageType.TestStarted)
      {
        testName = test.FullName,
        testTimeout = test.TestComponent.timeout
      };
    }

    public static ResultDTO CreateTestFinished(TestResult test)
    {
      return new ResultDTO(ResultDTO.MessageType.TestFinished)
      {
        testName = test.FullName,
        testResult = ResultDTO.GetSerializableTestResult(test)
      };
    }

    private static ITestResult GetSerializableTestResult(TestResult test)
    {
      return (ITestResult) new SerializableTestResult()
      {
        resultState = test.ResultState,
        message = test.messages,
        executed = test.Executed,
        name = test.Name,
        fullName = test.FullName,
        id = test.id,
        isSuccess = test.IsSuccess,
        duration = test.duration,
        stackTrace = test.stacktrace,
        isIgnored = test.IsIgnored
      };
    }

    public enum MessageType : byte
    {
      Ping,
      RunStarted,
      RunFinished,
      TestStarted,
      TestFinished,
      RunInterrupted,
    }
  }
}
