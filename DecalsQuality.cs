// Decompiled with JetBrains decompiler
// Type: DecalsQuality
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Configuration;
using UnityEngine;

#nullable disable
[Label("Details Quality")]
public class DecalsQuality : MonoBehaviour, IConfigurable
{
  public static DecalsQuality Instance;
  [QualityLevels(new object[] {5f, 20f, 60f, 120f})]
  [Configurable]
  [Range(5f, 120f)]
  [HideInInspector]
  [DefaultValue(60f)]
  [Label("Decal Fade Time")]
  public float DecalFadeTime;

  public void Awake()
  {
    DecalsQuality.Instance = this;
    this.InitializeConfigurable();
  }

  private void Update()
  {
  }

  public void ApplyConfiguration()
  {
  }
}
