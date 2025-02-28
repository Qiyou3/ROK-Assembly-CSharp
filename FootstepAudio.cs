// Decompiled with JetBrains decompiler
// Type: FootstepAudio
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch;
using CodeHatch.Common;
using CodeHatch.Engine.Audio;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Core.Cheat;
using CodeHatch.Engine.Core.EffectsPooling;
using CodeHatch.Engine.Networking;
using CodeHatch.Inventory.Blueprints.Components;
using CodeHatch.Networking.Events;
using CodeHatch.Thrones.Audio;
using CodeHatch.Thrones.Waving;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FootstepAudio : EntityBehaviour
{
  private const int _maxOverlappingSoundsPerFoot = 1;
  public FootstepAudio.AudioLayer[] audioLayers;
  public FootstepEffect[] footprintEffects;
  public AnimationCurve velocityToRange;
  public int locomotionLayer;
  public AnimatorWeighting weighting;
  [Range(0.0f, 255f)]
  public int localPlayerPriority;
  [Range(0.0f, 255f)]
  public int remotePlayerPriority;
  private Animator _animator;
  private CharacterDefinition _characterDefinition;
  private JumpAnimation _JumpAnimation;
  private EntityRigidbodyManager _entityRigidbodyManager;
  private readonly FootstepAudio.FootAudioSource[] _footAudioSources = new FootstepAudio.FootAudioSource[2];
  private float _prevNormalizedTime;
  private float _currentNormalizedTime;
  private ArmorSoundsBundle defaultBootsBundle;
  private ArmorSoundsBundle defaultChestBundle;
  private ArmorSoundsBundle defaultHatBundle;

  private Animator MyAnimator
  {
    get
    {
      if ((UnityEngine.Object) this._animator == (UnityEngine.Object) null)
        this._animator = this.Entity.Get<Animator>();
      return this._animator;
    }
  }

  private CharacterDefinition MyCharacterDefinition
  {
    get
    {
      if ((UnityEngine.Object) this._characterDefinition == (UnityEngine.Object) null)
        this._characterDefinition = this.Entity.GetOrCreate<CharacterDefinition>();
      return this._characterDefinition;
    }
  }

  private JumpAnimation JumpAnimation
  {
    get
    {
      if ((UnityEngine.Object) this._JumpAnimation == (UnityEngine.Object) null)
        this._JumpAnimation = this.Entity.TryGet<JumpAnimation>();
      return this._JumpAnimation;
    }
  }

  private EntityRigidbodyManager MyEntityRigidbodyManager
  {
    get
    {
      if ((UnityEngine.Object) this._entityRigidbodyManager == (UnityEngine.Object) null)
        this._entityRigidbodyManager = this.Entity.GetOrCreate<EntityRigidbodyManager>();
      return this._entityRigidbodyManager;
    }
  }

  public void Start()
  {
    this.CreateAudioSources();
    ArmorManager armorManager = this.Entity.TryGet<ArmorManager>();
    if ((bool) (UnityEngine.Object) armorManager)
    {
      armorManager.OnArmorApplied += new Action<ArmorBlueprint.Slot, ArmorBlueprint>(this.OnArmorApplied);
      this.InitializeBundles();
    }
    EventManager.Subscribe<EntityJumpEvent>(new EventSubscriber<EntityJumpEvent>(this.OnJump));
  }

  public void OnEnable()
  {
    if (Player.IsLocalDedi)
      UnityEngine.Object.Destroy((UnityEngine.Object) this);
    if (!this.HasEntity || !(bool) (UnityEngine.Object) this.Entity.TryGet<ArmorManager>())
      return;
    this.InitializeBundles();
  }

  private void InitializeBundles()
  {
    ArmorManager armorManager = this.Entity.Get<ArmorManager>();
    ArmorManager.ArmorAssociation armorAssociation1 = ((IEnumerable<ArmorManager.ArmorAssociation>) armorManager.ArmorAssociations).FirstOrDefault<ArmorManager.ArmorAssociation>((Func<ArmorManager.ArmorAssociation, bool>) (a => a.ArmorSlot == ArmorBlueprint.Slot.Boots), 0);
    ArmorManager.ArmorAssociation armorAssociation2 = ((IEnumerable<ArmorManager.ArmorAssociation>) armorManager.ArmorAssociations).FirstOrDefault<ArmorManager.ArmorAssociation>((Func<ArmorManager.ArmorAssociation, bool>) (a => a.ArmorSlot == ArmorBlueprint.Slot.Torso), 0);
    ArmorManager.ArmorAssociation armorAssociation3 = ((IEnumerable<ArmorManager.ArmorAssociation>) armorManager.ArmorAssociations).FirstOrDefault<ArmorManager.ArmorAssociation>((Func<ArmorManager.ArmorAssociation, bool>) (a => a.ArmorSlot == ArmorBlueprint.Slot.Helmet), 0);
    this.defaultBootsBundle = armorAssociation1.DefaultArmorBlueprint.AudioBundle;
    this.defaultChestBundle = armorAssociation2.DefaultArmorBlueprint.AudioBundle;
    this.defaultHatBundle = (ArmorSoundsBundle) null;
    ArmorSoundsBundle armorSoundsBundle1 = !((UnityEngine.Object) armorAssociation1.CurrentBlueprint != (UnityEngine.Object) null) ? this.defaultBootsBundle : armorAssociation1.CurrentBlueprint.AudioBundle;
    ArmorSoundsBundle armorSoundsBundle2 = !((UnityEngine.Object) armorAssociation2.CurrentBlueprint != (UnityEngine.Object) null) ? this.defaultChestBundle : armorAssociation2.CurrentBlueprint.AudioBundle;
    ArmorSoundsBundle armorSoundsBundle3 = !((UnityEngine.Object) armorAssociation3.CurrentBlueprint != (UnityEngine.Object) null) ? this.defaultHatBundle : armorAssociation3.CurrentBlueprint.AudioBundle;
    for (int index = 0; index < this.audioLayers.Length; ++index)
    {
      this.audioLayers[index].BootsBundle = armorSoundsBundle1;
      this.audioLayers[index].ChestBundle = armorSoundsBundle2;
      this.audioLayers[index].HatBundle = armorSoundsBundle3;
    }
  }

  private void OnArmorApplied(ArmorBlueprint.Slot Slot, ArmorBlueprint Blueprint)
  {
    for (int index = 0; index < this.audioLayers.Length; ++index)
    {
      if (Slot == ArmorBlueprint.Slot.Boots)
        this.audioLayers[index].BootsBundle = (UnityEngine.Object) Blueprint == (UnityEngine.Object) null || (UnityEngine.Object) Blueprint.AudioBundle == (UnityEngine.Object) null ? this.defaultBootsBundle : Blueprint.AudioBundle;
      if (Slot == ArmorBlueprint.Slot.Torso)
        this.audioLayers[index].ChestBundle = (UnityEngine.Object) Blueprint == (UnityEngine.Object) null || (UnityEngine.Object) Blueprint.AudioBundle == (UnityEngine.Object) null ? this.defaultChestBundle : Blueprint.AudioBundle;
      if (Slot == ArmorBlueprint.Slot.Helmet)
        this.audioLayers[index].HatBundle = (UnityEngine.Object) Blueprint == (UnityEngine.Object) null || (UnityEngine.Object) Blueprint.AudioBundle == (UnityEngine.Object) null ? this.defaultHatBundle : Blueprint.AudioBundle;
    }
  }

  public void OnDestroy()
  {
    for (int index = 0; index < this._footAudioSources.Length; ++index)
      this._footAudioSources[index]?.Clean();
    EventManager.Unsubscribe<EntityJumpEvent>(new EventSubscriber<EntityJumpEvent>(this.OnJump));
  }

  private void CreateAudioSources()
  {
    this._footAudioSources[0] = new FootstepAudio.FootAudioSource(0.0f, this.MyCharacterDefinition.GetTransform(HumanBodyBones.RightFoot), this.MyCharacterDefinition.GetTransform(HumanBodyBones.Chest), this.MyCharacterDefinition.GetTransform(HumanBodyBones.Head));
    this._footAudioSources[1] = new FootstepAudio.FootAudioSource(0.5f, this.MyCharacterDefinition.GetTransform(HumanBodyBones.LeftFoot), this.MyCharacterDefinition.GetTransform(HumanBodyBones.Chest), this.MyCharacterDefinition.GetTransform(HumanBodyBones.Head));
  }

  public void Update()
  {
    this.UpdateNormalizedTime();
    if ((double) this.Entity.Get<CodeHatch.Engine.Modules.ScriptLOD.ScriptLOD>().Distance >= 150.0)
      return;
    double weight = (double) this.weighting.CalculateWeight(this.MyAnimator);
    if ((double) this.weighting.weight == 0.0)
      return;
    float velocity = this.GetVelocity();
    float range = this.velocityToRange.Evaluate(velocity);
    if ((double) range <= 0.0 || !this.Entity.Get<MotorBridge>().Grounded || this.Entity.Get<EntityBasicFly>().enabled)
      return;
    foreach (FootstepAudio.FootAudioSource footAudioSource in this._footAudioSources)
    {
      if (this.PhaseReached(footAudioSource))
      {
        footAudioSource.ClearCandidates();
        footAudioSource.CalculateMaterial();
        foreach (FootstepAudio.AudioLayer audioLayer in this.audioLayers)
        {
          audioLayer.foot = footAudioSource.foot;
          audioLayer.material = footAudioSource.material;
          audioLayer.Update(velocity);
          footAudioSource.AddCandidate(audioLayer);
        }
        int priority = !this.Entity.IsLocallyOwned ? this.remotePlayerPriority : this.localPlayerPriority;
        footAudioSource.PlayBestCandidates(this.weighting.weight * AudioVolumeController.SFXVolume, range, priority);
        this.CreateEffects(footAudioSource);
      }
    }
  }

  private void OnJump(EntityJumpEvent e)
  {
    if (!((UnityEngine.Object) e.Entity == (UnityEngine.Object) this.Entity))
      return;
    float velocity = this.GetVelocity();
    float range = this.velocityToRange.Evaluate(velocity);
    FootstepAudio.FootAudioSource footAudioSource = this._footAudioSources[!this.JumpAnimation.Mirrored ? 0 : 1];
    footAudioSource.ClearCandidates();
    footAudioSource.CalculateMaterial();
    foreach (FootstepAudio.AudioLayer audioLayer in this.audioLayers)
    {
      audioLayer.foot = footAudioSource.foot;
      audioLayer.material = footAudioSource.material;
      audioLayer.Update(velocity);
      footAudioSource.AddCandidate(audioLayer);
    }
    int priority = !this.Entity.IsLocallyOwned ? this.remotePlayerPriority : this.localPlayerPriority;
    footAudioSource.PlayBestCandidates(this.weighting.weight * AudioVolumeController.SFXVolume, range, priority);
    this.CreateEffects(footAudioSource, false);
  }

  private void CreateEffects(FootstepAudio.FootAudioSource footAudioSource, bool addFootEffect = true)
  {
    foreach (FootstepEffect footprintEffect in this.footprintEffects)
    {
      if (footprintEffect.Valid(footAudioSource.material))
      {
        if ((UnityEngine.Object) footprintEffect.groundEffect != (UnityEngine.Object) null)
        {
          GameObject gameObject = EffectsPool.Instantiate(footprintEffect.groundEffect, footAudioSource.foot.position, footAudioSource.foot.rotation);
          gameObject.transform.parent = (Transform) null;
          gameObject.transform.localScale = footAudioSource.foot.lossyScale;
          gameObject.transform.parent = footAudioSource.collidedTransform;
          if ((double) footAudioSource.phase < 0.5)
            gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, new Vector3(1f, -1f, 1f));
        }
        if ((UnityEngine.Object) footprintEffect.footEffect != (UnityEngine.Object) null && addFootEffect)
          EffectsPool.Instantiate(footprintEffect.footEffect, footAudioSource.foot.position, footAudioSource.foot.rotation).transform.parent = footAudioSource.foot;
      }
    }
  }

  private void UpdateNormalizedTime()
  {
    this._prevNormalizedTime = this._currentNormalizedTime;
    this._currentNormalizedTime = this.MyAnimator.GetNormalizedTime(this.locomotionLayer);
  }

  private float GetVelocity()
  {
    return this.MyEntityRigidbodyManager.MyRigidbodies.AverageVelocity.magnitude;
  }

  private bool PhaseReached(FootstepAudio.FootAudioSource footAudioSource)
  {
    float phase = footAudioSource.phase;
    return AnimatorUtil.GetPhaseRepeatIndex(this._prevNormalizedTime, phase) != AnimatorUtil.GetPhaseRepeatIndex(this._currentNormalizedTime, phase);
  }

  public enum FootstepSpeed
  {
    Running,
    Walking,
  }

  [Serializable]
  public class AudioLayer
  {
    public FootstepAudio.FootstepSpeed Speed;
    public AnimationCurve velocityToBlend;
    public AnimationCurve velocityToPitch;
    [NonSerialized]
    public string[] currentClips = new string[3];
    [NonSerialized]
    public float currentBlend;
    [NonSerialized]
    public float currentPitch;
    [NonSerialized]
    public ArmorSoundsBundle BootsBundle;
    [NonSerialized]
    public ArmorSoundsBundle ChestBundle;
    [NonSerialized]
    public ArmorSoundsBundle HatBundle;
    [NonSerialized]
    public Transform foot;
    [NonSerialized]
    public MaterialAudioBindings material;
    private int _interval;

    public float CurrentAudibility => this.currentBlend;

    public string[] GetClips()
    {
      string str1 = string.Empty;
      if ((UnityEngine.Object) this.ChestBundle != (UnityEngine.Object) null && this.ChestBundle.RustleInterval != 0 && this._interval % this.ChestBundle.RustleInterval == 0)
        str1 = this.Speed != FootstepAudio.FootstepSpeed.Running ? this.ChestBundle.WalkingLoops : this.ChestBundle.RunningLoops;
      string str2 = string.Empty;
      if ((UnityEngine.Object) this.HatBundle != (UnityEngine.Object) null && this.HatBundle.RustleInterval != 0 && this._interval % this.HatBundle.RustleInterval == 0)
        str2 = this.Speed != FootstepAudio.FootstepSpeed.Running ? this.HatBundle.WalkingLoops : this.HatBundle.RunningLoops;
      string str3 = string.Empty;
      if ((UnityEngine.Object) this.BootsBundle != (UnityEngine.Object) null)
        str3 = this.Speed != FootstepAudio.FootstepSpeed.Running ? this.BootsBundle.GetWalkingClip(this.material) : this.BootsBundle.GetRunningClip(this.material);
      this.currentClips[0] = str3;
      this.currentClips[1] = str1;
      this.currentClips[2] = str2;
      return this.currentClips;
    }

    public void IncrementInterval() => ++this._interval;

    public void Update(float velocity)
    {
      this.currentBlend = this.velocityToBlend.Evaluate(velocity);
      this.currentPitch = this.velocityToPitch.Evaluate(velocity);
      this.GetClips();
    }
  }

  private class FootAudioSource
  {
    public readonly Transform foot;
    public readonly Transform chest;
    public readonly Transform head;
    public readonly float phase;
    private readonly FootstepAudio.AudioLayer[] _candidateAudioLayers = new FootstepAudio.AudioLayer[1];
    private int _candidateAudioLayerCount;

    public FootAudioSource(float phase, Transform foot, Transform chest, Transform head)
    {
      this.phase = phase;
      this.foot = foot;
      this.chest = chest;
      this.head = head;
    }

    public MaterialAudioBindings material { get; private set; }

    public Transform collidedTransform { get; private set; }

    public void Clean()
    {
    }

    public void ClearCandidates() => this._candidateAudioLayerCount = 0;

    public void CalculateMaterial()
    {
      RaycastHit hitInfo;
      Physics.Raycast(this.foot.position + Vector3.up * 0.4f, Vector3.down, out hitInfo, 0.8f, LayerMask.GetMask("Terrain", "Terrain Objects", "Default", "Blocks"));
      this.material = (MaterialAudioBindings) null;
      if (!((UnityEngine.Object) hitInfo.collider != (UnityEngine.Object) null))
        return;
      this.material = MaterialAudioBindings.GetBinding(hitInfo.collider.gameObject, hitInfo.point);
      this.collidedTransform = hitInfo.collider.transform;
    }

    public void AddCandidate(FootstepAudio.AudioLayer candidateAudioLayer)
    {
      if (this._candidateAudioLayerCount < 1)
      {
        this._candidateAudioLayers[this._candidateAudioLayerCount] = candidateAudioLayer;
        ++this._candidateAudioLayerCount;
      }
      else
      {
        float num = this._candidateAudioLayers[0].CurrentAudibility;
        int index1 = 0;
        for (int index2 = 1; index2 < 1; ++index2)
        {
          float currentAudibility = this._candidateAudioLayers[index2].CurrentAudibility;
          if ((double) currentAudibility < (double) num)
          {
            num = currentAudibility;
            index1 = index2;
          }
        }
        if ((double) candidateAudioLayer.CurrentAudibility <= (double) num)
          return;
        this._candidateAudioLayers[index1] = candidateAudioLayer;
      }
    }

    public void PlayBestCandidates(float volume, float range, int priority)
    {
      for (int index = 0; index < 1; ++index)
      {
        if ((double) volume * (double) this._candidateAudioLayers[index].currentBlend <= 0.05000000074505806)
        {
          this._candidateAudioLayers[index].IncrementInterval();
        }
        else
        {
          if (this._candidateAudioLayers[index].currentClips[0] != string.Empty)
          {
            AudioObject audioObject = AudioController.Play(this._candidateAudioLayers[index].currentClips[0], this.foot, volume * this._candidateAudioLayers[index].currentBlend, 0.0f, 0.0f);
            if ((UnityEngine.Object) audioObject != (UnityEngine.Object) null)
              audioObject.primaryAudioSource.priority = priority;
            this._candidateAudioLayers[index].IncrementInterval();
          }
          if (this._candidateAudioLayers[index].currentClips[1] != string.Empty)
          {
            AudioObject audioObject = AudioController.Play(this._candidateAudioLayers[index].currentClips[1], this.chest, volume * this._candidateAudioLayers[index].currentBlend, 0.0f, 0.0f);
            if ((UnityEngine.Object) audioObject != (UnityEngine.Object) null)
              audioObject.primaryAudioSource.priority = priority;
          }
          if (this._candidateAudioLayers[index].currentClips[2] != string.Empty)
          {
            AudioObject audioObject = AudioController.Play(this._candidateAudioLayers[index].currentClips[2], this.head, volume * this._candidateAudioLayers[index].currentBlend, 0.0f, 0.0f);
            if ((UnityEngine.Object) audioObject != (UnityEngine.Object) null)
              audioObject.primaryAudioSource.priority = priority;
          }
        }
      }
    }
  }
}
