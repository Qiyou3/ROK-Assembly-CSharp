// Decompiled with JetBrains decompiler
// Type: Censorer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Configuration;
using System.Collections.Generic;

#nullable disable
[Label("Parental Controls")]
public class Censorer : SingletonMonoBehaviour<Censorer>, IConfigurable
{
  private static readonly List<ICensorable> _allCensorables = new List<ICensorable>();
  public bool bloodIsCensored;

  public static void AddCensorable(ICensorable censorable)
  {
    Censorer._allCensorables.AddDistinct<ICensorable>(censorable);
  }

  public static void RemoveCensorable(ICensorable censorable)
  {
    Censorer._allCensorables.Remove(censorable);
  }

  [DefaultValue(false)]
  [Label("Disable Blood/Gore")]
  [Configurable]
  public bool BloodIsCensored
  {
    get => this.bloodIsCensored;
    set => this.SetCensored(CensorLayer.Blood, value);
  }

  public bool IsCensored(CensorLayer layer) => layer == CensorLayer.Blood && this.bloodIsCensored;

  public void SetCensored(CensorLayer layer, bool isCensored)
  {
    if (layer == CensorLayer.Blood)
    {
      if (this.bloodIsCensored == isCensored)
        return;
      this.bloodIsCensored = isCensored;
      this.UpdateCensorship(layer, isCensored);
    }
    else
      this.LogError<Censorer>("CensorLayer layer argument out of range. (layer: {0}, isCensored: {1})", (object) layer, (object) isCensored);
  }

  private void UpdateCensorship(CensorLayer layer, bool isCensored)
  {
    foreach (ICensorable allCensorable in Censorer._allCensorables)
      allCensorable.SetCensored(layer, isCensored);
  }

  public void UpdateAllCensorship()
  {
    foreach (ICensorable allCensorable in Censorer._allCensorables)
      allCensorable.SetCensored(CensorLayer.Blood, this.bloodIsCensored);
  }

  public override void Awake()
  {
    base.Awake();
    this.InitializeConfigurable();
  }

  public void ApplyConfiguration() => this.UpdateAllCensorship();

  public void Start() => this.UpdateAllCensorship();
}
