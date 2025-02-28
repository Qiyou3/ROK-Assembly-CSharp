// Decompiled with JetBrains decompiler
// Type: AstarDebugger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using Pathfinding;
using Pathfinding.Util;
using System;
using System.Text;
using UnityEngine;

#nullable disable
[AddComponentMenu("Pathfinding/Debugger")]
[ExecuteInEditMode]
public class AstarDebugger : MonoBehaviour
{
  public int yOffset = 5;
  public bool show = true;
  public bool showInEditor;
  public bool showFPS;
  public bool showPathProfile;
  public bool showMemProfile;
  public Font font;
  public int fontSize = 12;
  private StringBuilder text = new StringBuilder();
  private string cachedText;
  private float lastUpdate = -999f;
  private float delayedDeltaTime = 1f;
  private float lastCollect;
  private float lastCollectNum;
  private float delta;
  private float lastDeltaTime;
  private int allocRate;
  private int lastAllocMemory;
  private float lastAllocSet = -9999f;
  private int allocMem;
  private int collectAlloc;
  private int peakAlloc;
  private int lastFrameCount = -1;
  private int fpsDropCounterSize = 200;
  private float[] fpsDrops;
  private Rect boxRect;
  private GUIStyle style;
  private int maxVecPool;
  private int maxNodePool;
  private AstarDebugger.PathTypeDebug[] debugTypes = new AstarDebugger.PathTypeDebug[1]
  {
    new AstarDebugger.PathTypeDebug("ABPath", new Func<int>(PathPool<ABPath>.GetSize), new Func<int>(PathPool<ABPath>.GetTotalCreated))
  };

  public void Start()
  {
    this.useGUILayout = false;
    this.fpsDrops = new float[this.fpsDropCounterSize];
    for (int index = 0; index < this.fpsDrops.Length; ++index)
      this.fpsDrops[index] = 1f / Time.deltaTime;
  }

  public void Update()
  {
    if (!this.show || !Application.isPlaying && !this.showInEditor)
      return;
    int num1 = GC.CollectionCount(0);
    if ((double) this.lastCollectNum != (double) num1)
    {
      this.lastCollectNum = (float) num1;
      this.delta = Time.realtimeSinceStartup - this.lastCollect;
      this.lastCollect = Time.realtimeSinceStartup;
      this.lastDeltaTime = Time.deltaTime;
      this.collectAlloc = this.allocMem;
    }
    this.allocMem = (int) GC.GetTotalMemory(false);
    this.peakAlloc = this.allocMem <= this.peakAlloc ? this.peakAlloc : this.allocMem;
    if ((double) Time.realtimeSinceStartup - (double) this.lastAllocSet > 0.30000001192092896 || !Application.isPlaying)
    {
      int num2 = this.allocMem - this.lastAllocMemory;
      this.lastAllocMemory = this.allocMem;
      this.lastAllocSet = Time.realtimeSinceStartup;
      this.delayedDeltaTime = Time.deltaTime;
      if (num2 >= 0)
        this.allocRate = num2;
    }
    if (this.lastFrameCount == Time.frameCount && Application.isPlaying)
      return;
    this.fpsDrops[Time.frameCount % this.fpsDrops.Length] = (double) Time.deltaTime == 0.0 ? float.PositiveInfinity : 1f / Time.deltaTime;
  }

  public void OnGUI()
  {
    if (!this.show || !Application.isPlaying && !this.showInEditor)
      return;
    if (this.style == null)
    {
      this.style = new GUIStyle();
      this.style.normal.textColor = Color.white;
      this.style.padding = new RectOffset(5, 5, 5, 5);
    }
    if ((double) Time.realtimeSinceStartup - (double) this.lastUpdate > 0.20000000298023224 || this.cachedText == null || !Application.isPlaying)
    {
      this.lastUpdate = Time.time;
      this.boxRect = new Rect(5f, (float) this.yOffset, 310f, 40f);
      this.text.Length = 0;
      this.text.AppendLine("A* Pathfinding Project Debugger");
      this.text.Append("A* Version: ").Append(AstarPath.Version.ToString());
      if (this.showMemProfile)
      {
        this.boxRect.height += 200f;
        this.text.AppendLine();
        this.text.AppendLine();
        this.text.Append("Currently allocated".PadRight(25));
        this.text.Append(((float) this.allocMem / 1000000f).ToString("0.0 MB"));
        this.text.AppendLine();
        this.text.Append("Peak allocated".PadRight(25));
        this.text.Append(((float) this.peakAlloc / 1000000f).ToString("0.0 MB")).AppendLine();
        this.text.Append("Last collect peak".PadRight(25));
        this.text.Append(((float) this.collectAlloc / 1000000f).ToString("0.0 MB")).AppendLine();
        this.text.Append("Allocation rate".PadRight(25));
        this.text.Append(((float) this.allocRate / 1000000f).ToString("0.0 MB")).AppendLine();
        this.text.Append("Collection frequency".PadRight(25));
        this.text.Append(this.delta.ToString("0.00"));
        this.text.Append("s\n");
        this.text.Append("Last collect fps".PadRight(25));
        this.text.Append((1f / this.lastDeltaTime).ToString("0.0 fps"));
        this.text.Append(" (");
        this.text.Append(this.lastDeltaTime.ToString("0.000 s"));
        this.text.Append(")");
      }
      if (this.showFPS)
      {
        this.text.AppendLine();
        this.text.AppendLine();
        this.text.Append("FPS".PadRight(25)).Append((1f / this.delayedDeltaTime).ToString("0.0 fps"));
        float num = float.PositiveInfinity;
        for (int index = 0; index < this.fpsDrops.Length; ++index)
        {
          if ((double) this.fpsDrops[index] < (double) num)
            num = this.fpsDrops[index];
        }
        this.text.AppendLine();
        this.text.Append(("Lowest fps (last " + (object) this.fpsDrops.Length + ")").PadRight(25)).Append(num.ToString("0.0"));
      }
      if (this.showPathProfile)
      {
        AstarPath active = AstarPath.active;
        this.text.AppendLine();
        if ((UnityEngine.Object) active == (UnityEngine.Object) null)
        {
          this.text.Append("\nNo AstarPath Object In The Scene");
        }
        else
        {
          if (ListPool<Vector3>.GetSize() > this.maxVecPool)
            this.maxVecPool = ListPool<Vector3>.GetSize();
          if (ListPool<Pathfinding.Node>.GetSize() > this.maxNodePool)
            this.maxNodePool = ListPool<Pathfinding.Node>.GetSize();
          this.text.Append("\nPool Sizes (size/total created)");
          for (int index = 0; index < this.debugTypes.Length; ++index)
            this.debugTypes[index].Print(this.text);
        }
      }
      this.cachedText = this.text.ToString();
    }
    if ((UnityEngine.Object) this.font != (UnityEngine.Object) null)
    {
      this.style.font = this.font;
      this.style.fontSize = this.fontSize;
    }
    this.boxRect.height = this.style.CalcHeight(new GUIContent(this.cachedText), this.boxRect.width);
    GUI.Box(this.boxRect, string.Empty);
    GUI.Label(this.boxRect, this.cachedText, this.style);
  }

  private struct PathTypeDebug(string name, Func<int> getSize, Func<int> getTotalCreated)
  {
    private string name = name;
    private Func<int> getSize = getSize;
    private Func<int> getTotalCreated = getTotalCreated;

    public void Print(StringBuilder text)
    {
      int num = this.getTotalCreated();
      if (num <= 0)
        return;
      text.Append("\n").Append(("  " + this.name).PadRight(25)).Append(this.getSize()).Append("/").Append(num);
    }
  }
}
