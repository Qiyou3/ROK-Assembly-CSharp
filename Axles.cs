// Decompiled with JetBrains decompiler
// Type: Axles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Rigidbody))]
[ExecuteInEditMode]
public class Axles : MonoBehaviour
{
  [HideInInspector]
  public Wheel[] otherWheels = new Wheel[0];
  [HideInInspector]
  public Wheel[] allWheels;
  public Axle frontAxle = new Axle();
  public Axle rearAxle = new Axle();
  public Axle[] otherAxles = new Axle[0];

  private void CheckWheels()
  {
    if ((UnityEngine.Object) this.frontAxle.leftWheel == (UnityEngine.Object) null)
      this.LogWarning<Axles>("UnityCar: front left wheel not assigned  (" + this.transform.name + ")");
    if ((UnityEngine.Object) this.frontAxle.rightWheel == (UnityEngine.Object) null)
      this.LogWarning<Axles>("UnityCar: front right wheel not assigned  (" + this.transform.name + ")");
    if ((UnityEngine.Object) this.rearAxle.leftWheel == (UnityEngine.Object) null)
      this.LogWarning<Axles>("UnityCar: rear left wheel not assigned  (" + this.transform.name + ")");
    if (!((UnityEngine.Object) this.rearAxle.rightWheel == (UnityEngine.Object) null))
      return;
    this.LogWarning<Axles>("UnityCar: rear right wheel not assigned  (" + this.transform.name + ")");
  }

  public void Start() => this.CheckWheels();

  public void Awake() => this.SetWheels();

  public void SetWheels()
  {
    if ((bool) (UnityEngine.Object) this.frontAxle.leftWheel)
      this.frontAxle.leftWheel.wheelPos = WheelPos.FRONT_LEFT;
    if ((bool) (UnityEngine.Object) this.frontAxle.rightWheel)
      this.frontAxle.rightWheel.wheelPos = WheelPos.FRONT_RIGHT;
    if ((bool) (UnityEngine.Object) this.rearAxle.leftWheel)
      this.rearAxle.leftWheel.wheelPos = WheelPos.REAR_LEFT;
    if ((bool) (UnityEngine.Object) this.rearAxle.rightWheel)
      this.rearAxle.rightWheel.wheelPos = WheelPos.REAR_RIGHT;
    this.frontAxle.wheels = new Wheel[0];
    if ((UnityEngine.Object) this.frontAxle.leftWheel != (UnityEngine.Object) null && (UnityEngine.Object) this.frontAxle.rightWheel != (UnityEngine.Object) null)
    {
      this.frontAxle.wheels = new Wheel[2];
      this.frontAxle.wheels[0] = this.frontAxle.leftWheel;
      this.frontAxle.wheels[1] = this.frontAxle.rightWheel;
    }
    else if ((UnityEngine.Object) this.frontAxle.leftWheel != (UnityEngine.Object) null || (UnityEngine.Object) this.frontAxle.rightWheel != (UnityEngine.Object) null)
    {
      this.frontAxle.wheels = new Wheel[1];
      this.frontAxle.wheels[0] = !((UnityEngine.Object) this.frontAxle.leftWheel != (UnityEngine.Object) null) ? this.frontAxle.rightWheel : this.frontAxle.leftWheel;
    }
    this.frontAxle.camber = Mathf.Clamp(this.frontAxle.camber, -10f, 10f);
    this.rearAxle.wheels = new Wheel[0];
    if ((UnityEngine.Object) this.rearAxle.leftWheel != (UnityEngine.Object) null && (UnityEngine.Object) this.rearAxle.rightWheel != (UnityEngine.Object) null)
    {
      this.rearAxle.wheels = new Wheel[2];
      this.rearAxle.wheels[0] = this.rearAxle.leftWheel;
      this.rearAxle.wheels[1] = this.rearAxle.rightWheel;
    }
    else if ((UnityEngine.Object) this.rearAxle.leftWheel != (UnityEngine.Object) null || (UnityEngine.Object) this.rearAxle.rightWheel != (UnityEngine.Object) null)
    {
      this.rearAxle.wheels = new Wheel[1];
      this.rearAxle.wheels[0] = !((UnityEngine.Object) this.rearAxle.leftWheel != (UnityEngine.Object) null) ? this.rearAxle.rightWheel : this.rearAxle.leftWheel;
    }
    this.rearAxle.camber = Mathf.Clamp(this.rearAxle.camber, -10f, 10f);
    Wheel[] wheelArray = new Wheel[this.otherAxles.Length * 2];
    int length = 0;
    foreach (Axle otherAxle in this.otherAxles)
    {
      if ((UnityEngine.Object) otherAxle.leftWheel != (UnityEngine.Object) null && (UnityEngine.Object) otherAxle.rightWheel != (UnityEngine.Object) null)
      {
        otherAxle.wheels = new Wheel[2];
        otherAxle.wheels[0] = wheelArray[length] = otherAxle.leftWheel;
        otherAxle.wheels[1] = wheelArray[length + 1] = otherAxle.rightWheel;
        length += 2;
      }
      else
      {
        otherAxle.wheels = new Wheel[1];
        otherAxle.wheels[0] = !((UnityEngine.Object) otherAxle.leftWheel != (UnityEngine.Object) null) ? (wheelArray[0] = otherAxle.rightWheel) : (wheelArray[0] = otherAxle.leftWheel);
        ++length;
      }
      otherAxle.camber = Mathf.Clamp(otherAxle.camber, -10f, 10f);
    }
    this.otherWheels = new Wheel[length];
    wheelArray.CopyTo((Array) this.otherWheels, 0);
    this.allWheels = new Wheel[this.frontAxle.wheels.Length + this.rearAxle.wheels.Length + this.otherWheels.Length];
    this.frontAxle.wheels.CopyTo((Array) this.allWheels, 0);
    this.rearAxle.wheels.CopyTo((Array) this.allWheels, this.frontAxle.wheels.Length);
    if (this.otherWheels.Length == 0)
      return;
    this.otherWheels.CopyTo((Array) this.allWheels, this.frontAxle.wheels.Length + this.rearAxle.wheels.Length);
  }
}
