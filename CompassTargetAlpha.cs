// Decompiled with JetBrains decompiler
// Type: ThirdParty.CompassBar.CompassTargetAlpha
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
namespace ThirdParty.CompassBar
{
  [RequireComponent(typeof (CompassTarget))]
  public class CompassTargetAlpha : EntityBehaviour
  {
    public bool UseDistanceAlpha = true;
    [SerializeField]
    [Range(0.0f, 1f)]
    private float _fadedAlpha;
    public float FadedAlphaRadius = 100f;
    [Range(0.0f, 1f)]
    [SerializeField]
    private float _strongAlpha = 1f;
    public float StrongAlphaRadius = 25f;
    public bool UseExternalAlpha = true;
    [Range(0.0f, 1f)]
    [SerializeField]
    private float _externalAlpha = 1f;
    private float _distanceAlpha;
    private float _minimumAlpha;
    private float _maximumAlpha;
    private CompassTarget _target;
    private float _alpha;

    public float FadedAlpha
    {
      get => this._fadedAlpha;
      set => this._fadedAlpha = Mathf.Clamp01(value);
    }

    public float StrongAlpha
    {
      get => this._strongAlpha;
      set => this._strongAlpha = Mathf.Clamp01(value);
    }

    public float ExternalAlpha
    {
      get => this._externalAlpha;
      set => this._externalAlpha = Mathf.Clamp01(value);
    }

    public float DistanceAlpha
    {
      get => this._distanceAlpha;
      set => this._distanceAlpha = Mathf.Clamp01(value);
    }

    public float MinimumAlpha
    {
      get => this._minimumAlpha;
      set => this._minimumAlpha = Mathf.Clamp01(value);
    }

    public float MaximumAlpha
    {
      get => this._maximumAlpha;
      set => this._maximumAlpha = Mathf.Clamp01(value);
    }

    public void Start()
    {
      this._minimumAlpha = 0.0f;
      this._maximumAlpha = 1f;
      this._target = this.GetComponent<CompassTarget>();
      this._alpha = this._target.IconData.Colour.a;
    }

    public void OnEnable()
    {
      if (!((Object) this._target != (Object) null))
        return;
      this._alpha = this._target.IconData.Colour.a;
    }

    public void OnDisable()
    {
      if (!((Object) this._target != (Object) null))
        return;
      this._target.IconData.Colour.a = this._alpha;
    }

    public void LateUpdate()
    {
      if (!this._target.IconData.Visible)
        return;
      if (this.UseDistanceAlpha)
      {
        float distance = this._target.Distance;
        this.DistanceAlpha = (double) distance > (double) this.StrongAlphaRadius ? ((double) distance < (double) this.FadedAlphaRadius ? Mathf.Lerp(this.StrongAlpha, this.FadedAlpha, (distance - this.StrongAlphaRadius) / (this.FadedAlphaRadius - this.StrongAlphaRadius)) : this.FadedAlpha) : this.StrongAlpha;
        if (this.UseExternalAlpha)
          this._target.IconData.Colour.a = Mathf.Clamp(this.DistanceAlpha * this.ExternalAlpha, this.MinimumAlpha, this.MaximumAlpha);
        else
          this._target.IconData.Colour.a = Mathf.Clamp(this.DistanceAlpha, this.MinimumAlpha, this.MaximumAlpha);
      }
      else if (this.UseExternalAlpha)
        this._target.IconData.Colour.a = Mathf.Clamp(this.ExternalAlpha, this.MinimumAlpha, this.MaximumAlpha);
      else
        this._target.IconData.Colour.a = Mathf.Clamp(this._alpha, this.MinimumAlpha, this.MaximumAlpha);
    }
  }
}
