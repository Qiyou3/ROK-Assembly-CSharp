// Decompiled with JetBrains decompiler
// Type: ThirdParty.CompassBar.Compass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ThirdParty.CompassBar
{
  public class Compass : MonoBehaviour
  {
    public bool InvertOffsetAxisY;
    public Transform Player;
    internal float Compass_Width;
    internal float Compass_Height;
    public float Percentage_Compass_Pos_Width = 0.5f;
    public float Percentage_Compass_Pos_Height = 0.1f;
    public float Icon_Width = 25f;
    public float Icon_Height = 25f;
    public float Aspect_Ratio_Horizontal = 1f;
    public float Compass_Border_Indent = 25f;
    public Texture TARGET;
    public CompassIconData Background;
    public CompassIconData ArrowTop;
    public CompassIconData ArrowBottom;

    public static Compass Instance { get; private set; }

    public void Awake()
    {
      if ((Object) Compass.Instance == (Object) null)
        Compass.Instance = this;
      this.Compass_Width = this.Background.Width;
      this.Compass_Height = this.Background.Height;
      this.Background.Bounds = new Rect(0.0f, 0.0f, this.Compass_Width, this.Compass_Height);
      this.ArrowTop.Bounds = this.ArrowTop.Bounds;
      this.ArrowBottom.Bounds = this.ArrowBottom.Bounds;
    }

    public void Start()
    {
      if (!((Object) this.Player == (Object) null))
        return;
      this.Player = Camera.main.transform;
    }

    public void LateUpdate()
    {
      if (!((Object) this.Player != (Object) null))
        return;
      this.UpdateCompass();
    }

    private void UpdateCompass()
    {
      float y = this.Player.eulerAngles.y;
      this.UpdateIconData(this.ArrowTop);
      this.UpdateIconData(this.ArrowBottom);
      if (CompassDirection.ActiveDirections != null)
      {
        for (int index = 0; index < CompassDirection.ActiveDirections.Count; ++index)
          this.UpdateCompassObject(CompassDirection.ActiveDirections[index], y);
      }
      if (CompassTarget.ActiveTargets != null)
      {
        for (int index = 0; index < CompassTarget.ActiveTargets.Count; ++index)
          this.UpdateCompassObject(CompassTarget.ActiveTargets[index], y);
      }
      this.UpdateIconData(this.Background);
    }

    private void UpdateCompassObject(CompassItem item, float playerRotation)
    {
      item.Distance = Vector3.Distance(this.Player.position, item.transform.position);
      if (item.IconData.Persistent || (double) item.Distance < (double) item.IconData.Distance)
      {
        float atanDeg = item.AtanDeg;
        float num;
        if ((double) atanDeg < 180.0)
        {
          if ((double) playerRotation < 180.0)
          {
            num = (-playerRotation + atanDeg) * this.Aspect_Ratio_Horizontal;
          }
          else
          {
            num = (float) (-(double) playerRotation + (double) atanDeg + 360.0) * this.Aspect_Ratio_Horizontal;
            if ((double) num > 270.0)
              num = (-playerRotation + atanDeg) * this.Aspect_Ratio_Horizontal;
          }
        }
        else
          num = (double) playerRotation >= 180.0 ? (-playerRotation + atanDeg) * this.Aspect_Ratio_Horizontal : ((double) atanDeg > 270.0 ? (float) (-(double) playerRotation + (double) atanDeg - 360.0) * this.Aspect_Ratio_Horizontal : (-playerRotation + atanDeg) * this.Aspect_Ratio_Horizontal);
        item.IconData.Rotation = num;
        this.UpdateIconData(item.IconData);
      }
      else
        item.IconData.Visible = false;
    }

    private bool IsVisible(float position)
    {
      float num = (float) ((double) this.Compass_Width * 0.5 - (double) this.Icon_Width * 0.5) - this.Compass_Border_Indent;
      return (double) position >= -(double) num && (double) position <= (double) num;
    }

    private void UpdateIconData(CompassIconData data)
    {
      if (data == null || !(data.Visible = this.IsVisible(data.Rotation)))
        return;
      data.Position.x = (float) Screen.width * this.Percentage_Compass_Pos_Width + data.Rotation;
      data.Position.y = (float) Screen.height * this.Percentage_Compass_Pos_Height;
    }

    public enum IconOffsets
    {
      Center,
      Above,
      Below,
    }
  }
}
