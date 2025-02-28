// Decompiled with JetBrains decompiler
// Type: AnimatedColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (UIWidget))]
[ExecuteInEditMode]
public class AnimatedColor : MonoBehaviour
{
  public Color color = Color.white;
  private UIWidget mWidget;

  public void OnEnable()
  {
    this.mWidget = this.GetComponent<UIWidget>();
    this.LateUpdate();
  }

  public void LateUpdate() => this.mWidget.color = this.color;
}
