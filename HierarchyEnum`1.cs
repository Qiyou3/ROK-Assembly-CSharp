// Decompiled with JetBrains decompiler
// Type: HierarchyEnum`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class HierarchyEnum<T> : IEnumerator where T : Component
{
  public const int recursionLimit = 100;
  public const int searchLimit = 100;
  public Hierarchy<T> hierarchy;
  private List<HierarchyEnum<T>> childEnums;
  private int position = -1;
  private int childIndex;
  public T current;
  private int recursion;

  public HierarchyEnum(Hierarchy<T> _hierarchy, int _recursion)
  {
    this.hierarchy = _hierarchy;
    this.recursion = _recursion;
    this.childEnums = new List<HierarchyEnum<T>>();
  }

  public bool MoveNext()
  {
    if (this.hierarchy.includeMyComponents)
    {
      ++this.position;
      if (this.position < this.hierarchy.myComponents.Count)
      {
        this.current = this.hierarchy.myComponents[this.position];
        return true;
      }
    }
    if (this.recursion >= 100)
      throw new InvalidOperationException("Recursion limit of " + (object) 100 + " exceeded.");
    if (this.hierarchy.includeMyComponents)
      this.position = this.hierarchy.myComponents.Count;
    for (int index = 0; index < 100; ++index)
    {
      if (this.childIndex == this.hierarchy.childHierarchies.Count)
      {
        this.current = (T) null;
        return false;
      }
      if (this.childIndex > this.hierarchy.childHierarchies.Count)
        throw new InvalidOperationException("Unexpected childIndex increment.");
      if (this.childEnums.Count == this.childIndex)
        this.childEnums.Add(new HierarchyEnum<T>(this.hierarchy.childHierarchies[this.childIndex], this.recursion + 1));
      else if (this.childEnums.Count < this.childIndex)
        throw new InvalidOperationException("Unexpected childEnum list size and/or unexpected childIndex increment.");
      if (this.childEnums[this.childIndex].MoveNext())
      {
        this.current = this.childEnums[this.childIndex].current;
        return true;
      }
      ++this.childIndex;
    }
    throw new InvalidOperationException("Search limit of " + (object) 100 + " exceeded.");
  }

  public void Reset()
  {
    this.position = -1;
    this.childIndex = 0;
  }

  public object Current => (object) this.current;
}
