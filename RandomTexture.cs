// Decompiled with JetBrains decompiler
// Type: ThirdParty.Extensions.NGUI.RandomTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ThirdParty.Extensions.NGUI
{
  [RequireComponent(typeof (UITexture))]
  public class RandomTexture : MonoBehaviour
  {
    public Texture[] Textures = new Texture[0];

    public void OnEnable()
    {
      if (this.Textures == null || this.Textures.Length <= 0)
        return;
      int index = Random.Range(0, this.Textures.Length);
      if (!((Object) this.Textures[index] != (Object) null))
        return;
      this.GetComponent<UITexture>().mainTexture = this.Textures[index];
    }
  }
}
