// Decompiled with JetBrains decompiler
// Type: CollectableResource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Core.Interaction.Attributes;
using CodeHatch.Engine.Core.Interaction.Behaviours;
using CodeHatch.Engine.Core.Interaction.Players;
using CodeHatch.Engine.Modules.Inventory.Resource;
using UnityEngine;

#nullable disable
public class CollectableResource : InteractableBehaviour
{
  public ResourceAmount[] resources;
  public GameObject destructionParticleEffect;
  public string destructionSoundEffect;
  public bool destroyOnCollect;

  public InvResources Resources
  {
    get => Entity.LocalPlayerExists ? Entity.LocalPlayer.Get<InvResources>() : (InvResources) null;
  }

  public ResourceHandler Handler
  {
    get
    {
      return Entity.LocalPlayerExists ? Entity.LocalPlayer.Get<ResourceHandler>() : (ResourceHandler) null;
    }
  }

  [Interact]
  [Tutorial("Collect Resource")]
  [Player(CodeHatch.Engine.Core.Interaction.Players.Key.Use, Gesture.Click, false)]
  public void Collect()
  {
    if (this.resources == null)
    {
      this.LogError<CollectableResource>("This collectable resource has no resources to collect");
    }
    else
    {
      for (int index = 0; index < this.resources.Length; ++index)
        this.Handler.AddPending(this.resources[index]);
      Vector3 center = this.gameObject.GetBoundsFromRenderers().center;
      if ((Object) this.destructionParticleEffect != (Object) null)
        Object.Instantiate((Object) this.destructionParticleEffect, center, Quaternion.LookRotation(Vector3.up));
      if (this.destructionSoundEffect != string.Empty)
        AudioController.Play(this.destructionSoundEffect, center, this.transform.parent);
      if (!this.destroyOnCollect)
        return;
      Object.Destroy((Object) this.gameObject);
    }
  }
}
