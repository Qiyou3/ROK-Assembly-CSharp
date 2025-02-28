// Decompiled with JetBrains decompiler
// Type: AstarPath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Debugging;
using Pathfinding;
using Pathfinding.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

#nullable disable
[AddComponentMenu("Pathfinding/Pathfinder")]
public class AstarPath : MonoBehaviour
{
  public const int InitialBinaryHeapSize = 512;
  public static readonly AstarPath.AstarDistribution Distribution = AstarPath.AstarDistribution.WebsiteDownload;
  public static readonly bool HasPro = false;
  public AstarData astarData;
  public static AstarPath active;
  public bool showNavGraphs = true;
  public bool showUnwalkableNodes = true;
  public GraphDebugMode debugMode;
  public float debugFloor;
  public float debugRoof = 20000f;
  public bool showSearchTree;
  public float unwalkableNodeDebugSize = 0.3f;
  public bool stepByStep = true;
  public PathLog logPathResults = PathLog.Normal;
  public float maxNearestNodeDistance = 100f;
  public bool scanOnStartup = true;
  public bool fullGetNearestSearch;
  public bool prioritizeGraphs;
  public float prioritizeGraphsLimit = 1f;
  public AstarColor colorSettings;
  [SerializeField]
  protected string[] tagNames;
  public Heuristic heuristic = Heuristic.Euclidean;
  public float heuristicScale = 1f;
  public ThreadCount threadCount;
  public float maxFrameTime = 1f;
  public bool recyclePaths;
  public int minAreaSize = 10;
  public bool limitGraphUpdates = true;
  public float maxGraphUpdateFreq = 0.2f;
  public static int PathsCompleted = 0;
  public static long TotalSearchedNodes = 0;
  public static long TotalSearchTime = 0;
  public float lastScanTime;
  public Path debugPath;
  public string inGameDebugPath;
  public bool isScanning;
  private bool acceptNewPaths = true;
  private static int numActiveThreads = 0;
  private bool graphUpdateRoutineRunning;
  private bool isUpdatingGraphs;
  private bool isRegisteredForUpdate;
  public static OnVoidDelegate OnAwakeSettings;
  public static OnGraphDelegate OnGraphPreScan;
  public static OnGraphDelegate OnGraphPostScan;
  public static OnPathDelegate OnPathPreSearch;
  public static OnPathDelegate OnPathPostSearch;
  public static OnScanDelegate OnPreScan;
  public static OnScanDelegate OnPostScan;
  public static OnScanDelegate OnLatePostScan;
  public static OnScanDelegate OnGraphsUpdated;
  public static OnVoidDelegate On65KOverflow;
  private static OnVoidDelegate OnSafeCallback;
  private static OnVoidDelegate OnThreadSafeCallback;
  public OnVoidDelegate OnDrawGizmosCallback;
  public OnVoidDelegate OnGraphsWillBeUpdated;
  public OnVoidDelegate OnGraphsWillBeUpdated2;
  [NonSerialized]
  public Queue<GraphUpdateObject> graphUpdateQueue;
  [NonSerialized]
  public Stack<Pathfinding.Node> floodStack;
  public static Queue<Path> pathQueue = new Queue<Path>();
  public static Thread[] threads;
  public static PathThreadInfo[] threadInfos;
  public static IEnumerator threadEnumerator;
  public static LockFreeStack pathReturnStack = new LockFreeStack();
  public static Stack<Path> PathPool;
  public bool showGraphs;
  public int lastUniqueAreaIndex;
  private static readonly ManualResetEvent pathQueueFlag = new ManualResetEvent(false);
  private static readonly ManualResetEvent threadSafeUpdateFlag = new ManualResetEvent(false);
  private static readonly ManualResetEvent safeUpdateFlag = new ManualResetEvent(false);
  private static bool threadSafeUpdateState = false;
  private static readonly object safeUpdateLock = new object();
  private static bool doSetQueueState = true;
  private float lastGraphUpdate = -9999f;
  private ushort nextFreePathID = 1;
  private static int waitForPathDepth = 0;
  private Path pathReturnPop;

  public static Version Version => new Version(3, 2, 5, 1);

  public System.Type[] graphTypes => this.astarData.graphTypes;

  public NavGraph[] graphs
  {
    get
    {
      if (this.astarData == null)
        this.astarData = new AstarData();
      return this.astarData.graphs;
    }
    set
    {
      if (this.astarData == null)
        this.astarData = new AstarData();
      this.astarData.graphs = value;
    }
  }

  public float maxNearestNodeDistanceSqr
  {
    get => this.maxNearestNodeDistance * this.maxNearestNodeDistance;
  }

  public NodeRunData debugPathData
  {
    get => this.debugPath == null ? (NodeRunData) null : this.debugPath.runData;
  }

  public static int ActiveThreadsCount => AstarPath.numActiveThreads;

  public static int NumParallelThreads
  {
    get => AstarPath.threadInfos != null ? AstarPath.threadInfos.Length : 0;
  }

  public static bool IsUsingMultithreading
  {
    get
    {
      if (AstarPath.threads != null && AstarPath.threads.Length > 0)
        return true;
      if (AstarPath.threads != null && AstarPath.threads.Length == 0 && AstarPath.threadEnumerator != null)
        return false;
      throw new Exception("Not 'using threading' and not 'not using threading'... Are you sure pathfinding is set up correctly?\nIf scripts are reloaded in unity editor during play this could happen.");
    }
  }

  public bool IsAnyGraphUpdatesQueued
  {
    get => this.graphUpdateQueue != null && this.graphUpdateQueue.Count > 0;
  }

  private static void ResetQueueStates()
  {
    AstarPath.pathQueueFlag.Reset();
    AstarPath.threadSafeUpdateFlag.Reset();
    AstarPath.safeUpdateFlag.Reset();
    AstarPath.threadSafeUpdateState = false;
    AstarPath.doSetQueueState = true;
  }

  private static void TrickAbortThreads()
  {
    AstarPath.active.acceptNewPaths = false;
    AstarPath.pathQueueFlag.Set();
  }

  public string[] GetTagNames()
  {
    if (this.tagNames == null || this.tagNames.Length != 32)
    {
      this.tagNames = new string[32];
      for (int index = 0; index < this.tagNames.Length; ++index)
        this.tagNames[index] = string.Empty + (object) index;
      this.tagNames[0] = "Basic Ground";
    }
    return this.tagNames;
  }

  public static string[] FindTagNames()
  {
    if ((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) null)
      return AstarPath.active.GetTagNames();
    AstarPath objectOfType = UnityEngine.Object.FindObjectOfType(typeof (AstarPath)) as AstarPath;
    if ((UnityEngine.Object) objectOfType != (UnityEngine.Object) null)
    {
      AstarPath.active = objectOfType;
      return objectOfType.GetTagNames();
    }
    return new string[1]
    {
      "There is no AstarPath component in the scene"
    };
  }

  public ushort GetNextPathID()
  {
    if (this.nextFreePathID == (ushort) 0)
    {
      ++this.nextFreePathID;
      this.LogInfo<AstarPath>("65K cleanup");
      if (AstarPath.On65KOverflow != null)
      {
        OnVoidDelegate on65Koverflow = AstarPath.On65KOverflow;
        AstarPath.On65KOverflow = (OnVoidDelegate) null;
        on65Koverflow();
      }
    }
    return this.nextFreePathID++;
  }

  public void OnDrawGizmos()
  {
    if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
      AstarPath.active = this;
    else if ((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) this)
      return;
    if (this.graphs == null)
      return;
    for (int index = 0; index < this.graphs.Length; ++index)
    {
      if (this.graphs[index] != null && this.graphs[index].drawGizmos)
        this.graphs[index].OnDrawGizmos(this.showNavGraphs);
    }
    if (this.showUnwalkableNodes && this.showNavGraphs)
    {
      Gizmos.color = AstarColor.UnwalkableNode;
      for (int index1 = 0; index1 < this.graphs.Length; ++index1)
      {
        if (this.graphs[index1] != null && this.graphs[index1].nodes != null)
        {
          Pathfinding.Node[] nodes = this.graphs[index1].nodes;
          for (int index2 = 0; index2 < nodes.Length; ++index2)
          {
            if (nodes[index2] != null && !nodes[index2].walkable)
              Gizmos.DrawCube((Vector3) nodes[index2].position, Vector3.one * this.unwalkableNodeDebugSize);
          }
        }
      }
    }
    if (this.OnDrawGizmosCallback == null)
      return;
    this.OnDrawGizmosCallback();
  }

  public void OnGUI()
  {
    if (Input.GetKey("l"))
      GUI.Label(new Rect((float) (Screen.width - 100), 5f, 100f, 25f), (1f / Time.smoothDeltaTime).ToString("0") + " fps");
    if (this.logPathResults != PathLog.InGame || !(this.inGameDebugPath != string.Empty))
      return;
    GUI.Label(new Rect(5f, 5f, 400f, 600f), this.inGameDebugPath);
  }

  private static void AstarLog(string s)
  {
    if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
    {
      Logger.InfoFormat("No AstarPath object was found : {0}", (object) s);
    }
    else
    {
      if (AstarPath.active.logPathResults == PathLog.None || AstarPath.active.logPathResults == PathLog.OnlyErrors)
        return;
      Logger.Info(s);
    }
  }

  private static void AstarLogError(string s)
  {
    if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
    {
      Logger.InfoFormat("No AstarPath object was found : {0}", (object) s);
    }
    else
    {
      if (AstarPath.active.logPathResults == PathLog.None)
        return;
      Logger.Error(s);
    }
  }

  private void LogPathResults(Path p)
  {
    if (this.logPathResults == PathLog.None || this.logPathResults == PathLog.OnlyErrors && !p.error)
      return;
    string message = p.DebugString(this.logPathResults);
    if (this.logPathResults == PathLog.InGame)
      this.inGameDebugPath = message;
    else
      this.LogInfo<AstarPath>(message);
  }

  public void Update() => AstarPath.TryCallThreadSafeCallbacks();

  private static void TryCallThreadSafeCallbacks()
  {
    if (!AstarPath.threadSafeUpdateState)
      return;
    if (AstarPath.OnThreadSafeCallback != null)
    {
      OnVoidDelegate threadSafeCallback = AstarPath.OnThreadSafeCallback;
      AstarPath.OnThreadSafeCallback = (OnVoidDelegate) null;
      threadSafeCallback();
    }
    AstarPath.threadSafeUpdateFlag.Set();
    AstarPath.threadSafeUpdateState = false;
  }

  public static void ForceCallThreadSafeCallbacks()
  {
    if (!AstarPath.threadSafeUpdateState)
      throw new InvalidOperationException("You should only call this function from a thread safe callback. That does not seem to be the case for this call.");
    if (AstarPath.OnThreadSafeCallback == null)
      return;
    OnVoidDelegate threadSafeCallback = AstarPath.OnThreadSafeCallback;
    AstarPath.OnThreadSafeCallback = (OnVoidDelegate) null;
    threadSafeCallback();
  }

  public void QueueGraphUpdates()
  {
    if (this.isRegisteredForUpdate)
      return;
    this.isRegisteredForUpdate = true;
    AstarPath.RegisterSafeUpdate(new OnVoidDelegate(this.DoUpdateGraphs), true);
  }

  [DebuggerHidden]
  private IEnumerator DelayedGraphUpdate()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AstarPath.\u003CDelayedGraphUpdate\u003Ec__Iterator112()
    {
      \u003C\u003Ef__this = this
    };
  }

  [Obsolete("Use GraphUpdateUtilities.UpdateGraphsNoBlock instead")]
  public bool WillBlockPath(GraphUpdateObject ob, Pathfinding.Node n1, Pathfinding.Node n2)
  {
    return GraphUpdateUtilities.UpdateGraphsNoBlock(ob, n1, n2);
  }

  [Obsolete("Use GraphUpdateUtilities.IsPathPossible instead")]
  public static bool IsPathPossible(Pathfinding.Node n1, Pathfinding.Node n2) => n1.area == n2.area;

  public void UpdateGraphs(Bounds bounds, float t)
  {
    this.UpdateGraphs(new GraphUpdateObject(bounds), t);
  }

  public void UpdateGraphs(GraphUpdateObject ob, float t)
  {
    this.StartCoroutine(this.UpdateGraphsInteral(ob, t));
  }

  [DebuggerHidden]
  private IEnumerator UpdateGraphsInteral(GraphUpdateObject ob, float t)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AstarPath.\u003CUpdateGraphsInteral\u003Ec__Iterator113()
    {
      t = t,
      ob = ob,
      \u003C\u0024\u003Et = t,
      \u003C\u0024\u003Eob = ob,
      \u003C\u003Ef__this = this
    };
  }

  public void UpdateGraphs(Bounds bounds) => this.UpdateGraphs(new GraphUpdateObject(bounds));

  public void UpdateGraphs(GraphUpdateObject ob)
  {
    if (this.graphUpdateQueue == null)
      this.graphUpdateQueue = new Queue<GraphUpdateObject>();
    this.graphUpdateQueue.Enqueue(ob);
    if (this.isUpdatingGraphs)
      return;
    if (this.isScanning)
      this.DoUpdateGraphs();
    else if (this.limitGraphUpdates && (double) Time.time - (double) this.lastGraphUpdate < (double) this.maxGraphUpdateFreq)
    {
      if (this.graphUpdateRoutineRunning)
        return;
      this.StartCoroutine(this.DelayedGraphUpdate());
    }
    else
      this.QueueGraphUpdates();
  }

  public void FlushGraphUpdates()
  {
    if (!this.IsAnyGraphUpdatesQueued)
      return;
    this.QueueGraphUpdates();
    this.FlushThreadSafeCallbacks();
  }

  private void DoUpdateGraphs()
  {
    this.isRegisteredForUpdate = false;
    this.isUpdatingGraphs = true;
    this.lastGraphUpdate = Time.time;
    if (this.OnGraphsWillBeUpdated2 != null)
    {
      OnVoidDelegate graphsWillBeUpdated2 = this.OnGraphsWillBeUpdated2;
      this.OnGraphsWillBeUpdated2 = (OnVoidDelegate) null;
      graphsWillBeUpdated2();
    }
    if (this.OnGraphsWillBeUpdated != null)
    {
      OnVoidDelegate graphsWillBeUpdated = this.OnGraphsWillBeUpdated;
      this.OnGraphsWillBeUpdated = (OnVoidDelegate) null;
      graphsWillBeUpdated();
    }
    GraphModifier.TriggerEvent(GraphModifier.EventType.PreUpdate);
    bool flag = false;
    if (this.graphUpdateQueue != null)
    {
      while (this.graphUpdateQueue.Count > 0)
      {
        GraphUpdateObject o = this.graphUpdateQueue.Dequeue();
        if (o.requiresFloodFill)
          flag = true;
        foreach (IUpdatableGraph updateableGraph in this.astarData.GetUpdateableGraphs())
        {
          NavGraph graph = updateableGraph as NavGraph;
          if (o.nnConstraint == null || o.nnConstraint.SuitableGraph(AstarPath.active.astarData.GetGraphIndex(graph), graph))
            updateableGraph.UpdateArea(o);
        }
      }
    }
    this.isUpdatingGraphs = false;
    if (flag && !this.isScanning)
      this.FloodFill();
    if (AstarPath.OnGraphsUpdated != null && !this.isScanning)
      AstarPath.OnGraphsUpdated(this);
    GraphModifier.TriggerEvent(GraphModifier.EventType.PostUpdate);
  }

  public void FlushThreadSafeCallbacks()
  {
    if (AstarPath.OnThreadSafeCallback == null)
      return;
    bool flag1 = true;
    if (AstarPath.IsUsingMultithreading)
    {
      for (int index = 0; index < AstarPath.threadInfos.Length; ++index)
      {
        bool flag2 = false;
        while (!flag2 && !AstarPath.threadSafeUpdateState)
          flag2 = Monitor.TryEnter(AstarPath.threadInfos[index].Lock, 10);
        if (AstarPath.threadSafeUpdateState)
        {
          if (index != 0 || flag2)
            throw new Exception("Wait wut! This should not happen! " + (object) index + " " + (object) flag2);
          flag1 = false;
          break;
        }
      }
      AstarPath.threadSafeUpdateState = true;
    }
    else
    {
      while (!AstarPath.threadSafeUpdateState && AstarPath.threadEnumerator.MoveNext())
        ;
    }
    AstarPath.TryCallThreadSafeCallbacks();
    AstarPath.doSetQueueState = true;
    AstarPath.pathQueueFlag.Set();
    if (!AstarPath.IsUsingMultithreading || !flag1)
      return;
    for (int index = 0; index < AstarPath.threadInfos.Length; ++index)
      Monitor.Exit(AstarPath.threadInfos[index].Lock);
  }

  public void LogProfiler()
  {
  }

  public void ResetProfiler()
  {
  }

  public static int CalculateThreadCount(ThreadCount count)
  {
    if (count != ThreadCount.Automatic)
      return (int) count;
    int val1 = SystemInfo.processorCount;
    int systemMemorySize = SystemInfo.systemMemorySize;
    if (val1 <= 1 || systemMemorySize <= 512)
      return 0;
    if (systemMemorySize <= 1024)
      val1 = Math.Min(val1, 2);
    return Math.Min(val1, 6);
  }

  public void Awake()
  {
    AstarPath.active = this;
    if (UnityEngine.Object.FindObjectsOfType(typeof (AstarPath)).Length > 1)
      this.LogError<AstarPath>("You should NOT have more than one AstarPath component in the scene at any time.\nThis can cause serious errors since the AstarPath component builds around a singleton pattern.");
    this.useGUILayout = false;
    if (AstarPath.OnAwakeSettings != null)
      AstarPath.OnAwakeSettings();
    int threadCount = AstarPath.CalculateThreadCount(this.threadCount);
    AstarPath.threads = new Thread[threadCount];
    AstarPath.threadInfos = new PathThreadInfo[Math.Max(threadCount, 1)];
    for (int index = 0; index < AstarPath.threadInfos.Length; ++index)
      AstarPath.threadInfos[index] = new PathThreadInfo(index, this, new NodeRunData());
    for (int index = 0; index < AstarPath.threads.Length; ++index)
    {
      AstarPath.threads[index] = new Thread(new ParameterizedThreadStart(AstarPath.CalculatePathsThreaded));
      AstarPath.threads[index].IsBackground = true;
    }
    this.Initialize();
    this.StartCoroutine(this.ReturnsPathsHandler());
    if (this.scanOnStartup && (!this.astarData.cacheStartup || this.astarData.data_cachedStartup == null))
      this.Scan();
    this.UpdatePathThreadInfoNodes();
    if (AstarPath.threads.Length > 0)
      new Thread(new ParameterizedThreadStart(AstarPath.LockThread)).Start((object) this);
    for (int index = 0; index < AstarPath.threads.Length; ++index)
    {
      if (this.logPathResults == PathLog.Heavy)
        this.LogInfo<AstarPath>("Starting pathfinding thread {0}", (object) index);
      AstarPath.threads[index].Start((object) AstarPath.threadInfos[index]);
    }
    if (AstarPath.threads.Length != 0)
      return;
    this.StartCoroutine(AstarPath.CalculatePathsHandler((object) AstarPath.threadInfos[0]));
  }

  public void DataUpdate()
  {
    if ((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) this)
      throw new Exception("Singleton pattern broken. Make sure you only have one AstarPath object in the scene");
    if (this.astarData == null)
      throw new NullReferenceException("AstarData is null... Astar not set up correctly?");
    if (this.astarData.graphs == null)
      this.astarData.graphs = new NavGraph[0];
    this.astarData.AssignNodeIndices();
    if (!Application.isPlaying)
      return;
    this.astarData.CreateNodeRuns(AstarPath.threadInfos.Length);
  }

  public void UpdatePathThreadInfoNodes()
  {
    for (int index = 0; index < AstarPath.threadInfos.Length; ++index)
    {
      PathThreadInfo threadInfo = AstarPath.threadInfos[index];
      if (threadInfo.threadIndex != index)
        throw new Exception("threadInfos[" + (object) index + "] did not have a matching index member. Expected " + (object) index + " found " + (object) threadInfo.threadIndex);
      if (threadInfo.runData == null)
        throw new NullReferenceException("A thread info.node run data was null");
    }
  }

  public void SetUpReferences()
  {
    AstarPath.active = this;
    if (this.astarData == null)
      this.astarData = new AstarData();
    if (this.astarData.userConnections == null)
      this.astarData.userConnections = new UserConnection[0];
    if (this.colorSettings == null)
      this.colorSettings = new AstarColor();
    this.colorSettings.OnEnable();
  }

  private void Initialize()
  {
    this.SetUpReferences();
    this.astarData.FindGraphTypes();
    this.astarData.Awake();
    for (int index = 0; index < this.astarData.graphs.Length; ++index)
    {
      if (this.astarData.graphs[index] != null)
        this.astarData.graphs[index].Awake();
    }
  }

  public void OnDestroy()
  {
    if (this.logPathResults == PathLog.Heavy)
      this.LogInfo<AstarPath>("+++ AstarPath Component Destroyed - Cleaning Up Pathfinding Data +++");
    AstarPath.TrickAbortThreads();
    if (AstarPath.threads != null)
    {
      for (int index = 0; index < AstarPath.threads.Length; ++index)
      {
        if (!AstarPath.threads[index].Join(50))
        {
          this.LogError<AstarPath>("Could not terminate pathfinding thread[{0}] in 50ms, trying Thread.Abort", (object) index);
          AstarPath.threads[index].Abort();
        }
      }
    }
    if (this.logPathResults == PathLog.Heavy)
      this.LogInfo<AstarPath>("Destroying Graphs");
    if (this.astarData.graphs != null)
    {
      for (int index = 0; index < this.astarData.graphs.Length; ++index)
      {
        if (this.astarData.graphs[index] != null)
          this.astarData.graphs[index].OnDestroy();
      }
    }
    this.astarData.graphs = (NavGraph[]) null;
    if (this.logPathResults == PathLog.Heavy)
      this.LogInfo<AstarPath>("Returning Paths");
    this.ReturnPaths(false);
    AstarPath.pathReturnStack.PopAll();
    if (this.logPathResults == PathLog.Heavy)
      this.LogInfo<AstarPath>("Cleaning up variables");
    this.floodStack = (Stack<Pathfinding.Node>) null;
    this.graphUpdateQueue = (Queue<GraphUpdateObject>) null;
    this.OnDrawGizmosCallback = (OnVoidDelegate) null;
    AstarPath.OnAwakeSettings = (OnVoidDelegate) null;
    AstarPath.OnGraphPreScan = (OnGraphDelegate) null;
    AstarPath.OnGraphPostScan = (OnGraphDelegate) null;
    AstarPath.OnPathPreSearch = (OnPathDelegate) null;
    AstarPath.OnPathPostSearch = (OnPathDelegate) null;
    AstarPath.OnPreScan = (OnScanDelegate) null;
    AstarPath.OnPostScan = (OnScanDelegate) null;
    AstarPath.OnLatePostScan = (OnScanDelegate) null;
    AstarPath.On65KOverflow = (OnVoidDelegate) null;
    AstarPath.OnGraphsUpdated = (OnScanDelegate) null;
    AstarPath.OnSafeCallback = (OnVoidDelegate) null;
    AstarPath.OnThreadSafeCallback = (OnVoidDelegate) null;
    AstarPath.pathQueue.Clear();
    AstarPath.threads = (Thread[]) null;
    AstarPath.threadInfos = (PathThreadInfo[]) null;
    AstarPath.numActiveThreads = 0;
    AstarPath.ResetQueueStates();
    AstarPath.PathsCompleted = 0;
    AstarPath.active = (AstarPath) null;
  }

  public void FloodFill(Pathfinding.Node seed)
  {
    this.FloodFill(seed, this.lastUniqueAreaIndex + 1);
    ++this.lastUniqueAreaIndex;
  }

  public void FloodFill(Pathfinding.Node seed, int area)
  {
    if (area > (int) byte.MaxValue)
      this.LogError<AstarPath>("Too high area index - The maximum area index is 255");
    else if (area < 0)
    {
      this.LogError<AstarPath>("Too low area index - The minimum area index is 0");
    }
    else
    {
      if (this.floodStack == null)
        this.floodStack = new Stack<Pathfinding.Node>(1024);
      Stack<Pathfinding.Node> floodStack = this.floodStack;
      floodStack.Clear();
      floodStack.Push(seed);
      seed.area = area;
      while (floodStack.Count > 0)
        floodStack.Pop().FloodFill(floodStack, area);
    }
  }

  public void FloodFill()
  {
    if (this.astarData.graphs == null)
      return;
    int area = 0;
    this.lastUniqueAreaIndex = 0;
    if (this.floodStack == null)
      this.floodStack = new Stack<Pathfinding.Node>(1024);
    Stack<Pathfinding.Node> floodStack = this.floodStack;
    for (int index1 = 0; index1 < this.graphs.Length; ++index1)
    {
      NavGraph graph = this.graphs[index1];
      if (graph != null && graph.nodes != null)
      {
        for (int index2 = 0; index2 < graph.nodes.Length; ++index2)
        {
          if (graph.nodes[index2] != null)
            graph.nodes[index2].area = 0;
        }
      }
    }
    int num1 = 0;
    for (int index3 = 0; index3 < this.graphs.Length; ++index3)
    {
      NavGraph graph = this.graphs[index3];
      if (graph != null)
      {
        if (graph.nodes == null)
        {
          this.LogWarning<AstarPath>("Graph {0} has not defined any nodes", (object) index3);
        }
        else
        {
          for (int index4 = 0; index4 < graph.nodes.Length; ++index4)
          {
            if (graph.nodes[index4] != null && graph.nodes[index4].walkable && graph.nodes[index4].area == 0)
            {
              ++area;
              if (area > (int) byte.MaxValue)
              {
                this.LogError<AstarPath>("Too many areas - The maximum number of areas is 256");
                --area;
                break;
              }
              floodStack.Clear();
              floodStack.Push(graph.nodes[index4]);
              int num2 = 1;
              graph.nodes[index4].area = area;
              while (floodStack.Count > 0)
              {
                ++num2;
                floodStack.Pop().FloodFill(floodStack, area);
              }
              if (num2 < this.minAreaSize)
              {
                floodStack.Clear();
                floodStack.Push(graph.nodes[index4]);
                graph.nodes[index4].area = 254;
                while (floodStack.Count > 0)
                  floodStack.Pop().FloodFill(floodStack, 254);
                ++num1;
                --area;
              }
            }
          }
        }
      }
    }
    this.lastUniqueAreaIndex = area;
    if (num1 <= 0)
      return;
    AstarPath.AstarLog(num1.ToString() + " small areas were detected (fewer than " + (object) this.minAreaSize + " nodes),these might have the same IDs as other areas, but it shouldn't affect pathfinding in any significant way (you might get All Nodes Searched as a reason for path failure).\nWhich areas are defined as 'small' is controlled by the 'Min Area Size' variable, it can be changed in the A* inspector-->Settings-->Min Area Size\nThe small areas will use the area id 254");
  }

  public void Scan()
  {
    IEnumerator<Progress> enumerator = this.ScanLoop().GetEnumerator();
    do
      ;
    while (enumerator.MoveNext());
  }

  [DebuggerHidden]
  public IEnumerable<Progress> ScanLoop()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    AstarPath.\u003CScanLoop\u003Ec__Iterator114 loopCIterator114 = new AstarPath.\u003CScanLoop\u003Ec__Iterator114()
    {
      \u003C\u003Ef__this = this
    };
    // ISSUE: reference to a compiler-generated field
    loopCIterator114.\u0024PC = -2;
    return (IEnumerable<Progress>) loopCIterator114;
  }

  public void NodeCountChanged()
  {
    if (!Application.isPlaying)
      return;
    this.astarData.CreateNodeRuns(Math.Max(AstarPath.threads.Length, 1));
  }

  public void ApplyLinks()
  {
    for (int index = 0; index < this.astarData.userConnections.Length; ++index)
    {
      UserConnection userConnection = this.astarData.userConnections[index];
      if (userConnection.type == ConnectionType.Connection)
      {
        Pathfinding.Node node1 = this.GetNearest(userConnection.p1).node;
        Pathfinding.Node node2 = this.GetNearest(userConnection.p2).node;
        if (node1 != null && node2 != null)
        {
          int cost = !userConnection.doOverrideCost ? (node1.position - node2.position).costMagnitude : userConnection.overrideCost;
          if (userConnection.enable)
          {
            node1.AddConnection(node2, cost);
            if (!userConnection.oneWay)
              node2.AddConnection(node1, cost);
          }
          else
          {
            node1.RemoveConnection(node2);
            if (!userConnection.oneWay)
              node2.RemoveConnection(node1);
          }
        }
      }
      else
      {
        Pathfinding.Node node = this.GetNearest(userConnection.p1).node;
        if (node != null)
        {
          if (userConnection.doOverrideWalkability)
          {
            node.walkable = userConnection.enable;
            if (!node.walkable)
            {
              node.UpdateNeighbourConnections();
              node.UpdateConnections();
            }
          }
          if (userConnection.doOverridePenalty)
            node.penalty = userConnection.overridePenalty;
        }
      }
    }
    foreach (NodeLink nodeLink in UnityEngine.Object.FindObjectsOfType(typeof (NodeLink)) as NodeLink[])
      nodeLink.Apply();
  }

  public static void WaitForPath(Path p)
  {
    if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
      throw new Exception("Pathfinding is not correctly initialized in this scene (yet?). AstarPath.active is null.\nDo not call this function in Awake");
    if (p == null)
      throw new ArgumentNullException("Path must not be null");
    if (!AstarPath.active.acceptNewPaths)
      return;
    if (p.GetState() == PathState.Created)
      throw new Exception("The specified path has not been started yet.");
    ++AstarPath.waitForPathDepth;
    if (AstarPath.waitForPathDepth == 5)
      Logger.Error("You are calling the WaitForPath function recursively (maybe from a path callback). Please don't do this.");
    if (p.GetState() < PathState.ReturnQueue)
    {
      if (AstarPath.IsUsingMultithreading)
      {
        while (p.GetState() < PathState.ReturnQueue)
        {
          if (AstarPath.ActiveThreadsCount == 0)
          {
            --AstarPath.waitForPathDepth;
            throw new Exception("Pathfinding Threads seems to have crashed. No threads are running.");
          }
          Thread.Sleep(1);
          AstarPath.TryCallThreadSafeCallbacks();
        }
      }
      else
      {
        while (p.GetState() < PathState.ReturnQueue)
        {
          if (AstarPath.pathQueue.Count == 0 && p.GetState() != PathState.Processing)
          {
            --AstarPath.waitForPathDepth;
            throw new Exception("Critical error. Path Queue is empty but the path state is '" + (object) p.GetState() + "'");
          }
          AstarPath.threadEnumerator.MoveNext();
          AstarPath.TryCallThreadSafeCallbacks();
        }
      }
    }
    AstarPath.active.ReturnPaths(false);
    --AstarPath.waitForPathDepth;
  }

  public static void RegisterSafeUpdate(OnVoidDelegate callback, bool threadSafe)
  {
    if (callback == null || !Application.isPlaying)
      return;
    if (AstarPath.threadSafeUpdateState)
      callback();
    else if (AstarPath.IsUsingMultithreading)
    {
      int num = 0;
      for (int index = 0; index < AstarPath.threadInfos.Length && Monitor.TryEnter(AstarPath.threadInfos[index].Lock); ++index)
        num = index;
      if (num == AstarPath.threadInfos.Length - 1)
      {
        AstarPath.threadSafeUpdateState = true;
        callback();
        AstarPath.threadSafeUpdateState = false;
      }
      for (int index = 0; index <= num; ++index)
        Monitor.Exit(AstarPath.threadInfos[index].Lock);
      if (num == AstarPath.threadInfos.Length - 1)
        return;
      AstarPath.doSetQueueState = false;
      AstarPath.pathQueueFlag.Reset();
      lock (AstarPath.safeUpdateLock)
      {
        if (threadSafe)
          AstarPath.OnThreadSafeCallback += callback;
        else
          AstarPath.OnSafeCallback += callback;
        AstarPath.safeUpdateFlag.Set();
      }
    }
    else if (AstarPath.threadSafeUpdateState)
    {
      callback();
    }
    else
    {
      lock (AstarPath.safeUpdateLock)
      {
        if (threadSafe)
          AstarPath.OnThreadSafeCallback += callback;
        else
          AstarPath.OnSafeCallback += callback;
      }
    }
  }

  public static void StartPath(Path p)
  {
    if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
    {
      Logger.Error("There is no AstarPath object in the scene");
    }
    else
    {
      if (p.GetState() != PathState.Created)
        throw new Exception("The path has an invalid state. Expected " + (object) PathState.Created + " found " + (object) p.GetState() + "\nMake sure you are not requesting the same path twice");
      if (!AstarPath.active.acceptNewPaths)
      {
        p.Error();
        p.LogError("No new paths are accepted");
      }
      else if (AstarPath.active.graphs == null || AstarPath.active.graphs.Length == 0)
      {
        Logger.Error("There are no graphs in the scene");
        p.Error();
        p.LogError("There are no graphs in the scene");
        Logger.Error(p.errorLog);
      }
      else
      {
        p.Claim((object) AstarPath.active);
        lock (AstarPath.pathQueue)
        {
          p.AdvanceState(PathState.PathQueue);
          AstarPath.pathQueue.Enqueue(p);
          if (!AstarPath.doSetQueueState)
            return;
          AstarPath.pathQueueFlag.Set();
        }
      }
    }
  }

  public void OnApplicationQuit()
  {
    if (AstarPath.threads == null)
      return;
    for (int index = 0; index < AstarPath.threads.Length; ++index)
      AstarPath.threads[index].Abort();
  }

  [DebuggerHidden]
  public IEnumerator ReturnsPathsHandler()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AstarPath.\u003CReturnsPathsHandler\u003Ec__Iterator115()
    {
      \u003C\u003Ef__this = this
    };
  }

  public void ReturnPaths(bool timeSlice)
  {
    Path path1 = AstarPath.pathReturnStack.PopAll();
    if (this.pathReturnPop == null)
    {
      this.pathReturnPop = path1;
    }
    else
    {
      Path path2 = this.pathReturnPop;
      while (path2.next != null)
        path2 = path2.next;
      path2.next = path1;
    }
    long num1 = !timeSlice ? 0L : DateTime.UtcNow.Ticks + 5000L;
    int num2 = 0;
    while (this.pathReturnPop != null)
    {
      Path pathReturnPop = this.pathReturnPop;
      this.pathReturnPop = this.pathReturnPop.next;
      pathReturnPop.next = (Path) null;
      pathReturnPop.ReturnPath();
      pathReturnPop.AdvanceState(PathState.Returned);
      pathReturnPop.ReleaseSilent((object) this);
      ++num2;
      if (num2 > 5 && timeSlice)
      {
        num2 = 0;
        if (DateTime.UtcNow.Ticks >= num1)
          break;
      }
    }
  }

  private static void LockThread(object _astar)
  {
    AstarPath astarPath = (AstarPath) _astar;
    while (astarPath.acceptNewPaths)
    {
      AstarPath.safeUpdateFlag.WaitOne();
      PathThreadInfo[] threadInfos = AstarPath.threadInfos;
      if (threadInfos == null)
      {
        Logger.Error("Path Thread Infos are null");
        break;
      }
      for (int index = 0; index < threadInfos.Length; ++index)
        Monitor.Enter(threadInfos[index].Lock);
      lock (AstarPath.safeUpdateLock)
      {
        AstarPath.safeUpdateFlag.Reset();
        OnVoidDelegate onSafeCallback = AstarPath.OnSafeCallback;
        AstarPath.OnSafeCallback = (OnVoidDelegate) null;
        if (onSafeCallback != null)
          onSafeCallback();
        if (AstarPath.OnThreadSafeCallback != null)
          AstarPath.threadSafeUpdateFlag.Reset();
        else
          AstarPath.threadSafeUpdateFlag.Set();
      }
      AstarPath.threadSafeUpdateState = true;
      AstarPath.threadSafeUpdateFlag.WaitOne();
      AstarPath.doSetQueueState = true;
      AstarPath.pathQueueFlag.Set();
      for (int index = 0; index < threadInfos.Length; ++index)
        Monitor.Exit(threadInfos[index].Lock);
    }
  }

  private static void CalculatePathsThreaded(object _threadInfo)
  {
    Interlocked.Increment(ref AstarPath.numActiveThreads);
    PathThreadInfo pathThreadInfo;
    try
    {
      pathThreadInfo = (PathThreadInfo) _threadInfo;
    }
    catch (Exception ex)
    {
      Logger.ErrorFormat("Arguments to pathfinding threads must be of type ThreadStartInfo\n{0}", (object) ex);
      throw new ArgumentException("Argument must be of type ThreadStartInfo", ex);
    }
    AstarPath astar = pathThreadInfo.astar;
    try
    {
      NodeRunData runData = pathThreadInfo.runData;
      long targetTick = DateTime.UtcNow.Ticks + (long) ((double) astar.maxFrameTime * 10000.0);
      while (true)
      {
        long num1;
        do
        {
          ThreadProfiler.Sample("AstarPath.CalculatePathsThreaded.While");
          Path p = (Path) null;
          while (true)
          {
            ThreadProfiler.Sample("AstarPath.CalculatePathsThreaded.While.While1");
            if (astar.acceptNewPaths)
            {
              AstarPath.pathQueueFlag.WaitOne();
              if (astar.acceptNewPaths)
              {
                lock (AstarPath.pathQueue)
                {
                  if (AstarPath.pathQueue.Count > 0)
                  {
                    p = AstarPath.pathQueue.Dequeue();
                    goto label_15;
                  }
                  else
                    AstarPath.pathQueueFlag.Reset();
                }
              }
              else
                goto label_9;
            }
            else
              break;
          }
          Interlocked.Decrement(ref AstarPath.numActiveThreads);
          return;
label_9:
          Interlocked.Decrement(ref AstarPath.numActiveThreads);
          return;
label_15:
          Monitor.Enter(pathThreadInfo.Lock);
          num1 = (long) ((double) astar.maxFrameTime * 10000.0);
          p.PrepareBase(runData);
          p.AdvanceState(PathState.Processing);
          if (AstarPath.OnPathPreSearch != null)
            AstarPath.OnPathPreSearch(p);
          long ticks = DateTime.UtcNow.Ticks;
          long num2 = 0;
          p.Prepare();
          if (!p.IsDone())
          {
            astar.debugPath = p;
            p.Initialize();
            while (!p.IsDone())
            {
              ThreadProfiler.Sample("AstarPath.CalculatePathsThreaded.While.While2");
              p.CalculateStep(targetTick);
              ++p.searchIterations;
              if (!p.IsDone())
              {
                num2 += DateTime.UtcNow.Ticks - ticks;
                Thread.Sleep(0);
                ticks = DateTime.UtcNow.Ticks;
                targetTick = ticks + num1;
                if (!astar.acceptNewPaths)
                  p.Error();
              }
              else
                break;
            }
            long num3 = num2 + (DateTime.UtcNow.Ticks - ticks);
            p.duration = (float) num3 * 0.0001f;
          }
          astar.LogPathResults(p);
          if (AstarPath.OnPathPostSearch != null)
            AstarPath.OnPathPostSearch(p);
          AstarPath.pathReturnStack.Push(p);
          p.AdvanceState(PathState.ReturnQueue);
          Monitor.Exit(pathThreadInfo.Lock);
        }
        while (DateTime.UtcNow.Ticks <= targetTick);
        Thread.Sleep(1);
        targetTick = DateTime.UtcNow.Ticks + num1;
      }
    }
    catch (Exception ex)
    {
      if (ex is ThreadAbortException)
      {
        if (astar.logPathResults == PathLog.Heavy)
          Logger.WarningFormat("Shutting down pathfinding thread #{0} with Thread.Abort call", (object) pathThreadInfo.threadIndex);
        Interlocked.Decrement(ref AstarPath.numActiveThreads);
        return;
      }
      Logger.Exception(ex);
    }
    Logger.Error("Error : This part should never be reached");
    Interlocked.Decrement(ref AstarPath.numActiveThreads);
  }

  [DebuggerHidden]
  private static IEnumerator CalculatePathsHandler(object _threadData)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AstarPath.\u003CCalculatePathsHandler\u003Ec__Iterator116()
    {
      _threadData = _threadData,
      \u003C\u0024\u003E_threadData = _threadData
    };
  }

  [DebuggerHidden]
  private static IEnumerator CalculatePaths(object _threadInfo)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AstarPath.\u003CCalculatePaths\u003Ec__Iterator117()
    {
      _threadInfo = _threadInfo,
      \u003C\u0024\u003E_threadInfo = _threadInfo
    };
  }

  public NNInfo GetNearest(Vector3 position) => this.GetNearest(position, NNConstraint.None);

  public NNInfo GetNearest(Vector3 position, NNConstraint constraint)
  {
    return this.GetNearest(position, constraint, (Pathfinding.Node) null);
  }

  public NNInfo GetNearest(Vector3 position, NNConstraint constraint, Pathfinding.Node hint)
  {
    if (this.graphs == null)
      return new NNInfo();
    if (constraint == null)
      constraint = NNConstraint.None;
    float num = float.PositiveInfinity;
    NNInfo nearest = new NNInfo();
    int index = -1;
    for (int graphIndex = 0; graphIndex < this.graphs.Length; ++graphIndex)
    {
      NavGraph graph = this.graphs[graphIndex];
      if (graph != null && constraint.SuitableGraph(graphIndex, graph))
      {
        NNInfo nnInfo = !this.fullGetNearestSearch ? graph.GetNearest(position, constraint) : graph.GetNearestForce(position, constraint);
        if (nnInfo.node != null)
        {
          float magnitude = (nnInfo.clampedPosition - position).magnitude;
          if (this.prioritizeGraphs && (double) magnitude < (double) this.prioritizeGraphsLimit)
          {
            nearest = nnInfo;
            index = graphIndex;
            break;
          }
          if ((double) magnitude < (double) num)
          {
            num = magnitude;
            nearest = nnInfo;
            index = graphIndex;
          }
        }
      }
    }
    if (index == -1)
      return nearest;
    if (nearest.constrainedNode != null)
    {
      nearest.node = nearest.constrainedNode;
      nearest.clampedPosition = nearest.constClampedPosition;
    }
    if (!this.fullGetNearestSearch && nearest.node != null && !constraint.Suitable(nearest.node))
    {
      NNInfo nearestForce = this.graphs[index].GetNearestForce(position, constraint);
      if (nearestForce.node != null)
        nearest = nearestForce;
    }
    return !constraint.Suitable(nearest.node) || constraint.constrainDistance && (double) (nearest.clampedPosition - position).sqrMagnitude > (double) this.maxNearestNodeDistanceSqr ? new NNInfo() : nearest;
  }

  public Pathfinding.Node GetNearest(Ray ray)
  {
    if (this.graphs == null)
      return (Pathfinding.Node) null;
    float num1 = float.PositiveInfinity;
    Pathfinding.Node nearest = (Pathfinding.Node) null;
    Vector3 direction = ray.direction;
    Vector3 origin = ray.origin;
    for (int index1 = 0; index1 < this.graphs.Length; ++index1)
    {
      Pathfinding.Node[] nodes = this.graphs[index1].nodes;
      if (nodes != null)
      {
        for (int index2 = 0; index2 < nodes.Length; ++index2)
        {
          Pathfinding.Node node = nodes[index2];
          if (node != null)
          {
            Vector3 position = (Vector3) node.position;
            Vector3 vector3 = origin + Vector3.Dot(position - origin, direction) * direction;
            float num2 = Mathf.Abs(vector3.x - position.x);
            if ((double) (num2 * num2) <= (double) num1)
            {
              float num3 = Mathf.Abs(vector3.z - position.z);
              if ((double) (num3 * num3) <= (double) num1)
              {
                float sqrMagnitude = (vector3 - position).sqrMagnitude;
                if ((double) sqrMagnitude < (double) num1)
                {
                  num1 = sqrMagnitude;
                  nearest = node;
                }
              }
            }
          }
        }
      }
    }
    return nearest;
  }

  public enum AstarDistribution
  {
    WebsiteDownload,
    AssetStore,
  }
}
