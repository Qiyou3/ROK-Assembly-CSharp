// Decompiled with JetBrains decompiler
// Type: DefaultTalkController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using MoPhoGames.USpeak.Interface;
using UnityEngine;

#nullable disable
[AddComponentMenu("USpeak/Default Talk Controller")]
public class DefaultTalkController : MonoBehaviour, IUSpeakTalkController
{
  [SerializeField]
  [HideInInspector]
  public KeyCode TriggerKey;
  [HideInInspector]
  [SerializeField]
  public int ToggleMode;
  private bool val;

  public void OnInspectorGUI()
  {
  }

  public bool ShouldSend()
  {
    if (this.ToggleMode == 0)
      this.val = Input.GetKey(this.TriggerKey);
    else if (Input.GetKeyDown(this.TriggerKey))
      this.val = !this.val;
    return this.val;
  }
}
