// Decompiled with JetBrains decompiler
// Type: CodeHatch.Blocks.Interaction.Filters.BlockSelectedFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace CodeHatch.Blocks.Interaction.Filters
{
  public class BlockSelectedFilter : RootFilter
  {
    protected override CubeInteraction.State GetStateInternal(
      Vector3 worldPoint,
      RootCubeGrid target)
    {
      return (int) this.CubeProvider.MaterialID == (int) CubeInfo.Error.MaterialID || (int) this.CubeProvider.MaterialID == (int) CubeInfo.Air.MaterialID ? CubeInteraction.State.Hidden : CubeInteraction.State.Valid;
    }
  }
}
