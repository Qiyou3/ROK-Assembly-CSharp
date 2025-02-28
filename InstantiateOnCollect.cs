// Decompiled with JetBrains decompiler
// Type: InstantiateOnCollect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Networking;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Entities.Objects.Gadgets;
using UnityEngine;

#nullable disable
public class InstantiateOnCollect : EntityBehaviour
{
  public GameObject objectToInstantiate;

  public void Start()
  {
    EventManager.Subscribe<GadgetCollectEvent>(new EventSubscriber<GadgetCollectEvent>(this.OnCollect));
  }

  public void OnDestroy()
  {
    EventManager.Unsubscribe<GadgetCollectEvent>(new EventSubscriber<GadgetCollectEvent>(this.OnCollect));
  }

  public void OnCollect(GadgetCollectEvent e)
  {
    if (!((Object) e.Entity == (Object) this.Entity) || !Player.IsLocalServer || !(bool) (Object) this.objectToInstantiate)
      return;
    CustomNetworkInstantiate.Instantiate(this.objectToInstantiate, this.Entity.MainTransform.position, this.Entity.MainTransform.rotation);
  }
}
