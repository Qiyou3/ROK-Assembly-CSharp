// Decompiled with JetBrains decompiler
// Type: EntityBuilder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Networking;
using System;
using uLink;
using UnityEngine;

#nullable disable
public class EntityBuilder : EntityBehaviour
{
  public GameObject[] baseObjects;
  public GameObject[] dedicatedObjects;
  public GameObject[] basePlayerObjects;
  public GameObject[] localPlayerObjects;
  public GameObject[] remotePlayerObjects;
  public bool UsePrefabTransform;

  public void Awake()
  {
    switch (this.GetNetworkedEntityType())
    {
      case EntityBuilder.NetworkedEntityType.Dedicated:
        this.InstantiateBaseAndDedi();
        break;
      case EntityBuilder.NetworkedEntityType.PlayerAndRemotelyOwned:
        this.InstantiateBasePlayerAndRemote();
        break;
      case EntityBuilder.NetworkedEntityType.PlayerAndLocallyOwned:
        this.InstantiateBasePlayerAndLocal();
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  [ContextMenu("Instantiate Base and Dedicated")]
  private void InstantiateBaseAndDedi()
  {
    this.AddObjects(this.baseObjects);
    this.AddObjects(this.dedicatedObjects);
  }

  [ContextMenu("Instantiate Base, BasePlayer and LocalPlayer")]
  private void InstantiateBasePlayerAndLocal()
  {
    this.AddObjects(this.baseObjects);
    this.AddObjects(this.basePlayerObjects);
    this.AddObjects(this.localPlayerObjects);
  }

  [ContextMenu("Instantiate Base, BasePlayer and RemotePlayer")]
  private void InstantiateBasePlayerAndRemote()
  {
    this.AddObjects(this.baseObjects);
    this.AddObjects(this.basePlayerObjects);
    this.AddObjects(this.remotePlayerObjects);
  }

  private EntityBuilder.NetworkedEntityType GetNetworkedEntityType()
  {
    if (uLink.Network.status != NetworkStatus.Connected)
      return EntityBuilder.NetworkedEntityType.PlayerAndLocallyOwned;
    if (Player.IsLocalDedi)
      return EntityBuilder.NetworkedEntityType.Dedicated;
    return this.Entity.IsLocallyOwned ? EntityBuilder.NetworkedEntityType.PlayerAndLocallyOwned : EntityBuilder.NetworkedEntityType.PlayerAndRemotelyOwned;
  }

  private void AddObjects(GameObject[] gameObjects)
  {
    if (gameObjects == null)
    {
      this.LogWarning<EntityBuilder>("gameObjects == null", (object) this);
    }
    else
    {
      Transform transform = this.transform;
      for (int index = 0; index < gameObjects.Length; ++index)
      {
        GameObject gameObject1 = gameObjects[index];
        if (!((UnityEngine.Object) gameObject1 == (UnityEngine.Object) null))
        {
          try
          {
            GameObject gameObject2 = (GameObject) UnityEngine.Object.Instantiate((UnityEngine.Object) gameObject1, this.transform.position, this.transform.rotation);
            gameObject2.transform.parent = transform;
            if (this.UsePrefabTransform)
            {
              gameObject2.transform.localPosition = gameObject1.transform.position;
              gameObject2.transform.localRotation = gameObject1.transform.rotation;
              gameObject2.transform.localScale = gameObject1.transform.localScale;
            }
          }
          catch (Exception ex)
          {
            this.LogException<EntityBuilder>(ex);
          }
        }
      }
    }
  }

  private enum NetworkedEntityType
  {
    Dedicated,
    PlayerAndRemotelyOwned,
    PlayerAndLocallyOwned,
  }
}
