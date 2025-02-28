// Decompiled with JetBrains decompiler
// Type: ForestBiomeSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Utility.Attributes;
using CodeHatch.Shocktree;
using CodeHatch.Spawning;
using CodeHatch.TerrainAPI;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class ForestBiomeSettings : MonoBehaviour
{
  public static List<ForestBiomeSettings> ForestBiomeSettingsList = new List<ForestBiomeSettings>();
  private GameObject spawnObject;
  private IStaticSpawnable _spawnable;
  public float density;
  [Range(0.0f, 90f)]
  public float maxSpawnAngle = 45f;
  public List<ForestBiomeSettings.Biome> biomeSelection;
  private Dictionary<BiomeID, ForestBiomeSettings.Biome> biomeSelectionDictionary;
  private ForestBiomeSettings.Biome onlyBiome;
  public List<byte> excludeTerrainMaterials;
  private List<ForestBiomeSettingsSpawnPoint> determinedSpawnPoints = new List<ForestBiomeSettingsSpawnPoint>();
  public bool randomSpawns = true;
  public bool rotateToTerrain;
  [Range(0.0f, 2f)]
  public float randomNormalAmount;
  public bool performDistanceCheck = true;
  private DensityTier _densityTier;
  private float _minNormalizedY = -1f;
  public bool jitter = true;
  public float jitterPerlinScale = 100f;
  public bool spawnInCaves;

  public GameObject SpawnObject => this.spawnObject;

  public IStaticSpawnable Spawnable
  {
    get
    {
      if (this._spawnable == null)
      {
        this._spawnable = this.SpawnObject.GetImplementor<IStaticSpawnable>();
        if (this._spawnable == null)
          this._spawnable = (IStaticSpawnable) this.SpawnObject.AddComponent<InstantiateSpawnable>();
      }
      return this._spawnable;
    }
  }

  public List<ForestBiomeSettingsSpawnPoint> DeterminedSpawnPoints => this.determinedSpawnPoints;

  public DensityTier DensityTier
  {
    get => this._densityTier;
    set => this._densityTier = value;
  }

  public float MinNormalizedY
  {
    get
    {
      if ((double) this._minNormalizedY < 0.0)
        this._minNormalizedY = Mathf.Cos((float) ((double) this.maxSpawnAngle * 3.1415927410125732 / 180.0));
      return this._minNormalizedY;
    }
  }

  public void Awake()
  {
    ForestBiomeSettings.ForestBiomeSettingsList.Add(this);
    this.spawnObject = this.gameObject;
    this.biomeSelectionDictionary = new Dictionary<BiomeID, ForestBiomeSettings.Biome>((IEqualityComparer<BiomeID>) new BiomeIDEqualityComparer());
    if (this.biomeSelection.Count == 1)
      this.onlyBiome = this.biomeSelection[0];
    foreach (ForestBiomeSettings.Biome biome in this.biomeSelection)
    {
      if (!this.biomeSelectionDictionary.ContainsKey(biome.biome))
        this.biomeSelectionDictionary.Add(biome.biome, biome);
    }
  }

  public void OnDestroy() => ForestBiomeSettings.ForestBiomeSettingsList.Remove(this);

  public ForestBiomeSettings.Biome SettingsForBiome(BiomeID biomeID)
  {
    ForestBiomeSettings.Biome biome = (ForestBiomeSettings.Biome) null;
    if (this.onlyBiome != null)
      return this.onlyBiome.biome == biomeID ? this.onlyBiome : (ForestBiomeSettings.Biome) null;
    this.biomeSelectionDictionary.TryGetValue(biomeID, out biome);
    return biome;
  }

  public Texture2D TerrainTexture()
  {
    if (this.biomeSelection.Count == 0)
      return (Texture2D) null;
    if ((UnityEngine.Object) HeightMapAPI.Instance == (UnityEngine.Object) null)
      return (Texture2D) null;
    if ((UnityEngine.Object) Terrain.activeTerrain == (UnityEngine.Object) null)
      return (Texture2D) null;
    BiomeID biome = this.biomeSelection[0].biome;
    BiomeID[] terrainIndexToBiome = HeightMapAPI.Instance.TerrainIndexToBiome;
    for (int index = 0; index < terrainIndexToBiome.Length; ++index)
    {
      BiomeID biomeId = terrainIndexToBiome[index];
      if (biome == biomeId)
        return Terrain.activeTerrain.terrainData.splatPrototypes[index].texture;
    }
    return (Texture2D) null;
  }

  [ContextMenu("Add New Spawn Point")]
  public void AddNewSpawnPoint()
  {
    GameObject gameObject = new GameObject(this.gameObject.name + " Spawn Point");
    gameObject.AddComponent<ForestBiomeSettingsSpawnPoint>();
    gameObject.GetComponent<ForestBiomeSettingsSpawnPoint>().ForestBiomeSettings = this;
  }

  [Serializable]
  public class Biome
  {
    public BiomeID biome;
    public float likelihood = 1f;
    public AnimationCurve likelihoodAtAltitude = new AnimationCurve(new Keyframe[2]
    {
      new Keyframe(-200f, 1f),
      new Keyframe(200f, 1f)
    });
    [FlagEnum(typeof (ClimateFlags))]
    [SerializeField]
    public ClimateFlags TemperatureMoisture;
  }
}
