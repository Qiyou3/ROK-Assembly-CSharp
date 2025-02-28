// Decompiled with JetBrains decompiler
// Type: Hierarchy`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[Serializable]
public class Hierarchy<T> : IEnumerable where T : Component
{
  public GameObject gameObject;
  public List<T> myComponents;
  public List<Hierarchy<T>> childHierarchies;
  public bool includeMyComponents;
  private int count;

  public Hierarchy(GameObject _gameObject, bool _includeMyComponents)
  {
    this.gameObject = _gameObject;
    this.includeMyComponents = _includeMyComponents;
    if (this.includeMyComponents)
    {
      this.myComponents = ((IEnumerable<T>) this.gameObject.GetComponents<T>()).ToList<T>();
      this.count = this.myComponents.Count;
    }
    else
      this.count = 0;
    GameObject[] array = ((IEnumerable<Transform>) this.gameObject.GetComponentsInChildren<Transform>()).Where<Transform>((Func<Transform, bool>) (t => (UnityEngine.Object) t.parent == (UnityEngine.Object) this.gameObject.transform)).Select<Transform, GameObject>((Func<Transform, GameObject>) (t => t.gameObject)).OrderBy<GameObject, string>((Func<GameObject, string>) (g => g.name)).ToArray<GameObject>();
    this.childHierarchies = new List<Hierarchy<T>>();
    foreach (GameObject _gameObject1 in array)
    {
      Hierarchy<T> hierarchy = new Hierarchy<T>(_gameObject1, true);
      if (hierarchy.Count > 0)
      {
        this.count += hierarchy.Count;
        this.childHierarchies.Add(hierarchy);
      }
    }
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  public int Count => this.count;

  public HierarchyEnum<T> GetEnumerator() => new HierarchyEnum<T>(this, 0);
}
