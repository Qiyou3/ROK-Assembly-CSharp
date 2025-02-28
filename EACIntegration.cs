// Decompiled with JetBrains decompiler
// Type: EACIntegration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Core;
using CodeHatch.Engine.Core;
using CodeHatch.Engine.Core.Gaming;
using CodeHatch.Engine.Networking;
using CodeHatch.Engine.Networking.Events;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Gaming;
using CodeHatch.UserInterface.Dialogues;
using EasyAntiCheat;
using EasyAntiCheat.Client;
using EasyAntiCheat.Client.Hydra;
using EasyAntiCheat.Server;
using EasyAntiCheat.Server.Hydra;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

#nullable disable
public class EACIntegration : MonoBehaviour, IProgressLoader
{
  private static readonly int _networkID = nameof (EACIntegration).GetHashCode32();
  private static EasyAntiCheatServer<EasyAntiCheat.Server.Hydra.Client> _eacServer;
  private static readonly ReaderWriterLockSlim _clientLock = new ReaderWriterLockSlim();
  private static readonly Dictionary<ulong, EasyAntiCheat.Server.Hydra.Client> _clientsByID = new Dictionary<ulong, EasyAntiCheat.Server.Hydra.Client>();
  private static readonly Dictionary<EasyAntiCheat.Server.Hydra.Client, ulong> _idsByClient = new Dictionary<EasyAntiCheat.Server.Hydra.Client, ulong>();
  private static bool _clientFlag = false;
  private static bool _serverFlag = false;
  [ThreadStatic]
  private static MemoryStream _buffer;

  private void InitializeServer()
  {
    // ISSUE: method pointer
    EACIntegration._eacServer = new EasyAntiCheatServer<EasyAntiCheat.Server.Hydra.Client>(new EasyAntiCheatServer<EasyAntiCheat.Server.Hydra.Client>.ClientStatusHandler((object) this, __methodptr(OnClientStatusChange)), 60, "EasyAntiCheat.Game.Server");
    EACIntegration._serverFlag = true;
    EventManager.Subscribe<ConnectionStartEvent>(new EventSubscriber<ConnectionStartEvent>(this.ServerOnConnectionStart));
    EventManager.Subscribe<ConnectionLostEvent>(new EventSubscriber<ConnectionLostEvent>(this.ServerOnConnectionLost), EventHandlerOrder.VeryEarly);
    Logger.Info("EAC server initialized.");
  }

  private void ServerOnConnectionStart(ConnectionStartEvent e)
  {
    Player player = CodeHatch.Engine.Networking.Server.GetPlayerBySessionId(e.Connection.SessionId) ?? CodeHatch.Engine.Networking.Server.GetQueuedPlayer(e.Connection.SessionId);
    if (player.IsServer)
      return;
    EasyAntiCheat.Server.Hydra.Client compatibilityClient = EACIntegration._eacServer.GenerateCompatibilityClient();
    EACIntegration._clientLock.EnterWriteLock();
    EACIntegration._clientsByID.Add(player.Id, compatibilityClient);
    EACIntegration._idsByClient.Add(compatibilityClient, player.Id);
    EACIntegration._clientLock.ExitWriteLock();
    EACIntegration._eacServer.RegisterClient(compatibilityClient, player.Id.ToString(), player.Connection.IpAddress, string.Empty, player.Name, (PlayerRegisterFlags) 0);
  }

  private void ServerOnConnectionLost(ConnectionLostEvent e)
  {
    Player player = CodeHatch.Engine.Networking.Server.GetPlayerBySessionId(e.Connection.SessionId) ?? CodeHatch.Engine.Networking.Server.GetQueuedPlayer(e.Connection.SessionId);
    if (player == null)
    {
      Logger.Error("Unable to find player in order to remove EAC connection.");
    }
    else
    {
      EACIntegration._clientLock.EnterWriteLock();
      EasyAntiCheat.Server.Hydra.Client key = EACIntegration._clientsByID[player.Id];
      EACIntegration._clientsByID.Remove(player.Id);
      EACIntegration._idsByClient.Remove(key);
      EACIntegration._clientLock.ExitWriteLock();
      Logger.Info("Unregistering player {0}.", (object) player.Id);
      EACIntegration._eacServer.UnregisterClient(key);
    }
  }

  private void UninitializeServer()
  {
    CodeHatch.Engine.Networking.Server.OnNetworkReceive -= new Action<int, IStream, MessageFlags, Player>(this.ServerOnNetworkReceive);
    EventManager.Unsubscribe<ConnectionStartEvent>(new EventSubscriber<ConnectionStartEvent>(this.ServerOnConnectionStart));
    EventManager.Unsubscribe<ConnectionLostEvent>(new EventSubscriber<ConnectionLostEvent>(this.ServerOnConnectionLost));
    EACIntegration._clientLock.EnterWriteLock();
    EACIntegration._clientsByID.Clear();
    EACIntegration._idsByClient.Clear();
    EACIntegration._clientLock.ExitWriteLock();
    EACIntegration._eacServer.Dispose();
    EACIntegration._eacServer = (EasyAntiCheatServer<EasyAntiCheat.Server.Hydra.Client>) null;
    EACIntegration._serverFlag = false;
    Logger.Info("EAC server uninitialized.");
  }

  private void ServerUpdate()
  {
    IStream stream = StreamPool.Instance.Get("EAC Server");
    byte[] bytes;
    int length;
    EasyAntiCheat.Server.Hydra.Client key;
    while (EACIntegration._eacServer.PopNetworkMessage(ref key, ref bytes, ref length))
    {
      stream.Reset();
      stream.WriteBytes(bytes, 0, length);
      ulong playerId;
      try
      {
        EACIntegration._clientLock.EnterReadLock();
        if (!EACIntegration._idsByClient.TryGetValue(key, out playerId))
        {
          Logger.ErrorFormat("Unable to find the EAC client for a message received from EAC. ({0})", (object) ((EasyAntiCheat.Server.Hydra.Client) ref key).ClientID);
          continue;
        }
      }
      finally
      {
        EACIntegration._clientLock.ExitReadLock();
      }
      Player player = CodeHatch.Engine.Networking.Server.GetPlayerById(playerId) ?? CodeHatch.Engine.Networking.Server.GetQueuedPlayer(playerId);
      if (player == null)
        Logger.ErrorFormat("Unable to find the player using the id give to receive a message from EAC. ({0})", (object) playerId);
      else
        CodeHatch.Engine.Networking.Server.SendStream(EACIntegration._networkID, stream, (IList<Player>) RecipientsList.FromPlayer(player), MessageFlags.Default);
    }
    StreamPool.Instance.Release("EAC Server", stream);
    EACIntegration._eacServer.HandleClientUpdates();
  }

  private void ServerOnNetworkReceive(
    int networkID,
    IStream stream,
    MessageFlags flags,
    Player sender)
  {
    if (networkID != EACIntegration._networkID)
      return;
    EasyAntiCheat.Server.Hydra.Client client;
    try
    {
      EACIntegration._clientLock.EnterReadLock();
      if (!EACIntegration._clientsByID.TryGetValue(sender.Id, out client))
      {
        Logger.ErrorFormat("Unable to find a client in order to receive a EAC message. ({0})", (object) sender.Id);
        return;
      }
    }
    finally
    {
      EACIntegration._clientLock.ExitReadLock();
    }
    EACIntegration._eacServer.PushNetworkMessage(client, stream.Data, stream.ByteCount);
  }

  private void OnClientStatusChange(ClientStatusUpdate<EasyAntiCheat.Server.Hydra.Client> status)
  {
    ulong playerID;
    try
    {
      EACIntegration._clientLock.EnterReadLock();
      if (!EACIntegration._idsByClient.TryGetValue(status.Client, out playerID))
      {
        object[] objArray = new object[1];
        EasyAntiCheat.Server.Hydra.Client client = status.Client;
        objArray[0] = (object) ((EasyAntiCheat.Server.Hydra.Client) ref client).ClientID;
        Logger.ErrorFormat("Unable to find a player using the EAC client of {0}.", objArray);
        return;
      }
    }
    finally
    {
      EACIntegration._clientLock.ExitReadLock();
    }
    if (status.RequiresKick)
    {
      object[] objArray = new object[3];
      EasyAntiCheat.Server.Hydra.Client client = status.Client;
      objArray[0] = (object) ((EasyAntiCheat.Server.Hydra.Client) ref client).ClientID;
      objArray[1] = (object) playerID;
      objArray[2] = (object) status.Message;
      Logger.InfoFormat("Kicking client {0} with connectionId: {1} | cause = {2}", objArray);
      Coroutiner.RunActionNextUpdate((System.Action) (() =>
      {
        Player playerById = CodeHatch.Engine.Networking.Server.GetPlayerById(playerID);
        switch ((int) status.Status)
        {
          case 0:
            CodeHatch.Engine.Networking.Server.Kick(playerById, "No EAC Connection");
            break;
          case 1:
            CodeHatch.Engine.Networking.Server.Kick(playerById, "Failed to authenticate with EAC");
            break;
          case 3:
            CodeHatch.Engine.Networking.Server.Kick(playerById, "User banned by EAC");
            break;
          case 4:
            CodeHatch.Engine.Networking.Server.Kick(playerById, status.Message);
            break;
        }
      }));
    }
    else
    {
      if (status.Status != 2)
        return;
      Logger.InfoFormat("UserAuthenticated: {0}", (object) playerID);
    }
  }

  private void InitializeClient()
  {
    if (!Runtime.Initialized)
      Runtime.Initialize(new EventHandler<StateChangedEventArgs>(EACIntegration.OnStateChanged), new EventHandler<LoadCompletedEventArgs>(EACIntegration.OnLoadCompleted), new EventHandler<LoadProgressEventArgs>(EACIntegration.OnLoadProgress));
    Runtime.ConnectionReset();
    EACIntegration._clientFlag = true;
    Logger.Info("EAC client initialized.");
  }

  private void UninitializeClient()
  {
    CodeHatch.Engine.Networking.Server.OnNetworkReceive -= new Action<int, IStream, MessageFlags, Player>(this.ClientOnNetworkReceive);
    Runtime.ConnectionReset();
    Logger.Info("EAC client uninitialized.");
  }

  private void ClientUpdate()
  {
    IStream stream = StreamPool.Instance.Get("EAC Client");
    byte[] bytes;
    int length;
    while (Runtime.PopNetworkMessage(ref bytes, ref length))
    {
      stream.Reset();
      stream.WriteBytes(bytes, 0, length);
      CodeHatch.Engine.Networking.Server.SendStream(EACIntegration._networkID, stream, (IList<Player>) RecipientsList.Server, MessageFlags.Default);
    }
    StreamPool.Instance.Release("EAC Client", stream);
    Runtime.PollStatus();
  }

  private void ClientOnNetworkReceive(
    int networkID,
    IStream stream,
    MessageFlags flags,
    Player sender)
  {
    if (networkID != EACIntegration._networkID)
      return;
    Runtime.PushNetworkMessage(stream.Data, stream.ByteCount);
  }

  private static void OnLoadProgress(object sender, LoadProgressEventArgs eventArgs)
  {
    Logger.Info(string.Format("OnLoadProgress() :: {0}%", (object) eventArgs.Progress));
  }

  private static void OnLoadCompleted(object sender, LoadCompletedEventArgs eventArgs)
  {
    switch ((int) eventArgs.Status)
    {
      case 0:
        Logger.Info(string.Format("OnLoadCompleted() :: {0}", (object) ((LoadEventArgs) eventArgs).Message));
        break;
      default:
        Logger.Info(string.Format("OnLoadCompleted() :: ERROR: {0}", (object) ((LoadEventArgs) eventArgs).Message));
        break;
    }
  }

  private static void OnStateChanged(object sender, StateChangedEventArgs eventArgs)
  {
    if (eventArgs.Type == 1)
    {
      string message = string.Format("Game Client Violation: {0}", (object) eventArgs.Message);
      Logger.Info(message);
      Dialogue.OnSubmit handler = (Dialogue.OnSubmit) ((selection, dialogue, data) => Environment.Exit(0));
      Player.Local.ShowPopup("EAC Violation", message, handler: handler);
      Coroutiner.RunActionAfterTime(10f, (System.Action) (() => Environment.Exit(0)));
    }
    else
      Logger.Info(string.Format("Unhandled game callback :: {0} {1}", (object) eventArgs.Type, (object) eventArgs.Message));
  }

  private static MemoryStream Buffer
  {
    get => EACIntegration._buffer ?? (EACIntegration._buffer = new MemoryStream());
  }

  public static bool ProtectStream(ulong playerID, IStream stream)
  {
    int bytePosition = stream.BytePosition;
    int count = stream.ByteCount - stream.BytePosition;
    if (EACIntegration._serverFlag)
    {
      if (EACIntegration._eacServer == null)
      {
        Logger.Error("EAC has not initialized yet but we are trying to protect a message.");
        return false;
      }
      EasyAntiCheat.Server.Hydra.Client client;
      try
      {
        EACIntegration._clientLock.EnterReadLock();
        if (!EACIntegration._clientsByID.TryGetValue(playerID, out client))
        {
          Logger.ErrorFormat("Could not protect a stream because the player did not exist. ({0})", (object) playerID);
          return false;
        }
      }
      finally
      {
        EACIntegration._clientLock.ExitReadLock();
      }
      EACIntegration.Buffer.Seek(0L, SeekOrigin.Begin);
      EACIntegration.Buffer.SetLength(0L);
      EACIntegration.Buffer.Write(stream.Data, bytePosition, count);
      EACIntegration.Buffer.Seek(0L, SeekOrigin.Begin);
      if (!EACIntegration._eacServer.NetProtect.ProtectMessage(client, EACIntegration.Buffer, EACIntegration.Buffer))
        Logger.Error("Unable to protect a message via EAC.");
      stream.ByteCapacity = (int) EACIntegration.Buffer.Length + bytePosition;
      EACIntegration.Buffer.Seek(0L, SeekOrigin.Begin);
      EACIntegration.Buffer.Read(stream.Data, bytePosition, (int) EACIntegration.Buffer.Length);
      stream.ByteCount = (int) EACIntegration.Buffer.Length + bytePosition;
    }
    else if (EACIntegration._clientFlag)
    {
      if (!Runtime.IsActive() || !Runtime.Initialized)
      {
        Logger.Error("EAC has not initialized yet but we are trying to protect a message.");
        return false;
      }
      EACIntegration.Buffer.Seek(0L, SeekOrigin.Begin);
      EACIntegration.Buffer.SetLength(0L);
      EACIntegration.Buffer.Write(stream.Data, bytePosition, count);
      EACIntegration.Buffer.Seek(0L, SeekOrigin.Begin);
      if (!Runtime.NetProtect.ProtectMessage(EACIntegration.Buffer, EACIntegration.Buffer))
        Logger.Error("Unable to protect a message via EAC.");
      stream.ByteCapacity = (int) EACIntegration.Buffer.Length + bytePosition;
      EACIntegration.Buffer.Seek(0L, SeekOrigin.Begin);
      EACIntegration.Buffer.Read(stream.Data, bytePosition, (int) EACIntegration.Buffer.Length);
      stream.ByteCount = (int) EACIntegration.Buffer.Length + bytePosition;
    }
    return true;
  }

  public static bool UnprotectStream(ulong playerID, IStream stream)
  {
    int bytePosition = stream.BytePosition;
    int count = stream.ByteCount - stream.BytePosition;
    if (EACIntegration._serverFlag)
    {
      if (EACIntegration._eacServer == null)
      {
        Logger.Error("EAC has not initialized yet but we are trying to unprotect a message.");
        return false;
      }
      EasyAntiCheat.Server.Hydra.Client client;
      try
      {
        EACIntegration._clientLock.EnterReadLock();
        if (!EACIntegration._clientsByID.TryGetValue(playerID, out client))
        {
          Logger.ErrorFormat("Could not unprotect a stream because the player did not exist. ({0})", (object) playerID);
          return false;
        }
      }
      finally
      {
        EACIntegration._clientLock.ExitReadLock();
      }
      EACIntegration.Buffer.Seek(0L, SeekOrigin.Begin);
      EACIntegration.Buffer.SetLength(0L);
      EACIntegration.Buffer.Write(stream.Data, bytePosition, count);
      EACIntegration.Buffer.Seek(0L, SeekOrigin.Begin);
      if (!EACIntegration._eacServer.NetProtect.UnprotectMessage(client, EACIntegration.Buffer, EACIntegration.Buffer))
        Logger.Error("Unable to unprotect a message via EAC.");
      stream.ByteCapacity = (int) EACIntegration.Buffer.Length + bytePosition;
      EACIntegration.Buffer.Seek(0L, SeekOrigin.Begin);
      EACIntegration.Buffer.Read(stream.Data, bytePosition, (int) EACIntegration.Buffer.Length);
      stream.ByteCount = (int) EACIntegration.Buffer.Length + bytePosition;
    }
    else if (EACIntegration._clientFlag)
    {
      if (!Runtime.IsActive() || !Runtime.Initialized)
      {
        Logger.Error("EAC has not initialized yet but we are trying to unprotect a message.");
        return false;
      }
      EACIntegration.Buffer.Seek(0L, SeekOrigin.Begin);
      EACIntegration.Buffer.SetLength(0L);
      EACIntegration.Buffer.Write(stream.Data, bytePosition, count);
      EACIntegration.Buffer.Seek(0L, SeekOrigin.Begin);
      if (!Runtime.NetProtect.UnprotectMessage(EACIntegration.Buffer, EACIntegration.Buffer))
        Logger.Error("Unable to unprotect a message via EAC.");
      stream.ByteCapacity = (int) EACIntegration.Buffer.Length + bytePosition;
      EACIntegration.Buffer.Seek(0L, SeekOrigin.Begin);
      EACIntegration.Buffer.Read(stream.Data, bytePosition, (int) EACIntegration.Buffer.Length);
      stream.ByteCount = (int) EACIntegration.Buffer.Length + bytePosition;
    }
    return true;
  }

  [UsedImplicitly]
  private void Awake()
  {
    Logger.InfoFormat("Starting EAC Integration with network ID {0}.", (object) EACIntegration._networkID);
    if (DedicatedServerBypass.Enabled)
    {
      this.Initialize();
      this.InitializeServer();
    }
    else if (Runtime.IsActive())
    {
      this.Initialize();
      this.InitializeClient();
    }
    else
    {
      Player.Local.ShowPopup("Error", "Please run the game from Reign Of Kings.exe", "Exit", (Dialogue.OnSubmit) ((selection, dialogue, data) => Program.Exit()));
      Coroutiner.RunActionAfterTime(30f, new System.Action(Program.Exit));
    }
  }

  private void Initialize()
  {
    // ISSUE: reference to a compiler-generated field
    if (EACIntegration.\u003C\u003Ef__am\u0024cache11 == null)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      EACIntegration.\u003C\u003Ef__am\u0024cache11 = new LogWriterDelegate((object) null, __methodptr(\u003CInitialize\u003Em__3F));
    }
    // ISSUE: reference to a compiler-generated field
    Log.SetOut(EACIntegration.\u003C\u003Ef__am\u0024cache11);
    Log.Level = (LogLevel) 1;
    Game.RegisterLoader((IProgressLoader) this);
    EventManager.Subscribe<GameEndEvent>(new EventSubscriber<GameEndEvent>(this.OnGameEnd));
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
  }

  private void OnGameEnd(GameEndEvent e)
  {
    Logger.Info("Uninitializing EAC.");
    if (EACIntegration._clientFlag)
      this.UninitializeClient();
    else if (EACIntegration._serverFlag)
      this.UninitializeServer();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  [UsedImplicitly]
  private void FixedUpdate()
  {
    if (!this.HasLoaded)
      return;
    if (EACIntegration._serverFlag)
    {
      this.ServerUpdate();
    }
    else
    {
      if (!EACIntegration._clientFlag)
        return;
      this.ClientUpdate();
    }
  }

  [UsedImplicitly]
  private void OnDestroy()
  {
    EventManager.Unsubscribe<GameEndEvent>(new EventSubscriber<GameEndEvent>(this.OnGameEnd));
  }

  [UsedImplicitly]
  private void OnApplicationQuit()
  {
    Runtime.Release();
    if (!EACIntegration._serverFlag)
      return;
    EACIntegration._eacServer.Dispose();
    EACIntegration._eacServer = (EasyAntiCheatServer<EasyAntiCheat.Server.Hydra.Client>) null;
  }

  public string LoadDescription { get; set; }

  public float LoadProgress { get; set; }

  public float LoadWeight { get; private set; }

  public bool HasLoaded { get; set; }

  public bool CanLoadSynchronous { get; private set; }

  public bool LoadSynchronous { get; set; }

  public bool IsLoadDependancy(IProgressLoader loader) => loader is Serverable;

  public void BeginLoad()
  {
    if (EACIntegration._clientFlag)
      CodeHatch.Engine.Networking.Server.OnNetworkReceive += new Action<int, IStream, MessageFlags, Player>(this.ClientOnNetworkReceive);
    else if (EACIntegration._serverFlag)
      CodeHatch.Engine.Networking.Server.OnNetworkReceive += new Action<int, IStream, MessageFlags, Player>(this.ServerOnNetworkReceive);
    this.HasLoaded = true;
  }
}
