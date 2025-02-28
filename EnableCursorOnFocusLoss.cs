// Decompiled with JetBrains decompiler
// Type: EnableCursorOnFocusLoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EnableCursorOnFocusLoss : MonoBehaviour
{
  public void Awake() => this.enabled = false;

  public void Update()
  {
    if (Input.anyKeyDown)
    {
      if (Input.GetKeyDown(KeyCode.LeftWindows) || Input.GetKeyDown(KeyCode.RightWindows))
        return;
      this.enabled = false;
    }
    else
      DisableCursor.EnableCursorOneFrame();
  }

  public void OnApplicationFocus(bool isFocused)
  {
    if (isFocused)
      return;
    this.enabled = true;
  }
}
