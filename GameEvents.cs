// Decompiled with JetBrains decompiler
// Type: GameEvents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Networking;
using uLink;
using UnityEngine;

#nullable disable
public class GameEvents
{
  public static void InstantiateProceduralWeaponEvent(
    GameObject prefab,
    Vector3 position,
    Quaternion rotation,
    int buildID)
  {
  }

  [RPC]
  public void InstantiateProceduralWeaponEventRPC(
    string prefab,
    Vector3 position,
    Quaternion rotation,
    int buildID)
  {
    if (!Player.IsLocalServer)
      return;
    uLink.Network.Instantiate<ProceduralWeaponNetworking.NetworkedBuild>(prefab, position, rotation, (NetworkGroup) 0, ProceduralWeaponNetworking.GetNetworkedBuild(buildID));
  }
}
