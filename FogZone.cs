// Decompiled with JetBrains decompiler
// Type: FogZone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FogZone : MonoBehaviour
{
  public float ZoneRadius = 40f;
  public FogVolumeController.BiomeFogVolumeSetting BiomeFogVolumeSetting;
  public static List<FogZone> FogZones = new List<FogZone>();

  public void OnEnable() => FogZone.FogZones.Add(this);

  public void OnDisable() => FogZone.FogZones.Remove(this);

  public bool IsInZone(Vector3 position)
  {
    return (double) Vector3.Distance(position, this.transform.position) < (double) this.ZoneRadius;
  }
}
