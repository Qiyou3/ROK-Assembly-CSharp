// Decompiled with JetBrains decompiler
// Type: CodeHatch.Blocks.Collapsing.BlockCollapsing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Blocks.Geometry;
using CodeHatch.Blocks.Networking.Events;
using CodeHatch.Blocks.Networking.Events.Local;
using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Core.Gaming;
using CodeHatch.Engine.Core.Paging;
using CodeHatch.Engine.Networking;
using CodeHatch.Networking;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Sync;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace CodeHatch.Blocks.Collapsing
{
  public class BlockCollapsing : MonoBehaviour
  {
    public static Vector3Int[] SEARCH_DIRECTIONS = new Vector3Int[6]
    {
      Vector3Int.Up,
      Vector3Int.Down,
      Vector3Int.Left,
      Vector3Int.Right,
      Vector3Int.Forward,
      Vector3Int.Back
    };
    public int MaxSearchLength = 1000000;
    public int MaxParticleCount = 40;
    public float CollapseAnimationDelay = 0.1f;
    public MaterialsToSound[] MaterialsToSounds;
    public LayerMask GroundLayers;
    public PerformanceTracker PerformanceTracker = new PerformanceTracker()
    {
      minimumWorkUnitsPerFrame = 1
    };
    public static BlockCollapsing Instance;
    public static Queue<BlockCollapsing.PathingInput> CheckBlockQueue = new Queue<BlockCollapsing.PathingInput>();
    public static List<FloatingChunk> CurrentlyCollapsing = new List<FloatingChunk>();
    public static IEnumerator CurrentCheckProcess;
    [Syncable]
    public bool EnableBlockCollapsing = true;
    private HashSet<Vector3Int> alreadyCheckedUpdateHash = new HashSet<Vector3Int>((IEqualityComparer<Vector3Int>) new Vector3IntEqualityComparer());
    private HashSet<Vector3Int> chunkUpdateHash = new HashSet<Vector3Int>((IEqualityComparer<Vector3Int>) new Vector3IntEqualityComparer());
    private Queue<Vector3Int> searchQueueUpdateList = new Queue<Vector3Int>();

    public void Awake()
    {
      SyncManager.Register(nameof (BlockCollapsing), (object) this);
      BlockCollapsing.Instance = this;
    }

    public void Start()
    {
      EventManager.Subscribe<FloatingChunkDetectedEvent>(new EventSubscriber<FloatingChunkDetectedEvent>(this.OnFloatingChunkDetected), EventHandlerOrder.Early);
      EventManager.Subscribe<CubeDestroyEvent>(new EventSubscriber<CubeDestroyEvent>(this.OnCubeDestroy), EventHandlerOrder.VeryEarly);
      EventManager.Subscribe<CubePlaceLocalFinalizeEvent>(new EventSubscriber<CubePlaceLocalFinalizeEvent>(this.OnCubeFinalize), EventHandlerOrder.VeryLate);
    }

    public void OnDestroy()
    {
      EventManager.Unsubscribe<FloatingChunkDetectedEvent>(new EventSubscriber<FloatingChunkDetectedEvent>(this.OnFloatingChunkDetected));
      EventManager.Unsubscribe<CubeDestroyEvent>(new EventSubscriber<CubeDestroyEvent>(this.OnCubeDestroy));
    }

    public void Update()
    {
      if (!Player.IsLocalServer)
        return;
      this.EnableBlockCollapsing = DedicatedServerBypass.Settings.BlockCollapsing;
      if (!this.EnableBlockCollapsing)
        return;
      this.PerformanceTracker.BeginTracking();
      if (BlockCollapsing.CurrentCheckProcess != null && BlockCollapsing.CurrentCheckProcess.MoveNext())
        return;
      if (BlockCollapsing.CheckBlockQueue.Count > 0)
      {
        BlockCollapsing.PathingInput pathingInput = BlockCollapsing.CheckBlockQueue.Dequeue();
        this.alreadyCheckedUpdateHash.Clear();
        BlockCollapsing.CurrentCheckProcess = BlockCollapsing.BlockPath(pathingInput, this.PerformanceTracker, this.MaxSearchLength, false, (Action<FloatingChunk>) (floatingChunk => EventManager.CallEvent((BaseEvent) new FloatingChunkDetectedEvent(floatingChunk))), this.alreadyCheckedUpdateHash, this.chunkUpdateHash, this.searchQueueUpdateList);
      }
      else
        BlockCollapsing.CurrentCheckProcess = (IEnumerator) null;
    }

    public void OnFloatingChunkDetected(FloatingChunkDetectedEvent theEvent)
    {
      FloatingChunk floatingChunk = theEvent.FloatingChunk;
      if (floatingChunk == null || floatingChunk.BlockCoords == null)
        return;
      MaterialsToSound materialsToSound = (MaterialsToSound) null;
      Vector3 worldPosition = Vector3.zero;
      if (this.MaterialsToSounds != null)
      {
        int[] numArray = new int[this.MaterialsToSounds.Length];
        for (int index1 = 0; index1 < floatingChunk.BlockCoords.Length; ++index1)
        {
          Vector3Int blockCoord = floatingChunk.BlockCoords[index1];
          worldPosition += (Vector3) blockCoord;
          CubeInfo cubeInfoAtLocal = BlockManager.DefaultCubeGrid.GetCubeInfoAtLocal(blockCoord);
          for (int index2 = 0; index2 < this.MaterialsToSounds.Length; ++index2)
          {
            if (this.MaterialsToSounds[index2].Tilesets != null)
            {
              for (int index3 = 0; index3 < this.MaterialsToSounds[index2].Tilesets.Length; ++index3)
              {
                int tilesetId = this.MaterialsToSounds[index2].Tilesets[index3].TilesetID;
                if ((int) cubeInfoAtLocal.MaterialID == tilesetId)
                  ++numArray[index2];
              }
            }
          }
        }
        worldPosition = BlockManager.DefaultCubeGrid.LocalToWorldCoordinate(new Vector3Int(worldPosition / (float) floatingChunk.BlockCoords.Length));
        int num = 0;
        for (int index = 0; index < numArray.Length; ++index)
        {
          if (numArray[index] > num)
          {
            materialsToSound = this.MaterialsToSounds[index];
            num = numArray[index];
          }
        }
      }
      if (materialsToSound != null)
      {
        string empty = string.Empty;
        AudioController.Play(floatingChunk.BlockCoords.Length <= 80 ? (floatingChunk.BlockCoords.Length <= 30 ? materialsToSound.Small : materialsToSound.Medium) : materialsToSound.Large, worldPosition);
      }
      if (!((UnityEngine.Object) BlockCollapsing.Instance != (UnityEngine.Object) null))
        return;
      BlockCollapsing.Instance.StartCoroutine(BlockCollapsing.CollapseAnimation(floatingChunk));
    }

    [DebuggerHidden]
    private static IEnumerator CollapseAnimation(FloatingChunk floatingChunk)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new BlockCollapsing.\u003CCollapseAnimation\u003Ec__Iterator3()
      {
        floatingChunk = floatingChunk,
        \u003C\u0024\u003EfloatingChunk = floatingChunk
      };
    }

    private static void RemoveAtCoord(FloatingChunk floatingChunk, int i, Vector3Int coord)
    {
      if (Player.IsLocalServer)
      {
        List<PlaceableBlockAssociation> blockAssociationList = PlaceableBlockAssociation.RetrieveAssociations(coord);
        if (blockAssociationList != null)
        {
          for (int index = 0; index < blockAssociationList.Count; ++index)
          {
            Entity entity = blockAssociationList[index].Entity;
            if (!((UnityEngine.Object) entity == (UnityEngine.Object) null))
            {
              Health health = entity.TryGet<Health>();
              if (!((UnityEngine.Object) health == (UnityEngine.Object) null))
                health.Kill();
            }
          }
        }
      }
      TilesetColliderCube centralPrefabAtLocal = BlockManager.DefaultCubeGrid.GetCentralPrefabAtLocal(coord);
      int num1 = Mathf.FloorToInt((float) i / (float) floatingChunk.BlockCoords.Length * (float) BlockCollapsing.Instance.MaxParticleCount);
      int num2 = Mathf.CeilToInt((float) floatingChunk.BlockCoords.Length / (float) BlockCollapsing.Instance.MaxParticleCount * (float) num1);
      if ((UnityEngine.Object) centralPrefabAtLocal != (UnityEngine.Object) null && i == num2)
        centralPrefabAtLocal.HandleDeath();
      else
        BlockManager.DefaultCubeGrid.Bypass_PlaceCubeAtLocal(coord, CubeInfo.Air.MaterialID, CubeInfo.Air.PrefabID, Quaternion.identity);
    }

    public void OnCubeDestroy(CubeDestroyEvent theEvent)
    {
      if (!Player.IsLocalServer || !this.EnableBlockCollapsing)
        return;
      BlockCollapsing.CheckForFloatingChunk(theEvent.Position);
    }

    public void OnCubeFinalize(CubePlaceLocalFinalizeEvent theEvent)
    {
      if (!Player.IsLocalServer || !this.EnableBlockCollapsing || BlockCollapsing.CurrentlyCollapsing.Count == 0)
        return;
      foreach (FloatingChunk floatingChunk in BlockCollapsing.CurrentlyCollapsing)
      {
        for (int index = 0; index < BlockCollapsing.SEARCH_DIRECTIONS.Length; ++index)
        {
          Vector3Int vector3Int = theEvent.PlaceEvent.Position + BlockCollapsing.SEARCH_DIRECTIONS[index];
          if (floatingChunk.BlockHashset.Contains(vector3Int))
          {
            EventManager.CallEvent((BaseEvent) new CubeDestroyEvent(theEvent.PlaceEvent.GridID, theEvent.PlaceEvent.Position));
            return;
          }
        }
      }
    }

    public static bool IsBlockSolid(Vector3Int coordinate)
    {
      for (int index = 0; index < BlockCollapsing.CurrentlyCollapsing.Count; ++index)
      {
        if (BlockCollapsing.CurrentlyCollapsing[index].BlockHashset.Contains(coordinate))
          return false;
      }
      return BlockManager.DefaultCubeGrid.GetCubeInfoAtLocal(coordinate).MaterialID != (byte) 0;
    }

    public static bool IsBlockGrounded(Vector3Int coordinate)
    {
      return Game.IsLoading || Physics.CheckSphere(BlockManager.DefaultCubeGrid.LocalToWorldCoordinate(coordinate), 1.2f, (int) BlockCollapsing.Instance.GroundLayers);
    }

    public static bool IsInUnloadedPage(Vector3Int coordinate)
    {
      return !PagingAPI.IsInLoadedPage(ControlType.PrefabBlocks, BlockManager.DefaultCubeGrid.LocalToWorldCoordinate(coordinate) + Vector3.one * 0.5f);
    }

    public static void CheckForFloatingChunk(Vector3Int coord)
    {
      List<Vector3Int> vector3IntList = new List<Vector3Int>()
      {
        coord
      };
      TilesetColliderCube centralPrefabAtLocal = BlockManager.DefaultCubeGrid.GetCentralPrefabAtLocal(coord);
      if ((UnityEngine.Object) centralPrefabAtLocal != (UnityEngine.Object) null)
      {
        OctPrefab prefabInfo = centralPrefabAtLocal.PrefabInfo;
        if ((UnityEngine.Object) prefabInfo != (UnityEngine.Object) null)
          vector3IntList = prefabInfo.GetBlockCoords();
      }
      BlockCollapsing.CheckBlockQueue.Enqueue(new BlockCollapsing.PathingInput()
      {
        RemovedCoords = vector3IntList
      });
    }

    [DebuggerHidden]
    public static IEnumerator BlockPath(
      BlockCollapsing.PathingInput pathingInput,
      PerformanceTracker performanceTracker,
      int maxSearchLength,
      bool assumeGrounded,
      Action<FloatingChunk> handleFloatingChunk,
      HashSet<Vector3Int> alreadySearched = null,
      HashSet<Vector3Int> chunk = null,
      Queue<Vector3Int> searchQueue = null)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new BlockCollapsing.\u003CBlockPath\u003Ec__Iterator4()
      {
        pathingInput = pathingInput,
        assumeGrounded = assumeGrounded,
        alreadySearched = alreadySearched,
        chunk = chunk,
        searchQueue = searchQueue,
        performanceTracker = performanceTracker,
        maxSearchLength = maxSearchLength,
        handleFloatingChunk = handleFloatingChunk,
        \u003C\u0024\u003EpathingInput = pathingInput,
        \u003C\u0024\u003EassumeGrounded = assumeGrounded,
        \u003C\u0024\u003EalreadySearched = alreadySearched,
        \u003C\u0024\u003Echunk = chunk,
        \u003C\u0024\u003EsearchQueue = searchQueue,
        \u003C\u0024\u003EperformanceTracker = performanceTracker,
        \u003C\u0024\u003EmaxSearchLength = maxSearchLength,
        \u003C\u0024\u003EhandleFloatingChunk = handleFloatingChunk
      };
    }

    [DebuggerHidden]
    private static IEnumerator PathPerCoord(
      Vector3Int startCoord,
      BlockCollapsing.PathingInput pathingInput,
      PerformanceTracker performanceTracker,
      int maxSearchLength,
      bool assumeGrounded,
      Action<FloatingChunk> handleFloatingChunk,
      HashSet<Vector3Int> alreadySearched,
      List<Vector3Int> checkCoords,
      HashSet<Vector3Int> chunk,
      Queue<Vector3Int> searchQueue)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new BlockCollapsing.\u003CPathPerCoord\u003Ec__Iterator5()
      {
        startCoord = startCoord,
        alreadySearched = alreadySearched,
        chunk = chunk,
        assumeGrounded = assumeGrounded,
        checkCoords = checkCoords,
        searchQueue = searchQueue,
        maxSearchLength = maxSearchLength,
        performanceTracker = performanceTracker,
        pathingInput = pathingInput,
        handleFloatingChunk = handleFloatingChunk,
        \u003C\u0024\u003EstartCoord = startCoord,
        \u003C\u0024\u003EalreadySearched = alreadySearched,
        \u003C\u0024\u003Echunk = chunk,
        \u003C\u0024\u003EassumeGrounded = assumeGrounded,
        \u003C\u0024\u003EcheckCoords = checkCoords,
        \u003C\u0024\u003EsearchQueue = searchQueue,
        \u003C\u0024\u003EmaxSearchLength = maxSearchLength,
        \u003C\u0024\u003EperformanceTracker = performanceTracker,
        \u003C\u0024\u003EpathingInput = pathingInput,
        \u003C\u0024\u003EhandleFloatingChunk = handleFloatingChunk
      };
    }

    public class PathingInput
    {
      public List<Vector3Int> RemovedCoords;
      public List<Vector3Int> AddedCoords;
    }

    public delegate bool CustomCheck(HashSet<Vector3Int> alreadyChecked);
  }
}
