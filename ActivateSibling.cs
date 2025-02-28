// Decompiled with JetBrains decompiler
// Type: ThirdParty.Extensions.NGUI.ActivateSibling
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using UnityEngine;

#nullable disable
namespace ThirdParty.Extensions.NGUI
{
  public class ActivateSibling : MonoBehaviour
  {
    public GameObject Target;
    public bool IsInverted;

    public void OnEnable() => this.Target.TryActivate(!this.IsInverted);

    public void OnDisable() => this.Target.TryActivate(this.IsInverted);
  }
}
