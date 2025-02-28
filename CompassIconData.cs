// Decompiled with JetBrains decompiler
// Type: ThirdParty.CompassBar.CompassIconData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace ThirdParty.CompassBar
{
  [Serializable]
  public class CompassIconData
  {
    [SerializeField]
    private Texture _icon;
    public Color Colour;
    [SerializeField]
    private Rect _bounds;
    private Vector2 _offset;
    [SerializeField]
    private Compass.IconOffsets _directionalOffset;
    private Vector3 _scale;
    public bool Persistent;
    public float Distance;
    public bool Visible;
    [HideInInspector]
    public Vector2 Position;
    [HideInInspector]
    public float Rotation;

    public CompassIconData()
    {
      this._offset = new Vector2(0.0f, 0.0f);
      this._scale = new Vector3(1f, 1f, 1f);
      this.Position = new Vector2();
      this.Rotation = 0.0f;
      this.Distance = 0.0f;
      this.Visible = false;
    }

    public CompassIconData(
      Texture icon,
      Color colour,
      Compass.IconOffsets offset,
      bool persistent,
      Rect bounds)
    {
      this._offset = new Vector2(0.0f, 0.0f);
      this._scale = new Vector3(1f, 1f, 1f);
      this.Bounds = bounds;
      this.Icon = icon;
      this.Colour = colour;
      this.DirectionalOffset = offset;
      this.Persistent = persistent;
      this.Position = new Vector2();
      this.Rotation = 0.0f;
      this.Distance = 0.0f;
      this.Visible = false;
    }

    public Texture Icon
    {
      get
      {
        if ((UnityEngine.Object) this._icon != (UnityEngine.Object) null)
          return this._icon;
        return (UnityEngine.Object) Compass.Instance != (UnityEngine.Object) null ? Compass.Instance.TARGET : (Texture) null;
      }
      set => this._icon = value;
    }

    public Rect Bounds
    {
      get => this._bounds;
      set
      {
        this._bounds.x = value.x;
        this._bounds.y = value.y;
        this._bounds.width = (double) value.width != 0.0 ? value.width : (!((UnityEngine.Object) Compass.Instance != (UnityEngine.Object) null) ? 0.0f : Compass.Instance.Icon_Width);
        this._bounds.height = (double) value.height != 0.0 ? value.height : (!((UnityEngine.Object) Compass.Instance != (UnityEngine.Object) null) ? 0.0f : Compass.Instance.Icon_Height);
      }
    }

    public Vector2 Offset
    {
      get
      {
        float num1 = 0.0f;
        float num2 = 0.0f;
        if (this.DirectionalOffset == Compass.IconOffsets.Above)
          num2 = (!((UnityEngine.Object) Compass.Instance != (UnityEngine.Object) null) ? 0 : (Compass.Instance.InvertOffsetAxisY ? 1 : 0)) == 0 ? -1f : 1f;
        else if (this.DirectionalOffset == Compass.IconOffsets.Below)
          num2 = (!((UnityEngine.Object) Compass.Instance != (UnityEngine.Object) null) ? 0 : (Compass.Instance.InvertOffsetAxisY ? 1 : 0)) == 0 ? 1f : -1f;
        this._offset.x = this._bounds.x + (float) ((!((UnityEngine.Object) Compass.Instance != (UnityEngine.Object) null) ? 0.0 : (double) Compass.Instance.Compass_Width) * 0.5) * num1;
        this._offset.y = this._bounds.y + (!((UnityEngine.Object) Compass.Instance != (UnityEngine.Object) null) ? 0.0f : Compass.Instance.Compass_Height) * num2;
        return this._offset;
      }
    }

    public Compass.IconOffsets DirectionalOffset
    {
      get => this._directionalOffset;
      set => this._directionalOffset = value;
    }

    public Vector3 Scale
    {
      get
      {
        this._scale.x = this._bounds.width;
        this._scale.y = this._bounds.height;
        return this._scale;
      }
    }

    public float Width => this._bounds.width;

    public float Height => this._bounds.height;
  }
}
