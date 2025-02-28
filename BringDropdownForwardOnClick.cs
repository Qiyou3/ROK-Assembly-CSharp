// Decompiled with JetBrains decompiler
// Type: BringDropdownForwardOnClick
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BringDropdownForwardOnClick : MonoBehaviour
{
  private const string DROPDOWN = "Drop-down List";
  private const float DEPTH_ADJUSTMENT = 5f;

  public void OnClick()
  {
    this.transform.parent.FindChild("Drop-down List").localPosition += Vector3.back * 5f;
  }
}
