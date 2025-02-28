// Decompiled with JetBrains decompiler
// Type: UnityTest.MemberResolver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

#nullable disable
namespace UnityTest
{
  public class MemberResolver
  {
    private object m_CallingObjectRef;
    private MemberInfo[] m_Callstack;
    private readonly GameObject m_GameObject;
    private readonly string m_Path;

    public MemberResolver(GameObject gameObject, string path)
    {
      path = path.Trim();
      this.ValidatePath(path);
      this.m_GameObject = gameObject;
      this.m_Path = path.Trim();
    }

    public object GetValue(bool useCache)
    {
      if (useCache && this.m_CallingObjectRef != null)
      {
        object obj = this.m_CallingObjectRef;
        for (int index = 0; index < this.m_Callstack.Length; ++index)
          obj = this.GetValueFromMember(obj, this.m_Callstack[index]);
        return obj;
      }
      object obj1 = this.GetBaseObject();
      MemberInfo[] callstack = this.GetCallstack();
      this.m_CallingObjectRef = obj1;
      List<MemberInfo> memberInfoList = new List<MemberInfo>();
      for (int index = 0; index < callstack.Length; ++index)
      {
        MemberInfo memberInfo = callstack[index];
        obj1 = this.GetValueFromMember(obj1, memberInfo);
        memberInfoList.Add(memberInfo);
        if (obj1 == null)
          return (object) null;
        System.Type type = obj1.GetType();
        if (!MemberResolver.IsValueType(type) && type != typeof (string))
        {
          memberInfoList.Clear();
          this.m_CallingObjectRef = obj1;
        }
      }
      this.m_Callstack = memberInfoList.ToArray();
      return obj1;
    }

    public System.Type GetMemberType()
    {
      MemberInfo[] callstack = this.GetCallstack();
      if (callstack.Length == 0)
        return this.GetBaseObject().GetType();
      MemberInfo memberInfo = callstack[callstack.Length - 1];
      switch (memberInfo)
      {
        case FieldInfo _:
          return (memberInfo as FieldInfo).FieldType;
        case MethodInfo _:
          return (memberInfo as MethodInfo).ReturnType;
        default:
          return (System.Type) null;
      }
    }

    public static bool TryGetMemberType(GameObject gameObject, string path, out System.Type value)
    {
      try
      {
        MemberResolver memberResolver = new MemberResolver(gameObject, path);
        value = memberResolver.GetMemberType();
        return true;
      }
      catch (InvalidPathException ex)
      {
        value = (System.Type) null;
        return false;
      }
    }

    public static bool TryGetValue(GameObject gameObject, string path, out object value)
    {
      try
      {
        MemberResolver memberResolver = new MemberResolver(gameObject, path);
        value = memberResolver.GetValue(false);
        return true;
      }
      catch (InvalidPathException ex)
      {
        value = (object) null;
        return false;
      }
    }

    private object GetValueFromMember(object obj, MemberInfo memberInfo)
    {
      switch (memberInfo)
      {
        case FieldInfo _:
          return (memberInfo as FieldInfo).GetValue(obj);
        case MethodInfo _:
          return (memberInfo as MethodInfo).Invoke(obj, (object[]) null);
        default:
          throw new InvalidPathException(memberInfo.Name);
      }
    }

    private object GetBaseObject()
    {
      if (string.IsNullOrEmpty(this.m_Path))
        return (object) this.m_GameObject;
      Component component = this.m_GameObject.GetComponent(this.m_Path.Split('.')[0]);
      return (UnityEngine.Object) component != (UnityEngine.Object) null ? (object) component : (object) this.m_GameObject;
    }

    private MemberInfo[] GetCallstack()
    {
      if (this.m_Path == string.Empty)
        return new MemberInfo[0];
      Queue<string> stringQueue = new Queue<string>((IEnumerable<string>) this.m_Path.Split('.'));
      System.Type type = this.GetBaseObject().GetType();
      if (type != typeof (GameObject))
        stringQueue.Dequeue();
      List<MemberInfo> memberInfoList = new List<MemberInfo>();
      while (stringQueue.Count != 0)
      {
        string str = stringQueue.Dequeue();
        FieldInfo field = MemberResolver.GetField(type, str);
        if (field != null)
        {
          type = field.FieldType;
          memberInfoList.Add((MemberInfo) field);
        }
        else
        {
          PropertyInfo property = MemberResolver.GetProperty(type, str);
          type = property != null ? property.PropertyType : throw new InvalidPathException(str);
          MethodInfo getMethod = MemberResolver.GetGetMethod(property);
          memberInfoList.Add((MemberInfo) getMethod);
        }
      }
      return memberInfoList.ToArray();
    }

    private void ValidatePath(string path)
    {
      bool flag = false;
      if (path.StartsWith(".") || path.EndsWith("."))
        flag = true;
      if (path.IndexOf("..") >= 0)
        flag = true;
      if (Regex.IsMatch(path, "\\s"))
        flag = true;
      if (flag)
        throw new InvalidPathException(path);
    }

    private static bool IsValueType(System.Type type) => type.IsValueType;

    private static FieldInfo GetField(System.Type type, string fieldName)
    {
      return type.GetField(fieldName);
    }

    private static PropertyInfo GetProperty(System.Type type, string propertyName)
    {
      return type.GetProperty(propertyName);
    }

    private static MethodInfo GetGetMethod(PropertyInfo propertyInfo)
    {
      return propertyInfo.GetGetMethod();
    }
  }
}
