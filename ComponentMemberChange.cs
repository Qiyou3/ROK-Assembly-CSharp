// Decompiled with JetBrains decompiler
// Type: ComponentMemberChange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using System;
using System.Reflection;
using UnityEngine;

#nullable disable
public class ComponentMemberChange : MonoBehaviour
{
  public static bool debug;
  public ComponentMemberChange.Toggle[] toggles;

  public void Update()
  {
    foreach (ComponentMemberChange.Toggle toggle in this.toggles)
      toggle.Update();
  }

  [Serializable]
  public class Toggle
  {
    public string name;
    public ComponentMemberChange.Toggle.Key[] keys;
    public ComponentMemberChange.Toggle.Modification[] modifications;

    public void Update()
    {
      bool flag = false;
      foreach (ComponentMemberChange.Toggle.Key key in this.keys)
      {
        flag = key.IsTriggered();
        if (flag)
          break;
      }
      if (!flag)
        return;
      if (ComponentMemberChange.debug)
        this.LogInfo<ComponentMemberChange.Toggle>("Toggle {0} was triggered.", (object) this.name);
      foreach (ComponentMemberChange.Toggle.Modification modification in this.modifications)
        modification.Apply();
    }

    [Serializable]
    public class Key
    {
      public KeyCode code;
      public InputUtil.KeyState state = InputUtil.KeyState.Down;

      public bool IsTriggered() => this.code.GetKeyState(this.state);
    }

    [Serializable]
    public class Modification
    {
      public string name;
      public GameObject[] gameObjects;
      public int componentIndexIfMultiple;
      private UnityEngine.Object unityObject;
      public ComponentMemberChange.Toggle.Modification.Member[] members;

      public void Apply()
      {
        foreach (GameObject gameObject in this.gameObjects)
          this.ApplyToGameObject(gameObject);
      }

      public void ApplyToGameObject(GameObject gameObject)
      {
        this.unityObject = (UnityEngine.Object) null;
        if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
        {
          if (!ComponentMemberChange.debug)
            return;
          this.LogInfo<ComponentMemberChange.Toggle.Modification>("gameObject == null && component == null");
        }
        else
        {
          System.Type type = System.Type.GetType(this.name);
          if (type == null)
          {
            if (ComponentMemberChange.debug)
              this.LogWarning<ComponentMemberChange.Toggle.Modification>("Type {0} was not found.", (object) this.name);
            type = System.Type.GetType("UnityEngine." + this.name);
          }
          if (type == null)
          {
            if (ComponentMemberChange.debug)
              this.LogWarning<ComponentMemberChange.Toggle.Modification>("Type UnityEngine.{0} was not found.", (object) this.name);
            type = System.Type.GetType("System." + this.name);
          }
          if (type == null)
          {
            if (ComponentMemberChange.debug)
              this.LogWarning<ComponentMemberChange.Toggle.Modification>("Type System.{0} was not found; assuming gameObject.", (object) this.name);
            this.unityObject = (UnityEngine.Object) gameObject;
          }
          else
            this.unityObject = type == typeof (GameObject) ? (UnityEngine.Object) gameObject : (UnityEngine.Object) gameObject.GetComponents(type)[this.componentIndexIfMultiple];
          if (ComponentMemberChange.debug)
            this.LogInfo<ComponentMemberChange.Toggle.Modification>("{0}.{1}", (object) gameObject, (object) this.name);
          foreach (ComponentMemberChange.Toggle.Modification.Member member in this.members)
            this.AssignMember(gameObject, member);
        }
      }

      public void AssignMember(
        GameObject gameObject,
        ComponentMemberChange.Toggle.Modification.Member member)
      {
        try
        {
          if (member.info == null)
          {
            MemberInfo[] member1 = this.unityObject.GetType().GetMember(member.name);
            if (member1.Length > 0)
              member.info = member1[0];
            if (member1.Length != 1)
              this.LogError<ComponentMemberChange.Toggle.Modification>("{0}.{1} has {2} members named {3}", (object) gameObject, (object) this.name, (object) member1.Length, (object) member.name);
            if (member.info == null)
            {
              if (!ComponentMemberChange.debug)
                return;
              this.LogError<ComponentMemberChange.Toggle.Modification>("{0}.{1} does not have a member named {2}", (object) gameObject, (object) this.name, (object) member.name);
              return;
            }
          }
          if (member.info is FieldInfo info2)
          {
            System.Type fieldType = info2.FieldType;
            object valueToAssign = member.assignment.GetValueToAssign(fieldType);
            info2.SetValue((object) this.unityObject, valueToAssign);
            if (valueToAssign == null)
            {
              if (!ComponentMemberChange.debug)
                return;
              this.LogWarning<ComponentMemberChange.Toggle.Modification>(gameObject.ToString() + "." + (object) this.unityObject + "(" + this.name + ")." + member.name + " (type of " + (object) fieldType + ") = null");
            }
            else
            {
              if (!ComponentMemberChange.debug)
                return;
              this.LogInfo<ComponentMemberChange.Toggle.Modification>(gameObject.ToString() + "." + (object) this.unityObject + "(" + this.name + ")." + member.name + " (type of " + (object) fieldType + ") = " + valueToAssign);
            }
          }
          else if (member.info is PropertyInfo info1)
          {
            if (info1.CanWrite)
            {
              System.Type propertyType = info1.PropertyType;
              object valueToAssign = member.assignment.GetValueToAssign(propertyType);
              info1.SetValue((object) this.unityObject, valueToAssign, (object[]) null);
              if (valueToAssign == null)
              {
                if (!ComponentMemberChange.debug)
                  return;
                this.LogWarning<ComponentMemberChange.Toggle.Modification>(gameObject.ToString() + "." + (object) this.unityObject + "(" + this.name + ")." + member.name + " (type of " + (object) propertyType + ") = null");
              }
              else
              {
                if (!ComponentMemberChange.debug)
                  return;
                this.LogInfo<ComponentMemberChange.Toggle.Modification>(gameObject.ToString() + "." + (object) this.unityObject + "(" + this.name + ")." + member.name + " (type of " + (object) propertyType + ") = " + valueToAssign);
              }
            }
            else
            {
              if (!ComponentMemberChange.debug)
                return;
              this.LogError<ComponentMemberChange.Toggle.Modification>("{0}.{1}.{2} was found, but it is read-only.", (object) gameObject, (object) this.name, (object) member.name);
            }
          }
          else
          {
            if (!ComponentMemberChange.debug)
              return;
            this.LogError<ComponentMemberChange.Toggle.Modification>("{0}.{1} does not have a field or property named {2}", (object) gameObject, (object) this.name, (object) member.name);
          }
        }
        catch (Exception ex)
        {
          this.LogError<ComponentMemberChange.Toggle.Modification>(ex.Message + ex.StackTrace + ex.Source + ex.HelpLink);
        }
      }

      [Serializable]
      public class Member
      {
        public string name;
        public MemberInfo info;
        public ComponentMemberChange.Toggle.Modification.Member.Assignment assignment;

        [Serializable]
        public class Assignment
        {
          public UnityEngine.Object objectToAssign;
          public float floatToAssign;
          public int intToAssign;
          public bool boolToAssign;
          public string stringToAssign;
          public object otherToAssign;

          public object GetValueToAssign(System.Type type)
          {
            if (type == typeof (UnityEngine.Object))
              return (object) this.objectToAssign;
            if (type == typeof (float))
              return (object) this.floatToAssign;
            if (type == typeof (int))
              return (object) this.intToAssign;
            if (type == typeof (bool))
              return (object) this.boolToAssign;
            return type == typeof (string) ? (object) this.stringToAssign : this.otherToAssign;
          }
        }
      }
    }
  }
}
