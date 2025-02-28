// Decompiled with JetBrains decompiler
// Type: ControlTips
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ControlTips : MonoBehaviour
{
  public static ControlTips Instance;
  public GameObject LockKeyTip;
  public GameObject MouseRotateTip;
  public GameObject UseKeyTip;
  public GameObject WasdRotateTip;

  public void Awake() => ControlTips.Instance = this;
}
