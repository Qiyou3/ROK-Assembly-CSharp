// Decompiled with JetBrains decompiler
// Type: ThirdParty.Extensions.NGUI.TableRepositionLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ThirdParty.Extensions.NGUI
{
  [RequireComponent(typeof (UITable))]
  public class TableRepositionLink : MonoBehaviour
  {
    public UITable Table;
    public UITable Target;
    public bool AfterTarget = true;
    public bool DelayRequests;
    private static List<TableRepositionLink> _links = new List<TableRepositionLink>();
    private static bool _repositionLinks;

    public void Awake() => this.Attach();

    public void OnDestroy() => this.Detach();

    public void Update()
    {
      if (!TableRepositionLink._repositionLinks)
        return;
      TableRepositionLink._repositionLinks = false;
      for (int index = 0; index < TableRepositionLink._links.Count; ++index)
        TableRepositionLink._links[index].Reposition();
      TableRepositionLink._links.Clear();
    }

    public void OnValidate()
    {
      if ((Object) this.Table == (Object) null)
        this.Table = this.GetComponent<UITable>();
      if (!((Object) this.Target != (Object) null) || !((Object) this.Target == (Object) this.Table))
        return;
      this.Target = (UITable) null;
    }

    private void OnReposition()
    {
      UITable uiTable = !this.AfterTarget ? this.Target : this.Table;
      bool flag = false;
      for (int index = TableRepositionLink._links.Count - 1; index >= 0; --index)
      {
        if ((Object) TableRepositionLink._links[index] == (Object) this || (Object) uiTable == (!TableRepositionLink._links[index].AfterTarget ? (Object) TableRepositionLink._links[index].Target : (Object) TableRepositionLink._links[index].Table))
        {
          flag = true;
          break;
        }
      }
      if (flag)
        return;
      TableRepositionLink._links.Add(this);
      TableRepositionLink._repositionLinks = true;
    }

    public void Attach()
    {
      if (!((Object) this.Table != (Object) null) || !((Object) this.Target != (Object) null))
        return;
      if (this.DelayRequests)
      {
        if (this.AfterTarget)
          this.Target.onReposition += new UITable.OnReposition(this.OnReposition);
        else
          this.Table.onReposition += new UITable.OnReposition(this.OnReposition);
      }
      else if (this.AfterTarget)
        this.Target.onReposition += new UITable.OnReposition(this.Table.Reposition);
      else
        this.Table.onReposition += new UITable.OnReposition(this.Target.Reposition);
    }

    public void Detach()
    {
      if (!((Object) this.Table != (Object) null) || !((Object) this.Target != (Object) null))
        return;
      if (this.DelayRequests)
      {
        if (this.AfterTarget)
          this.Target.onReposition -= new UITable.OnReposition(this.OnReposition);
        else
          this.Table.onReposition -= new UITable.OnReposition(this.OnReposition);
      }
      else if (this.AfterTarget)
        this.Target.onReposition -= new UITable.OnReposition(this.Table.Reposition);
      else
        this.Table.onReposition += new UITable.OnReposition(this.Target.Reposition);
    }

    private void Reposition()
    {
      if (this.AfterTarget)
        this.Table.Reposition();
      else
        this.Target.Reposition();
    }
  }
}
