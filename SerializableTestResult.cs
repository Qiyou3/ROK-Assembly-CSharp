// Decompiled with JetBrains decompiler
// Type: UnityTest.SerializableTestResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace UnityTest
{
  [Serializable]
  internal class SerializableTestResult : ITestResult
  {
    public TestResultState resultState;
    public string message;
    public bool executed;
    public string name;
    public string fullName;
    public string id;
    public bool isSuccess;
    public double duration;
    public string stackTrace;
    public bool isIgnored;

    public TestResultState ResultState => this.resultState;

    public string Message => this.message;

    public string Logs => (string) null;

    public bool Executed => this.executed;

    public string Name => this.name;

    public string FullName => this.fullName;

    public string Id => this.id;

    public bool IsSuccess => this.isSuccess;

    public double Duration => this.duration;

    public string StackTrace => this.stackTrace;

    public bool IsIgnored => this.isIgnored;
  }
}
