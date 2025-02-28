// Decompiled with JetBrains decompiler
// Type: CodeHatch.Blocks.Interaction.Filters.CrestPlaceBlockFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Translation;
using CodeHatch.Engine.Modules.SocialSystem;
using CodeHatch.Engine.Networking;
using CodeHatch.Thrones.SocialSystem;
using UnityEngine;

#nullable disable
namespace CodeHatch.Blocks.Interaction.Filters
{
  public class CrestPlaceBlockFilter : RootFilter
  {
    private const string MESSAGE_KEY = "Message.Block.CrestPlace";
    private const string DEFAULT = "You do not have clearance to place there.";

    protected override CubeInteraction.State GetStateInternal(
      Vector3 worldPoint,
      RootCubeGrid target)
    {
      if (this.CubeProvider.PrefabID == byte.MaxValue || this.CubeProvider.MaterialID == byte.MaxValue)
        return CubeInteraction.State.Valid;
      CrestScheme crestScheme = SocialAPI.Get<CrestScheme>();
      bool hasCrest;
      if (!CubeInfo.IsPrefabTypeSingleBlock(this.CubeProvider.PrefabID))
      {
        Vector3[] blockOffsets = TilesetLibrary.GetTilesetWithID(this.CubeProvider.MaterialID).GetPrefabWithID((int) this.CubeProvider.PrefabID).BlockOffsets;
        if (blockOffsets != null && blockOffsets.Length > 1)
        {
          this.transform.rotation = this.Interaction.Rotation;
          Vector3Int localCoordinate = BlockManager.DefaultCubeGrid.WorldToLocalCoordinate(worldPoint);
          for (int index = 0; index < blockOffsets.Length; ++index)
          {
            Vector3 worldCoordinate = BlockManager.DefaultCubeGrid.LocalToWorldCoordinate((Vector3Int) ((Vector3) localCoordinate + this.transform.rotation * blockOffsets[index]));
            bool checkSecurity;
            if (!crestScheme.OwnsLocation(Player.Local.Id, worldCoordinate, out hasCrest, out checkSecurity) && !crestScheme.IsTransitionArea(worldCoordinate))
            {
              if (checkSecurity)
                this.LastFailedReason = Translator.TryGet("Message.Block.CrestPlace", "You do not have clearance to place there.");
              return CubeInteraction.State.Invalid;
            }
          }
          return CubeInteraction.State.Valid;
        }
      }
      bool checkSecurity1;
      if (crestScheme.OwnsLocation(Player.Local.Id, worldPoint, out hasCrest, out checkSecurity1) || crestScheme.IsTransitionArea(worldPoint))
        return CubeInteraction.State.Valid;
      if (checkSecurity1)
        this.LastFailedReason = Translator.TryGet("Message.Block.CrestPlace", "You do not have clearance to place there.");
      return CubeInteraction.State.Invalid;
    }
  }
}
