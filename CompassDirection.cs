// Decompiled with JetBrains decompiler
// Type: ThirdParty.CompassBar.CompassDirection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ThirdParty.CompassBar
{
  public class CompassDirection : CompassItem
  {
    public static List<CompassItem> ActiveDirections = new List<CompassItem>();
    [Range(0.0f, 360f)]
    public float Rotation;

    public override float AtanDeg => this.Rotation;

    public void OnEnable() => CompassDirection.ActiveDirections.Add((CompassItem) this);

    public void OnDisable() => CompassDirection.ActiveDirections.Remove((CompassItem) this);
  }
}
