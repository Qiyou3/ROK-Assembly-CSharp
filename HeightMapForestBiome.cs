// Decompiled with JetBrains decompiler
// Type: HeightMapForestBiome
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Configuration;
using CodeHatch.Core;
using CodeHatch.Engine.Core.Gaming;
using CodeHatch.Engine.Modding.Abstract;
using CodeHatch.Noise;
using CodeHatch.Shocktree;
using CodeHatch.Spawning;
using CodeHatch.TerrainAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
[Label("Details Quality")]
public class HeightMapForestBiome : ForestBiome, IConfigurable, IModHandler
{
  private float _perlinScale = 1f;
  public float AltitudeScale = 0.75f;
  private static Transform _lastSpawnParent;
  private static ForestBiomeSettingsSpawnPoint[] _lastSpawnPoints;

  public DensityTier DensityToTier(float density)
  {
    int p = Mathf.CeilToInt(Mathf.Log(density) / Mathf.Log(2f));
    foreach (DensityTier densityTier in this.DensityTiers)
    {
      if (densityTier.groupID == p)
        return densityTier;
    }
    DensityTier tier = new DensityTier()
    {
      density = Mathf.Pow(2f, (float) p),
      groupID = p
    };
    this.DensityTiers.Add(tier);
    return tier;
  }

  public void Awake()
  {
    if ((UnityEngine.Object) ForestBiome.Instance != (UnityEngine.Object) null)
    {
      UnityEngine.Debug.LogWarning((object) "ForestBiome Instance already exists! Destroying self");
      UnityEngine.Object.Destroy((UnityEngine.Object) this);
    }
    else
    {
      ForestBiome.Instance = (ForestBiome) this;
      this.InitializeConfigurable();
    }
  }

  public void Start() => this.StartCoroutine(this.WaitForModLoad());

  [DebuggerHidden]
  public IEnumerator WaitForModLoad()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new HeightMapForestBiome.\u003CWaitForModLoad\u003Ec__Iterator144()
    {
      \u003C\u003Ef__this = this
    };
  }

  public void OnDisable()
  {
    ModManager.Instance.RevertModHandler((IModHandler) this);
    ModManager.Instance.UnregisterHandler((IModHandler) this);
  }

  public void Initialize()
  {
    this.spawnObjects = UnityEngine.Object.FindObjectsOfType<ForestBiomeSettings>();
    foreach (ForestBiomeSettings spawnObject in this.spawnObjects)
    {
      if (spawnObject.SpawnObject.GetComponent(typeof (IStaticSpawnable)) is IStaticSpawnable component)
      {
        component.ID = this.spawnables.Count;
        this.spawnables.Add(component);
      }
      DensityTier tier = this.DensityToTier(spawnObject.density);
      spawnObject.DensityTier = tier;
      tier.spawnObjects.Add(spawnObject);
    }
    float num1 = 0.0f;
    float num2 = 0.0f;
    for (int index = 0; index < 100; ++index)
    {
      float num3 = this.PerlinFunction((float) (index * 10000), (float) (index * 1000), (float) (index * 10)) - 0.5f;
      num2 += num3 * num3;
      num1 += num3;
    }
    float num4 = Mathf.Sqrt(num2 / 100f);
    float num5 = num1 / 100f;
    Logger.DebugFormat<HeightMapForestBiome>("Standard Deviation: {0}.", (object) num4);
    this._perlinScale = 0.28868f / num4;
  }

  public float PerlinFunction(float x, float y, float z)
  {
    return (float) ((double) PerlinNoiseFloat.Get3DSingle(new Vector3(x * 3.14159274f, y * 3.14159274f, z * 3.14159274f)) * (double) this._perlinScale + 0.5);
  }

  public override float SpawnBiomeObjects(
    List<object> spawnedObjects,
    Transform spawnParent,
    Bounds2 spawnArea,
    float minSpawnRange,
    float maxSpawnRange)
  {
    float maxUpdateDistance = float.NegativeInfinity;
    for (int index1 = 0; index1 < this.DensityTiers.Count; ++index1)
    {
      DensityTier densityTier = this.DensityTiers[index1];
      float spawnDistance = densityTier.SpawnDistance;
      if ((double) minSpawnRange < (double) spawnDistance && (double) maxSpawnRange >= (double) spawnDistance)
      {
        float density = densityTier.density;
        float num1 = 0.0001f;
        float spacing = 1f / Mathf.Sqrt(density);
        for (float x = Mathf.Ceil(spawnArea.min.x / spacing) * spacing; (double) x < (double) spawnArea.Max.x - (double) num1; x += spacing)
        {
          for (float z = Mathf.Ceil(spawnArea.min.y / spacing) * spacing; (double) z < (double) spawnArea.Max.y - (double) num1; z += spacing)
          {
            Vector3 position1 = new Vector3(x, 0.0f, z);
            float y = TerrainAPIBase.GetNearestDownwardSurface(position1);
            Vector3 spawnLocation = new Vector3(position1.x, y, position1.z);
            int num2 = 0;
            for (int index2 = 0; index2 < densityTier.spawnObjects.Count; ++index2)
            {
              ForestBiomeSettings spawnObject = densityTier.spawnObjects[index2];
              if (spawnObject.randomSpawns)
              {
                ++num2;
                Vector3 position2 = position1;
                if (spawnObject.jitter)
                {
                  float t1 = this.PerlinFunction(position2.x, position2.z, (float) (1 + num2)) * 100f;
                  float t2 = this.PerlinFunction(position2.x, position2.z, (float) (2 + num2)) * 100f;
                  position2.x += (Mathf.Repeat(t1, 1f) - 0.5f) * spacing;
                  position2.z += (Mathf.Repeat(t2, 1f) - 0.5f) * spacing;
                }
                BiomeID biomeID = TerrainAPIBase.GetBiomeAt(position2);
                ForestBiomeSettings.Biome biome = spawnObject.SettingsForBiome(biomeID);
                if (biome != null)
                {
                  float num3 = Mathf.Repeat(this.PerlinFunction(position2.x * spawnObject.jitterPerlinScale, position2.z * spawnObject.jitterPerlinScale, (float) (3 + num2)), 1f);
                  if ((double) spawnObject.density / (double) spawnObject.DensityTier.density * (double) biome.likelihood * (double) biome.likelihoodAtAltitude.Evaluate(spawnLocation.y / this.AltitudeScale) >= (double) num3)
                  {
                    Vector3 normal = new Vector3(0.0f, 1f, 0.0f);
                    if (this.terrainSurfacePositionNormal(spawnObject, ref position2, ref normal))
                      HeightMapForestBiome.SpawnObject(spawnedObjects, spawnParent, spacing, spawnLocation, spawnObject, position2, normal, ref maxUpdateDistance);
                  }
                }
              }
            }
          }
        }
      }
    }
    ForestBiomeSettingsSpawnPoint[] settingsSpawnPointArray = HeightMapForestBiome._lastSpawnPoints;
    if ((UnityEngine.Object) HeightMapForestBiome._lastSpawnParent != (UnityEngine.Object) spawnParent)
    {
      settingsSpawnPointArray = spawnParent.GetComponentsInChildren<ForestBiomeSettingsSpawnPoint>();
      HeightMapForestBiome._lastSpawnPoints = settingsSpawnPointArray;
      HeightMapForestBiome._lastSpawnParent = spawnParent;
    }
    for (int index = 0; index < settingsSpawnPointArray.Length; ++index)
    {
      ForestBiomeSettingsSpawnPoint sp = settingsSpawnPointArray[index];
      if (!((UnityEngine.Object) sp == (UnityEngine.Object) null) && spawnArea.Contains(new Vector2(sp.transform.position.x, sp.transform.position.z)))
        this.SpawnObjectForSpawnPoint(spawnedObjects, sp);
    }
    return maxUpdateDistance;
  }

  public override void SpawnObjectForSpawnPoint(
    List<object> spawnedObjects,
    ForestBiomeSettingsSpawnPoint sp)
  {
    ForestBiomeSettings forestBiomeSettings = sp.ForestBiomeSettings;
    if (sp.traceToGround)
      sp.transform.position += Vector3.up * (TerrainAPIBase.GetNearestDownwardSurface(sp.transform.position) - sp.transform.position.y);
    Vector3 vector3 = new Vector3(sp.transform.position.x, sp.transform.position.y, sp.transform.position.z);
    Vector3 normal = new Vector3(0.0f, 1f, 0.0f);
    if (forestBiomeSettings.rotateToTerrain)
      normal = TerrainAPIBase.GetNearestDownwardSurfaceNormal(sp.transform.position);
    float maxUpdateDistance = 0.0f;
    HeightMapForestBiome.SpawnObject(spawnedObjects, sp.transform, 0.0f, vector3, forestBiomeSettings, vector3, normal, ref maxUpdateDistance);
  }

  private static void SpawnObject(
    List<object> spawnedObjects,
    Transform spawnParent,
    float spacing,
    Vector3 spawnLocation,
    ForestBiomeSettings so,
    Vector3 position,
    Vector3 normal,
    ref float maxUpdateDistance)
  {
    object obj = StaticSpawnUtil.Spawn(so.Spawnable, spawnParent, position, spawnLocation, normal);
    if (obj == null)
      return;
    spawnedObjects.Add(obj);
    if (!(obj is IStaticObject staticObject))
      return;
    maxUpdateDistance = Math.Max(maxUpdateDistance, staticObject.CheckDistance() + spacing);
  }

  public bool terrainSurfacePositionNormal(
    ForestBiomeSettings so,
    ref Vector3 position,
    ref Vector3 normal)
  {
    float num = TerrainAPIBase.GetNearestDownwardSurface(position);
    position.y = num;
    if (so.rotateToTerrain)
      normal = TerrainAPIBase.GetNearestDownwardSurfaceNormal(position);
    if ((double) so.randomNormalAmount > 0.0)
    {
      Vector3 normalized = new Vector3((float) PerlinNoise.Get2DWithVariation(new Vector2Double((double) position.x, (double) position.z) * 3.1415927410125732, -3.1415927410125732, 1.0, 1.0, 1) * 100f, (float) PerlinNoise.Get2DWithVariation(new Vector2Double((double) position.x, (double) position.z) * 3.1415927410125732, -6.2831854820251465, 1.0, 1.0, 1) * 100f, (float) PerlinNoise.Get2DWithVariation(new Vector2Double((double) position.x, (double) position.z) * 3.1415927410125732, -9.42477798461914, 1.0, 1.0, 1) * 100f).normalized;
      normal += normalized * so.randomNormalAmount;
      normal.Normalize();
    }
    return (double) normal.y >= (double) so.MinNormalizedY;
  }

  public void ApplyConfiguration()
  {
    if ((double) this._previousSpawnRatio == (double) this.SpawnRatio && (double) this._previousModelRatio == (double) this.ModelRatio && this._previousEnableUndergrowth == this.EnableUndergrowth)
      return;
    this._previousSpawnRatio = this.SpawnRatio;
    this._previousModelRatio = this.ModelRatio;
    this._previousEnableUndergrowth = this.EnableUndergrowth;
    SpriteObject.closeObjects.Clear();
    SpriteObjectInstance.DestroyAll();
    foreach (Component activeRenderer in CodeHatch.Shocktree.SpriteRenderer.activeRenderers)
      UnityEngine.Object.Destroy((UnityEngine.Object) activeRenderer.gameObject);
    foreach (SpriteGroup group in SpriteGroup.groups)
      group.UpdateMaterials();
    foreach (SpawnColumn spawnColumn in SpawnColumn.spawnColumns)
      spawnColumn.Respawn();
    if (!((UnityEngine.Object) SpawnColumnHandler.Instance != (UnityEngine.Object) null))
      ;
    Game.RegisterLoader((IProgressLoader) SpawnColumnHandler.Instance);
  }

  public string ModName => "Resources";

  public void GetModDefaults(IList<ModEntry> defaultModList)
  {
    defaultModList.Add(new ModEntry("RestoreDelayMultiplier", (object) this.RestoreDelayMultiplier));
    for (int index1 = 0; index1 < ForestBiomeSettings.ForestBiomeSettingsList.Count; ++index1)
    {
      ForestBiomeSettings forestBiomeSettings = ForestBiomeSettings.ForestBiomeSettingsList[index1];
      if (forestBiomeSettings.biomeSelection.Count > 0)
        defaultModList.Add(new ModEntry(string.Format("Spawning.{0}.{1}.Density", (object) Game.CurrentGameLevel.Name, (object) forestBiomeSettings.gameObject.name), (object) forestBiomeSettings.density));
      if (forestBiomeSettings.DeterminedSpawnPoints.Count > 0)
      {
        List<Vector3ModParseable> vector3ModParseableList = new List<Vector3ModParseable>();
        for (int index2 = 0; index2 < forestBiomeSettings.DeterminedSpawnPoints.Count; ++index2)
        {
          ForestBiomeSettingsSpawnPoint determinedSpawnPoint = forestBiomeSettings.DeterminedSpawnPoints[index2];
          vector3ModParseableList.Add(new Vector3ModParseable()
          {
            Value = determinedSpawnPoint.transform.position
          });
        }
        defaultModList.Add(new ModEntry(string.Format("Spawning.{0}.{1}.DeterminedPoints", (object) Game.CurrentGameLevel.Name, (object) forestBiomeSettings.gameObject.name), (object) vector3ModParseableList));
      }
    }
  }

  public void ApplyMod(string key, object value)
  {
    string[] strArray = key.Split('.');
    string key1 = strArray[0];
    if (key1 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    if (HeightMapForestBiome.\u003C\u003Ef__switch\u0024map38 == null)
    {
      // ISSUE: reference to a compiler-generated field
      HeightMapForestBiome.\u003C\u003Ef__switch\u0024map38 = new Dictionary<string, int>(2)
      {
        {
          "RestoreDelayMultiplier",
          0
        },
        {
          "Spawning",
          1
        }
      };
    }
    int num1;
    // ISSUE: reference to a compiler-generated field
    if (!HeightMapForestBiome.\u003C\u003Ef__switch\u0024map38.TryGetValue(key1, out num1))
      return;
    switch (num1)
    {
      case 0:
        this.RestoreDelayMultiplier = Mathf.Max(0.0f, (float) value);
        break;
      case 1:
        if (strArray.Length < 4 || string.Compare(Game.CurrentGameLevel.Name, strArray[1]) != 0)
          break;
        for (int index1 = 0; index1 < ForestBiomeSettings.ForestBiomeSettingsList.Count; ++index1)
        {
          ForestBiomeSettings forestBiomeSettings = ForestBiomeSettings.ForestBiomeSettingsList[index1];
          if (string.Compare(forestBiomeSettings.gameObject.name, strArray[2]) == 0)
          {
            string key2 = strArray[3];
            if (key2 == null)
              break;
            // ISSUE: reference to a compiler-generated field
            if (HeightMapForestBiome.\u003C\u003Ef__switch\u0024map37 == null)
            {
              // ISSUE: reference to a compiler-generated field
              HeightMapForestBiome.\u003C\u003Ef__switch\u0024map37 = new Dictionary<string, int>(2)
              {
                {
                  "Density",
                  0
                },
                {
                  "DeterminedPoints",
                  1
                }
              };
            }
            int num2;
            // ISSUE: reference to a compiler-generated field
            if (!HeightMapForestBiome.\u003C\u003Ef__switch\u0024map37.TryGetValue(key2, out num2))
              break;
            switch (num2)
            {
              case 0:
                forestBiomeSettings.density = Mathf.Max(0.0f, (float) value);
                return;
              case 1:
                List<Vector3ModParseable> vector3ModParseableList = (List<Vector3ModParseable>) value;
                List<ForestBiomeSettingsSpawnPoint> settingsSpawnPointList = new List<ForestBiomeSettingsSpawnPoint>((IEnumerable<ForestBiomeSettingsSpawnPoint>) forestBiomeSettings.DeterminedSpawnPoints);
                ForestBiomeSettingsSpawnPoint determinedSpawnPoint = forestBiomeSettings.DeterminedSpawnPoints.Count <= 0 ? (ForestBiomeSettingsSpawnPoint) null : forestBiomeSettings.DeterminedSpawnPoints[0];
                forestBiomeSettings.DeterminedSpawnPoints.Clear();
                for (int index2 = 0; index2 < vector3ModParseableList.Count; ++index2)
                {
                  ForestBiomeSettingsSpawnPoint settingsSpawnPoint = (ForestBiomeSettingsSpawnPoint) null;
                  for (int index3 = 0; index3 < settingsSpawnPointList.Count; ++index3)
                  {
                    if ((double) settingsSpawnPointList[index3].transform.position.GetDistance(vector3ModParseableList[index2].Value) < 9.9999997473787516E-05)
                    {
                      settingsSpawnPoint = settingsSpawnPointList[index3];
                      settingsSpawnPointList.RemoveAt(index3);
                      break;
                    }
                  }
                  if ((UnityEngine.Object) settingsSpawnPoint == (UnityEngine.Object) null)
                  {
                    settingsSpawnPoint = !((UnityEngine.Object) determinedSpawnPoint != (UnityEngine.Object) null) ? new GameObject("ForestBiomeSettingsSpawnPoint").AddComponent<ForestBiomeSettingsSpawnPoint>() : UnityEngine.Object.Instantiate<ForestBiomeSettingsSpawnPoint>(determinedSpawnPoint).GetComponent<ForestBiomeSettingsSpawnPoint>();
                    settingsSpawnPoint.transform.position = vector3ModParseableList[index2].Value;
                    settingsSpawnPoint.ForestBiomeSettings = forestBiomeSettings;
                  }
                  forestBiomeSettings.DeterminedSpawnPoints.Add(settingsSpawnPoint);
                }
                for (int index4 = 0; index4 < settingsSpawnPointList.Count; ++index4)
                  UnityEngine.Object.Destroy((UnityEngine.Object) settingsSpawnPointList[index4].gameObject);
                return;
              default:
                return;
            }
          }
        }
        break;
    }
  }
}
