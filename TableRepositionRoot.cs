// Decompiled with JetBrains decompiler
// Type: ThirdParty.Extensions.NGUI.TableRepositionRoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ThirdParty.Extensions.NGUI
{
  public class TableRepositionRoot : MonoBehaviour
  {
    private List<TableRepositionTree> _trees = new List<TableRepositionTree>();

    public void OnDisable() => this._trees.Clear();

    public void LateUpdate()
    {
      if (this._trees.Count <= 0)
        return;
      for (int index = this._trees.Count - 1; index >= 0; --index)
      {
        if ((Object) this._trees[index] != (Object) null)
          this._trees[index].Table.Reposition();
        this._trees.RemoveAt(index);
      }
    }

    public void Enqueue(TableRepositionTree tree)
    {
      if ((Object) tree == (Object) null || this._trees.Contains(tree))
        return;
      TableRepositionTree parent = tree.Parent;
      if ((Object) parent != (Object) null)
        this.Enqueue(parent);
      tree.Depth = 0;
      for (; (Object) parent != (Object) null; parent = parent.Parent)
        ++tree.Depth;
      bool flag = false;
      int count = this._trees.Count;
      int index = count - 1;
      while (index >= 0)
      {
        if (this._trees[index].Depth <= tree.Depth)
        {
          this._trees.Insert(count, tree);
          flag = true;
          break;
        }
        --index;
        --count;
      }
      if (flag)
        return;
      this._trees.Insert(0, tree);
    }

    public void Dequeue(TableRepositionTree tree)
    {
      if (!((Object) tree != (Object) null))
        return;
      this._trees.Remove(tree);
    }
  }
}
