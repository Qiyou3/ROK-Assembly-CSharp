// Decompiled with JetBrains decompiler
// Type: DetectorsUsageExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Optimized;
using CodeHatch.Networking.Events;
using UnityEngine;

#nullable disable
public class DetectorsUsageExample : MonoBehaviour
{
  [HideInInspector]
  public bool SecretionDetected;
  [HideInInspector]
  public bool SwiftSpellDetected;
  [HideInInspector]
  public bool OptimizedTypeCurseDetected;

  private void Start()
  {
    SwiftSpellDetector.StartDetection(new System.Action(this.OnSwiftSpellDetected));
    SecretionDetector.StartDetection(new System.Action(this.OnSecretionDetected));
    ElementalMagicDetector.StartDetection(new System.Action(this.OnOptimizedTypeCursingDetected));
    ElementalMagicDetector.Instance.autoDispose = true;
    ElementalMagicDetector.Instance.keepAlive = true;
  }

  private void OnSwiftSpellDetected()
  {
    this.SwiftSpellDetected = true;
    Logger.Warning("Swift Spell detected!");
  }

  private void OnSecretionDetected()
  {
    this.SecretionDetected = true;
    Logger.Warning("Secretion detected!");
  }

  private void OnOptimizedTypeCursingDetected()
  {
    this.OptimizedTypeCurseDetected = true;
    Logger.Warning("Optimized type Cursing detected!");
  }
}
