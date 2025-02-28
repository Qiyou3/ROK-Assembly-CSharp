// Decompiled with JetBrains decompiler
// Type: UnityTest.AssertionComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

#nullable disable
namespace UnityTest
{
  [Serializable]
  public class AssertionComponent : MonoBehaviour, IAssertionComponentConfigurator
  {
    [SerializeField]
    public float checkAfterTime = 1f;
    [SerializeField]
    public bool repeatCheckTime = true;
    [SerializeField]
    public float repeatEveryTime = 1f;
    [SerializeField]
    public int checkAfterFrames = 1;
    [SerializeField]
    public bool repeatCheckFrame = true;
    [SerializeField]
    public int repeatEveryFrame = 1;
    [SerializeField]
    public bool hasFailed;
    [SerializeField]
    public CheckMethod checkMethods = CheckMethod.Start;
    [SerializeField]
    private ActionBase m_ActionBase;
    [SerializeField]
    public int checksPerformed;
    private int m_CheckOnFrame;
    private string m_CreatedInFilePath = string.Empty;
    private int m_CreatedInFileLine = -1;

    public ActionBase Action
    {
      get => this.m_ActionBase;
      set
      {
        this.m_ActionBase = value;
        this.m_ActionBase.go = this.gameObject;
      }
    }

    public UnityEngine.Object GetFailureReferenceObject() => (UnityEngine.Object) this;

    public string GetCreationLocation()
    {
      return !string.IsNullOrEmpty(this.m_CreatedInFilePath) ? string.Format("{0}, line {1} ({2})", (object) this.m_CreatedInFilePath.Substring(this.m_CreatedInFilePath.LastIndexOf("\\") + 1), (object) this.m_CreatedInFileLine, (object) this.m_CreatedInFilePath) : string.Empty;
    }

    public void Awake()
    {
      if (!UnityEngine.Debug.isDebugBuild)
        UnityEngine.Object.Destroy((UnityEngine.Object) this);
      this.OnComponentCopy();
    }

    public void OnValidate()
    {
      if (!Application.isEditor)
        return;
      this.OnComponentCopy();
    }

    private void OnComponentCopy()
    {
      if ((UnityEngine.Object) this.m_ActionBase == (UnityEngine.Object) null)
        return;
      IEnumerable<UnityEngine.Object> source = ((IEnumerable<UnityEngine.Object>) Resources.FindObjectsOfTypeAll(typeof (AssertionComponent))).Where<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (o => (UnityEngine.Object) ((AssertionComponent) o).m_ActionBase == (UnityEngine.Object) this.m_ActionBase && o != (UnityEngine.Object) this));
      if (!source.Any<UnityEngine.Object>())
        return;
      if (source.Count<UnityEngine.Object>() > 1)
        UnityEngine.Debug.LogWarning((object) "More than one refence to comparer found. This shouldn't happen");
      AssertionComponent assertionComponent = source.First<UnityEngine.Object>() as AssertionComponent;
      this.m_ActionBase = assertionComponent.m_ActionBase.CreateCopy(assertionComponent.gameObject, this.gameObject);
    }

    public void Start()
    {
      this.CheckAssertionFor(CheckMethod.Start);
      if (this.IsCheckMethodSelected(CheckMethod.AfterPeriodOfTime))
        this.StartCoroutine("CheckPeriodically");
      if (!this.IsCheckMethodSelected(CheckMethod.Update))
        return;
      this.m_CheckOnFrame = Time.frameCount + this.checkAfterFrames;
    }

    [DebuggerHidden]
    public IEnumerator CheckPeriodically()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AssertionComponent.\u003CCheckPeriodically\u003Ec__Iterator201()
      {
        \u003C\u003Ef__this = this
      };
    }

    public bool ShouldCheckOnFrame()
    {
      if (Time.frameCount <= this.m_CheckOnFrame)
        return false;
      if (this.repeatCheckFrame)
        this.m_CheckOnFrame += this.repeatEveryFrame;
      else
        this.m_CheckOnFrame = int.MaxValue;
      return true;
    }

    public void OnDisable() => this.CheckAssertionFor(CheckMethod.OnDisable);

    public void OnEnable() => this.CheckAssertionFor(CheckMethod.OnEnable);

    public void OnDestroy() => this.CheckAssertionFor(CheckMethod.OnDestroy);

    public void Update()
    {
      if (!this.IsCheckMethodSelected(CheckMethod.Update) || !this.ShouldCheckOnFrame())
        return;
      this.CheckAssertionFor(CheckMethod.Update);
    }

    public void FixedUpdate() => this.CheckAssertionFor(CheckMethod.FixedUpdate);

    public void LateUpdate() => this.CheckAssertionFor(CheckMethod.LateUpdate);

    public void OnControllerColliderHit()
    {
      this.CheckAssertionFor(CheckMethod.OnControllerColliderHit);
    }

    public void OnParticleCollision() => this.CheckAssertionFor(CheckMethod.OnParticleCollision);

    public void OnJointBreak() => this.CheckAssertionFor(CheckMethod.OnJointBreak);

    public void OnBecameInvisible() => this.CheckAssertionFor(CheckMethod.OnBecameInvisible);

    public void OnBecameVisible() => this.CheckAssertionFor(CheckMethod.OnBecameVisible);

    public void OnTriggerEnter() => this.CheckAssertionFor(CheckMethod.OnTriggerEnter);

    public void OnTriggerExit() => this.CheckAssertionFor(CheckMethod.OnTriggerExit);

    public void OnTriggerStay() => this.CheckAssertionFor(CheckMethod.OnTriggerStay);

    public void OnCollisionEnter() => this.CheckAssertionFor(CheckMethod.OnCollisionEnter);

    public void OnCollisionExit() => this.CheckAssertionFor(CheckMethod.OnCollisionExit);

    public void OnCollisionStay() => this.CheckAssertionFor(CheckMethod.OnCollisionStay);

    public void OnTriggerEnter2D() => this.CheckAssertionFor(CheckMethod.OnTriggerEnter2D);

    public void OnTriggerExit2D() => this.CheckAssertionFor(CheckMethod.OnTriggerExit2D);

    public void OnTriggerStay2D() => this.CheckAssertionFor(CheckMethod.OnTriggerStay2D);

    public void OnCollisionEnter2D() => this.CheckAssertionFor(CheckMethod.OnCollisionEnter2D);

    public void OnCollisionExit2D() => this.CheckAssertionFor(CheckMethod.OnCollisionExit2D);

    public void OnCollisionStay2D() => this.CheckAssertionFor(CheckMethod.OnCollisionStay2D);

    private void CheckAssertionFor(CheckMethod checkMethod)
    {
      if (!this.IsCheckMethodSelected(checkMethod))
        return;
      Assertions.CheckAssertions(this);
    }

    public bool IsCheckMethodSelected(CheckMethod method) => method == (this.checkMethods & method);

    public static T Create<T>(
      CheckMethod checkOnMethods,
      GameObject gameObject,
      string propertyPath)
      where T : ActionBase
    {
      return AssertionComponent.Create<T>(out IAssertionComponentConfigurator _, checkOnMethods, gameObject, propertyPath);
    }

    public static T Create<T>(
      out IAssertionComponentConfigurator configurator,
      CheckMethod checkOnMethods,
      GameObject gameObject,
      string propertyPath)
      where T : ActionBase
    {
      return AssertionComponent.CreateAssertionComponent<T>(out configurator, checkOnMethods, gameObject, propertyPath);
    }

    public static T Create<T>(
      CheckMethod checkOnMethods,
      GameObject gameObject,
      string propertyPath,
      GameObject gameObject2,
      string propertyPath2)
      where T : ComparerBase
    {
      return AssertionComponent.Create<T>(out IAssertionComponentConfigurator _, checkOnMethods, gameObject, propertyPath, gameObject2, propertyPath2);
    }

    public static T Create<T>(
      out IAssertionComponentConfigurator configurator,
      CheckMethod checkOnMethods,
      GameObject gameObject,
      string propertyPath,
      GameObject gameObject2,
      string propertyPath2)
      where T : ComparerBase
    {
      T assertionComponent = AssertionComponent.CreateAssertionComponent<T>(out configurator, checkOnMethods, gameObject, propertyPath);
      assertionComponent.compareToType = ComparerBase.CompareToType.CompareToObject;
      assertionComponent.other = gameObject2;
      assertionComponent.otherPropertyPath = propertyPath2;
      return assertionComponent;
    }

    public static T Create<T>(
      CheckMethod checkOnMethods,
      GameObject gameObject,
      string propertyPath,
      object constValue)
      where T : ComparerBase
    {
      return AssertionComponent.Create<T>(out IAssertionComponentConfigurator _, checkOnMethods, gameObject, propertyPath, constValue);
    }

    public static T Create<T>(
      out IAssertionComponentConfigurator configurator,
      CheckMethod checkOnMethods,
      GameObject gameObject,
      string propertyPath,
      object constValue)
      where T : ComparerBase
    {
      T assertionComponent = AssertionComponent.CreateAssertionComponent<T>(out configurator, checkOnMethods, gameObject, propertyPath);
      if (constValue == null)
      {
        assertionComponent.compareToType = ComparerBase.CompareToType.CompareToNull;
        return assertionComponent;
      }
      assertionComponent.compareToType = ComparerBase.CompareToType.CompareToConstantValue;
      assertionComponent.ConstValue = constValue;
      return assertionComponent;
    }

    private static T CreateAssertionComponent<T>(
      out IAssertionComponentConfigurator configurator,
      CheckMethod checkOnMethods,
      GameObject gameObject,
      string propertyPath)
      where T : ActionBase
    {
      AssertionComponent assertionComponent = gameObject.AddComponent<AssertionComponent>();
      assertionComponent.checkMethods = checkOnMethods;
      T instance = ScriptableObject.CreateInstance<T>();
      assertionComponent.Action = (ActionBase) instance;
      assertionComponent.Action.go = gameObject;
      assertionComponent.Action.thisPropertyPath = propertyPath;
      configurator = (IAssertionComponentConfigurator) assertionComponent;
      StackTrace stackTrace = new StackTrace(true);
      string fileName = stackTrace.GetFrame(0).GetFileName();
      for (int index = 1; index < stackTrace.FrameCount; ++index)
      {
        StackFrame frame = stackTrace.GetFrame(index);
        if (frame.GetFileName() != fileName)
        {
          string str = frame.GetFileName().Substring(Application.dataPath.Length - "Assets".Length);
          assertionComponent.m_CreatedInFilePath = str;
          assertionComponent.m_CreatedInFileLine = frame.GetFileLineNumber();
          break;
        }
      }
      return instance;
    }

    public int UpdateCheckStartOnFrame
    {
      set => this.checkAfterFrames = value;
    }

    public int UpdateCheckRepeatFrequency
    {
      set => this.repeatEveryFrame = value;
    }

    public bool UpdateCheckRepeat
    {
      set => this.repeatCheckFrame = value;
    }

    public float TimeCheckStartAfter
    {
      set => this.checkAfterTime = value;
    }

    public float TimeCheckRepeatFrequency
    {
      set => this.repeatEveryTime = value;
    }

    public bool TimeCheckRepeat
    {
      set => this.repeatCheckTime = value;
    }

    public AssertionComponent Component => this;
  }
}
