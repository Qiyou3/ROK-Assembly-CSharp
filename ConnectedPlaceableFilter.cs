// Decompiled with JetBrains decompiler
// Type: CodeHatch.Blocks.Interaction.Filters.ConnectedPlaceableFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Blocks.Collapsing;
using CodeHatch.Blocks.Geometry;
using CodeHatch.Common;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace CodeHatch.Blocks.Interaction.Filters
{
  public class ConnectedPlaceableFilter : RootFilter
  {
    protected override CubeInteraction.State GetStateInternal(
      Vector3 worldPoint,
      RootCubeGrid target)
    {
      List<Vector3Int> vector3IntList = new List<Vector3Int>()
      {
        target.WorldToLocalCoordinate(worldPoint)
      };
      TilesetColliderCube centralPrefabAtWorld = BlockManager.DefaultCubeGrid.TryGetCentralPrefabAtWorld(worldPoint);
      if ((Object) centralPrefabAtWorld != (Object) null)
      {
        OctPrefab prefabInfo = centralPrefabAtWorld.PrefabInfo;
        if ((Object) prefabInfo != (Object) null)
          vector3IntList = prefabInfo.GetBlockCoords();
      }
      for (int index = 0; index < vector3IntList.Count; ++index)
      {
        List<PlaceableBlockAssociation> blockAssociationList = PlaceableBlockAssociation.RetrieveAssociations(vector3IntList[index]);
        if (blockAssociationList != null && blockAssociationList.Count > 0)
        {
          this.LastFailedReason = "Remove attached object before deleting this block.";
          return CubeInteraction.State.Invalid;
        }
      }
      return CubeInteraction.State.Valid;
    }
  }
}
