// Decompiled with JetBrains decompiler
// Type: DeferredDecalSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
[ExecuteInEditMode]
public class DeferredDecalSystem : GlobalCommandBufferImplementor
{
  private static DeferredDecalSystem _Instance;
  private Mesh _CubeMesh;
  private int _DecalCount;
  internal static HashSet<Decal> _Decals = new HashSet<Decal>();
  private PerformanceTracker _performanceTracker = new PerformanceTracker()
  {
    minimumWorkUnitsPerFrame = 1,
    minimumDeltaTimeHeadroom = 0.0005f,
    targetDeltaTime = 1f / 1000f
  };
  private IEnumerator DecalCoroutine;

  private static string Name => typeof (DeferredDecalSystem).Name;

  public static DeferredDecalSystem Instance
  {
    get
    {
      if ((Object) DeferredDecalSystem._Instance == (Object) null)
      {
        GameObject gameObject = GameObject.Find(DeferredDecalSystem.Name);
        if ((Object) gameObject != (Object) null)
          DeferredDecalSystem._Instance = gameObject.GetComponent<DeferredDecalSystem>();
      }
      if ((Object) DeferredDecalSystem._Instance == (Object) null)
        DeferredDecalSystem._Instance = new GameObject(DeferredDecalSystem.Name).AddComponent<DeferredDecalSystem>();
      return DeferredDecalSystem._Instance;
    }
  }

  public static void AddDecal(Decal d)
  {
    DeferredDecalSystem.RemoveDecal(d);
    DeferredDecalSystem._Decals.Add(d);
    DeferredDecalSystem.RequestDecalUpdate();
  }

  public static void RemoveDecal(Decal d)
  {
    DeferredDecalSystem._Decals.Remove(d);
    DeferredDecalSystem.RequestDecalUpdate();
  }

  public static void RequestDecalUpdate()
  {
    if (!((Object) DeferredDecalSystem._Instance != (Object) null))
      return;
    DeferredDecalSystem._Instance.RequestUpdate();
  }

  [DebuggerHidden]
  public IEnumerator DecalUpdateEnumerator()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new DeferredDecalSystem.\u003CDecalUpdateEnumerator\u003Ec__Iterator10A()
    {
      \u003C\u003Ef__this = this
    };
  }

  public override void Update()
  {
    if (this.DecalCoroutine == null)
      this.DecalCoroutine = this.DecalUpdateEnumerator();
    if (!this.DecalCoroutine.MoveNext())
      this.DecalCoroutine = (IEnumerator) null;
    base.Update();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.UpdateCommandBufferImplementation = new GlobalCommandBufferImplementor.UpdateCommandBufferDelegate(this.UpdateCommandBuffer);
    this.RenderOrder = CameraEvent.BeforeLighting;
    if ((Object) DeferredDecalSystem._Instance != (Object) this && (Object) DeferredDecalSystem._Instance != (Object) null)
    {
      Object.DestroyImmediate((Object) this.gameObject);
    }
    else
    {
      if (!((Object) this._CubeMesh == (Object) null))
        return;
      GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
      this._CubeMesh = primitive.GetComponent<MeshFilter>().sharedMesh;
      if (Application.isPlaying)
        Object.Destroy((Object) primitive);
      else
        Object.DestroyImmediate((Object) primitive);
    }
  }

  private void UpdateCommandBuffer(CommandBuffer buf)
  {
    buf.Clear();
    int id = Shader.PropertyToID("_NormalsCopy");
    buf.GetTemporaryRT(id, -1, -1, 0, FilterMode.Point, RenderTextureFormat.ARGBHalf);
    buf.Blit((RenderTargetIdentifier) BuiltinRenderTextureType.GBuffer2, (RenderTargetIdentifier) id);
    RenderTargetIdentifier[] colors = new RenderTargetIdentifier[3]
    {
      (RenderTargetIdentifier) BuiltinRenderTextureType.GBuffer0,
      (RenderTargetIdentifier) BuiltinRenderTextureType.GBuffer1,
      (RenderTargetIdentifier) BuiltinRenderTextureType.GBuffer2
    };
    buf.SetRenderTarget(colors, (RenderTargetIdentifier) BuiltinRenderTextureType.CameraTarget);
    foreach (Decal decal in DeferredDecalSystem._Decals)
      buf.DrawMesh(this._CubeMesh, decal.transform.localToWorldMatrix, decal.Material);
    buf.ReleaseTemporaryRT(id);
  }
}
