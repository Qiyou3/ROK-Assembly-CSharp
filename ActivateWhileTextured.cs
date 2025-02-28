// Decompiled with JetBrains decompiler
// Type: ThirdParty.Extensions.NGUI.ActivateWhileTextured
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using UnityEngine;

#nullable disable
namespace ThirdParty.Extensions.NGUI
{
  public class ActivateWhileTextured : MonoBehaviour
  {
    public UITexture Texture;
    public GameObject Target;
    public bool IsInverted;

    public void Update()
    {
      if (!((Object) this.Texture != (Object) null))
        return;
      if (this.IsInverted)
        this.Target.TryActivate((Object) this.Texture.mainTexture == (Object) null);
      else
        this.Target.TryActivate((Object) this.Texture.mainTexture != (Object) null);
    }
  }
}
