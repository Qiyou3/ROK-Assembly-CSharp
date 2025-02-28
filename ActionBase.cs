// Decompiled with JetBrains decompiler
// Type: UnityTest.ActionBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace UnityTest
{
  public abstract class ActionBase : ScriptableObject
  {
    public GameObject go;
    protected object m_ObjVal;
    private MemberResolver m_MemberResolver;
    public string thisPropertyPath = string.Empty;

    public virtual System.Type[] GetAccepatbleTypesForA() => (System.Type[]) null;

    public virtual int GetDepthOfSearch() => 2;

    public virtual string[] GetExcludedFieldNames() => new string[0];

    public bool Compare()
    {
      if (this.m_MemberResolver == null)
        this.m_MemberResolver = new MemberResolver(this.go, this.thisPropertyPath);
      this.m_ObjVal = this.m_MemberResolver.GetValue(this.UseCache);
      return this.Compare(this.m_ObjVal);
    }

    protected abstract bool Compare(object objVal);

    protected virtual bool UseCache => false;

    public virtual System.Type GetParameterType() => typeof (object);

    public virtual string GetConfigurationDescription()
    {
      string configurationDescription = string.Empty;
      foreach (FieldInfo fieldInfo in ((IEnumerable<FieldInfo>) this.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)).Where<FieldInfo>((Func<FieldInfo, bool>) (info => info.FieldType.IsSerializable)))
      {
        object obj = fieldInfo.GetValue((object) this);
        if (obj is double num1)
          obj = (object) num1.ToString("0.########");
        if (obj is float num2)
          obj = (object) num2.ToString("0.########");
        configurationDescription = configurationDescription + obj + " ";
      }
      return configurationDescription;
    }

    private IEnumerable<FieldInfo> GetFields(System.Type type)
    {
      return (IEnumerable<FieldInfo>) type.GetFields(BindingFlags.Instance | BindingFlags.Public);
    }

    public ActionBase CreateCopy(GameObject oldGameObject, GameObject newGameObject)
    {
      ActionBase instance = ScriptableObject.CreateInstance(this.GetType()) as ActionBase;
      foreach (FieldInfo field in this.GetFields(this.GetType()))
      {
        object obj = field.GetValue((object) this);
        if (obj is GameObject && (UnityEngine.Object) (obj as GameObject) == (UnityEngine.Object) oldGameObject)
          obj = (object) newGameObject;
        field.SetValue((object) instance, obj);
      }
      return instance;
    }

    public virtual void Fail(AssertionComponent assertion)
    {
      Debug.LogException((Exception) new AssertionException(assertion), assertion.GetFailureReferenceObject());
    }

    public virtual string GetFailureMessage()
    {
      return this.GetType().Name + " assertion failed.\n(" + (object) this.go + ")." + this.thisPropertyPath + " failed. Value: " + this.m_ObjVal;
    }
  }
}
