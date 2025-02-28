// Decompiled with JetBrains decompiler
// Type: CodeHatch.Blocks.Interaction.Filters.CombinationFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace CodeHatch.Blocks.Interaction.Filters
{
  public class CombinationFilter : RootFilter
  {
    public RootFilter FilterOne;
    public RootFilter FilterTwo;
    public CombinationFilter.Combination CombineType;

    public bool CanPlaceAt(Vector3 worldPoint, RootCubeGrid target)
    {
      if ((Object) this.FilterOne == (Object) null || (Object) this.FilterTwo == (Object) null)
      {
        Logger.Error("Combination Filter requires at least two objects.");
        return false;
      }
      bool flag = false;
      switch (this.CombineType)
      {
        case CombinationFilter.Combination.Or:
        case CombinationFilter.Combination.And:
          return flag;
        default:
          flag = true;
          goto case CombinationFilter.Combination.Or;
      }
    }

    protected override CubeInteraction.State GetStateInternal(
      Vector3 worldPoint,
      RootCubeGrid target)
    {
      if ((Object) this.FilterOne == (Object) null || (Object) this.FilterTwo == (Object) null)
      {
        Logger.Error("Combination Filter requires at least two objects.");
        return CubeInteraction.State.Invalid;
      }
      CubeInteraction.State stateInternal = CubeInteraction.State.Invalid;
      switch (this.CombineType)
      {
        case CombinationFilter.Combination.Or:
          CubeInteraction.State state1 = this.FilterOne.GetState(worldPoint, target);
          CubeInteraction.State state2 = this.FilterTwo.GetState(worldPoint, target);
          if (state1 == CubeInteraction.State.Valid || state2 == CubeInteraction.State.Valid)
          {
            stateInternal = CubeInteraction.State.Valid;
            break;
          }
          break;
        case CombinationFilter.Combination.And:
          CubeInteraction.State state3 = this.FilterOne.GetState(worldPoint, target);
          CubeInteraction.State state4 = this.FilterTwo.GetState(worldPoint, target);
          if (state3 == CubeInteraction.State.Valid && state4 == CubeInteraction.State.Valid)
          {
            stateInternal = CubeInteraction.State.Valid;
            break;
          }
          break;
      }
      return stateInternal;
    }

    public enum Combination
    {
      None,
      Or,
      And,
    }
  }
}
