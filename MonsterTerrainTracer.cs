// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.MonsterTerrainTracer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Tracing;
using System;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  [Serializable]
  public class MonsterTerrainTracer
  {
    public float TracerRayLength = 3f;
    public float GroundNormalUpdateRateMin = 0.75f;
    public float GroundNormalUpdateRateMax = 1.25f;
    public RaycastHit LastRaycastHit;
    private float _groundNormalUpdateTime;

    public void Update(Vector3 tracePosition, TracerIgnoreParams ignore)
    {
      if (!TimeUtil.ExceededTime(this._groundNormalUpdateTime))
        return;
      this.LastRaycastHit = new Ray(tracePosition, Physics.gravity).Raycast(this.TracerRayLength, ignore);
      this._groundNormalUpdateTime = TimeUtil.ResetTimer(UnityEngine.Random.Range(this.GroundNormalUpdateRateMin, this.GroundNormalUpdateRateMax));
    }
  }
}
