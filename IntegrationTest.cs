// Decompiled with JetBrains decompiler
// Type: IntegrationTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

#nullable disable
public static class IntegrationTest
{
  public const string passMessage = "IntegrationTest Pass";
  public const string failMessage = "IntegrationTest Fail";

  public static void Pass() => IntegrationTest.LogResult("IntegrationTest Pass");

  public static void Pass(GameObject go) => IntegrationTest.LogResult(go, "IntegrationTest Pass");

  public static void Fail(string reason)
  {
    IntegrationTest.Fail();
    if (string.IsNullOrEmpty(reason))
      return;
    Debug.Log((object) reason);
  }

  public static void Fail(GameObject go, string reason)
  {
    IntegrationTest.Fail(go);
    if (string.IsNullOrEmpty(reason))
      return;
    Debug.Log((object) reason);
  }

  public static void Fail() => IntegrationTest.LogResult("IntegrationTest Fail");

  public static void Fail(GameObject go) => IntegrationTest.LogResult(go, "IntegrationTest Fail");

  public static void Assert(bool condition) => IntegrationTest.Assert(condition, string.Empty);

  public static void Assert(GameObject go, bool condition)
  {
    IntegrationTest.Assert(go, condition, string.Empty);
  }

  public static void Assert(bool condition, string message)
  {
    if (condition)
      IntegrationTest.Pass();
    else
      IntegrationTest.Fail(message);
  }

  public static void Assert(GameObject go, bool condition, string message)
  {
    if (condition)
      IntegrationTest.Pass(go);
    else
      IntegrationTest.Fail(go, message);
  }

  private static void LogResult(string message) => Debug.Log((object) message);

  private static void LogResult(GameObject go, string message)
  {
    Debug.Log((object) (message + " (" + IntegrationTest.FindTestObject(go).name + ")"), (UnityEngine.Object) go);
  }

  private static GameObject FindTestObject(GameObject go)
  {
    for (GameObject testObject = go; (UnityEngine.Object) testObject.transform.parent != (UnityEngine.Object) null; testObject = testObject.transform.parent.gameObject)
    {
      if ((UnityEngine.Object) testObject.GetComponent("TestComponent") != (UnityEngine.Object) null)
        return testObject;
    }
    return go;
  }

  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class ExcludePlatformAttribute : Attribute
  {
    public string[] platformsToExclude;

    public ExcludePlatformAttribute(params RuntimePlatform[] platformsToExclude)
    {
      this.platformsToExclude = ((IEnumerable<RuntimePlatform>) platformsToExclude).Select<RuntimePlatform, string>((Func<RuntimePlatform, string>) (platform => platform.ToString())).ToArray<string>();
    }
  }

  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class ExpectExceptions : Attribute
  {
    public string[] exceptionTypeNames;
    public bool succeedOnException;

    public ExpectExceptions()
      : this(false)
    {
    }

    public ExpectExceptions(bool succeedOnException)
      : this(succeedOnException, new string[0])
    {
    }

    public ExpectExceptions(bool succeedOnException, params string[] exceptionTypeNames)
    {
      this.succeedOnException = succeedOnException;
      this.exceptionTypeNames = exceptionTypeNames;
    }

    public ExpectExceptions(bool succeedOnException, params System.Type[] exceptionTypes)
      : this(succeedOnException, ((IEnumerable<System.Type>) exceptionTypes).Select<System.Type, string>((Func<System.Type, string>) (type => type.FullName)).ToArray<string>())
    {
    }

    public ExpectExceptions(params string[] exceptionTypeNames)
      : this(false, exceptionTypeNames)
    {
    }

    public ExpectExceptions(params System.Type[] exceptionTypes)
      : this(false, exceptionTypes)
    {
    }
  }

  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class IgnoreAttribute : Attribute
  {
  }

  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class DynamicTestAttribute : Attribute
  {
    private readonly string m_SceneName;

    public DynamicTestAttribute(string sceneName)
    {
      if (sceneName.EndsWith(".unity"))
        sceneName = sceneName.Substring(0, sceneName.Length - ".unity".Length);
      this.m_SceneName = sceneName;
    }

    public bool IncludeOnScene(string sceneName)
    {
      return Path.GetFileNameWithoutExtension(sceneName) == this.m_SceneName;
    }
  }

  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class SucceedWithAssertions : Attribute
  {
  }

  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class TimeoutAttribute : Attribute
  {
    public float timeout;

    public TimeoutAttribute(float seconds) => this.timeout = seconds;
  }
}
