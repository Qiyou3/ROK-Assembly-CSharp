// Decompiled with JetBrains decompiler
// Type: AnimatedAlpha
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class AnimatedAlpha : MonoBehaviour
{
  [Range(0.0f, 1f)]
  public float alpha = 1f;
  private UIWidget mWidget;
  private UIPanel mPanel;

  public void OnEnable()
  {
    this.mWidget = this.GetComponent<UIWidget>();
    this.mPanel = this.GetComponent<UIPanel>();
    this.LateUpdate();
  }

  public void LateUpdate()
  {
    if ((Object) this.mWidget != (Object) null)
      this.mWidget.alpha = this.alpha;
    if (!((Object) this.mPanel != (Object) null))
      return;
    this.mPanel.alpha = this.alpha;
  }
}
