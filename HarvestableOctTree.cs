// Decompiled with JetBrains decompiler
// Type: CodeHatch.Blocks.Components.HarvestableOctTree
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Damaging;
using CodeHatch.Engine.Modules.Inventory.Resource;
using UnityEngine;

#nullable disable
namespace CodeHatch.Blocks.Components
{
  public class HarvestableOctTree : MonoBehaviour, IHarvestable
  {
    private PageGrid _LocalOctTree;

    public PageGrid LocalOctTree
    {
      get
      {
        if ((Object) this._LocalOctTree == (Object) null)
          this._LocalOctTree = this.GetComponent<PageGrid>();
        return this._LocalOctTree;
      }
    }

    public ResourceAmount[] Modify(Harvester modifier)
    {
      CubeInfo cubeInfoAt = this.LocalOctTree.GetCubeInfoAt(modifier.ModificationPoint);
      if (cubeInfoAt.MaterialID != (byte) 0)
      {
        OctTileset tilesetWithId = TilesetLibrary.GetTilesetWithID(cubeInfoAt.MaterialID);
        ResourceAmount[] resourceAmountArray = new ResourceAmount[tilesetWithId.ResourceInfo.HarvestableResources.Length];
        float netDamage = 0.0f;
        for (int index = 0; index < tilesetWithId.ResourceInfo.HarvestableResources.Length; ++index)
        {
          resourceAmountArray[index] = new ResourceAmount(tilesetWithId.ResourceInfo.HarvestableResources[index], 0.0f);
          if (modifier.ModifierLookup.ContainsKey(tilesetWithId.ResourceInfo.HarvestableResources[index]) && modifier.ModifierLookup[tilesetWithId.ResourceInfo.HarvestableResources[index]].Collectable)
          {
            float gatheredPerUse = modifier.ModifierLookup[tilesetWithId.ResourceInfo.HarvestableResources[index]].GatheredPerUse;
            resourceAmountArray[index].Amount = gatheredPerUse * tilesetWithId.ResourceInfo.ResourceModifier;
            netDamage += gatheredPerUse * tilesetWithId.ResourceInfo.DamageModifier;
          }
        }
        if ((double) tilesetWithId.ResourceInfo.DamageModifier != 0.0)
          this.ApplyDamage(netDamage, this.LocalOctTree.WorldToLocalCoordinate(modifier.ModificationPoint));
        return resourceAmountArray;
      }
      this.LogError<HarvestableOctTree>("Cannot collect resources from air.");
      return new ResourceAmount[0];
    }

    private void ApplyDamage(float netDamage, Vector3Int position)
    {
      Damage damage = new Damage((Object) this);
      damage.Amount = netDamage;
      damage.DamageTypes = DamageType.Harvest;
      IDamageReceiver implementorInChildren = this.LocalOctTree.GetCentralPrefabAtLocal(position).gameObject.GetImplementorInChildren<IDamageReceiver>();
      if (implementorInChildren != null)
        implementorInChildren.OnDamage(damage);
      else
        this.LogError<HarvestableOctTree>(string.Format("Tried to deal damage when collecting resources from this object {0}, but failed to find an IDamageReciever.", (object) this.name));
    }
  }
}
