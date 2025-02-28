// Decompiled with JetBrains decompiler
// Type: CodeHatch.Blocks.Interaction.Filters.BuildRangeFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Blocks.Geometry;
using UnityEngine;

#nullable disable
namespace CodeHatch.Blocks.Interaction.Filters
{
  public class BuildRangeFilter : RootFilter
  {
    public float DefaultBuildRange = 7f;

    private CubePositionResolver PositionResolver => this.Entity.Get<CubePositionResolver>();

    protected override CubeInteraction.State GetStateInternal(
      Vector3 worldPoint,
      RootCubeGrid target)
    {
      float buildRange = this.GetBuildRange();
      if ((double) Vector3.SqrMagnitude(this.PositionResolver.RayOrigin - worldPoint) <= (double) buildRange * (double) buildRange)
        return CubeInteraction.State.Valid;
      Debug.DrawLine(this.PositionResolver.RayOrigin, worldPoint, Color.red, 0.0f, false);
      return CubeInteraction.State.Hidden;
    }

    private float GetBuildRange()
    {
      if ((int) this.CubeProvider.MaterialID != (int) CubeInfo.Error.MaterialID && (int) this.CubeProvider.MaterialID != (int) CubeInfo.Air.MaterialID && (int) this.CubeProvider.PrefabID != (int) CubeInfo.Error.PrefabID && (int) this.CubeProvider.PrefabID != (int) CubeInfo.Air.PrefabID)
      {
        OctTileset tilesetWithId = TilesetLibrary.GetTilesetWithID(this.CubeProvider.MaterialID);
        if ((Object) tilesetWithId != (Object) null)
        {
          OctPrefab prefabWithId = tilesetWithId.GetPrefabWithID((int) this.CubeProvider.PrefabID);
          if ((Object) prefabWithId != (Object) null && !prefabWithId.UseDefaultBuildRange)
            return prefabWithId.BuildRange;
        }
      }
      return this.DefaultBuildRange;
    }
  }
}
