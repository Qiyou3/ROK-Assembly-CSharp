// Decompiled with JetBrains decompiler
// Type: DashBoard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class DashBoard : MonoBehaviour
{
  public int depth = 2;
  public Texture2D tachoMeter;
  [HideInInspector]
  public bool tachoMeterDisabled;
  public Vector2 tachoMeterPosition;
  public DashBoard.Docking tachoMeterDocking;
  public float tachoMeterDimension = 1f;
  public Texture2D tachoMeterNeedle;
  public Vector2 tachoMeterNeedleSize;
  public float tachoMeterNeedleAngle;
  private float actualtachoMeterNeedleAngle;
  public float RPMFactor = 3.5714f;
  public Texture2D speedoMeter;
  [HideInInspector]
  public bool speedoMeterDisabled;
  public Vector2 speedoMeterPosition;
  public DashBoard.Docking speedoMeterDocking = DashBoard.Docking.Right;
  public float speedoMeterDimension = 1f;
  public Texture2D speedoMeterNeedle;
  public Vector2 speedoMeterNeedleSize;
  public float speedoMeterNeedleAngle;
  private float actualspeedoMeterNeedleAngle;
  public float speedoMeterFactor;
  public DashBoard.SpeedoMeterType speedoMeterType;
  public DashBoard.Unit digitalSpeedoUnit;
  public GUIStyle digitalSpeedoStyle;
  public Vector2 digitalSpeedoPosition;
  public DashBoard.Docking digitalSpeedoDocking;
  public GUIStyle gearMonitorStyle;
  public Vector2 gearMonitorPosition;
  public DashBoard.Docking gearMonitorDocking;
  public Texture2D clutchMonitor;
  public Texture2D throttleMonitor;
  public Texture2D brakeMonitor;
  public Vector2 pedalsMonitorPosition;
  public Texture2D ABS;
  public Texture2D TCS;
  public Texture2D ESP;
  public Vector2 dashboardLightsPosition;
  public DashBoard.Docking dashboardLightsDocking;
  public float dashboardLightsDimension = 1f;
  public GameObject digitalSpeedoOnBoard;
  public GameObject digitalGearOnBoard;
  public GameObject tachoMeterNeedleOnBoard;
  public GameObject speedoMeterNeedleOnBoard;
  private TextMesh textMeshSpeed;
  private TextMesh textMeshGear;
  [HideInInspector]
  public bool showGUIDashboard = true;
  [HideInInspector]
  public CarController carController;
  private Drivetrain drivetrain;
  private Rect tachoMeterRect;
  private Rect tachoMeterNeedleRect;
  private Rect speedoMeterRect;
  private Rect speedoMeterNeedleRect;
  private Rect instrumentalPanelRect;
  private Rect gearRect;
  private Rect speedoRect;
  private Vector2 pivot;
  private float speedoMeterVelo;
  private float digitalSpeedoVelo;
  private float factor;
  private float absVelo;
  private int sign = 1;
  private int shift;

  public void Start()
  {
    this.drivetrain = this.transform.parent.GetComponent<Drivetrain>();
    if ((Object) this.digitalSpeedoOnBoard != (Object) null)
      this.textMeshSpeed = this.digitalSpeedoOnBoard.GetComponent<TextMesh>();
    if ((Object) this.digitalGearOnBoard != (Object) null)
      this.textMeshGear = this.digitalGearOnBoard.GetComponent<TextMesh>();
    if (this.digitalSpeedoUnit == DashBoard.Unit.Kmh)
      this.factor = 3.6f;
    else
      this.factor = 2.237f;
  }

  public void OnGUI()
  {
    this.absVelo = this.speedoMeterType != DashBoard.SpeedoMeterType.RigidBody ? Mathf.Abs(this.drivetrain.wheelTireVelo) : Mathf.Abs(this.drivetrain.velo);
    this.speedoMeterVelo = this.absVelo * 3.6f;
    this.digitalSpeedoVelo = this.absVelo * this.factor;
    GUI.depth = this.depth;
    if ((double) this.RPMFactor < 0.0)
      this.RPMFactor = 0.0f;
    if ((bool) (Object) this.drivetrain)
      this.actualtachoMeterNeedleAngle = this.drivetrain.rpm * (this.RPMFactor / 10f) + this.tachoMeterNeedleAngle;
    if ((double) this.speedoMeterFactor < 0.5)
      this.speedoMeterFactor = 0.5f;
    if ((bool) (Object) this.drivetrain)
      this.actualspeedoMeterNeedleAngle = this.speedoMeterVelo * this.speedoMeterFactor + this.speedoMeterNeedleAngle;
    if (!Application.isPlaying || this.showGUIDashboard)
    {
      if ((bool) (Object) this.tachoMeter)
      {
        float num = 0.0f;
        if (this.tachoMeterDocking == DashBoard.Docking.Right)
          num = (float) (Screen.width - this.speedoMeter.width);
        this.tachoMeterRect = new Rect(this.tachoMeterPosition.x + num, (float) ((double) (Screen.height - this.tachoMeter.height) - (double) this.tachoMeterPosition.y + 4.0), (float) this.tachoMeter.width * this.tachoMeterDimension, (float) this.tachoMeter.height * this.tachoMeterDimension);
        GUI.DrawTexture(this.tachoMeterRect, (Texture) this.tachoMeter);
        if ((bool) (Object) this.tachoMeterNeedle)
        {
          if (this.tachoMeterNeedleSize == Vector2.zero)
          {
            this.tachoMeterNeedleSize.x = (float) this.tachoMeterNeedle.width;
            this.tachoMeterNeedleSize.y = (float) this.tachoMeterNeedle.height;
          }
          this.tachoMeterNeedleRect = new Rect((float) ((double) this.tachoMeterRect.x + (double) this.tachoMeterRect.width / 2.0 - (double) this.tachoMeterNeedleSize.x * (double) this.tachoMeterDimension * 0.5), (float) ((double) this.tachoMeterRect.y + (double) this.tachoMeterRect.height / 2.0 - (double) this.tachoMeterNeedleSize.y * (double) this.tachoMeterDimension * 0.5), this.tachoMeterNeedleSize.x * this.tachoMeterDimension, this.tachoMeterNeedleSize.y * this.tachoMeterDimension);
          this.pivot = new Vector2(this.tachoMeterNeedleRect.xMin + this.tachoMeterNeedleRect.width * 0.5f, this.tachoMeterNeedleRect.yMin + this.tachoMeterNeedleRect.height * 0.5f);
          Matrix4x4 matrix = GUI.matrix;
          GUIUtility.RotateAroundPivot(this.actualtachoMeterNeedleAngle, this.pivot);
          GUI.DrawTexture(this.tachoMeterNeedleRect, (Texture) this.tachoMeterNeedle);
          GUI.matrix = matrix;
        }
      }
      if ((bool) (Object) this.speedoMeter)
      {
        float num = 0.0f;
        if (this.speedoMeterDocking == DashBoard.Docking.Right)
          num = (float) (Screen.width - this.speedoMeter.width);
        Rect position = new Rect(this.speedoMeterPosition.x + num, (float) ((double) (Screen.height - this.speedoMeter.height) - (double) this.speedoMeterPosition.y + 4.0), (float) this.speedoMeter.width * this.speedoMeterDimension, (float) this.speedoMeter.height * this.speedoMeterDimension);
        GUI.DrawTexture(position, (Texture) this.speedoMeter);
        if ((bool) (Object) this.speedoMeterNeedle)
        {
          if (this.speedoMeterNeedleSize == Vector2.zero)
          {
            this.speedoMeterNeedleSize.x = (float) this.speedoMeterNeedle.width;
            this.speedoMeterNeedleSize.y = (float) this.speedoMeterNeedle.height;
          }
          this.speedoMeterNeedleRect = new Rect((float) ((double) position.x + (double) position.width / 2.0 - (double) this.speedoMeterNeedleSize.x * (double) this.speedoMeterDimension * 0.5), (float) ((double) position.y + (double) position.height / 2.0 - (double) this.speedoMeterNeedleSize.y * (double) this.speedoMeterDimension * 0.5), this.speedoMeterNeedleSize.x * this.speedoMeterDimension, this.speedoMeterNeedleSize.y * this.speedoMeterDimension);
          this.pivot = new Vector2(this.speedoMeterNeedleRect.xMin + this.speedoMeterNeedleRect.width * 0.5f, this.speedoMeterNeedleRect.yMin + this.speedoMeterNeedleRect.height * 0.5f);
          Matrix4x4 matrix = GUI.matrix;
          GUIUtility.RotateAroundPivot(this.actualspeedoMeterNeedleAngle, this.pivot);
          GUI.DrawTexture(this.speedoMeterNeedleRect, (Texture) this.speedoMeterNeedle);
          GUI.matrix = matrix;
        }
      }
      this.sign = 1;
      this.shift = 0;
      if (this.dashboardLightsDocking == DashBoard.Docking.Right)
      {
        this.sign = -1;
        this.shift = Mathf.RoundToInt((float) Screen.width - (float) (this.TCS.width * 3) * this.dashboardLightsDimension);
      }
      bool flag1 = (Object) this.carController == (Object) null;
      if ((bool) (Object) this.TCS)
      {
        bool flag2 = false;
        if (!flag1)
          flag2 = this.carController.TCSTriggered;
        else if (!Application.isPlaying)
          flag2 = true;
        if (flag2)
          GUI.DrawTexture(new Rect((float) this.sign * this.dashboardLightsPosition.x + (float) this.shift, (float) Screen.height - (float) this.TCS.height * this.dashboardLightsDimension - this.dashboardLightsPosition.y, (float) this.TCS.width * this.dashboardLightsDimension, (float) this.TCS.height * this.dashboardLightsDimension), (Texture) this.TCS);
      }
      if ((bool) (Object) this.ABS)
      {
        bool flag3 = false;
        if (!flag1)
          flag3 = this.carController.ABSTriggered;
        else if (!Application.isPlaying)
          flag3 = true;
        if (flag3)
          GUI.DrawTexture(new Rect((float) ((double) this.TCS.width * (double) this.dashboardLightsDimension + (double) this.sign * (double) this.dashboardLightsPosition.x) + (float) this.shift, (float) Screen.height - (float) this.ABS.height * this.dashboardLightsDimension - this.dashboardLightsPosition.y, (float) this.ABS.width * this.dashboardLightsDimension, (float) this.ABS.height * this.dashboardLightsDimension), (Texture) this.ABS);
      }
      if ((bool) (Object) this.ESP)
      {
        bool flag4 = false;
        if (!flag1)
          flag4 = this.carController.ESPTriggered;
        else if (!Application.isPlaying)
          flag4 = true;
        if (flag4)
          GUI.DrawTexture(new Rect((float) ((double) this.TCS.width * (double) this.dashboardLightsDimension + (double) this.ABS.width * (double) this.dashboardLightsDimension + (double) this.sign * (double) this.dashboardLightsPosition.x) + (float) this.shift, (float) Screen.height - (float) this.ESP.height * this.dashboardLightsDimension - this.dashboardLightsPosition.y, (float) this.ESP.width * this.dashboardLightsDimension, (float) this.ESP.height * this.dashboardLightsDimension), (Texture) this.ESP);
      }
      if ((bool) (Object) this.throttleMonitor)
      {
        float num = 0.0f;
        if (!flag1)
          num = this.carController.currentThrottle;
        else if (!Application.isPlaying)
          num = 1f;
        GUI.DrawTexture(new Rect((float) ((double) this.pedalsMonitorPosition.x + (double) Screen.width - 10.0), (float) Screen.height - this.pedalsMonitorPosition.y, 10f, (float) -this.throttleMonitor.height * num), (Texture) this.throttleMonitor);
      }
      if ((bool) (Object) this.brakeMonitor)
      {
        float num = 0.0f;
        if (!flag1)
          num = this.carController.currentBrake;
        else if (!Application.isPlaying)
          num = 1f;
        GUI.DrawTexture(new Rect((float) ((double) this.pedalsMonitorPosition.x - 12.0 + (double) Screen.width - 10.0), (float) Screen.height - this.pedalsMonitorPosition.y, 10f, (float) -this.brakeMonitor.height * num), (Texture) this.brakeMonitor);
      }
      if ((bool) (Object) this.clutchMonitor)
      {
        float num = 0.0f;
        if ((Object) this.drivetrain != (Object) null && this.drivetrain.clutch != null)
          num = this.drivetrain.clutch.GetClutchPosition();
        else if (!Application.isPlaying)
          num = 0.0f;
        GUI.DrawTexture(new Rect((float) ((double) this.pedalsMonitorPosition.x - 24.0 + (double) Screen.width - 10.0), (float) Screen.height - this.pedalsMonitorPosition.y, 10f, (float) -this.clutchMonitor.height * (1f - num)), (Texture) this.clutchMonitor);
      }
      if ((bool) (Object) this.drivetrain)
      {
        this.sign = 1;
        this.shift = 0;
        if (this.gearMonitorDocking == DashBoard.Docking.Right)
        {
          this.sign = -1;
          this.shift = Screen.width - 25;
        }
        this.gearRect = new Rect((float) this.sign * this.gearMonitorPosition.x + (float) this.shift, (float) (-(double) this.gearMonitorPosition.y + (double) Screen.height - 50.0), 50f, 50f);
        if (this.drivetrain.gear < this.drivetrain.neutral)
          GUI.Label(this.gearRect, "R", this.gearMonitorStyle);
        else if (this.drivetrain.gear == this.drivetrain.neutral)
          GUI.Label(this.gearRect, "H", this.gearMonitorStyle);
        else
          GUI.Label(this.gearRect, string.Empty + (object) (this.drivetrain.gear - this.drivetrain.neutral), this.gearMonitorStyle);
        this.sign = 1;
        this.shift = 0;
        if (this.digitalSpeedoDocking == DashBoard.Docking.Right)
        {
          this.sign = -1;
          this.shift = Screen.width - 25;
        }
        this.speedoRect = new Rect((float) this.sign * this.digitalSpeedoPosition.x + (float) this.shift, (float) (-(double) this.digitalSpeedoPosition.y + (double) Screen.height - 50.0), 50f, 50f);
        GUI.Label(this.speedoRect, string.Empty + (object) Mathf.Round(this.digitalSpeedoVelo), this.digitalSpeedoStyle);
      }
    }
    if ((Object) this.textMeshSpeed != (Object) null)
      this.textMeshSpeed.text = string.Empty + (object) Mathf.Round(this.digitalSpeedoVelo);
    if ((Object) this.tachoMeterNeedleOnBoard != (Object) null)
      this.tachoMeterNeedleOnBoard.transform.localRotation = Quaternion.Euler(this.tachoMeterNeedleOnBoard.transform.localEulerAngles.x, this.tachoMeterNeedleOnBoard.transform.localEulerAngles.y, this.actualtachoMeterNeedleAngle);
    if ((Object) this.speedoMeterNeedleOnBoard != (Object) null)
      this.speedoMeterNeedleOnBoard.transform.localRotation = Quaternion.Euler(this.speedoMeterNeedleOnBoard.transform.localEulerAngles.x, this.speedoMeterNeedleOnBoard.transform.localEulerAngles.y, this.actualspeedoMeterNeedleAngle);
    if (!((Object) this.textMeshGear != (Object) null))
      return;
    if (this.drivetrain.gear < this.drivetrain.neutral)
      this.textMeshGear.text = "R";
    else if (this.drivetrain.gear == this.drivetrain.neutral)
      this.textMeshGear.text = "H";
    else
      this.textMeshGear.text = string.Empty + (object) (this.drivetrain.gear - this.drivetrain.neutral);
  }

  public enum Docking
  {
    Left,
    Right,
  }

  public enum Unit
  {
    Kmh,
    Mph,
  }

  public enum SpeedoMeterType
  {
    RigidBody,
    Wheel,
  }
}
