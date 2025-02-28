// Decompiled with JetBrains decompiler
// Type: CodeHatch.Blocks.Collapsing.PlaceableBlockAssociation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Networking;
using CodeHatch.Engine.Serialization;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Entities;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace CodeHatch.Blocks.Collapsing
{
  public class PlaceableBlockAssociation : EntityBehaviour, ISerializable
  {
    private static Dictionary<Vector3Int, List<PlaceableBlockAssociation>> CoordinateToAssociation = new Dictionary<Vector3Int, List<PlaceableBlockAssociation>>((IEqualityComparer<Vector3Int>) new Vector3IntEqualityComparer());
    public bool DestroyDuringCollapse = true;
    private bool _CoordinateSet;
    private Vector3Int _BlockCoordinate;

    public Vector3Int BlockCoordinate
    {
      get => this._BlockCoordinate;
      set
      {
        if (this._CoordinateSet)
          PlaceableBlockAssociation.RemoveAssociation(this._BlockCoordinate, this);
        this._BlockCoordinate = value;
        PlaceableBlockAssociation.AddAssociation(this._BlockCoordinate, this);
        this._CoordinateSet = true;
      }
    }

    public bool IsFloating
    {
      get
      {
        return DedicatedServerBypass.Settings.BlockCollapsing && !BlockCollapsing.IsInUnloadedPage(this.BlockCoordinate) && !BlockCollapsing.IsBlockGrounded(this.BlockCoordinate) && !BlockCollapsing.IsBlockSolid(this.BlockCoordinate);
      }
    }

    public void OnEnable()
    {
      EventManager.Subscribe<EntityBlockAssociationEvent>(new EventSubscriber<EntityBlockAssociationEvent>(this.OnEntityBlockAssociation));
    }

    public void OnEntityBlockAssociation(EntityBlockAssociationEvent theEvent)
    {
      if ((Object) this.Entity != (Object) theEvent.Entity)
        return;
      this.BlockCoordinate = theEvent.BlockCoordinate;
    }

    public static void AddAssociation(Vector3Int coord, PlaceableBlockAssociation association)
    {
      List<PlaceableBlockAssociation> blockAssociationList = (List<PlaceableBlockAssociation>) null;
      if (!PlaceableBlockAssociation.CoordinateToAssociation.TryGetValue(coord, out blockAssociationList))
      {
        blockAssociationList = new List<PlaceableBlockAssociation>();
        PlaceableBlockAssociation.CoordinateToAssociation.Add(coord, blockAssociationList);
      }
      blockAssociationList.Add(association);
    }

    public static void RemoveAssociation(Vector3Int coord, PlaceableBlockAssociation association)
    {
      List<PlaceableBlockAssociation> blockAssociationList = (List<PlaceableBlockAssociation>) null;
      if (!PlaceableBlockAssociation.CoordinateToAssociation.TryGetValue(coord, out blockAssociationList))
        return;
      blockAssociationList.Remove(association);
      if (blockAssociationList.Count != 0)
        return;
      PlaceableBlockAssociation.CoordinateToAssociation.Remove(coord);
    }

    public static List<PlaceableBlockAssociation> RetrieveAssociations(Vector3Int coord)
    {
      if (!DedicatedServerBypass.Settings.BlockCollapsing)
        return (List<PlaceableBlockAssociation>) null;
      List<PlaceableBlockAssociation> blockAssociationList = (List<PlaceableBlockAssociation>) null;
      PlaceableBlockAssociation.CoordinateToAssociation.TryGetValue(coord, out blockAssociationList);
      return blockAssociationList;
    }

    public void OnDisable()
    {
      if (this._CoordinateSet)
        PlaceableBlockAssociation.RemoveAssociation(this.BlockCoordinate, this);
      this._CoordinateSet = false;
      EventManager.Unsubscribe<EntityBlockAssociationEvent>(new EventSubscriber<EntityBlockAssociationEvent>(this.OnEntityBlockAssociation));
    }

    public string Identifier => nameof (PlaceableBlockAssociation);

    public void Serialize(IStream stream) => stream.Write<Vector3Int>(this.BlockCoordinate);

    public void Deserialize(IStream stream) => this.BlockCoordinate = stream.Read<Vector3Int>();
  }
}
