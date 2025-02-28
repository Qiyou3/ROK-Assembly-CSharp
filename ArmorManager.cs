// Decompiled with JetBrains decompiler
// Type: ArmorManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch;
using CodeHatch.Damaging;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Networking;
using CodeHatch.Inventory.Blueprints.Components;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Characters;
using CodeHatch.Networking.Events.Players;
using CodeHatch.Thrones.CharacterCreation;
using CodeHatch.Thrones.Hair;
using System;
using System.Collections.Generic;
using UMA;
using UnityEngine;

#nullable disable
public class ArmorManager : EntityBehaviour, IDamageTypeModifier
{
  public ArmorBlueprint.Slot[][] EquipSlotToArmorSlot = new ArmorBlueprint.Slot[6][]
  {
    new ArmorBlueprint.Slot[1]{ ArmorBlueprint.Slot.Helmet },
    new ArmorBlueprint.Slot[2]
    {
      ArmorBlueprint.Slot.Torso,
      ArmorBlueprint.Slot.Vest
    },
    new ArmorBlueprint.Slot[2]
    {
      ArmorBlueprint.Slot.Hands,
      ArmorBlueprint.Slot.Gloves
    },
    new ArmorBlueprint.Slot[3]
    {
      ArmorBlueprint.Slot.Legs,
      ArmorBlueprint.Slot.Loins,
      ArmorBlueprint.Slot.Pants
    },
    new ArmorBlueprint.Slot[2]
    {
      ArmorBlueprint.Slot.Boots,
      ArmorBlueprint.Slot.OverBoots
    },
    new ArmorBlueprint.Slot[1]{ ArmorBlueprint.Slot.Tabard }
  };
  public Dictionary<ArmorBlueprint.Slot, int> ArmorSlotToEquipSlotDictionary = new Dictionary<ArmorBlueprint.Slot, int>();
  public Action<ArmorBlueprint.Slot, ArmorBlueprint> OnArmorApplied;
  public System.Action OnArmorUpdate;
  public ArmorManager.ArmorAssociation[] ArmorAssociations = new ArmorManager.ArmorAssociation[0];
  public bool RequiresUpdate;
  public bool RequiresRecolor;
  private CharacterImmunity _characterIsHidden;
  private SkinnedMeshRenderer _lastRenderer;
  private Mesh _lastMesh;
  private CharacterBody _CharacterBody;
  private CharacterHair _CharacterHair;
  private WearableGuildEmblem _WearableGuildEmblem;
  private GuildColoredArmor _GuildColoredArmor;
  private UMADynamicAvatar _DynamicAvatar;
  private IArmorBonus[] _ArmorBonuses;
  private float _SpeedMultiplier = 1f;
  [SerializeField]
  private DamageVulnerability[] _modifyTypes;

  private CharacterImmunity CharacterIsHidden
  {
    get
    {
      if ((UnityEngine.Object) this._characterIsHidden == (UnityEngine.Object) null)
        this._characterIsHidden = this.Entity.Get<CharacterImmunity>();
      return this._characterIsHidden;
    }
  }

  public SkinnedMeshRenderer SkinnedMeshRenderer
  {
    get
    {
      return (UnityEngine.Object) this._DynamicAvatar == (UnityEngine.Object) null || (UnityEngine.Object) this._DynamicAvatar.umaData == (UnityEngine.Object) null ? (SkinnedMeshRenderer) null : this._DynamicAvatar.umaData.myRenderer;
    }
  }

  public CharacterBody CharacterBody
  {
    get
    {
      if ((UnityEngine.Object) this._CharacterBody == (UnityEngine.Object) null && this.HasEntity)
        this._CharacterBody = this.Entity.TryGet<CharacterBody>();
      return this._CharacterBody;
    }
  }

  public CharacterHair CharacterHair
  {
    get
    {
      if ((UnityEngine.Object) this._CharacterHair == (UnityEngine.Object) null && this.HasEntity)
        this._CharacterHair = this.Entity.TryGet<CharacterHair>();
      return this._CharacterHair;
    }
  }

  public WearableGuildEmblem WearableGuildEmblem
  {
    get
    {
      if ((UnityEngine.Object) this._WearableGuildEmblem == (UnityEngine.Object) null && this.HasEntity)
        this._WearableGuildEmblem = this.Entity.TryGet<WearableGuildEmblem>();
      return this._WearableGuildEmblem;
    }
  }

  public GuildColoredArmor GuildColoredArmor
  {
    get
    {
      if ((UnityEngine.Object) this._GuildColoredArmor == (UnityEngine.Object) null && this.HasEntity)
        this._GuildColoredArmor = this.Entity.TryGet<GuildColoredArmor>();
      return this._GuildColoredArmor;
    }
  }

  public UMADynamicAvatar DynamicAvatar
  {
    get
    {
      if ((UnityEngine.Object) this._DynamicAvatar == (UnityEngine.Object) null && this.HasEntity)
      {
        this._DynamicAvatar = this.Entity.TryGet<UMADynamicAvatar>();
        if ((UnityEngine.Object) this._DynamicAvatar != (UnityEngine.Object) null)
        {
          this.DynamicAvatar.umaData.OnCharacterCreated += new Action<UMAData>(this.Recolor);
          this.DynamicAvatar.umaData.OnCharacterUpdated += new Action<UMAData>(this.Recolor);
        }
      }
      return this._DynamicAvatar;
    }
  }

  public IArmorBonus[] ArmorBonuses
  {
    get
    {
      if (this._ArmorBonuses == null && (UnityEngine.Object) this.Entity != (UnityEngine.Object) null)
        this._ArmorBonuses = this.Entity.TryGetArray<IArmorBonus>();
      return this._ArmorBonuses;
    }
  }

  private bool RendererChanged
  {
    get
    {
      SkinnedMeshRenderer skinnedMeshRenderer = this.SkinnedMeshRenderer;
      if ((UnityEngine.Object) this._lastRenderer != (UnityEngine.Object) skinnedMeshRenderer)
      {
        this._lastRenderer = skinnedMeshRenderer;
        return true;
      }
      if ((UnityEngine.Object) skinnedMeshRenderer == (UnityEngine.Object) null || !((UnityEngine.Object) skinnedMeshRenderer.sharedMesh != (UnityEngine.Object) this._lastMesh))
        return false;
      this._lastMesh = skinnedMeshRenderer.sharedMesh;
      return true;
    }
  }

  public void OnDestroy()
  {
    EventManager.Unsubscribe<PlayerColorEvent>(new EventSubscriber<PlayerColorEvent>(this.OnPlayerColored));
    EventManager.Unsubscribe<PlayerDeathEvent>(new EventSubscriber<PlayerDeathEvent>(this.OnPlayerDeath));
  }

  public void Awake()
  {
    for (int index = 0; index < this.EquipSlotToArmorSlot.Length; ++index)
    {
      foreach (ArmorBlueprint.Slot key in this.EquipSlotToArmorSlot[index])
        this.ArmorSlotToEquipSlotDictionary.Add(key, index);
    }
    if (Player.IsLocalDedi)
    {
      this.enabled = false;
    }
    else
    {
      if (this.ArmorAssociations == null)
      {
        this.LogError<ArmorManager>("No armor associations were setup.");
        this.ArmorAssociations = new ArmorManager.ArmorAssociation[0];
      }
      this.UnequipAll();
      EventManager.Subscribe<PlayerColorEvent>(new EventSubscriber<PlayerColorEvent>(this.OnPlayerColored));
      EventManager.Subscribe<PlayerDeathEvent>(new EventSubscriber<PlayerDeathEvent>(this.OnPlayerDeath));
    }
  }

  public void Start() => this.RecalculatePerformance();

  public void Update()
  {
    if (this.RequiresUpdate)
    {
      this.RequiresUpdate = false;
      this.DynamicAvatar.UpdateSameRace();
      EventManager.CallEvent((BaseEvent) new ArmorUpdateEvent(this.Entity));
    }
    if (!((UnityEngine.Object) this.SkinnedMeshRenderer != (UnityEngine.Object) null) || this.Entity.IsLocallyOwned)
      return;
    bool flag = !((UnityEngine.Object) this.CharacterIsHidden == (UnityEngine.Object) null) && this.CharacterIsHidden.IsHidden;
    this.SkinnedMeshRenderer.enabled = this.Entity.IsInLoadedPage() && !flag;
  }

  public void OnPlayerColored(PlayerColorEvent theEvent)
  {
    if ((UnityEngine.Object) this.Entity.NetView != (UnityEngine.Object) null)
    {
      if (this.Entity.Owner != theEvent.Player)
        return;
      this.ColorRenderer(theEvent.PlayerColor);
    }
    else
      this.ColorRenderer(theEvent.PlayerColor);
  }

  public void Recolor(UMAData data)
  {
    try
    {
      this.CharacterBody.SetColor();
      this.CharacterBody.SetTattoo();
      this.CharacterHair.ColorHair();
      this.WearableGuildEmblem.SetEmblem();
      this.GuildColoredArmor.SetEmblem();
    }
    catch (Exception ex)
    {
      this.LogException<ArmorManager>(ex);
    }
  }

  public void ColorRenderer(Color color)
  {
    SkinnedMeshRenderer skinnedMeshRenderer = this.SkinnedMeshRenderer;
    if ((UnityEngine.Object) skinnedMeshRenderer == (UnityEngine.Object) null)
      return;
    foreach (Material material in skinnedMeshRenderer.materials)
    {
      material.SetColor("_ColorR", color);
      material.SetColor("_ColorG", color);
      material.SetColor("_ColorB", color);
      material.SetColor("_ColorA", color);
    }
  }

  public void UnequipArmor(ArmorBlueprint.Slot armorSlot)
  {
    foreach (ArmorManager.ArmorAssociation armorAssociation in this.ArmorAssociations)
    {
      if (armorAssociation.ArmorSlot == armorSlot)
      {
        this.ApplyArmor(armorAssociation.ArmorSlot, (ArmorBlueprint) null);
        break;
      }
    }
  }

  public ArmorBlueprint GetBlueprintFromSlot(ArmorBlueprint.Slot armorSlot)
  {
    foreach (ArmorManager.ArmorAssociation armorAssociation in this.ArmorAssociations)
    {
      if (armorAssociation.ArmorSlot == armorSlot)
        return armorAssociation.CurrentBlueprint;
    }
    return (ArmorBlueprint) null;
  }

  public void UnequipAll()
  {
    foreach (ArmorManager.ArmorAssociation armorAssociation in this.ArmorAssociations)
      this.ApplyArmor(armorAssociation.ArmorSlot, (ArmorBlueprint) null);
  }

  public float SpeedMultiplier
  {
    get => this._SpeedMultiplier;
    set => this._SpeedMultiplier = value;
  }

  public float EnergyCapacity { get; set; }

  public float EnergyRestoration { get; set; }

  public float RecoilReduction { get; set; }

  public void RecalculatePerformance()
  {
    this.SpeedMultiplier = 1f;
    this.RecoilReduction = 0.0f;
    foreach (ArmorManager.ArmorAssociation armorAssociation in this.ArmorAssociations)
    {
      if (!((UnityEngine.Object) armorAssociation.CurrentBlueprint == (UnityEngine.Object) null))
      {
        this.SpeedMultiplier -= (float) armorAssociation.CurrentBlueprint.SpeedPenalty;
        this.RecoilReduction += (float) armorAssociation.CurrentBlueprint.RecoilReduction;
      }
    }
    this.RecoilReduction *= 0.8f;
    if (this.ArmorBonuses == null)
      return;
    for (int index = 0; index < this.ArmorBonuses.Length; ++index)
      this.ArmorBonuses[index].RecalculateBonus();
  }

  public DamageVulnerability[] ModifyTypes
  {
    get => this._modifyTypes;
    private set => this._modifyTypes = value;
  }

  public virtual void ModifyDamage(Damage damage)
  {
    if (damage == null)
      return;
    foreach (ArmorManager.ArmorAssociation armorAssociation in this.GetAssociationsFromBone(damage.HitBoxBone))
    {
      if (!((UnityEngine.Object) armorAssociation.CurrentBlueprint == (UnityEngine.Object) null))
      {
        foreach (ArmorBlueprint.Defense defense in armorAssociation.CurrentBlueprint.Defenses)
        {
          if ((defense.Type & damage.DamageTypes) != DamageType.Unknown && (double) damage.Amount > 0.0)
          {
            damage.Amount -= defense.ValueWithBonuses;
            damage.Amount = (double) damage.Amount >= 1.0 ? damage.Amount : 1f;
          }
        }
      }
    }
  }

  public List<ArmorManager.ArmorAssociation> GetAssociationsFromBone(HumanBodyBones theBone)
  {
    List<ArmorManager.ArmorAssociation> associationsFromBone = new List<ArmorManager.ArmorAssociation>();
    if (this.ArmorAssociations == null)
      return associationsFromBone;
    foreach (ArmorManager.ArmorAssociation armorAssociation in this.ArmorAssociations)
    {
      if (armorAssociation != null && armorAssociation.AssociatedBones != null)
      {
        foreach (HumanBodyBones associatedBone in armorAssociation.AssociatedBones)
        {
          if (associatedBone == theBone)
            associationsFromBone.Add(armorAssociation);
        }
      }
    }
    return associationsFromBone;
  }

  public void ApplyArmor(ArmorBlueprint.Slot armorSlot, ArmorBlueprint blueprint)
  {
    if ((UnityEngine.Object) blueprint != (UnityEngine.Object) null)
    {
      for (int index = 0; index < blueprint.Defenses.Length; ++index)
        blueprint.Defenses[index].ArmorBonuses = this.ArmorBonuses;
    }
    ArmorProcessor.Instance.ApplyArmor(this, armorSlot, blueprint);
    if (this.OnArmorApplied == null)
      return;
    this.OnArmorApplied(armorSlot, blueprint);
  }

  public void OnPlayerDeath(PlayerDeathEvent theEvent)
  {
    if ((long) theEvent.PlayerId != (long) this.Entity.OwnerId)
      return;
    this.UnequipAll();
  }

  [Serializable]
  public class ArmorAssociation
  {
    public ArmorBlueprint.Slot ArmorSlot;
    public HumanBodyBones[] AssociatedBones;
    public float SummaryWeight = 1f;
    public ArmorBlueprint DefaultArmorBlueprint;

    public ArmorBlueprint CurrentBlueprint { get; set; }
  }
}
