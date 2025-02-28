// Decompiled with JetBrains decompiler
// Type: ThirdParty.Extensions.NGUI.TableRepositionTree
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ThirdParty.Extensions.NGUI
{
  [RequireComponent(typeof (UITable))]
  public class TableRepositionTree : MonoBehaviour
  {
    public UITable Table;
    public TableRepositionTree Parent;
    public TableRepositionRoot Root;

    public int Depth { get; set; }

    public void Awake() => this.OnValidate();

    public void OnDestroy()
    {
      if (!((Object) this.Root != (Object) null))
        return;
      this.Root.Dequeue(this);
    }

    public void OnValidate()
    {
      if ((Object) this.Table == (Object) null)
        this.Table = this.GetComponent<UITable>();
      if ((Object) this.Parent == (Object) this)
        this.Parent = (TableRepositionTree) null;
      if (!((Object) this.Root == (Object) null))
        return;
      this.Root = NGUITools.FindInParents<TableRepositionRoot>(this.gameObject);
    }

    public void Reposition()
    {
      if (!((Object) this.Root != (Object) null))
        return;
      this.Root.Enqueue(this);
    }
  }
}
