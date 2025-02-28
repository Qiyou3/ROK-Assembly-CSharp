// Decompiled with JetBrains decompiler
// Type: UnityTest.TestComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace UnityTest
{
  public class TestComponent : MonoBehaviour, IComparable<ITestComponent>, ITestComponent
  {
    public static ITestComponent NullTestComponent = (ITestComponent) new TestComponent.NullTestComponentImpl();
    public float timeout = 5f;
    public bool ignored;
    public bool succeedAfterAllAssertionsAreExecuted;
    public bool expectException;
    public string expectedExceptionList = string.Empty;
    public bool succeedWhenExceptionIsThrown;
    public TestComponent.IncludedPlatforms includedPlatforms = (TestComponent.IncludedPlatforms) -1;
    public string[] platformsToIgnore;
    public bool dynamic;
    public string dynamicTypeName;

    public bool IsExludedOnThisPlatform()
    {
      return this.platformsToIgnore != null && ((IEnumerable<string>) this.platformsToIgnore).Any<string>((Func<string, bool>) (platform => platform == Application.platform.ToString()));
    }

    private static bool IsAssignableFrom(System.Type a, System.Type b) => a.IsAssignableFrom(b);

    public bool IsExceptionExpected(string exception)
    {
      if (!this.expectException)
        return false;
      exception = exception.Trim();
      string expectedExceptionList = this.expectedExceptionList;
      char[] chArray = new char[1]{ ',' };
      foreach (string str in ((IEnumerable<string>) expectedExceptionList.Split(chArray)).Select<string, string>((Func<string, string>) (e => e.Trim())))
      {
        if (exception == str)
          return true;
        System.Type b = System.Type.GetType(exception) ?? TestComponent.GetTypeByName(exception);
        System.Type a = System.Type.GetType(str) ?? TestComponent.GetTypeByName(str);
        if (b != null && a != null && TestComponent.IsAssignableFrom(a, b))
          return true;
      }
      return false;
    }

    public bool ShouldSucceedOnException() => this.succeedWhenExceptionIsThrown;

    public double GetTimeout() => (double) this.timeout;

    public bool IsIgnored() => this.ignored;

    public bool ShouldSucceedOnAssertions() => this.succeedAfterAllAssertionsAreExecuted;

    private static System.Type GetTypeByName(string className)
    {
      return ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).SelectMany<Assembly, System.Type>((Func<Assembly, IEnumerable<System.Type>>) (a => (IEnumerable<System.Type>) a.GetTypes())).FirstOrDefault<System.Type>((Func<System.Type, bool>) (type => type.Name == className));
    }

    public void OnValidate()
    {
      if ((double) this.timeout >= 0.0099999997764825821)
        return;
      this.timeout = 0.01f;
    }

    public void EnableTest(bool enable)
    {
      if (enable && this.dynamic)
      {
        System.Type type = System.Type.GetType(this.dynamicTypeName);
        MonoBehaviour component = this.gameObject.GetComponent(type) as MonoBehaviour;
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) component);
        this.gameObject.AddComponent(type);
      }
      if (this.gameObject.activeSelf == enable)
        return;
      this.gameObject.SetActive(enable);
    }

    public int CompareTo(ITestComponent obj)
    {
      if (obj == TestComponent.NullTestComponent)
        return 1;
      int num = this.gameObject.name.CompareTo(obj.gameObject.name);
      if (num == 0)
        num = this.gameObject.GetInstanceID().CompareTo(obj.gameObject.GetInstanceID());
      return num;
    }

    public bool IsTestGroup()
    {
      for (int index = 0; index < this.gameObject.transform.childCount; ++index)
      {
        if ((UnityEngine.Object) this.gameObject.transform.GetChild(index).GetComponent(typeof (TestComponent)) != (UnityEngine.Object) null)
          return true;
      }
      return false;
    }

    public string Name
    {
      get => (UnityEngine.Object) this.gameObject == (UnityEngine.Object) null ? string.Empty : this.gameObject.name;
    }

    public ITestComponent GetTestGroup()
    {
      Transform parent = this.gameObject.transform.parent;
      return (UnityEngine.Object) parent == (UnityEngine.Object) null ? TestComponent.NullTestComponent : (ITestComponent) parent.GetComponent<TestComponent>();
    }

    public override bool Equals(object o)
    {
      return (object) (o as TestComponent) != null && this == o as TestComponent;
    }

    public override int GetHashCode() => base.GetHashCode();

    public static TestComponent CreateDynamicTest(System.Type type)
    {
      GameObject test = TestComponent.CreateTest(type.Name);
      test.hideFlags |= HideFlags.DontSave;
      test.SetActive(false);
      TestComponent component = test.GetComponent<TestComponent>();
      component.dynamic = true;
      component.dynamicTypeName = type.AssemblyQualifiedName;
      foreach (object customAttribute in type.GetCustomAttributes(false))
      {
        switch (customAttribute)
        {
          case IntegrationTest.TimeoutAttribute _:
            component.timeout = (customAttribute as IntegrationTest.TimeoutAttribute).timeout;
            break;
          case IntegrationTest.IgnoreAttribute _:
            component.ignored = true;
            break;
          case IntegrationTest.SucceedWithAssertions _:
            component.succeedAfterAllAssertionsAreExecuted = true;
            break;
          case IntegrationTest.ExcludePlatformAttribute _:
            component.platformsToIgnore = (customAttribute as IntegrationTest.ExcludePlatformAttribute).platformsToExclude;
            break;
          case IntegrationTest.ExpectExceptions _:
            IntegrationTest.ExpectExceptions expectExceptions = customAttribute as IntegrationTest.ExpectExceptions;
            component.expectException = true;
            component.expectedExceptionList = string.Join(",", expectExceptions.exceptionTypeNames);
            component.succeedWhenExceptionIsThrown = expectExceptions.succeedOnException;
            break;
        }
      }
      test.AddComponent(type);
      return component;
    }

    public static GameObject CreateTest() => TestComponent.CreateTest("New Test");

    private static GameObject CreateTest(string name)
    {
      GameObject test = new GameObject(name);
      test.AddComponent<TestComponent>();
      return test;
    }

    public static List<TestComponent> FindAllTestsOnScene()
    {
      return Resources.FindObjectsOfTypeAll(typeof (TestComponent)).Cast<TestComponent>().ToList<TestComponent>();
    }

    public static List<TestComponent> FindAllTopTestsOnScene()
    {
      return TestComponent.FindAllTestsOnScene().Where<TestComponent>((Func<TestComponent, bool>) (component => (UnityEngine.Object) component.gameObject.transform.parent == (UnityEngine.Object) null)).ToList<TestComponent>();
    }

    public static List<TestComponent> FindAllDynamicTestsOnScene()
    {
      return TestComponent.FindAllTestsOnScene().Where<TestComponent>((Func<TestComponent, bool>) (t => t.dynamic)).ToList<TestComponent>();
    }

    public static void DestroyAllDynamicTests()
    {
      foreach (Component component in TestComponent.FindAllDynamicTestsOnScene())
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) component.gameObject);
    }

    public static void DisableAllTests()
    {
      foreach (TestComponent testComponent in TestComponent.FindAllTestsOnScene())
        testComponent.EnableTest(false);
    }

    public static bool AnyTestsOnScene()
    {
      return TestComponent.FindAllTestsOnScene().Any<TestComponent>();
    }

    public static bool AnyDynamicTestForCurrentScene()
    {
      return TestComponent.GetTypesWithHelpAttribute(Application.loadedLevelName).Any<System.Type>();
    }

    [DebuggerHidden]
    public static IEnumerable<System.Type> GetTypesWithHelpAttribute(string sceneName)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TestComponent.\u003CGetTypesWithHelpAttribute\u003Ec__Iterator202 withHelpAttribute = new TestComponent.\u003CGetTypesWithHelpAttribute\u003Ec__Iterator202()
      {
        sceneName = sceneName,
        \u003C\u0024\u003EsceneName = sceneName
      };
      // ISSUE: reference to a compiler-generated field
      withHelpAttribute.\u0024PC = -2;
      return (IEnumerable<System.Type>) withHelpAttribute;
    }

    public static bool operator ==(TestComponent a, TestComponent b)
    {
      if (object.ReferenceEquals((object) a, (object) b))
        return true;
      if ((object) a == null || (object) b == null)
        return false;
      if (a.dynamic && b.dynamic)
        return a.dynamicTypeName == b.dynamicTypeName;
      return !a.dynamic && !b.dynamic && (UnityEngine.Object) a.gameObject == (UnityEngine.Object) b.gameObject;
    }

    public static bool operator !=(TestComponent a, TestComponent b) => !(a == b);

    virtual GameObject ITestComponent.get_gameObject() => this.gameObject;

    [Flags]
    public enum IncludedPlatforms
    {
      WindowsEditor = 1,
      OSXEditor = 2,
      WindowsPlayer = 4,
      OSXPlayer = 8,
      LinuxPlayer = 16, // 0x00000010
      MetroPlayerX86 = 32, // 0x00000020
      MetroPlayerX64 = 64, // 0x00000040
      MetroPlayerARM = 128, // 0x00000080
      WindowsWebPlayer = 256, // 0x00000100
      OSXWebPlayer = 512, // 0x00000200
      Android = 1024, // 0x00000400
      IPhonePlayer = 2048, // 0x00000800
      TizenPlayer = 4096, // 0x00001000
      WP8Player = 8192, // 0x00002000
      BB10Player = 16384, // 0x00004000
      NaCl = 32768, // 0x00008000
      PS3 = 65536, // 0x00010000
      XBOX360 = 131072, // 0x00020000
      WiiPlayer = 262144, // 0x00040000
      PSP2 = 524288, // 0x00080000
      PS4 = 1048576, // 0x00100000
      PSMPlayer = 2097152, // 0x00200000
      XboxOne = 4194304, // 0x00400000
    }

    private sealed class NullTestComponentImpl : IComparable<ITestComponent>, ITestComponent
    {
      public int CompareTo(ITestComponent other) => other == this ? 0 : -1;

      public void EnableTest(bool enable)
      {
      }

      public bool IsTestGroup() => throw new NotImplementedException();

      public GameObject gameObject { get; private set; }

      public string Name => string.Empty;

      public ITestComponent GetTestGroup() => (ITestComponent) null;

      public bool IsExceptionExpected(string exceptionType) => throw new NotImplementedException();

      public bool ShouldSucceedOnException() => throw new NotImplementedException();

      public double GetTimeout() => throw new NotImplementedException();

      public bool IsIgnored() => throw new NotImplementedException();

      public bool ShouldSucceedOnAssertions() => throw new NotImplementedException();

      public bool IsExludedOnThisPlatform() => throw new NotImplementedException();
    }
  }
}
