// Decompiled with JetBrains decompiler
// Type: DepthOfFieldController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (DepthOfField34))]
[RequireComponent(typeof (Camera))]
public class DepthOfFieldController : MonoBehaviour
{
  private DepthOfField34 DepthOfField34;
  private Camera Camera;
  public AnimationCurve SunHeightMultiplier;
  public float MultiplierWhileImmersed = 0.1f;

  public bool Immersed
  {
    get
    {
      return !((Object) WaterController.Instance == (Object) null) && (double) this.transform.position.y < (double) WaterController.Instance.GetWaterHeightAt(this.transform.position);
    }
  }

  private void Start()
  {
    this.DepthOfField34 = this.GetComponent<DepthOfField34>();
    this.Camera = this.GetComponent<Camera>();
  }

  private void Update()
  {
    this.DepthOfField34.smoothness = this.Camera.farClipPlane;
    if (this.Immersed)
      this.DepthOfField34.smoothness *= this.MultiplierWhileImmersed;
    if (!((Object) AtmosphereView.Instance != (Object) null))
      return;
    this.DepthOfField34.smoothness *= this.SunHeightMultiplier.Evaluate(AtmosphereView.Instance.SunHeight);
  }
}
