// Decompiled with JetBrains decompiler
// Type: ThirdParty.Extensions.NGUI.ScrollbarDisableThreshold
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ThirdParty.Extensions.NGUI
{
  [RequireComponent(typeof (UIScrollBar))]
  public class ScrollbarDisableThreshold : MonoBehaviour
  {
    [Range(0.0f, 1f)]
    public float Threshold = 0.99f;
    public GameObject[] DisabledObjects = new GameObject[0];
    public Collider[] DisabledColliders = new Collider[0];
    public Collider2D[] Disabled2DColliders = new Collider2D[0];
    private UIScrollBar _scrollbar;
    private bool _isEnabled = true;

    public void Start()
    {
      this._scrollbar = this.GetComponent<UIScrollBar>();
      this.Toggle();
    }

    public void Update()
    {
      if ((double) this._scrollbar.barSize >= (double) this.Threshold)
      {
        if (!this._isEnabled)
          return;
        this._isEnabled = false;
        this.Toggle();
      }
      else
      {
        if (this._isEnabled)
          return;
        this._isEnabled = true;
        this.Toggle();
      }
    }

    private void Toggle()
    {
      for (int index = 0; index < this.DisabledObjects.Length; ++index)
      {
        if ((Object) this.DisabledObjects[index] != (Object) null && this.DisabledObjects[index].activeSelf != this._isEnabled)
          this.DisabledObjects[index].SetActive(this._isEnabled);
      }
      for (int index = 0; index < this.DisabledColliders.Length; ++index)
      {
        if ((Object) this.DisabledColliders[index] != (Object) null && this.DisabledColliders[index].enabled != this._isEnabled)
          this.DisabledColliders[index].enabled = this._isEnabled;
      }
      for (int index = 0; index < this.Disabled2DColliders.Length; ++index)
      {
        if ((Object) this.Disabled2DColliders[index] != (Object) null && this.Disabled2DColliders[index].enabled != this._isEnabled)
          this.Disabled2DColliders[index].enabled = this._isEnabled;
      }
    }
  }
}
