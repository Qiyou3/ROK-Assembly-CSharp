// Decompiled with JetBrains decompiler
// Type: DEMO_UOC_GUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DEMO_UOC_GUI : MonoBehaviour
{
  public UltimateOrbitCamera target;
  private int height;
  private bool showOrbitOptions;
  private bool showMouseOptions;
  private bool showKeyboardOptions;
  private bool showAutoRotateOptions;
  private bool showSpinOptions;
  private bool showCollisionOptions;
  private int LabelWidth = 80;
  private int SliderWidth = 180;
  private Vector2 scrollPos = new Vector2(0.0f, 0.0f);

  private void Start()
  {
  }

  private void Update()
  {
    this.target.invertXValue = !this.target.invertAxisX ? 1 : -1;
    this.target.invertYValue = !this.target.invertAxisY ? 1 : -1;
    this.target.invertZoomValue = !this.target.invertAxisZoom ? 1 : -1;
    if (this.target.autoRotateReverse)
      this.target.autoRotateReverseValue = -1;
    else
      this.target.autoRotateReverseValue = 1;
  }

  private void OnGUI()
  {
    this.height = 204;
    if (this.showOrbitOptions)
      this.height += 570;
    if (this.showMouseOptions)
      this.height += 130;
    if (this.showKeyboardOptions)
      this.height += 40;
    if (this.showAutoRotateOptions)
      this.height += 80;
    if (this.showSpinOptions)
      this.height += 100;
    if (this.showCollisionOptions)
      this.height += 80;
    if (GUI.Button(new Rect((float) (Screen.width - 105), 5f, 100f, 50f), "Reset"))
      Application.LoadLevel(Application.loadedLevel);
    GUI.Box(new Rect(10f, 10f, 340f, (float) Mathf.Min(this.height, Screen.height - 20)), string.Empty);
    GUILayout.BeginArea(new Rect(12f, 12f, 336f, (float) Mathf.Min(this.height - 4, Screen.height - 24)));
    this.scrollPos = GUILayout.BeginScrollView(this.scrollPos);
    this.showOrbitOptions = GUILayout.Toggle(this.showOrbitOptions, " Show Orbit Options");
    GUILayout.Space(4f);
    if (this.showOrbitOptions)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Label("Axis X Speed", GUILayout.Width((float) this.LabelWidth));
      this.target.xSpeed = GUILayout.HorizontalSlider(this.target.xSpeed, 0.0f, 2f, GUILayout.Width((float) this.SliderWidth));
      GUILayout.Label(this.target.xSpeed.ToString("F"));
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Axis Y Speed", GUILayout.Width((float) this.LabelWidth));
      this.target.ySpeed = GUILayout.HorizontalSlider(this.target.ySpeed, 0.0f, 2f, GUILayout.Width((float) this.SliderWidth));
      GUILayout.Label(this.target.ySpeed.ToString("F"));
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Zoom Sensitivity", GUILayout.Width((float) this.LabelWidth));
      this.target.zoomSpeed = GUILayout.HorizontalSlider(this.target.zoomSpeed, 0.0f, 50f, GUILayout.Width((float) this.SliderWidth));
      GUILayout.Label(this.target.zoomSpeed.ToString("F"));
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Axis X Dampening", GUILayout.Width((float) this.LabelWidth));
      this.target.dampeningX = GUILayout.HorizontalSlider(this.target.dampeningX, 0.01f, 0.99f, GUILayout.Width((float) this.SliderWidth));
      GUILayout.Label(this.target.dampeningX.ToString("F"));
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Axis Y Dampening", GUILayout.Width((float) this.LabelWidth));
      this.target.dampeningY = GUILayout.HorizontalSlider(this.target.dampeningY, 0.01f, 0.99f, GUILayout.Width((float) this.SliderWidth));
      GUILayout.Label(this.target.dampeningY.ToString("F"));
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Zoom Smoothing", GUILayout.Width((float) this.LabelWidth));
      this.target.smoothingZoom = GUILayout.HorizontalSlider(this.target.smoothingZoom, 0.01f, 1f, GUILayout.Width((float) this.SliderWidth));
      GUILayout.Label(this.target.smoothingZoom.ToString("F"));
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Min Distance", GUILayout.Width((float) this.LabelWidth));
      this.target.minDistance = Mathf.Min(this.target.maxDistance, GUILayout.HorizontalSlider(this.target.minDistance, 1f, 50f, GUILayout.Width((float) this.SliderWidth)));
      GUILayout.Label(this.target.minDistance.ToString("F"));
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("MaxDistance", GUILayout.Width((float) this.LabelWidth));
      this.target.maxDistance = Mathf.Max(this.target.minDistance, GUILayout.HorizontalSlider(this.target.maxDistance, 1f, 50f, GUILayout.Width((float) this.SliderWidth)));
      GUILayout.Label(this.target.maxDistance.ToString("F"));
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Limit Angle X", GUILayout.Width((float) this.LabelWidth));
      this.target.limitX = GUILayout.Toggle(this.target.limitX, string.Empty);
      GUILayout.EndHorizontal();
      if (!this.target.limitX)
        GUI.color = new Color(0.5f, 0.5f, 0.5f);
      GUILayout.BeginHorizontal();
      GUILayout.Label("Min Angle X", GUILayout.Width((float) this.LabelWidth));
      this.target.xMinLimit = Mathf.Min(this.target.xMaxLimit, GUILayout.HorizontalSlider(this.target.xMinLimit, -180f, 180f, GUILayout.Width((float) this.SliderWidth)));
      GUILayout.Label(this.target.xMinLimit.ToString("F"));
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Max Angle X", GUILayout.Width((float) this.LabelWidth));
      this.target.xMaxLimit = Mathf.Max(this.target.xMinLimit, GUILayout.HorizontalSlider(this.target.xMaxLimit, -180f, 180f, GUILayout.Width((float) this.SliderWidth)));
      GUILayout.Label(this.target.xMaxLimit.ToString("F"));
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("X Limit Offset", GUILayout.Width((float) this.LabelWidth));
      this.target.xLimitOffset = GUILayout.HorizontalSlider(this.target.xLimitOffset, 0.0f, 360f, GUILayout.Width((float) this.SliderWidth));
      GUILayout.Label(this.target.xLimitOffset.ToString("F"));
      GUILayout.EndHorizontal();
      GUI.color = new Color(1f, 1f, 1f);
      GUILayout.BeginHorizontal();
      GUILayout.Label("Limit Angle Y", GUILayout.Width((float) this.LabelWidth));
      this.target.limitY = GUILayout.Toggle(this.target.limitY, string.Empty);
      GUILayout.EndHorizontal();
      if (!this.target.limitY)
        GUI.color = new Color(0.5f, 0.5f, 0.5f);
      GUILayout.BeginHorizontal();
      GUILayout.Label("Min Angle Y", GUILayout.Width((float) this.LabelWidth));
      this.target.yMinLimit = Mathf.Min(this.target.yMaxLimit, GUILayout.HorizontalSlider(this.target.yMinLimit, -180f, 180f, GUILayout.Width((float) this.SliderWidth)));
      GUILayout.Label(this.target.yMinLimit.ToString("F"));
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Max Angle Y", GUILayout.Width((float) this.LabelWidth));
      this.target.yMaxLimit = Mathf.Max(this.target.yMinLimit, GUILayout.HorizontalSlider(this.target.yMaxLimit, -180f, 180f, GUILayout.Width((float) this.SliderWidth)));
      GUILayout.Label(this.target.yMaxLimit.ToString("F"));
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Y Limit Offset", GUILayout.Width((float) this.LabelWidth));
      this.target.yLimitOffset = GUILayout.HorizontalSlider(this.target.yLimitOffset, 0.0f, 360f, GUILayout.Width((float) this.SliderWidth));
      GUILayout.Label(this.target.yLimitOffset.ToString("F"));
      GUILayout.EndHorizontal();
      GUI.color = new Color(1f, 1f, 1f);
      GUILayout.BeginHorizontal();
      GUILayout.Label("Invert Axis X", GUILayout.Width((float) this.LabelWidth));
      this.target.invertAxisX = GUILayout.Toggle(this.target.invertAxisX, string.Empty);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Invert Axis Y", GUILayout.Width((float) this.LabelWidth));
      this.target.invertAxisY = GUILayout.Toggle(this.target.invertAxisY, string.Empty);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Invert Zoom", GUILayout.Width((float) this.LabelWidth));
      this.target.invertAxisZoom = GUILayout.Toggle(this.target.invertAxisZoom, string.Empty);
      GUILayout.EndHorizontal();
    }
    GUILayout.Space(8f);
    this.showMouseOptions = GUILayout.Toggle(this.showMouseOptions, " Show Mouse Input Options");
    GUILayout.Space(4f);
    if (this.showMouseOptions)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Label("Mouse Controls", GUILayout.Width((float) this.LabelWidth));
      this.target.mouseControl = GUILayout.Toggle(this.target.mouseControl, string.Empty);
      GUILayout.EndHorizontal();
      if (!this.target.mouseControl)
        GUI.color = new Color(0.5f, 0.5f, 0.5f);
      GUILayout.BeginHorizontal();
      GUILayout.Label("Click To Rotate", GUILayout.Width((float) this.LabelWidth));
      this.target.clickToRotate = GUILayout.Toggle(this.target.clickToRotate, string.Empty);
      GUILayout.EndHorizontal();
      if (!this.target.clickToRotate)
        GUI.color = new Color(0.5f, 0.5f, 0.5f);
      GUILayout.BeginHorizontal();
      GUILayout.Label("Left Click", GUILayout.Width((float) this.LabelWidth));
      this.target.leftClickToRotate = GUILayout.Toggle(this.target.leftClickToRotate, string.Empty);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Right Click", GUILayout.Width((float) this.LabelWidth));
      this.target.rightClickToRotate = GUILayout.Toggle(this.target.rightClickToRotate, string.Empty);
      GUILayout.EndHorizontal();
      GUI.color = new Color(1f, 1f, 1f);
    }
    GUILayout.Space(8f);
    this.showKeyboardOptions = GUILayout.Toggle(this.showKeyboardOptions, " Show Keyboard Input Options");
    GUILayout.Space(4f);
    if (this.showKeyboardOptions)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Label("Keyboard Controls", GUILayout.Width((float) this.LabelWidth));
      this.target.keyboardControl = GUILayout.Toggle(this.target.keyboardControl, string.Empty);
      GUILayout.EndHorizontal();
    }
    GUILayout.Space(8f);
    this.showAutoRotateOptions = GUILayout.Toggle(this.showAutoRotateOptions, " Show Auto-Rotate Options");
    GUILayout.Space(4f);
    if (this.showAutoRotateOptions)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Label("Auto Rotate", GUILayout.Width((float) this.LabelWidth));
      this.target.autoRotateOn = GUILayout.Toggle(this.target.autoRotateOn, string.Empty);
      GUILayout.EndHorizontal();
      if (!this.target.autoRotateOn)
        GUI.color = new Color(0.5f, 0.5f, 0.5f);
      GUILayout.BeginHorizontal();
      GUILayout.Label("Reverse", GUILayout.Width((float) this.LabelWidth));
      this.target.autoRotateReverse = GUILayout.Toggle(this.target.autoRotateReverse, string.Empty);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Rotate Speed", GUILayout.Width((float) this.LabelWidth));
      this.target.autoRotateSpeed = GUILayout.HorizontalSlider(this.target.autoRotateSpeed, 0.01f, 20f, GUILayout.Width((float) this.SliderWidth));
      GUILayout.Label(this.target.autoRotateSpeed.ToString("F"));
      GUILayout.EndHorizontal();
      GUI.color = new Color(1f, 1f, 1f);
    }
    GUILayout.Space(8f);
    this.showSpinOptions = GUILayout.Toggle(this.showSpinOptions, " Show Spin Options");
    GUILayout.Space(4f);
    if (this.showSpinOptions)
    {
      GUILayout.Label("Hold Left-CTRL and throw to spin.");
      GUILayout.BeginHorizontal();
      GUILayout.Label("Spin Enabled", GUILayout.Width((float) this.LabelWidth));
      this.target.SpinEnabled = GUILayout.Toggle(this.target.SpinEnabled, string.Empty);
      GUILayout.EndHorizontal();
      if (!this.target.SpinEnabled)
        GUI.color = new Color(0.5f, 0.5f, 0.5f);
      GUILayout.BeginHorizontal();
      GUILayout.Label("Max Spin Speed", GUILayout.Width((float) this.LabelWidth));
      this.target.maxSpinSpeed = GUILayout.HorizontalSlider(this.target.maxSpinSpeed, 0.01f, 5f, GUILayout.Width((float) this.SliderWidth));
      GUILayout.Label(this.target.maxSpinSpeed.ToString("F"));
      GUILayout.EndHorizontal();
      GUI.color = new Color(1f, 1f, 1f);
    }
    GUILayout.Space(8f);
    this.showCollisionOptions = GUILayout.Toggle(this.showCollisionOptions, " Show Collision Options");
    GUILayout.Space(4f);
    if (this.showCollisionOptions)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Label("Collision Enabled", GUILayout.Width((float) this.LabelWidth));
      this.target.cameraCollision = GUILayout.Toggle(this.target.cameraCollision, string.Empty);
      GUILayout.EndHorizontal();
      if (!this.target.cameraCollision)
        GUI.color = new Color(0.5f, 0.5f, 0.5f);
      GUILayout.BeginHorizontal();
      GUILayout.Label("Collision Radius", GUILayout.Width((float) this.LabelWidth));
      this.target.collisionRadius = GUILayout.HorizontalSlider(this.target.collisionRadius, 0.01f, 1f, GUILayout.Width((float) this.SliderWidth));
      GUILayout.Label(this.target.collisionRadius.ToString("F"));
      GUILayout.EndHorizontal();
      GUI.color = new Color(1f, 1f, 1f);
    }
    GUILayout.EndScrollView();
    GUILayout.EndArea();
  }
}
