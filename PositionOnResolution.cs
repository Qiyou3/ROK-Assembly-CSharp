// Decompiled with JetBrains decompiler
// Type: ThirdParty.Extensions.NGUI.PositionOnResolution
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace ThirdParty.Extensions.NGUI
{
  public class PositionOnResolution : MonoBehaviour
  {
    [Range(1f, 10000f)]
    public int Width = 1440;
    [Range(1f, 10000f)]
    public int Height = 1080;
    public PositionOnResolution.WindowAnchor AnchorAboveRatio;
    public PositionOnResolution.WindowAnchor AnchorBelowRatio;

    public float Ratio { get; private set; }

    public bool IsAboveRatio { get; private set; }

    public void Awake()
    {
      this.Ratio = (float) this.Width / (float) this.Height;
      if ((double) ((float) Screen.width / (float) Screen.height) >= (double) this.Ratio)
        return;
      this.IsAboveRatio = true;
    }

    public void Update()
    {
      if ((double) ((float) Screen.width / (float) Screen.height) < (double) this.Ratio)
      {
        if (!this.IsAboveRatio)
          return;
        this.IsAboveRatio = false;
        if (this.AnchorBelowRatio == null)
          return;
        if ((UnityEngine.Object) this.AnchorBelowRatio.Border != (UnityEngine.Object) null)
        {
          this.AnchorBelowRatio.Border.pivot = this.AnchorBelowRatio.BorderPivot;
          this.AnchorBelowRatio.Border.transform.localPosition = this.AnchorBelowRatio.BorderPosition;
        }
        if (!((UnityEngine.Object) this.AnchorBelowRatio.Panel != (UnityEngine.Object) null) || !((UnityEngine.Object) this.AnchorBelowRatio.PanelParent != (UnityEngine.Object) null))
          return;
        Vector3 localPosition = this.AnchorBelowRatio.Panel.transform.localPosition;
        Quaternion localRotation = this.AnchorBelowRatio.Panel.transform.localRotation;
        Vector3 localScale = this.AnchorBelowRatio.Panel.transform.localScale;
        this.AnchorBelowRatio.Panel.transform.parent = this.AnchorBelowRatio.PanelParent;
        this.AnchorBelowRatio.Panel.transform.localPosition = localPosition;
        this.AnchorBelowRatio.Panel.transform.localRotation = localRotation;
        this.AnchorBelowRatio.Panel.transform.localScale = localScale;
      }
      else
      {
        if (this.IsAboveRatio)
          return;
        this.IsAboveRatio = true;
        if (this.AnchorAboveRatio == null)
          return;
        if ((UnityEngine.Object) this.AnchorAboveRatio.Border != (UnityEngine.Object) null)
        {
          this.AnchorAboveRatio.Border.pivot = this.AnchorAboveRatio.BorderPivot;
          this.AnchorAboveRatio.Border.transform.localPosition = this.AnchorAboveRatio.BorderPosition;
        }
        if (!((UnityEngine.Object) this.AnchorAboveRatio.Panel != (UnityEngine.Object) null) || !((UnityEngine.Object) this.AnchorAboveRatio.PanelParent != (UnityEngine.Object) null))
          return;
        Vector3 localPosition = this.AnchorAboveRatio.Panel.transform.localPosition;
        Quaternion localRotation = this.AnchorAboveRatio.Panel.transform.localRotation;
        Vector3 localScale = this.AnchorAboveRatio.Panel.transform.localScale;
        this.AnchorAboveRatio.Panel.transform.parent = this.AnchorAboveRatio.PanelParent;
        this.AnchorAboveRatio.Panel.transform.localPosition = localPosition;
        this.AnchorAboveRatio.Panel.transform.localRotation = localRotation;
        this.AnchorAboveRatio.Panel.transform.localScale = localScale;
      }
    }

    [Serializable]
    public class WindowAnchor
    {
      public UIPanel Panel;
      public Transform PanelParent;
      public UISprite Border;
      public UIWidget.Pivot BorderPivot = UIWidget.Pivot.Center;
      public Vector3 BorderPosition;
    }
  }
}
