// Decompiled with JetBrains decompiler
// Type: DestroyOnWorldSwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using UnityEngine;

#nullable disable
public class DestroyOnWorldSwitch : MonoBehaviour, IResetable
{
  public ResetableOrder ResetOrder => ResetableOrder.Default;

  public void OnResetScene()
  {
  }

  public void OnClearScene()
  {
  }

  public void OnSwitch() => Object.Destroy((Object) this.gameObject);
}
