// Decompiled with JetBrains decompiler
// Type: AsyncLoaderList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Core;
using CodeHatch.Engine.Core.Gaming;
using CodeHatch.Engine.Loading;
using CodeHatch.Engine.Networking;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AsyncLoaderList : MonoBehaviour
{
  public List<AsyncResourceLoader> BaseResources;
  public List<AsyncResourceLoader> DedicatedResources;
  public List<AsyncResourceLoader> LocalPlayerResources;

  private List<AsyncResourceLoader> ResourcesToLoad
  {
    get
    {
      List<AsyncResourceLoader> resourcesToLoad = new List<AsyncResourceLoader>();
      resourcesToLoad.AddRange((IEnumerable<AsyncResourceLoader>) this.BaseResources);
      if (!Player.IsLocalDedi || !Player.IsLocalServer)
        resourcesToLoad.AddRange((IEnumerable<AsyncResourceLoader>) this.LocalPlayerResources);
      if (Player.IsLocalServer)
        resourcesToLoad.AddRange((IEnumerable<AsyncResourceLoader>) this.DedicatedResources);
      return resourcesToLoad;
    }
  }

  public void Awake()
  {
    if (LevelLoader.Abort)
      return;
    foreach (AsyncResourceLoader asyncResourceLoader in this.ResourcesToLoad)
    {
      if ((Object) asyncResourceLoader.Parent == (Object) null)
        asyncResourceLoader.Parent = this.transform;
      Game.RegisterLoader((IProgressLoader) asyncResourceLoader);
    }
  }

  [ContextMenu("Instantiate Base")]
  public void InstantiateBase()
  {
    for (int index = 0; index < this.BaseResources.Count; ++index)
    {
      AsyncResourceLoader baseResource = this.BaseResources[index];
      GameObject original = Resources.Load<GameObject>(baseResource.ResourceString);
      if ((Object) original != (Object) null)
        Object.Instantiate<GameObject>(original);
      else
        this.LogError<AsyncLoaderList>("Could not find resource with string of '{0}'.", (object) baseResource.ResourceString);
    }
  }

  [ContextMenu("Instantiate Base and Dedicated")]
  public void InstantiateBaseAndDedicated()
  {
    for (int index = 0; index < this.BaseResources.Count; ++index)
    {
      AsyncResourceLoader baseResource = this.BaseResources[index];
      GameObject original = Resources.Load<GameObject>(baseResource.ResourceString);
      if ((Object) original != (Object) null)
        Object.Instantiate<GameObject>(original);
      else
        this.LogError<AsyncLoaderList>("Could not find resource with string of '{0}'.", (object) baseResource.ResourceString);
    }
    for (int index = 0; index < this.DedicatedResources.Count; ++index)
    {
      AsyncResourceLoader dedicatedResource = this.DedicatedResources[index];
      GameObject original = Resources.Load<GameObject>(dedicatedResource.ResourceString);
      if ((Object) original != (Object) null)
        Object.Instantiate<GameObject>(original);
      else
        this.LogError<AsyncLoaderList>("Could not find resource with string of '{0}'.", (object) dedicatedResource.ResourceString);
    }
  }

  [ContextMenu("Instantiate Base and Local Player")]
  public void InstantiateBaseAndLocal()
  {
    for (int index = 0; index < this.BaseResources.Count; ++index)
    {
      AsyncResourceLoader baseResource = this.BaseResources[index];
      GameObject original = Resources.Load<GameObject>(baseResource.ResourceString);
      if ((Object) original != (Object) null)
        Object.Instantiate<GameObject>(original);
      else
        this.LogError<AsyncLoaderList>("Could not find resource with string of '{0}'.", (object) baseResource.ResourceString);
    }
    for (int index = 0; index < this.LocalPlayerResources.Count; ++index)
    {
      AsyncResourceLoader localPlayerResource = this.LocalPlayerResources[index];
      GameObject original = Resources.Load<GameObject>(localPlayerResource.ResourceString);
      if ((Object) original != (Object) null)
        Object.Instantiate<GameObject>(original);
      else
        this.LogError<AsyncLoaderList>("Could not find resource with string of '{0}'.", (object) localPlayerResource.ResourceString);
    }
  }
}
