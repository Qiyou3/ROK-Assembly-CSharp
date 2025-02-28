// Decompiled with JetBrains decompiler
// Type: UnityTest.TestResultState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace UnityTest
{
  public enum TestResultState : byte
  {
    Inconclusive,
    NotRunnable,
    Skipped,
    Ignored,
    Success,
    Failure,
    Error,
    Cancelled,
  }
}
