// Decompiled with JetBrains decompiler
// Type: CrosshairManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CrosshairManager : EntityBehaviour
{
  public GameObject CrosshairPrefab;
  private readonly List<Crosshair> _crosshairInstances = new List<Crosshair>();
  private int _nextInstanceToUse;
  private int _lastFrame = -1;

  private Crosshair GetInstance()
  {
    if (this._nextInstanceToUse > this._crosshairInstances.Count)
    {
      this.ClearInstances();
      throw new Exception("Unexpected issue with the crosshair instance buffer.");
    }
    Crosshair instance;
    if (this._nextInstanceToUse == this._crosshairInstances.Count)
    {
      instance = this.InstantiateCrosshair();
      this._crosshairInstances.Add(instance);
    }
    else
      instance = this._crosshairInstances[this._nextInstanceToUse];
    ++this._nextInstanceToUse;
    instance.gameObject.SetActive(true);
    instance.Enable();
    return instance;
  }

  private Crosshair InstantiateCrosshair()
  {
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.CrosshairPrefab);
    Crosshair crosshair = !((UnityEngine.Object) gameObject == (UnityEngine.Object) null) ? gameObject.GetComponentInChildren<Crosshair>() : throw new Exception("CrosshairPrefab is null or is not a Crosshair.");
    gameObject.transform.parent = this.transform;
    return crosshair;
  }

  private void ClearInstances()
  {
    for (int index = 0; index < this._crosshairInstances.Count; ++index)
      this._crosshairInstances[index].Disable();
    this._nextInstanceToUse = 0;
  }

  public void Update()
  {
    if (this._lastFrame == Time.frameCount)
      return;
    this.ClearInstances();
    this._lastFrame = Time.frameCount;
  }

  public void DrawCrosshairThisFrame(
    Texture2D texture,
    Color color,
    Vector3 position,
    Quaternion rotation,
    float scale)
  {
    if (this._lastFrame != Time.frameCount)
    {
      this.ClearInstances();
      this._lastFrame = Time.frameCount;
    }
    Crosshair instance = this.GetInstance();
    instance.Material.mainTexture = (Texture) texture;
    instance.Material.color = color;
    instance.Position = position;
    instance.Rotation = rotation;
    instance.Scale = scale;
  }
}
