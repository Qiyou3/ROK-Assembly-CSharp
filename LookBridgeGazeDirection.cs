// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.LookBridgeGazeDirection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class LookBridgeGazeDirection : AIBehaviour
  {
    public bool lookAwayFrom;
    private Transform _eyesCenter;
    private LookBridge _lookBridge;

    private Transform EyesCenter
    {
      get
      {
        if ((Object) this._eyesCenter == (Object) null)
          this._eyesCenter = this.Entity.GetOrCreate<CharacterDefinition>().GetTransform(CharacterDefinition.Part.EyesCenter);
        return this._eyesCenter;
      }
    }

    private LookBridge MyLookBridge
    {
      get
      {
        if ((Object) this._lookBridge == (Object) null)
          this._lookBridge = this.Entity.GetOrCreate<LookBridge>();
        return this._lookBridge;
      }
    }

    public void Update()
    {
      if (this.CurrentLocation == null)
        return;
      Vector3 worldPosition = this.CurrentLocation.WorldPosition;
      Vector3 position = this.EyesCenter.position;
      if (this.lookAwayFrom)
        this.MyLookBridge.Rotation = Quaternion.LookRotation(position - worldPosition);
      else
        this.MyLookBridge.Rotation = Quaternion.LookRotation(worldPosition - position);
    }
  }
}
