// Decompiled with JetBrains decompiler
// Type: Generate2DReflection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Generate2DReflection : MonoBehaviour
{
  public bool useRealtimeReflection;
  public int textureSize = 128;
  public LayerMask mask = (LayerMask) 1;
  private Camera cam;
  public RenderTexture rtex;
  public Material reflectingMaterial;
  public Texture staticCubemap;

  public void Start() => this.reflectingMaterial.SetTexture("_Cube", this.staticCubemap);

  public void LateUpdate()
  {
    if (!this.useRealtimeReflection || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
      return;
    this.UpdateReflection();
  }

  public void OnDisable()
  {
    if ((bool) (UnityEngine.Object) this.rtex)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.rtex);
    this.reflectingMaterial.SetTexture("_Cube", this.staticCubemap);
  }

  private void UpdateReflection()
  {
    if (!(bool) (UnityEngine.Object) this.rtex)
    {
      this.rtex = new RenderTexture(this.textureSize, this.textureSize, 16);
      this.rtex.hideFlags = HideFlags.HideAndDontSave;
      this.rtex.isPowerOfTwo = true;
      this.rtex.isCubemap = true;
      this.rtex.useMipMap = false;
      this.rtex.Create();
      this.reflectingMaterial.SetTexture("_Cube", (Texture) this.rtex);
    }
    if (!(bool) (UnityEngine.Object) this.cam)
    {
      GameObject gameObject = new GameObject("CubemapCamera", new System.Type[1]
      {
        typeof (Camera)
      });
      gameObject.hideFlags = HideFlags.HideAndDontSave;
      this.cam = gameObject.GetComponent<Camera>();
      this.cam.farClipPlane = 150f;
      this.cam.enabled = false;
      this.cam.cullingMask = (int) this.mask;
    }
    this.cam.transform.position = Camera.main.transform.position;
    this.cam.transform.rotation = Camera.main.transform.rotation;
    this.cam.RenderToCubemap(this.rtex, 63);
  }
}
