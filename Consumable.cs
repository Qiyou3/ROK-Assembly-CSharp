// Decompiled with JetBrains decompiler
// Type: Consumable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Consumable : MonoBehaviour
{
  public void ConsumeItem(InvGameItemStack itemStack)
  {
    int amount = 1;
    SingletonMonoBehaviour<InvEquipment>.Instance.UseAmount(itemStack, ref amount);
    InvConsumeDef component = itemStack.Blueprint.GetComponent<InvConsumeDef>();
    if (!(bool) (UnityEngine.Object) component || component.ReduceHunger <= 0)
      return;
    this.ReduceHunger(component.ReduceHunger);
    AudioController.Play(component.PlaySoundOnConsume);
  }

  public void ReduceHunger(int amount) => throw new NotImplementedException();
}
