// Decompiled with JetBrains decompiler
// Type: GunSpreadCrosshair
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Modules.Inventory.Holdables;
using CodeHatch.Weapons;
using UnityEngine;

#nullable disable
public class GunSpreadCrosshair : EntityBehaviour
{
  public Texture2D hairTexture;
  public Shader hairShader;
  public Mesh quadMesh;
  public int hairCount = 3;
  public float scale = 0.05f;
  public float offset = 1f;
  private float _spread = 5f;
  private GameObject[] _hairs;
  private Material _material;

  public float Spread => this._spread;

  public void Awake()
  {
    this._material = new Material(this.hairShader);
    this._hairs = new GameObject[this.hairCount];
    for (int index = 0; index < this.hairCount; ++index)
    {
      this._hairs[index] = new GameObject("Hair");
      this._hairs[index].transform.parent = this.transform;
      this._hairs[index].transform.localPosition = Vector3.zero;
      this._hairs[index].transform.localScale = new Vector3(1f, 1f, 1f);
      this._hairs[index].transform.localRotation = Quaternion.identity;
      GameObject gameObject = new GameObject("Quad");
      gameObject.layer = this.gameObject.layer;
      gameObject.AddComponent<MeshFilter>().mesh = this.quadMesh;
      gameObject.AddComponent<MeshRenderer>();
      gameObject.AddComponent<CameraUIObject>().pivot = this.transform;
      gameObject.GetComponent<Renderer>().material = this._material;
      gameObject.GetComponent<Renderer>().material.mainTexture = (Texture) this.hairTexture;
      gameObject.transform.parent = this._hairs[index].transform;
      gameObject.transform.localPosition = Vector3.forward + Vector3.right * this.scale * 0.5f;
      gameObject.transform.localScale = new Vector3(1f, 1f, 1f) * this.scale;
      gameObject.transform.localRotation = Quaternion.identity;
    }
  }

  public void LateUpdate()
  {
    this.transform.rotation = this.Entity.GetOrCreate<LookBridge>().Rotation;
    if (this.Entity.Has<GunShake>())
      this.transform.rotation = this.Entity.Get<GunShake>().ProcessRotation(this.transform.rotation);
    GunAimCrosshair gunAimCrosshair = this.Entity.Get<GunAimCrosshair>();
    gunAimCrosshair.UpdateCrosshair();
    this._spread = (float) (((double) Vector3.Angle(gunAimCrosshair.transform.forward, this.transform.forward) + (double) gunAimCrosshair.Spread) / 2.0);
    bool flag = this.ShouldDisplayCrosshair();
    for (int index = 0; index < this.hairCount; ++index)
    {
      this._hairs[index].transform.localRotation = Quaternion.AngleAxis((float) ((double) index / (double) this.hairCount * 360.0 + (this.hairCount % 2 != 1 ? 0.0 : 90.0)), Vector3.forward) * Quaternion.AngleAxis(this._spread + this.offset, Vector3.up);
      this._hairs[index].GetComponentInChildren<Renderer>().enabled = flag;
    }
  }

  private bool ShouldDisplayCrosshair()
  {
    BipedHolder bipedHolder = this.Entity.Get<BipedHolder>();
    if ((Object) bipedHolder == (Object) null)
      return false;
    this.LogError<GunSpreadCrosshair>("TODO: since multiple holdables are now possible,more may need to be done to determine whether any of the items being held require a crosshair.\nCurrently, it assumes that only the most precedent item may fire.");
    BipedHoldable holdable = bipedHolder.TryGetHoldable(0);
    if ((Object) holdable == (Object) null)
      return false;
    Entity entity = holdable.Entity;
    if ((Object) entity.TryGet<GunRecoilAnimation>() == (Object) null)
      return false;
    GunReceiver gunReceiver = entity.TryGet<GunReceiver>();
    if ((Object) gunReceiver == (Object) null || !gunReceiver.CanFire())
      return false;
    GunSightUp gunSightUp = entity.TryGet<GunSightUp>();
    return !((Object) gunSightUp != (Object) null) || !gunSightUp.IsSightedUp() || !gunSightUp.hideGunAimCrosshair;
  }
}
