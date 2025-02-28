// Decompiled with JetBrains decompiler
// Type: ButtonHold
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class ButtonHold : MonoBehaviour
{
  public string button;
  public bool acceptsMouseInput = true;
  private float lastButtonPressTime;
  public float buttonHoldTime = 0.5f;
  private bool buttonUpWasLastFrame;
  public ButtonHold.Message[] startHoldMessages;
  public ButtonHold.Message[] endHoldMessages;

  public void Start() => this.lastButtonPressTime = Time.timeSinceLevelLoad;

  public void Update()
  {
    if (Input.GetButtonDown(this.button))
      this.lastButtonPressTime = Time.timeSinceLevelLoad;
    if (Input.GetButton(this.button) && (double) Time.timeSinceLevelLoad - (double) this.lastButtonPressTime > (double) this.buttonHoldTime)
    {
      foreach (ButtonHold.Message startHoldMessage in this.startHoldMessages)
        startHoldMessage.component.SendMessage(startHoldMessage.message);
    }
    if (this.buttonUpWasLastFrame)
    {
      foreach (ButtonHold.Message endHoldMessage in this.endHoldMessages)
        endHoldMessage.component.SendMessage(endHoldMessage.message);
      this.buttonUpWasLastFrame = false;
    }
    if (!Input.GetButtonUp(this.button))
      return;
    this.buttonUpWasLastFrame = true;
  }

  [Serializable]
  public class Message
  {
    public Component component;
    public string message;
  }
}
