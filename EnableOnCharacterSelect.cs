// Decompiled with JetBrains decompiler
// Type: EnableOnCharacterSelect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public class EnableOnCharacterSelect : MonoBehaviour
{
  public GameObject[] EnableOnSelect;
  private bool _prevLocalCharacterExists;

  public void Update()
  {
    if (Entity.LocalPlayerExists != this._prevLocalCharacterExists)
    {
      foreach (GameObject gameObject in this.EnableOnSelect)
      {
        if ((Object) gameObject != (Object) null)
          gameObject.SetActive(Entity.LocalPlayerExists);
      }
    }
    this._prevLocalCharacterExists = Entity.LocalPlayerExists;
  }
}
