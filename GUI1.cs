// Decompiled with JetBrains decompiler
// Type: GUI1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core;
using UMA.UMAWizard;
using UMA.UMAWizard.Races;
using UnityEngine;

#nullable disable
public class GUI1 : UMAWGUIBase
{
  public const int OPTION_SHAPES = 0;
  public const int OPTION_PARTS = 1;
  public bool show;
  private float[] sliders;
  private GameObject[] avatars;
  private bool showAvatars;
  private bool showOptions;
  private Vector2 shapepos;
  private Vector2 clothingpos;
  private string[] OptionSections = new string[2]
  {
    "Shapes",
    "Parts"
  };
  private int optionSection;
  private int shapeSection;

  private void Awake()
  {
    this.showAvatars = false;
    this.showOptions = false;
    this.sliders = (float[]) null;
    this.avatars = (GameObject[]) null;
  }

  private void Update()
  {
    if (!Input.GetKeyUp(KeyCode.Escape))
      return;
    Program.Exit();
  }

  private void OnGUI()
  {
    if (!this.show)
      return;
    GUILayout.BeginArea(new Rect(10f, 10f, 300f, 400f));
    if (this.showAvatars)
    {
      for (int index = 0; index < this.avatars.Length; ++index)
      {
        if (GUITools.ProposeSelection(this.avatars[index].name) && this.SelectAvatar(this.avatars[index]))
        {
          this.sliders = this.GetAvatarShapeValues();
          this.showAvatars = false;
          this.showOptions = true;
          this.optionSection = 0;
        }
      }
    }
    else if (GUILayout.Button("Choose avatar"))
    {
      this.showAvatars = true;
      this.showOptions = false;
      this.avatars = UMAWGUIBase.FindAvatarsInScene();
    }
    if (this.showOptions)
    {
      this.optionSection = GUILayout.Toolbar(this.optionSection, this.OptionSections);
      GUILayout.Space(16f);
      switch (this.optionSection)
      {
        case 0:
          this.shapeSection = GUILayout.Toolbar(this.shapeSection, Humanoid.ShapeSections);
          this.shapepos = GUILayout.BeginScrollView(this.shapepos);
          GUITools.ShowHumanShapeControls(this.sliders, this.IsFemale(), this.shapeSection);
          GUILayout.EndScrollView();
          if (GUILayout.Button("Apply"))
          {
            this.SetAvatarShapeValues(this.sliders);
            break;
          }
          break;
        case 1:
          this.clothingpos = GUILayout.BeginScrollView(this.clothingpos);
          GUILayout.BeginHorizontal();
          string slotname = GUITools.ShowAllSlotPreviews(this.context.slotLibrary, this.umawGUI.slotPreviewKeys, this.umawGUI.slotPreviewValues, this.umawGUI.slotPreviewTex);
          GUILayout.EndHorizontal();
          GUILayout.EndScrollView();
          if (!string.IsNullOrEmpty(slotname))
            this.AddSlotAndOverlayToAvatar(slotname, (string) null);
          if (GUILayout.Button("Apply"))
          {
            this.SetAvatarShapeValues(this.sliders);
            break;
          }
          break;
      }
    }
    GUILayout.Space(16f);
    if (GUILayout.Button("Done"))
      this.show = false;
    GUILayout.EndArea();
  }
}
