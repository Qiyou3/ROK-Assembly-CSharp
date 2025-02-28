// Decompiled with JetBrains decompiler
// Type: ClippingPanelStretch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ClippingPanelStretch : MonoBehaviour
{
  public UIPanel Panel;
  public Vector2 SizeRelativeToScreen = Vector2.one;
  private Vector2 _prevSizeRelativeToScreen = Vector2.one;
  public Vector2 WorldToScreen = Vector2.one;
  private Vector2 _prevWorldToScreen = Vector2.one;
  private Vector2 _prevScreenSize = Vector2.zero;

  public void Update()
  {
    if ((double) this._prevScreenSize.x == (double) Screen.width && (double) this._prevScreenSize.y == (double) Screen.height && !(this._prevSizeRelativeToScreen != this.SizeRelativeToScreen) && !(this._prevWorldToScreen != this.WorldToScreen))
      return;
    this._prevScreenSize = new Vector2((float) Screen.width, (float) Screen.height);
    this._prevSizeRelativeToScreen = this.SizeRelativeToScreen;
    this._prevWorldToScreen = this.WorldToScreen;
    this.Panel.clipRange = new Vector4(this.Panel.clipRange.x, this.Panel.clipRange.y, (double) this.SizeRelativeToScreen.x != 0.0 ? (float) Screen.width * this.WorldToScreen.x * this.SizeRelativeToScreen.x : this.Panel.clipRange.z, (double) this.SizeRelativeToScreen.y != 0.0 ? (float) Screen.height * this.WorldToScreen.y * this.SizeRelativeToScreen.y : this.Panel.clipRange.w);
  }
}
