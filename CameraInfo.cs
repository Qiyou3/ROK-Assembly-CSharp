// Decompiled with JetBrains decompiler
// Type: CameraInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Camera/CameraInfo")]
public class CameraInfo : MonoBehaviour
{
  private bool m_d3d;

  public static Matrix4x4 ViewMatrix { get; private set; }

  public static Matrix4x4 ProjectionMatrix { get; private set; }

  public static Matrix4x4 ViewProjectionMatrix { get; private set; }

  public static Matrix4x4 PrevViewMatrix { get; private set; }

  public static Matrix4x4 PrevProjectionMatrix { get; private set; }

  public static Matrix4x4 PrevViewProjMatrix { get; private set; }

  public void Awake()
  {
    this.m_d3d = SystemInfo.graphicsDeviceVersion.IndexOf("Direct3D") > -1;
    CameraInfo.ViewMatrix = Matrix4x4.identity;
    CameraInfo.ProjectionMatrix = Matrix4x4.identity;
    CameraInfo.ViewProjectionMatrix = Matrix4x4.identity;
    CameraInfo.PrevViewMatrix = Matrix4x4.identity;
    CameraInfo.PrevProjectionMatrix = Matrix4x4.identity;
    CameraInfo.PrevViewProjMatrix = Matrix4x4.identity;
    this.UpdateCurrentMatrices();
  }

  public void Update()
  {
    CameraInfo.PrevViewMatrix = CameraInfo.ViewMatrix;
    CameraInfo.PrevProjectionMatrix = CameraInfo.ProjectionMatrix;
    CameraInfo.PrevViewProjMatrix = CameraInfo.ViewProjectionMatrix;
    this.UpdateCurrentMatrices();
  }

  private void UpdateCurrentMatrices()
  {
    CameraInfo.ViewMatrix = this.GetComponent<Camera>().worldToCameraMatrix;
    Matrix4x4 projectionMatrix = this.GetComponent<Camera>().projectionMatrix;
    if (this.m_d3d)
    {
      for (int column = 0; column < 4; ++column)
        projectionMatrix[1, column] = -projectionMatrix[1, column];
      for (int column = 0; column < 4; ++column)
        projectionMatrix[2, column] = (float) ((double) projectionMatrix[2, column] * 0.5 + (double) projectionMatrix[3, column] * 0.5);
    }
    CameraInfo.ProjectionMatrix = projectionMatrix;
    CameraInfo.ViewProjectionMatrix = CameraInfo.ProjectionMatrix * CameraInfo.ViewMatrix;
  }
}
