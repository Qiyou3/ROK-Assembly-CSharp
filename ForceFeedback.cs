// Decompiled with JetBrains decompiler
// Type: ForceFeedback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Runtime.InteropServices;
using UnityEngine;

#nullable disable
public class ForceFeedback : MonoBehaviour
{
  [HideInInspector]
  public int force;
  private float forceFeedback;
  private bool forceFeedbackEnabled;
  private CarDynamics cardynamics;
  public int factor = 1000;
  public float multiplier = 0.5f;
  public float smoothingFactor = 0.5f;
  public int clampValue = 20;
  public bool invertForceFeedback;
  private int sign = 1;
  private float m_force;

  [DllImport("user32")]
  private static extern int GetForegroundWindow();

  [DllImport("UnityForceFeedback")]
  private static extern int InitDirectInput(int HWND);

  [DllImport("UnityForceFeedback")]
  private static extern void Aquire();

  [DllImport("UnityForceFeedback")]
  private static extern int SetDeviceForcesXY(int x, int y);

  [DllImport("UnityForceFeedback")]
  private static extern bool StartEffect();

  [DllImport("UnityForceFeedback")]
  private static extern bool StopEffect();

  [DllImport("UnityForceFeedback")]
  private static extern bool SetAutoCenter(bool autoCentre);

  [DllImport("UnityForceFeedback")]
  private static extern void FreeDirectInput();

  public void Start()
  {
    this.cardynamics = this.GetComponent<CarDynamics>();
    this.InitialiseForceFeedback();
    ForceFeedback.SetAutoCenter(false);
  }

  public void Update()
  {
    this.sign = 1;
    if (this.invertForceFeedback)
      this.sign = -1;
    this.forceFeedback = this.cardynamics.forceFeedback;
    if ((double) Mathf.Abs(this.forceFeedback) > (double) this.clampValue)
      this.forceFeedback = (float) this.clampValue * Mathf.Sign(this.forceFeedback);
    this.force = (int) ((double) this.forceFeedback * (double) this.multiplier) * this.factor * this.sign;
    ForceFeedback.SetDeviceForcesXY(this.force, 0);
  }

  public void OnApplicationQuit() => this.ShutDownForceFeedback();

  private void InitialiseForceFeedback()
  {
    if (this.forceFeedbackEnabled)
    {
      this.LogWarning<ForceFeedback>("UnityCar: Force feedback attempted to initialise but was aleady running!");
    }
    else
    {
      ForceFeedback.InitDirectInput(ForceFeedback.GetForegroundWindow());
      ForceFeedback.Aquire();
      ForceFeedback.StartEffect();
      this.forceFeedbackEnabled = true;
    }
  }

  private void ShutDownForceFeedback()
  {
    ForceFeedback.StopEffect();
    if (this.forceFeedbackEnabled)
      ForceFeedback.FreeDirectInput();
    else
      this.LogWarning<ForceFeedback>("UnityCar:  Force feedback attempted to shutdown but wasn't running!");
  }
}
