// Decompiled with JetBrains decompiler
// Type: AttachToComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
public class AttachToComponent : EntityBehaviour
{
  private static System.Type _compomentType = typeof (Component);
  private static Dictionary<string, System.Type> _componentTypes;
  public string name;
  public bool attachToCenterOfMass;
  public bool allowNull;
  private Transform _transformToFollow;

  private static Dictionary<string, System.Type> ComponentTypes
  {
    get
    {
      if (AttachToComponent._componentTypes == null)
      {
        AttachToComponent._componentTypes = new Dictionary<string, System.Type>();
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
          foreach (System.Type type in assembly.GetTypes())
          {
            if (!type.IsInterface && AttachToComponent._compomentType.IsAssignableFrom(type))
              AttachToComponent._componentTypes[type.Name] = type;
          }
        }
      }
      return AttachToComponent._componentTypes;
    }
  }

  [CanBeNull]
  private static System.Type GetComponentTypeFromName(string typeName)
  {
    System.Type componentTypeFromName;
    AttachToComponent.ComponentTypes.TryGetValue(typeName, out componentTypeFromName);
    return componentTypeFromName;
  }

  public void Start()
  {
    Component attachComponent = this.GetAttachComponent();
    if ((UnityEngine.Object) attachComponent == (UnityEngine.Object) null)
    {
      if (this.allowNull)
        return;
      this.LogError<AttachToComponent>("Could not find component named {0}.", (object) this.name);
      this.enabled = false;
    }
    else
    {
      this.transform.parent = attachComponent.transform;
      if (this.attachToCenterOfMass && (UnityEngine.Object) attachComponent.GetComponent<Rigidbody>() != (UnityEngine.Object) null)
        this.transform.localPosition = attachComponent.GetComponent<Rigidbody>().centerOfMass;
      else
        this.transform.localPosition = Vector3.zero;
      this.transform.localRotation = Quaternion.identity;
      this.transform.localScale = Vector3.one;
    }
  }

  private Component GetAttachComponent()
  {
    Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
    if ((UnityEngine.Object) entity == (UnityEngine.Object) null)
      return (Component) null;
    System.Type componentTypeFromName = AttachToComponent.GetComponentTypeFromName(this.name);
    return componentTypeFromName != null ? entity.gameObject.GetComponentInChildren(componentTypeFromName) : (Component) null;
  }
}
