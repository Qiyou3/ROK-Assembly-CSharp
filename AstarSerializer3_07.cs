// Decompiled with JetBrains decompiler
// Type: AstarSerializer3_07
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using Pathfinding;
using System.IO;
using UnityEngine;

#nullable disable
public class AstarSerializer3_07(AstarPath script) : AstarSerializer(script)
{
  public override void AddUnityReferenceValue(string key, UnityEngine.Object value)
  {
    BinaryWriter writerStream = this.writerStream;
    this.AddVariableAnchor(key);
    if (value == (UnityEngine.Object) null)
    {
      writerStream.Write((byte) 0);
    }
    else
    {
      writerStream.Write((byte) 1);
      if (value == (UnityEngine.Object) this.active.gameObject)
        writerStream.Write((int) sbyte.MinValue);
      else if (value == (UnityEngine.Object) this.active.transform)
        writerStream.Write(-129);
      else
        writerStream.Write(value.GetInstanceID());
      writerStream.Write(value.name);
      Component component = value as Component;
      GameObject gameObject = value as GameObject;
      if ((UnityEngine.Object) component == (UnityEngine.Object) null && (UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      {
        writerStream.Write(string.Empty);
      }
      else
      {
        if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
          gameObject = component.gameObject;
        string str = gameObject.name;
        while ((UnityEngine.Object) gameObject.transform.parent != (UnityEngine.Object) null)
        {
          gameObject = gameObject.transform.parent.gameObject;
          str = gameObject.name + "/" + str;
        }
        writerStream.Write(str);
      }
      if (AstarSerializer.writeUnityReference_Editor != null)
      {
        writerStream.Write(true);
        AstarSerializer.writeUnityReference_Editor((AstarSerializer) this, value);
      }
      else
        writerStream.Write(false);
    }
  }

  public override UnityEngine.Object GetUnityReferenceValue(
    string key,
    System.Type type,
    UnityEngine.Object defaultValue = null)
  {
    if (!this.MoveToVariableAnchor(key))
    {
      this.LogInfo<AstarSerializer3_07>("Couldn't find key '{0}' in the data, returning default", (object) key);
      return (!(defaultValue == (UnityEngine.Object) null) ? (object) defaultValue : this.GetDefaultValue(type)) as UnityEngine.Object;
    }
    BinaryReader readerStream = this.readerStream;
    switch (readerStream.ReadByte())
    {
      case 0:
        return (!(defaultValue == (UnityEngine.Object) null) ? (object) defaultValue : this.GetDefaultValue(type)) as UnityEngine.Object;
      case 2:
        this.LogInfo<AstarSerializer3_07>("The variable '{0}' was not serialized correctly and can therefore not be deserialized", (object) key);
        return (!(defaultValue == (UnityEngine.Object) null) ? (object) defaultValue : this.GetDefaultValue(type)) as UnityEngine.Object;
      default:
        int instanceID = readerStream.ReadInt32();
        string str = readerStream.ReadString();
        if (instanceID == (int) sbyte.MinValue)
          return (UnityEngine.Object) this.active.gameObject;
        if (instanceID == -129)
          return (UnityEngine.Object) this.active.transform;
        string name = readerStream.ReadString();
        UnityEngine.Object unityReferenceValue1 = (UnityEngine.Object) null;
        if (name != string.Empty)
        {
          GameObject unityReferenceValue2 = GameObject.Find(name);
          if ((UnityEngine.Object) unityReferenceValue2 != (UnityEngine.Object) null)
          {
            if (type == typeof (GameObject))
              return (UnityEngine.Object) unityReferenceValue2;
            unityReferenceValue1 = (UnityEngine.Object) unityReferenceValue2.GetComponent(type);
            if (unityReferenceValue1 != (UnityEngine.Object) null && unityReferenceValue1.name == str)
              return unityReferenceValue1;
          }
        }
        bool flag = readerStream.ReadBoolean();
        if (AstarSerializer.readUnityReference_Editor != null && flag)
        {
          UnityEngine.Object @object = AstarSerializer.readUnityReference_Editor((AstarSerializer) this, str, instanceID, type);
          return @object != (UnityEngine.Object) null && @object.name == str || !(unityReferenceValue1 != (UnityEngine.Object) null) ? @object : unityReferenceValue1;
        }
        if (unityReferenceValue1 != (UnityEngine.Object) null)
          return unityReferenceValue1;
        UnityEngine.Object[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(type);
        UnityEngine.Object object1 = (UnityEngine.Object) null;
        for (int index = 0; index < objectsOfTypeAll.Length; ++index)
        {
          if (objectsOfTypeAll[index].GetInstanceID() == instanceID)
          {
            object1 = objectsOfTypeAll[index];
            break;
          }
        }
        return object1 != (UnityEngine.Object) null ? object1 : Resources.Load(str);
    }
  }
}
