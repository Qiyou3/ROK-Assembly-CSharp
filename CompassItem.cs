// Decompiled with JetBrains decompiler
// Type: ThirdParty.CompassBar.CompassItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ThirdParty.CompassBar
{
  public class CompassItem : MonoBehaviour
  {
    [SerializeField]
    private CompassIconData _data;
    private float _distance;

    public CompassIconData IconData => this._data;

    public virtual float AtanDeg => 0.0f;

    public virtual float Distance
    {
      get => this._distance;
      set => this._distance = value;
    }

    public void Start() => this._data.Bounds = this._data.Bounds;
  }
}
