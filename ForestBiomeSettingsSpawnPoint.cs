// Decompiled with JetBrains decompiler
// Type: ForestBiomeSettingsSpawnPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Shocktree;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class ForestBiomeSettingsSpawnPoint : MonoBehaviour
{
  public static List<ForestBiomeSettingsSpawnPoint> AvailableSpawnPoints = new List<ForestBiomeSettingsSpawnPoint>();
  public ForestBiomeSettings ForestBiomeSettings;
  public bool traceToGround = true;
  public bool useCustomScale;

  public void OnDrawGizmos()
  {
    if (Application.isPlaying)
      return;
    RaycastHit hitInfo;
    if (this.traceToGround && Physics.Raycast(this.transform.position + Vector3.up * 100f, Vector3.down, out hitInfo, 200f))
      this.transform.position = hitInfo.point;
    Gizmos.DrawIcon(this.transform.position, "StaticSpawnPoint.png");
  }

  public void Awake()
  {
    if (!((Object) this.ForestBiomeSettings != (Object) null))
      return;
    this.ForestBiomeSettings.DeterminedSpawnPoints.Add(this);
  }

  public void Start()
  {
    if (!((Object) this.ForestBiomeSettings != (Object) null))
      return;
    this.transform.parent = this.ForestBiomeSettings.transform;
    if (this.ForestBiomeSettings.DeterminedSpawnPoints.Contains(this))
      return;
    this.ForestBiomeSettings.DeterminedSpawnPoints.Add(this);
  }

  public void OnDestroy()
  {
    if ((Object) this.ForestBiomeSettings == (Object) null)
      return;
    this.ForestBiomeSettings.DeterminedSpawnPoints.Remove(this);
  }

  public static ForestBiomeSettingsSpawnPoint AddPointFromPrefab(
    GameObject prefab,
    Vector3 position,
    float scale = 1)
  {
    foreach (ForestBiomeSettings forestBiomeSettings in ForestBiomeSettings.ForestBiomeSettingsList)
    {
      SpriteGroup component = forestBiomeSettings.SpawnObject.GetComponent<SpriteGroup>();
      if (!((Object) component == (Object) null) && !((Object) component.prefab != (Object) prefab))
      {
        ForestBiomeSettingsSpawnPoint sp = new GameObject(nameof (ForestBiomeSettingsSpawnPoint)).AddComponent<ForestBiomeSettingsSpawnPoint>();
        sp.transform.position = position;
        sp.transform.localScale = Vector3.one * scale;
        sp.ForestBiomeSettings = forestBiomeSettings;
        sp.useCustomScale = true;
        SpawnColumn.SpawnObjectForSpawnPoint(sp);
        return sp;
      }
    }
    return (ForestBiomeSettingsSpawnPoint) null;
  }
}
