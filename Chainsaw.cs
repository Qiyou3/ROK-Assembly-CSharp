// Decompiled with JetBrains decompiler
// Type: Chainsaw
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Damaging;
using CodeHatch.Engine;
using CodeHatch.Engine.Audio;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Core.Utility.Attributes;
using CodeHatch.Engine.Modules.Inventory.Holdables;
using CodeHatch.Engine.Modules.Inventory.Resource;
using CodeHatch.Hitboxes;
using CodeHatch.Shocktree;
using CodeHatch.Tracing;
using SmartAssembly.Attributes;
using UnityEngine;

#nullable disable
[ObfuscateControlFlow]
public class Chainsaw : Harvester, IVolumeAware, ICensorable, IDamager, IHolderListener
{
  public Transform traceStart;
  public Transform traceEnd;
  public Vector3 traceLocalOffset = Vector3.forward;
  public int bladeMaterialIndex = 1;
  public Renderer bladeRenderer;
  public Rigidbody rigidbodyToForce;
  public float torqueCancellation = 0.5f;
  private Material _bladeMaterial;
  public WeaponBridge weaponBridge;
  public float spinSpeedAttackTime = 0.1f;
  public float spinSpeedDecayTime = 0.1f;
  public float spinSpeedDamageThreshold = 0.1f;
  private float _spinSpeed;
  public float maxSpinSpeed = 50f;
  public AudioClip[] squeals;
  private AudioSource _audioSource;
  private Loop currentWoodcuttingLoop;
  public GameObject spark;
  public Vector2 chainTextureOffsetMultiplier = new Vector2(0.77172f, 0.0f);
  private Vector2 _chainTextureOffset = Vector2.zero;
  public float characterDamagePerSecond = 100f;
  public float timeBetweenHits = 0.2f;
  private float _lastHitTime;
  public float rigidbodyForcePerSecond = 50000f;
  [HideInInspector]
  public float resistance;
  public float defaultDamagableResistance = 7f;
  public float defaultNonDamagableResistance = 25f;
  public float resistanceVariation = 1.1f;
  private ChainsawMaterialEffect _currentEffect;
  private float _fadeRate = 1f;
  private Entity _heldBy;
  [SerializeField]
  [Tooltip("The types of damage which are done.")]
  [FlagEnum(typeof (DamageType))]
  private DamageType _DamageTypes = DamageType.Melee | DamageType.Cut;

  public float BGMVolume => AudioVolumeController.BGMVolume;

  public float SFXVolume => AudioVolumeController.SFXVolume;

  public float AFXVolume => AudioVolumeController.AFXVolume;

  public float VFXVolume => AudioVolumeController.VFXVolume;

  public Vector3 TraceStart
  {
    get
    {
      return (Object) this.traceStart == (Object) null ? this.transform.position : this.traceStart.position;
    }
  }

  public Vector3 TraceEnd
  {
    get
    {
      return (Object) this.traceEnd != (Object) null ? this.traceEnd.position : this.TraceStart + this.transform.TransformDirection(this.traceLocalOffset);
    }
  }

  public Vector3 TraceDirection => (this.TraceEnd - this.TraceStart).normalized;

  public float TraceDistance => (this.TraceEnd - this.TraceStart).magnitude;

  public float DamageThisFrame
  {
    get
    {
      return this.characterDamagePerSecond * this._spinSpeed / this.maxSpinSpeed * this.timeBetweenHits;
    }
  }

  public ResourceHandler Handler => this._heldBy.Get<ResourceHandler>();

  public Material BladeMaterial
  {
    get
    {
      if ((Object) this._bladeMaterial == (Object) null)
      {
        if ((Object) this.bladeRenderer == (Object) null)
          this.bladeRenderer = this.GetComponent<Renderer>();
        if ((Object) this.bladeRenderer != (Object) null && this.bladeMaterialIndex >= 0 && this.bladeMaterialIndex < this.bladeRenderer.materials.Length)
          this._bladeMaterial = this.bladeRenderer.materials[this.bladeMaterialIndex];
      }
      return this._bladeMaterial;
    }
  }

  public float SpinSpeedDecayTime => this.spinSpeedDecayTime / (1f + this.resistance);

  public float SpinSpeed => this._spinSpeed;

  private AudioSource AudioSource
  {
    get
    {
      if ((Object) this._audioSource == (Object) null)
        this._audioSource = this.GetComponent<AudioSource>();
      if ((Object) this._audioSource == (Object) null)
        this._audioSource = this.gameObject.AddComponent<AudioSource>();
      return this._audioSource;
    }
  }

  public void Start()
  {
    HoldableGun holdableGun = this.Entity.TryGet<HoldableGun>();
    if (!(bool) (Object) holdableGun)
      return;
    holdableGun.maxAllowedPenetration = Mathf.Max(holdableGun.maxAllowedPenetration, Vector3.Dot(this.Entity.MainTransform.forward, this.TraceEnd - this.TraceStart));
  }

  public void Update()
  {
    if ((Object) this.weaponBridge == (Object) null)
      return;
    float num = !this.weaponBridge.IsFiring ? 0.0f : this.maxSpinSpeed;
    if ((double) this._spinSpeed < (double) num)
      this._spinSpeed += (float) (((double) num - (double) this._spinSpeed) * (1.0 - (double) Mathf.Pow(0.5f, Time.deltaTime / this.spinSpeedAttackTime)));
    else
      this._spinSpeed += (float) (((double) num - (double) this._spinSpeed) * (1.0 - (double) Mathf.Pow(0.5f, Time.deltaTime / this.SpinSpeedDecayTime)));
    this._chainTextureOffset += this._spinSpeed * this.chainTextureOffsetMultiplier * Time.deltaTime;
    if ((Object) this.BladeMaterial != (Object) null)
      this.BladeMaterial.mainTextureOffset = this._chainTextureOffset;
    if ((double) this._spinSpeed >= (double) this.spinSpeedDamageThreshold && (double) Time.time - (double) this._lastHitTime > (double) this.timeBetweenHits)
    {
      if (this.Entity.IsLocallyOwned)
        this.Use(new Vector3?(this.TraceStart), new Vector3?(this.TraceEnd));
      RaycastHit raycastHit = new Ray(this.TraceStart, this.TraceDirection).Raycast(this.TraceDistance, this.Entity);
      if ((Object) raycastHit.collider != (Object) null)
      {
        DamagableTreeSimple damagableTreeSimple = raycastHit.collider.gameObject.FirstComponentAncestor<DamagableTreeSimple>();
        if ((Object) damagableTreeSimple != (Object) null)
        {
          this.currentWoodcuttingLoop = damagableTreeSimple.woodCutLoop;
          this.currentWoodcuttingLoop.Update(this.transform, 1f, 1f);
        }
        this._lastHitTime = Time.time;
        this.rigidbodyToForce.AddForceAtPosition(this.transform.forward * this.rigidbodyForcePerSecond * this.timeBetweenHits, Vector3.Lerp(this.transform.position, this.rigidbodyToForce.worldCenterOfMass, this.torqueCancellation), ForceMode.Impulse);
        if ((bool) (Object) raycastHit.rigidbody)
          raycastHit.rigidbody.AddForceAtPosition(this.transform.forward * (-this.rigidbodyForcePerSecond * this.timeBetweenHits), this.transform.position);
        IDamageReceiver damageReceiver = raycastHit.collider.gameObject.GetImplementor<IDamageReceiver>() ?? raycastHit.collider.gameObject.FirstImplementingAncestor<IDamageReceiver>();
        if (this.Entity.IsLocallyOwned)
        {
          switch (damageReceiver)
          {
            case null:
            case IHarvestable _:
              break;
            default:
              this.DamageSource = this.HeldBy;
              Damage damage = new Damage((Object) this, this.DamageSource, raycastHit.collider.GetComponent<BipedHitbox>())
              {
                Amount = this.DamageThisFrame,
                DamageTypes = this.DamageTypes,
                point = raycastHit.point,
                Force = this.transform.forward * this.rigidbodyForcePerSecond
              };
              damageReceiver.OnDamage(damage);
              break;
          }
        }
        this._currentEffect = raycastHit.collider.gameObject.FirstComponentAncestor<ChainsawMaterialEffect>();
        if ((Object) this._currentEffect != (Object) null)
        {
          AudioClip[] clips = this._currentEffect.GetClips(SingletonMonoBehaviour<Censorer>.Instance.BloodIsCensored);
          if (clips != null && !clips.Contains<AudioClip>(this.AudioSource.clip))
          {
            this.AudioSource.clip = clips.GetRandom<AudioClip>();
            if (this._currentEffect.loop)
              this.AudioSource.time = Random.Range(0.0f, this.AudioSource.clip.length);
            this.AudioSource.Play();
          }
          this.resistance = this._currentEffect.resistance * Mathf.Pow(this._currentEffect.resistanceVariation, (float) ((double) Random.value * 2.0 - 1.0));
          GameObject effect = this._currentEffect.GetEffect(SingletonMonoBehaviour<Censorer>.Instance.BloodIsCensored);
          if ((Object) effect != (Object) null)
          {
            GameObject gameObject = (GameObject) Object.Instantiate((Object) effect, raycastHit.point, this.transform.rotation);
            if (this._currentEffect.parentEffectToChainsaw)
              gameObject.transform.parent = this.transform;
          }
          float time = this._spinSpeed / this.maxSpinSpeed;
          this.AudioSource.volume = this._currentEffect.speedForVolume.Evaluate(time) * this.SFXVolume;
          this.AudioSource.pitch = this._currentEffect.speedForPitch.Evaluate(time);
          this._fadeRate = this._currentEffect.fadeRate;
        }
        else if (damageReceiver != null)
        {
          this.resistance = this.defaultDamagableResistance * Mathf.Pow(this.resistanceVariation, (float) ((double) Random.value * 2.0 - 1.0));
        }
        else
        {
          this.AudioSource.clip = this.squeals.GetRandom<AudioClip>();
          this.AudioSource.loop = false;
          this.AudioSource.volume = this.SFXVolume * 0.2f;
          this.AudioSource.pitch = 1f;
          this.AudioSource.Play();
          this.resistance = this.defaultNonDamagableResistance * Mathf.Pow(this.resistanceVariation, (float) ((double) Random.value * 2.0 - 1.0));
          if ((Object) this.spark != (Object) null)
            Object.Instantiate((Object) this.spark, raycastHit.point, this.transform.rotation);
        }
      }
      else
      {
        this.resistance = 0.0f;
        this._currentEffect = (ChainsawMaterialEffect) null;
      }
    }
    if ((double) this._spinSpeed < (double) this.spinSpeedDamageThreshold)
    {
      this.resistance = 0.0f;
      this._currentEffect = (ChainsawMaterialEffect) null;
    }
    if ((Object) this._currentEffect != (Object) null)
    {
      this.AudioSource.loop = this._currentEffect.loop;
      if (this.AudioSource.loop)
      {
        float time = this._spinSpeed / this.maxSpinSpeed;
        this.AudioSource.volume = this._currentEffect.speedForVolume.Evaluate(time);
        this.AudioSource.pitch = this._currentEffect.speedForPitch.Evaluate(time);
      }
    }
    else if (this.AudioSource.loop)
    {
      this.AudioSource.volume -= this._fadeRate * Time.deltaTime;
      if ((double) this.AudioSource.volume <= 0.0)
      {
        this.AudioSource.volume = 0.0f;
        this.AudioSource.clip = (AudioClip) null;
        this.AudioSource.Stop();
      }
    }
    if (this.currentWoodcuttingLoop == null || (double) Time.time - (double) this._lastHitTime <= (double) this.timeBetweenHits)
      return;
    this.currentWoodcuttingLoop.MuteIfNotUpdated();
  }

  public void SetCensored(CensorLayer layer, bool isCensored)
  {
  }

  public void AddSelfToCensorableList() => Censorer.AddCensorable((ICensorable) this);

  public void RemoveSelfFromCensorableList() => Censorer.RemoveCensorable((ICensorable) this);

  public override void Awake()
  {
    this.AddSelfToCensorableList();
    base.Awake();
  }

  public void OnDestroy() => this.RemoveSelfFromCensorableList();

  public void OnDisable() => this._spinSpeed = 0.0f;

  public Entity HeldBy
  {
    set
    {
      this._heldBy = value;
      this.weaponBridge = this._heldBy.GetOrCreate<WeaponBridge>();
      this.rigidbodyToForce = (Rigidbody) (GameObjectAttribute<Rigidbody>) this._heldBy.Get<MainRigidbody>();
    }
    get => this._heldBy;
  }

  public Entity DamageSource
  {
    get => this.HeldBy;
    set
    {
    }
  }

  public DamageType DamageTypes
  {
    get => this._DamageTypes;
    set => this._DamageTypes = value;
  }
}
