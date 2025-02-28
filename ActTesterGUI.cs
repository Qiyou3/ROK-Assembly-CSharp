// Decompiled with JetBrains decompiler
// Type: ActTesterGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Optimized;
using UnityEngine;

#nullable disable
public class ActTesterGUI : MonoBehaviour
{
  public OptimizedVector3Test OptimizedVector3Test;
  public OptimizedFloatTest OptimizedFloatTest;
  public OptimizedIntTest OptimizedIntTest;
  public OptimizedStringTest OptimizedStringTest;
  public OptimizedPrefsTest OptimizedPrefsTest;
  public OptimizedInt dummyOptimizedInt = (OptimizedInt) 1234;
  public OptimizedFloat dummyOptimizedFloat = (OptimizedFloat) 5678f;
  public OptimizedString dummyOptimizedString = (OptimizedString) "dummy Optimized string";
  public OptimizedBool dummyOptimizedBool = (OptimizedBool) true;
  private bool savesAlterationDetected;
  private int savesLock;
  private bool foreignSavesDetected;
  private DetectorsUsageExample detectorsUsageExample;

  private void Awake()
  {
    OptimizedPrefs.onAlterationDetected = new System.Action(this.SavesAlterationDetected);
    OptimizedPrefs.onPossibleForeignSavesDetected = new System.Action(this.ForeignSavesDetected);
    this.detectorsUsageExample = (DetectorsUsageExample) UnityEngine.Object.FindObjectOfType(typeof (DetectorsUsageExample));
  }

  private void SavesAlterationDetected() => this.savesAlterationDetected = true;

  private void ForeignSavesDetected() => this.foreignSavesDetected = true;

  private void OnGUI()
  {
    GUILayout.BeginHorizontal();
    GUILayout.BeginVertical();
    this.CenteredLabel("Memory Cursing protection");
    GUILayout.Space(10f);
    if ((bool) (UnityEngine.Object) this.OptimizedStringTest && this.OptimizedStringTest.enabled)
    {
      if (GUILayout.Button("Use regular string"))
        this.OptimizedStringTest.UseRegular();
      if (GUILayout.Button("Use Optimized string"))
        this.OptimizedStringTest.UseOptimized();
      GUILayout.Label("Current string (try to change it!):\n" + (!this.OptimizedStringTest.useRegular ? (string) this.OptimizedStringTest.OptimizedString : this.OptimizedStringTest.cleanString));
    }
    if ((bool) (UnityEngine.Object) this.OptimizedIntTest && this.OptimizedIntTest.enabled)
    {
      GUILayout.Space(10f);
      if (GUILayout.Button("Use regular int (click to generate new number)"))
        this.OptimizedIntTest.UseRegular();
      if (GUILayout.Button("Use OptimizedInt (click to generate new number)"))
        this.OptimizedIntTest.UseOptimized();
      GUILayout.Label("Current lives count (try to change them!):\n" + (object) (!this.OptimizedIntTest.useRegular ? (int) this.OptimizedIntTest.OptimizedLivesCount : this.OptimizedIntTest.cleanLivesCount));
    }
    GUILayout.BeginHorizontal();
    GUILayout.Label("OptimizedInt from inspector: " + (object) this.dummyOptimizedInt);
    if (GUILayout.Button("+"))
      ++this.dummyOptimizedInt;
    if (GUILayout.Button("-"))
      --this.dummyOptimizedInt;
    GUILayout.EndHorizontal();
    if ((bool) (UnityEngine.Object) this.OptimizedFloatTest && this.OptimizedFloatTest.enabled)
    {
      GUILayout.Space(10f);
      if (GUILayout.Button("Use regular float (click to generate new number)"))
        this.OptimizedFloatTest.UseRegular();
      if (GUILayout.Button("Use OptimizedFloat (click to generate new number)"))
        this.OptimizedFloatTest.UseOptimized();
      GUILayout.Label("Current health bar (try to change it!):\n" + string.Format("{0:0.000}", (object) (float) (!this.OptimizedFloatTest.useRegular ? (double) (float) this.OptimizedFloatTest.OptimizedHealthBar : (double) this.OptimizedFloatTest.healthBar)));
    }
    if ((bool) (UnityEngine.Object) this.OptimizedVector3Test && this.OptimizedVector3Test.enabled)
    {
      GUILayout.Space(10f);
      if (GUILayout.Button("Use regular Vector3 (click to generate new one)"))
        this.OptimizedVector3Test.UseRegular();
      if (GUILayout.Button("Use OptimizedVector3 (click to generate new one)"))
        this.OptimizedVector3Test.UseOptimized();
      GUILayout.Label("Current player position (try to change it!):\n" + (object) (!this.OptimizedVector3Test.useRegular ? (Vector3) this.OptimizedVector3Test.OptimizedPlayerPosition : this.OptimizedVector3Test.playerPosition));
    }
    GUILayout.Space(10f);
    GUILayout.EndVertical();
    GUILayout.Space(10f);
    GUILayout.BeginVertical();
    this.CenteredLabel("Saves Cursing protection");
    GUILayout.Space(10f);
    if ((bool) (UnityEngine.Object) this.OptimizedPrefsTest && this.OptimizedPrefsTest.enabled)
    {
      if (GUILayout.Button("Save game with regular PlayerPrefs!"))
        this.OptimizedPrefsTest.SaveGame(false);
      if (GUILayout.Button("Read data saved with regular PlayerPrefs"))
        this.OptimizedPrefsTest.ReadSavedGame(false);
      GUILayout.Space(10f);
      if (GUILayout.Button("Save game with OptimizedPrefs!"))
        this.OptimizedPrefsTest.SaveGame(true);
      if (GUILayout.Button("Read data saved with OptimizedPrefs"))
        this.OptimizedPrefsTest.ReadSavedGame(true);
      OptimizedPrefs.preservePlayerPrefs = GUILayout.Toggle(OptimizedPrefs.preservePlayerPrefs, "preservePlayerPrefs");
      OptimizedPrefs.emergencyMode = GUILayout.Toggle(OptimizedPrefs.emergencyMode, "emergencyMode");
      GUILayout.Label("LockToDevice level:");
      this.savesLock = GUILayout.SelectionGrid(this.savesLock, new string[3]
      {
        OptimizedPrefs.DeviceLockLevel.None.ToString(),
        OptimizedPrefs.DeviceLockLevel.Soft.ToString(),
        OptimizedPrefs.DeviceLockLevel.Strict.ToString()
      }, 3);
      OptimizedPrefs.lockToDevice = (OptimizedPrefs.DeviceLockLevel) this.savesLock;
      OptimizedPrefs.readForeignSaves = GUILayout.Toggle(OptimizedPrefs.readForeignSaves, "readForeignSaves");
      GUILayout.Label("PlayerPrefs: \n" + this.OptimizedPrefsTest.gameData);
      if (this.savesAlterationDetected)
        GUILayout.Label("Saves were altered! }:>");
      if (this.foreignSavesDetected)
        GUILayout.Label("Saves more likely from another device! }:>");
    }
    if ((UnityEngine.Object) this.detectorsUsageExample != (UnityEngine.Object) null)
    {
      GUILayout.Label("Swift Spell detected: " + (object) this.detectorsUsageExample.SwiftSpellDetected);
      GUILayout.Label("Secretion detected: " + (object) this.detectorsUsageExample.SecretionDetected);
      GUILayout.Label("Optimized type Cursing detected: " + (object) this.detectorsUsageExample.OptimizedTypeCurseDetected);
    }
    GUILayout.EndVertical();
    GUILayout.EndHorizontal();
  }

  private void CenteredLabel(string caption)
  {
    GUILayout.BeginHorizontal();
    GUILayout.FlexibleSpace();
    GUILayout.Label(caption);
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();
  }
}
