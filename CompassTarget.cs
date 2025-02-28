// Decompiled with JetBrains decompiler
// Type: ThirdParty.CompassBar.CompassTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ThirdParty.CompassBar
{
  public class CompassTarget : CompassItem
  {
    public static List<CompassItem> ActiveTargets = new List<CompassItem>();
    private Transform _transform;

    public override float AtanDeg
    {
      get
      {
        float atanDeg = 0.0f;
        if ((Object) Compass.Instance.Player != (Object) null)
        {
          float x1 = Compass.Instance.Player.position.x;
          float z1 = Compass.Instance.Player.position.z;
          float x2 = this._transform.position.x;
          float z2 = this._transform.position.z;
          atanDeg = Mathf.Atan(Mathf.Abs(x1 - x2) / Mathf.Abs(z1 - z2)) * 57.29578f;
          if ((double) z1 >= (double) z2)
          {
            if ((double) x1 < (double) x2)
            {
              if ((double) atanDeg < 90.0 || (double) atanDeg >= 180.0)
                atanDeg = (float) (((double) atanDeg - 90.0) * -1.0 + 90.0);
            }
            else if ((double) x1 >= (double) x2 && ((double) atanDeg < 180.0 || (double) atanDeg >= 270.0))
              atanDeg += 180f;
          }
          if ((double) x1 >= (double) x2 && (double) z1 < (double) z2 && ((double) atanDeg < 270.0 || (double) atanDeg >= 360.0))
            atanDeg = (float) (((double) atanDeg - 90.0) * -1.0 + 270.0);
        }
        return atanDeg;
      }
    }

    public void Awake() => this._transform = this.transform;

    public void OnEnable() => CompassTarget.ActiveTargets.Add((CompassItem) this);

    public void OnDisable() => CompassTarget.ActiveTargets.Remove((CompassItem) this);
  }
}
