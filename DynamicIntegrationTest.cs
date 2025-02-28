// Decompiled with JetBrains decompiler
// Type: DynamicIntegrationTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[IntegrationTest.ExcludePlatform(new RuntimePlatform[] {RuntimePlatform.Android, RuntimePlatform.LinuxPlayer})]
[IntegrationTest.ExpectExceptions(false, new System.Type[] {typeof (ArgumentException)})]
[IntegrationTest.SucceedWithAssertions]
[IntegrationTest.Timeout(1f)]
[IntegrationTest.DynamicTest("ExampleIntegrationTests")]
public class DynamicIntegrationTest : MonoBehaviour
{
  public void Start() => IntegrationTest.Pass(this.gameObject);
}
