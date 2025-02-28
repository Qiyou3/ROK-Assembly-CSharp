// Decompiled with JetBrains decompiler
// Type: CharacterThirdPersonCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch;
using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public class CharacterThirdPersonCamera : CameraThirdPersonTrace, IEntityAware
{
  private LookBridge _lookBridge;
  private CameraSwitch _cameraSwitch;
  private CameraSwitchPoint _switchPoint;

  private CameraSwitch CameraSwitch
  {
    get
    {
      if ((Object) this._cameraSwitch == (Object) null)
        this._cameraSwitch = Camera.main.GetComponent<CameraSwitch>();
      return this._cameraSwitch;
    }
  }

  private CameraSwitchPoint SwitchPoint
  {
    get
    {
      if ((Object) this._switchPoint == (Object) null)
        this._switchPoint = this.GetComponentInChildren<CameraSwitchPoint>();
      return this._switchPoint;
    }
  }

  public void Start()
  {
    this.mount = this.Entity.GetOrCreate<CharacterDefinition>().GetTransform(CharacterDefinition.Part.EyesCenter);
    this.shakeLookDir = this.Entity.Get<MainRigidbody>().transform;
    this._lookBridge = this.Entity.GetOrCreate<LookBridge>();
  }

  public new void LateUpdate()
  {
    if ((Object) this.CameraSwitch != (Object) null && (Object) this.CameraSwitch.currentSwitchPoint != (Object) this.SwitchPoint && (Object) this.CameraSwitch.previousSwitchPoint != (Object) this.SwitchPoint)
      return;
    this.lookDir = (Transform) null;
    this.lookQuat = this._lookBridge.Rotation;
    base.LateUpdate();
  }
}
