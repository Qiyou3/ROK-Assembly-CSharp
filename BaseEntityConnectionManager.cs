// Decompiled with JetBrains decompiler
// Type: BaseEntityConnectionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

#nullable disable
public abstract class BaseEntityConnectionManager : EntityBehaviour
{
  private readonly List<BaseEntityConnectionManager> _directConnected = new List<BaseEntityConnectionManager>();
  private List<BaseEntityConnectionManager> _allConnected = new List<BaseEntityConnectionManager>();

  protected static void ConnectManagers(
    BaseEntityConnectionManager a,
    BaseEntityConnectionManager b,
    bool connect)
  {
    if (connect)
    {
      a.AddConnection(b);
      b.AddConnection(a);
      BaseEntityConnectionManager.UpdateConnectionChunk(a);
    }
    else
    {
      a.RemoveConnection(b);
      b.RemoveConnection(a);
      BaseEntityConnectionManager.UpdateConnectionChunk(a);
      BaseEntityConnectionManager.UpdateConnectionChunk(b);
    }
  }

  protected internal void AddConnection(BaseEntityConnectionManager other)
  {
    if ((Object) other == (Object) this)
      return;
    this._directConnected.AddDistinct<BaseEntityConnectionManager>(other);
  }

  protected internal void RemoveConnection(BaseEntityConnectionManager other)
  {
    this._directConnected.Remove(other);
  }

  protected static void UpdateConnectionChunk(BaseEntityConnectionManager managerInChunk)
  {
    List<BaseEntityConnectionManager> list = managerInChunk.GetAllConnectedRecursive().ToList<BaseEntityConnectionManager>();
    object customObject = managerInChunk.GetCustomObject(list);
    foreach (BaseEntityConnectionManager connectionManager in list)
    {
      connectionManager.UpdateConnectionList(list);
      connectionManager.ApplyCustomObject(customObject);
    }
  }

  [DebuggerHidden]
  protected IEnumerable<BaseEntityConnectionManager> GetAllConnectedRecursive(
    BaseEntityConnectionManager previous = null)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    BaseEntityConnectionManager.\u003CGetAllConnectedRecursive\u003Ec__Iterator141 connectedRecursive = new BaseEntityConnectionManager.\u003CGetAllConnectedRecursive\u003Ec__Iterator141()
    {
      previous = previous,
      \u003C\u0024\u003Eprevious = previous,
      \u003C\u003Ef__this = this
    };
    // ISSUE: reference to a compiler-generated field
    connectedRecursive.\u0024PC = -2;
    return (IEnumerable<BaseEntityConnectionManager>) connectedRecursive;
  }

  protected void UpdateConnectionList(List<BaseEntityConnectionManager> newList)
  {
    List<BaseEntityConnectionManager> allConnected = this._allConnected;
    for (int index = 0; index < allConnected.Count; ++index)
    {
      BaseEntityConnectionManager connectionManager = allConnected[index];
      if (!newList.Contains(connectionManager) && (Object) connectionManager != (Object) this)
        this.SetConnected(connectionManager.Entity, false);
    }
    for (int index = 0; index < newList.Count; ++index)
    {
      BaseEntityConnectionManager connectionManager = newList[index];
      if (!allConnected.Contains(connectionManager) && (Object) connectionManager != (Object) this)
        this.SetConnected(connectionManager.Entity, true);
    }
    this._allConnected = newList;
    this.OnConnectionsUpdated();
  }

  protected virtual void SetConnected(Entity other, bool connected)
  {
  }

  protected virtual void OnConnectionsUpdated()
  {
  }

  protected virtual object GetCustomObject(List<BaseEntityConnectionManager> list) => (object) null;

  protected virtual void ApplyCustomObject(object customObject)
  {
  }
}
