// Decompiled with JetBrains decompiler
// Type: DedicatedServerBypass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Build;
using CodeHatch.Engine.Core;
using CodeHatch.Engine.Core.Gaming;
using CodeHatch.Engine.Core.Utility.Static;
using CodeHatch.Engine.Networking;
using CodeHatch.Engine.Serialization;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Gaming;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

#nullable disable
public class DedicatedServerBypass : MonoBehaviour
{
  private ServerSettingsFile _serverSettingsFile;
  private static DedicatedServerBypass _instance;
  [Tooltip("If true the game will be forced into dedicated mode.")]
  public bool OverrideDedicated;
  [Tooltip("This scene will be loaded when dedicated server mode is detected.")]
  public string SceneToLoad = string.Empty;
  [SerializeField]
  [Tooltip("The default settings for the server.")]
  public ServerSettingsFile.ServerSettings DefaultSettings;
  [Tooltip("Gameobjects in this list will be enabled when this script is executed.")]
  public GameObject[] EnableList;
  [Tooltip("Gameobjects in this list will be disabled when this script is executed.")]
  public GameObject[] DisableList;

  public static DedicatedServerBypass Instance
  {
    get
    {
      return DedicatedServerBypass._instance ?? (DedicatedServerBypass._instance = Object.FindObjectOfType<DedicatedServerBypass>());
    }
  }

  public static ServerSettingsFile SettingsFile
  {
    get
    {
      ServerSettingsFile settingsFile = DedicatedServerBypass.Instance._serverSettingsFile;
      if (settingsFile == null)
      {
        settingsFile = new ServerSettingsFile(GameInfo.ServerConfig);
        settingsFile.Settings = DedicatedServerBypass.Instance.DefaultSettings;
        if (DedicatedServerBypass.Enabled && settingsFile.Properties == null && !settingsFile.Load())
        {
          Logger.Error("Could not load server settings.");
          settingsFile = (ServerSettingsFile) null;
        }
      }
      DedicatedServerBypass.Instance._serverSettingsFile = settingsFile;
      return settingsFile;
    }
  }

  public static ServerSettingsFile.ServerSettings Settings
  {
    get => DedicatedServerBypass.SettingsFile.Settings;
  }

  public static bool Enabled
  {
    get
    {
      if (!object.Equals((object) DedicatedServerBypass._instance, (object) null) && !((Object) DedicatedServerBypass._instance == (Object) null))
        return DedicatedServerBypass._instance.enabled;
      Dictionary<string, string> commandLineProps = SystemUtil.CommandLineProps;
      return commandLineProps.ContainsKey("batchmode") && !commandLineProps.ContainsKey("ip");
    }
  }

  [ContextMenu("Open Config File")]
  private void OpenConfigFile() => Process.Start(new FileInfo(GameInfo.ServerConfig).FullName);

  [ContextMenu("Generate Config File")]
  private void GenerateConfigFile()
  {
    if (DedicatedServerBypass.SettingsFile != null && DedicatedServerBypass.SettingsFile.Load())
      return;
    Logger.Error("Could not load server settings.");
  }

  [ContextMenu("Clear Config File")]
  private void ClearConfigFile()
  {
    FileInfo fileInfo = new FileInfo(GameInfo.ServerConfig);
    if (!fileInfo.Exists)
      return;
    fileInfo.Delete();
  }

  public void Awake()
  {
    DedicatedServerBypass._instance = this;
    if (!this.enabled)
      return;
    this.enabled = SystemUtil.CommandLineProps.ContainsKey("batchmode") || this.OverrideDedicated;
    if (!this.enabled)
      return;
    this.LogInfo<DedicatedServerBypass>("{0} is now in dedicated mode.", (object) GameInfo.ProductName);
    EventManager.Subscribe<GameStartEvent>(new EventSubscriber<GameStartEvent>(this.OnGameStart));
    EventManager.Subscribe<GameEndEvent>(new EventSubscriber<GameEndEvent>(this.OnGameEnd));
  }

  public void Start()
  {
    for (int index = 0; index < this.EnableList.Length; ++index)
    {
      GameObject enable = this.EnableList[index];
      if ((Object) enable != (Object) null)
        enable.SetActive(true);
    }
    for (int index = 0; index < this.DisableList.Length; ++index)
    {
      GameObject disable = this.DisableList[index];
      if ((Object) disable != (Object) null)
        disable.SetActive(false);
    }
    if (DedicatedServerBypass.SettingsFile.Load())
      this.StartServer();
    else
      Program.Exit();
  }

  public void OnDestroy()
  {
    EventManager.Unsubscribe<GameStartEvent>(new EventSubscriber<GameStartEvent>(this.OnGameStart));
    EventManager.Unsubscribe<GameEndEvent>(new EventSubscriber<GameEndEvent>(this.OnGameEnd));
    if (!DedicatedServerBypass.Enabled)
      return;
    CodeHatch.Engine.Core.Debugging.Profiler.Recording = false;
    CodeHatch.Engine.Core.Debugging.Profiler.StopStreaming();
    CodeHatch.Engine.Core.Debugging.Profiler.Clear();
  }

  public void StartServer()
  {
    ServerDataModule data = new ServerDataModule();
    data.Name = DedicatedServerBypass.Settings.ServerName;
    data.Game = Game.GameName;
    data.Version = GameInfo.VersionName;
    data.Port = DedicatedServerBypass.Settings.PortNumber;
    data.Password = DedicatedServerBypass.Settings.Password;
    data.Type = DedicatedServerBypass.Settings.LevelName;
    data.PingLimit = DedicatedServerBypass.Settings.PingLimit;
    data.PlayerLimit = (ushort) DedicatedServerBypass.Settings.MaxPlayers;
    data.IsPingLimited = DedicatedServerBypass.Settings.EnablePingLimit;
    data.IsDedicated = true;
    data.IsPrivate = DedicatedServerBypass.Settings.IsPrivate;
    data.Greeting = DedicatedServerBypass.Settings.Greeting;
    data.IPBind = DedicatedServerBypass.Settings.BindIP;
    SerializationSettings.Instance.ServerSavePath = DedicatedServerBypass.Settings.SaveLocation;
    if (DedicatedServerBypass.Settings.WorldSlot >= 0 && Game.SlotManager.SlotIsLocked(DedicatedServerBypass.Settings.WorldSlot))
    {
      string str = string.Format("Could not load world {0}. Loading new world instead.", (object) DedicatedServerBypass.Settings.WorldSlot);
      Console.AddWarning(str);
      this.LogWarning<DedicatedServerBypass>(str);
      DedicatedServerBypass.SettingsFile.SetConfigValue("worldSlot", "-1");
    }
    Game.AutoStartGame = false;
    Game.LoadOtherScene = true;
    Game.SceneToLoad = this.SceneToLoad;
    Game.SceneThatStartsGame = this.SceneToLoad;
    if (DedicatedServerBypass.Settings.WorldSlot < 0 || !Game.SlotManager.SlotExists(DedicatedServerBypass.Settings.WorldSlot))
      Game.New(true, DedicatedServerBypass.Settings.ServerName, DedicatedServerBypass.Settings.GameMode, DedicatedServerBypass.Settings.LevelName, data, DedicatedServerBypass.Settings.WorldSlot);
    else
      Game.Load(true, DedicatedServerBypass.Settings.WorldSlot, data);
    DedicatedServerBypass.SettingsFile.Save();
  }

  private void OnGameStart(GameStartEvent e)
  {
    if (!Player.IsLocalDedi)
      return;
    DedicatedServerBypass.SettingsFile.SetConfigValue("worldSlot", Game.CurrentSlot.SlotNumber.ToString());
    DedicatedServerBypass.SettingsFile.Save();
  }

  private void OnGameEnd(GameEndEvent e)
  {
    if (Game.HasLoaded || CodeHatch.Engine.Networking.Server.IsRunning)
      return;
    Program.Exit();
  }
}
