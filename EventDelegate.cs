// Decompiled with JetBrains decompiler
// Type: EventDelegate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
[Serializable]
public class EventDelegate
{
  [SerializeField]
  private MonoBehaviour mTarget;
  [SerializeField]
  private string mMethodName;
  [SerializeField]
  private EventDelegate.Parameter[] mParameters;
  public bool oneShot;
  [NonSerialized]
  private EventDelegate.Callback mCachedCallback;
  [NonSerialized]
  private bool mRawDelegate;
  [NonSerialized]
  private bool mCached;
  [NonSerialized]
  private MethodInfo mMethod;
  [NonSerialized]
  private object[] mArgs;
  private static int s_Hash = nameof (EventDelegate).GetHashCode();

  public EventDelegate()
  {
  }

  public EventDelegate(EventDelegate.Callback call) => this.Set(call);

  public EventDelegate(MonoBehaviour target, string methodName) => this.Set(target, methodName);

  public MonoBehaviour target
  {
    get => this.mTarget;
    set
    {
      this.mTarget = value;
      this.mCachedCallback = (EventDelegate.Callback) null;
      this.mRawDelegate = false;
      this.mCached = false;
      this.mMethod = (MethodInfo) null;
      this.mParameters = (EventDelegate.Parameter[]) null;
    }
  }

  public string methodName
  {
    get => this.mMethodName;
    set
    {
      this.mMethodName = value;
      this.mCachedCallback = (EventDelegate.Callback) null;
      this.mRawDelegate = false;
      this.mCached = false;
      this.mMethod = (MethodInfo) null;
      this.mParameters = (EventDelegate.Parameter[]) null;
    }
  }

  public EventDelegate.Parameter[] parameters
  {
    get
    {
      if (!this.mCached)
        this.Cache();
      return this.mParameters;
    }
  }

  public bool isValid
  {
    get
    {
      if (!this.mCached)
        this.Cache();
      if (this.mRawDelegate && this.mCachedCallback != null)
        return true;
      return (UnityEngine.Object) this.mTarget != (UnityEngine.Object) null && !string.IsNullOrEmpty(this.mMethodName);
    }
  }

  public bool isEnabled
  {
    get
    {
      if (!this.mCached)
        this.Cache();
      if (this.mRawDelegate && this.mCachedCallback != null)
        return true;
      if ((UnityEngine.Object) this.mTarget == (UnityEngine.Object) null)
        return false;
      MonoBehaviour mTarget = this.mTarget;
      return (UnityEngine.Object) mTarget == (UnityEngine.Object) null || mTarget.enabled;
    }
  }

  private static string GetMethodName(EventDelegate.Callback callback) => callback.Method.Name;

  private static bool IsValid(EventDelegate.Callback callback)
  {
    return callback != null && callback.Method != null;
  }

  public override bool Equals(object obj)
  {
    switch (obj)
    {
      case null:
        return !this.isValid;
      case EventDelegate.Callback _:
        EventDelegate.Callback callback = obj as EventDelegate.Callback;
        if (callback.Equals((object) this.mCachedCallback))
          return true;
        return (UnityEngine.Object) this.mTarget == (UnityEngine.Object) (callback.Target as MonoBehaviour) && string.Equals(this.mMethodName, EventDelegate.GetMethodName(callback));
      case EventDelegate _:
        EventDelegate eventDelegate = obj as EventDelegate;
        return (UnityEngine.Object) this.mTarget == (UnityEngine.Object) eventDelegate.mTarget && string.Equals(this.mMethodName, eventDelegate.mMethodName);
      default:
        return false;
    }
  }

  public override int GetHashCode() => EventDelegate.s_Hash;

  private void Set(EventDelegate.Callback call)
  {
    this.Clear();
    if (call == null || !EventDelegate.IsValid(call))
      return;
    this.mTarget = call.Target as MonoBehaviour;
    if ((UnityEngine.Object) this.mTarget == (UnityEngine.Object) null)
    {
      this.mRawDelegate = true;
      this.mCachedCallback = call;
      this.mMethodName = (string) null;
    }
    else
    {
      this.mMethodName = EventDelegate.GetMethodName(call);
      this.mRawDelegate = false;
    }
  }

  public void Set(MonoBehaviour target, string methodName)
  {
    this.Clear();
    this.mTarget = target;
    this.mMethodName = methodName;
  }

  private void Cache()
  {
    this.mCached = true;
    if (this.mRawDelegate || this.mCachedCallback != null && !((UnityEngine.Object) (this.mCachedCallback.Target as MonoBehaviour) != (UnityEngine.Object) this.mTarget) && !(EventDelegate.GetMethodName(this.mCachedCallback) != this.mMethodName) || !((UnityEngine.Object) this.mTarget != (UnityEngine.Object) null) || string.IsNullOrEmpty(this.mMethodName))
      return;
    System.Type type = this.mTarget.GetType();
    this.mMethod = (MethodInfo) null;
    for (; type != null; type = type.BaseType)
    {
      try
      {
        this.mMethod = type.GetMethod(this.mMethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (this.mMethod != null)
          break;
      }
      catch (Exception ex)
      {
      }
    }
    if (this.mMethod == null)
      this.LogError<EventDelegate>("Could not find method '{0}' on {1}", (object) this.mMethodName, (object) this.mTarget.GetType(), (object) this.mTarget);
    else if (this.mMethod.ReturnType != typeof (void))
    {
      this.LogError<EventDelegate>(this.mTarget.GetType().ToString() + "." + this.mMethodName + " must have a 'void' return type.", (object) this.mTarget);
    }
    else
    {
      ParameterInfo[] parameters = this.mMethod.GetParameters();
      if (parameters.Length == 0)
      {
        this.mCachedCallback = (EventDelegate.Callback) Delegate.CreateDelegate(typeof (EventDelegate.Callback), (object) this.mTarget, this.mMethodName);
        this.mArgs = (object[]) null;
        this.mParameters = (EventDelegate.Parameter[]) null;
      }
      else
      {
        this.mCachedCallback = (EventDelegate.Callback) null;
        if (this.mParameters == null || this.mParameters.Length != parameters.Length)
        {
          this.mParameters = new EventDelegate.Parameter[parameters.Length];
          int index = 0;
          for (int length = this.mParameters.Length; index < length; ++index)
            this.mParameters[index] = new EventDelegate.Parameter();
        }
        int index1 = 0;
        for (int length = this.mParameters.Length; index1 < length; ++index1)
          this.mParameters[index1].expectedType = parameters[index1].ParameterType;
      }
    }
  }

  public bool Execute()
  {
    if (!this.mCached)
      this.Cache();
    if (this.mCachedCallback != null)
    {
      this.mCachedCallback();
      return true;
    }
    if (this.mMethod == null)
      return false;
    if ((this.mParameters == null ? 0 : this.mParameters.Length) == 0)
    {
      this.mMethod.Invoke((object) this.mTarget, (object[]) null);
    }
    else
    {
      if (this.mArgs == null || this.mArgs.Length != this.mParameters.Length)
        this.mArgs = new object[this.mParameters.Length];
      int index1 = 0;
      for (int length = this.mParameters.Length; index1 < length; ++index1)
        this.mArgs[index1] = this.mParameters[index1].value;
      try
      {
        this.mMethod.Invoke((object) this.mTarget, this.mArgs);
      }
      catch (ArgumentException ex)
      {
        string str1 = "Error calling ";
        string str2;
        if ((UnityEngine.Object) this.mTarget == (UnityEngine.Object) null)
          str2 = str1 + this.mMethod.Name;
        else
          str2 = str1 + (object) this.mTarget.GetType() + "." + this.mMethod.Name;
        string str3 = str2 + ": " + ex.Message + "\n  Expected: ";
        ParameterInfo[] parameters = this.mMethod.GetParameters();
        string str4;
        if (parameters.Length == 0)
        {
          str4 = str3 + "no arguments";
        }
        else
        {
          str4 = str3 + (object) parameters[0];
          for (int index2 = 1; index2 < parameters.Length; ++index2)
            str4 = str4 + ", " + (object) parameters[index2].ParameterType;
        }
        string str5 = str4 + "\n  Received: ";
        string str6;
        if (this.mParameters.Length == 0)
        {
          str6 = str5 + "no arguments";
        }
        else
        {
          str6 = str5 + (object) this.mParameters[0].type;
          for (int index3 = 1; index3 < this.mParameters.Length; ++index3)
            str6 = str6 + ", " + (object) this.mParameters[index3].type;
        }
        this.LogError<EventDelegate>(str6 + "\n");
      }
      int index4 = 0;
      for (int length = this.mArgs.Length; index4 < length; ++index4)
        this.mArgs[index4] = (object) null;
    }
    return true;
  }

  public void Clear()
  {
    this.mTarget = (MonoBehaviour) null;
    this.mMethodName = (string) null;
    this.mRawDelegate = false;
    this.mCachedCallback = (EventDelegate.Callback) null;
    this.mParameters = (EventDelegate.Parameter[]) null;
    this.mCached = false;
    this.mMethod = (MethodInfo) null;
    this.mArgs = (object[]) null;
  }

  public override string ToString()
  {
    if ((UnityEngine.Object) this.mTarget != (UnityEngine.Object) null)
    {
      string str = this.mTarget.GetType().ToString();
      int num = str.LastIndexOf('.');
      if (num > 0)
        str = str.Substring(num + 1);
      return !string.IsNullOrEmpty(this.methodName) ? str + "/" + this.methodName : str + "/[delegate]";
    }
    return this.mRawDelegate ? "[delegate]" : (string) null;
  }

  public static void Execute(List<EventDelegate> list)
  {
    if (list == null)
      return;
    int index = 0;
    while (index < list.Count)
    {
      EventDelegate eventDelegate = list[index];
      if (eventDelegate != null)
      {
        try
        {
          eventDelegate.Execute();
        }
        catch (Exception ex)
        {
          if (ex.InnerException != null)
            Logger.Error(ex.InnerException.Message);
          else
            Logger.Error(ex.Message);
        }
        if (index >= list.Count)
          break;
        if (list[index] == eventDelegate)
        {
          if (eventDelegate.oneShot)
          {
            list.RemoveAt(index);
            continue;
          }
        }
        else
          continue;
      }
      ++index;
    }
  }

  public static bool IsValid(List<EventDelegate> list)
  {
    if (list != null)
    {
      int index = 0;
      for (int count = list.Count; index < count; ++index)
      {
        EventDelegate eventDelegate = list[index];
        if (eventDelegate != null && eventDelegate.isValid)
          return true;
      }
    }
    return false;
  }

  public static EventDelegate Set(List<EventDelegate> list, EventDelegate.Callback callback)
  {
    if (list == null)
      return (EventDelegate) null;
    EventDelegate eventDelegate = new EventDelegate(callback);
    list.Clear();
    list.Add(eventDelegate);
    return eventDelegate;
  }

  public static void Set(List<EventDelegate> list, EventDelegate del)
  {
    if (list == null)
      return;
    list.Clear();
    list.Add(del);
  }

  public static EventDelegate Add(List<EventDelegate> list, EventDelegate.Callback callback)
  {
    return EventDelegate.Add(list, callback, false);
  }

  public static EventDelegate Add(
    List<EventDelegate> list,
    EventDelegate.Callback callback,
    bool oneShot)
  {
    if (list != null)
    {
      int index = 0;
      for (int count = list.Count; index < count; ++index)
      {
        EventDelegate eventDelegate = list[index];
        if (eventDelegate != null && eventDelegate.Equals((object) callback))
          return eventDelegate;
      }
      EventDelegate eventDelegate1 = new EventDelegate(callback);
      eventDelegate1.oneShot = oneShot;
      list.Add(eventDelegate1);
      return eventDelegate1;
    }
    Logger.Warning("Attempting to add a callback to a list that's null");
    return (EventDelegate) null;
  }

  public static void Add(List<EventDelegate> list, EventDelegate ev)
  {
    EventDelegate.Add(list, ev, ev.oneShot);
  }

  public static void Add(List<EventDelegate> list, EventDelegate ev, bool oneShot)
  {
    if (ev.mRawDelegate || (UnityEngine.Object) ev.target == (UnityEngine.Object) null || string.IsNullOrEmpty(ev.methodName))
      EventDelegate.Add(list, ev.mCachedCallback, oneShot);
    else if (list != null)
    {
      int index1 = 0;
      for (int count = list.Count; index1 < count; ++index1)
      {
        EventDelegate eventDelegate = list[index1];
        if (eventDelegate != null && eventDelegate.Equals((object) ev))
          return;
      }
      EventDelegate eventDelegate1 = new EventDelegate(ev.target, ev.methodName);
      eventDelegate1.oneShot = oneShot;
      if (ev.mParameters != null && ev.mParameters.Length > 0)
      {
        eventDelegate1.mParameters = new EventDelegate.Parameter[ev.mParameters.Length];
        for (int index2 = 0; index2 < ev.mParameters.Length; ++index2)
          eventDelegate1.mParameters[index2] = ev.mParameters[index2];
      }
      list.Add(eventDelegate1);
    }
    else
      Logger.Warning("Attempting to add a callback to a list that's null");
  }

  public static bool Remove(List<EventDelegate> list, EventDelegate.Callback callback)
  {
    if (list != null)
    {
      int index = 0;
      for (int count = list.Count; index < count; ++index)
      {
        EventDelegate eventDelegate = list[index];
        if (eventDelegate != null && eventDelegate.Equals((object) callback))
        {
          list.RemoveAt(index);
          return true;
        }
      }
    }
    return false;
  }

  public static bool Remove(List<EventDelegate> list, EventDelegate ev)
  {
    if (list != null)
    {
      int index = 0;
      for (int count = list.Count; index < count; ++index)
      {
        EventDelegate eventDelegate = list[index];
        if (eventDelegate != null && eventDelegate.Equals((object) ev))
        {
          list.RemoveAt(index);
          return true;
        }
      }
    }
    return false;
  }

  [Serializable]
  public class Parameter
  {
    public UnityEngine.Object obj;
    public string field;
    [NonSerialized]
    private object mValue;
    [NonSerialized]
    public System.Type expectedType = typeof (void);
    [NonSerialized]
    public bool cached;
    [NonSerialized]
    public PropertyInfo propInfo;
    [NonSerialized]
    public FieldInfo fieldInfo;

    public Parameter()
    {
    }

    public Parameter(UnityEngine.Object obj, string field)
    {
      this.obj = obj;
      this.field = field;
    }

    public Parameter(object val) => this.mValue = val;

    public object value
    {
      get
      {
        if (this.mValue != null)
          return this.mValue;
        if (!this.cached)
        {
          this.cached = true;
          this.fieldInfo = (FieldInfo) null;
          this.propInfo = (PropertyInfo) null;
          if (this.obj != (UnityEngine.Object) null && !string.IsNullOrEmpty(this.field))
          {
            System.Type type = this.obj.GetType();
            this.propInfo = type.GetProperty(this.field);
            if (this.propInfo == null)
              this.fieldInfo = type.GetField(this.field);
          }
        }
        if (this.propInfo != null)
          return this.propInfo.GetValue((object) this.obj, (object[]) null);
        if (this.fieldInfo != null)
          return this.fieldInfo.GetValue((object) this.obj);
        if (this.obj != (UnityEngine.Object) null)
          return (object) this.obj;
        return this.expectedType != null && this.expectedType.IsValueType ? (object) null : Convert.ChangeType((object) null, this.expectedType);
      }
      set => this.mValue = value;
    }

    public System.Type type
    {
      get
      {
        if (this.mValue != null)
          return this.mValue.GetType();
        return this.obj == (UnityEngine.Object) null ? typeof (void) : this.obj.GetType();
      }
    }
  }

  public delegate void Callback();
}
