// Decompiled with JetBrains decompiler
// Type: ThirdParty.Extensions.NGUI.RandomLabel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ThirdParty.Extensions.NGUI
{
  [RequireComponent(typeof (UILabel))]
  public class RandomLabel : MonoBehaviour
  {
    public TextAsset Strings;

    public void OnEnable()
    {
      if (!((Object) this.Strings != (Object) null))
        return;
      string[] strArray = this.Strings.text.Split('\n');
      if (strArray.Length <= 0)
        return;
      int index = Random.Range(0, strArray.Length);
      if (string.IsNullOrEmpty(strArray[index]))
        return;
      this.GetComponent<UILabel>().text = strArray[index];
    }
  }
}
