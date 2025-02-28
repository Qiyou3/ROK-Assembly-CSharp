// Decompiled with JetBrains decompiler
// Type: DestroyOnReset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using UnityEngine;

#nullable disable
public class DestroyOnReset : MonoBehaviour, IResetable
{
  public ResetableOrder ResetOrder => ResetableOrder.Default;

  public void OnResetScene() => Object.Destroy((Object) this.gameObject);

  public void OnClearScene() => Object.Destroy((Object) this.gameObject);

  public void OnSwitch() => Object.Destroy((Object) this.gameObject);
}
