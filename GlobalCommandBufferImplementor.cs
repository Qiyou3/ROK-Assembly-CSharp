// Decompiled with JetBrains decompiler
// Type: GlobalCommandBufferImplementor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
[ExecuteInEditMode]
public class GlobalCommandBufferImplementor : MonoBehaviour
{
  private Dictionary<Camera, CommandBufferInfo> _CameraToCommandBuffer = new Dictionary<Camera, CommandBufferInfo>();
  private int _UpdateFrame;
  private int _LastLayerMask;
  protected GlobalCommandBufferImplementor.UpdateCommandBufferDelegate UpdateCommandBufferImplementation;
  private CameraEvent _LastRenderOrder = CameraEvent.BeforeLighting;
  protected CameraEvent RenderOrder = CameraEvent.BeforeLighting;
  private OnRenderHelper _OnRenderHelper;

  private int Layer => this.gameObject.layer;

  public void RequestUpdate() => this._UpdateFrame = Time.frameCount;

  public bool ShouldUpdate(int UpdateFrame) => this._UpdateFrame >= UpdateFrame;

  public virtual void OnEnable() => this.SetupRenderHelper();

  private void SetupRenderHelper()
  {
    this._OnRenderHelper = OnRenderHelper.GetOrCreateOnRenderHelper(this.gameObject);
    this._OnRenderHelper.OnWillRenderObjectImplementation = new System.Action(this.OnWillRenderObject);
    this._OnRenderHelper.OnRenderObjectImplementation = new System.Action(this.OnRenderObject);
  }

  public virtual void OnDisable() => this.ClearCommandBuffers();

  public void ClearCommandBuffers()
  {
    foreach (KeyValuePair<Camera, CommandBufferInfo> keyValuePair in this._CameraToCommandBuffer)
    {
      if ((bool) (UnityEngine.Object) keyValuePair.Key)
        keyValuePair.Key.RemoveCommandBuffer(this.RenderOrder, keyValuePair.Value.CommandBuffer);
    }
    this._CameraToCommandBuffer.Clear();
  }

  private void ApplySettings()
  {
    if (this._LastRenderOrder != this.RenderOrder)
    {
      this._LastRenderOrder = this.RenderOrder;
      this.RequestUpdate();
    }
    if (this._LastLayerMask == this.Layer)
      return;
    this._LastLayerMask = this.Layer;
    this.SetupRenderHelper();
    this.ClearCommandBuffers();
    this.RequestUpdate();
  }

  public virtual void Update()
  {
    List<Camera> cameraList = new List<Camera>();
    foreach (KeyValuePair<Camera, CommandBufferInfo> keyValuePair in this._CameraToCommandBuffer)
    {
      if (!((UnityEngine.Object) keyValuePair.Key == (UnityEngine.Object) null) && (keyValuePair.Key.cullingMask & 1 << this.Layer) == 0)
      {
        keyValuePair.Key.RemoveCommandBuffer(this.RenderOrder, keyValuePair.Value.CommandBuffer);
        cameraList.Add(keyValuePair.Key);
      }
    }
    foreach (Camera key in cameraList)
      this._CameraToCommandBuffer.Remove(key);
  }

  public virtual void OnWillRenderObject()
  {
    if (!this.gameObject.activeInHierarchy || !this.enabled)
    {
      this.OnDisable();
    }
    else
    {
      Camera current = Camera.current;
      if (!(bool) (UnityEngine.Object) current)
        return;
      current.depthTextureMode = DepthTextureMode.DepthNormals;
      CommandBufferInfo bufInfo = this.RetrieveCommandBuffer(current);
      if (!this.ShouldUpdate(bufInfo.UpdateFrame))
        return;
      this.UpdateCommandBuffer(bufInfo);
    }
  }

  public virtual void OnRenderObject() => this.ApplySettings();

  private CommandBufferInfo RetrieveCommandBuffer(Camera cam)
  {
    CommandBufferInfo commandBufferInfo;
    if (this._CameraToCommandBuffer.ContainsKey(cam))
    {
      commandBufferInfo = this._CameraToCommandBuffer[cam];
    }
    else
    {
      commandBufferInfo = new CommandBufferInfo()
      {
        CommandBuffer = new CommandBuffer()
      };
      commandBufferInfo.CommandBuffer.name = this.GetType().Name;
      this._CameraToCommandBuffer[cam] = commandBufferInfo;
      cam.AddCommandBuffer(this.RenderOrder, commandBufferInfo.CommandBuffer);
    }
    return commandBufferInfo;
  }

  private void UpdateCommandBuffer(CommandBufferInfo bufInfo)
  {
    bufInfo.UpdateFrame = Time.frameCount;
    CommandBuffer commandBuffer = bufInfo.CommandBuffer;
    if (this.UpdateCommandBufferImplementation == null)
      return;
    this.UpdateCommandBufferImplementation(commandBuffer);
  }

  protected delegate void UpdateCommandBufferDelegate(CommandBuffer commandBuffer);
}
