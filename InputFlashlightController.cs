// Decompiled with JetBrains decompiler
// Type: InputFlashlightController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Core.Input;
using UnityEngine;

#nullable disable
public class InputFlashlightController : EntityBehaviour
{
  public KeyButton flashlightButton = new KeyButton("Flashlight", KeyCode.F);
  public string onSound;
  public string offSound;

  public void Update()
  {
    Flashlight flashlight = this.Entity.TryGet<Flashlight>();
    if ((Object) flashlight == (Object) null || !this.flashlightButton.GetDown())
      return;
    flashlight.on = !flashlight.on;
    if (flashlight.on)
      AudioController.Play(this.onSound, this.Entity.MainTransform);
    else
      AudioController.Play(this.offSound, this.Entity.MainTransform);
  }
}
