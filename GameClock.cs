// Decompiled with JetBrains decompiler
// Type: GameClock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Core;
using CodeHatch.Engine.Core.Gaming;
using CodeHatch.Engine.Modding.Abstract;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Players;
using CodeHatch.Networking.Sync;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Syncable(15f)]
public class GameClock : MonoBehaviour, IProgressLoader, IResetable, IModable
{
  public const float RealTimeSpeed = 1f;
  public static GameClock Instance;
  [Tooltip("Used as a temporary GameClock, without all the synchronizing, so that it may be destroyed whenever.")]
  public bool TemporaryMode;
  public float GameStartTime = 8.5f;
  public float HourOfSunriseStart = 6f;
  public float HourOfSunriseEnd = 8f;
  public float HourOfDayPeak = 13f;
  public float HourOfSunsetStart = 20f;
  public float HourOfSunsetEnd = 22f;
  [Syncable]
  public float TimeOfDay;
  [Syncable]
  public float DaySpeed = 1f;
  public AnimationCurve DaySpeedModifier;
  public GameClock.TimeBlock CurrentTimeBlock = GameClock.TimeBlock.Night;
  public float BlockCompletion;
  private float _StartDaySpeed;
  private SyncObject _syncObject;
  private string TimeOfDayOutput;

  public bool HasLoaded { get; set; }

  public string LoadDescription { get; set; }

  public float LoadProgress { get; set; }

  public float LoadWeight
  {
    get => 1f;
    set
    {
    }
  }

  public bool IsLoadDependancy(IProgressLoader loader) => loader is Serverable;

  public void Awake()
  {
    GameClock.Instance = this;
    this._StartDaySpeed = this.DaySpeed;
    this.TimeOfDay = this.GameStartTime;
    if (this.TemporaryMode)
      return;
    this._syncObject = SyncManager.Register(nameof (GameClock), (object) this);
    Game.RegisterLoader((IProgressLoader) this);
    this.enabled = false;
  }

  public void BeginLoad()
  {
    this.LoadDescription = "Winding Clock";
    this.enabled = true;
    this.HasLoaded = true;
  }

  public void OnDestroy()
  {
    if ((Object) GameClock.Instance == (Object) this)
      GameClock.Instance = (GameClock) null;
    EventManager.Unsubscribe<PlayerJoinEvent>(new EventSubscriber<PlayerJoinEvent>(this.OnPlayerJoin));
    EventManager.Unsubscribe<PlayerLeaveEvent>(new EventSubscriber<PlayerLeaveEvent>(this.OnPlayerLeave));
    if (this.TemporaryMode)
      return;
    this._syncObject = (SyncObject) null;
    SyncManager.Unregister((object) this);
  }

  private void OnPlayerJoin(PlayerJoinEvent e) => this._syncObject.AddController(e.Player);

  private void OnPlayerLeave(PlayerLeaveEvent e)
  {
    if (!this._syncObject.IsControlledBy(e.Player))
      return;
    this._syncObject.AddController(e.Player);
  }

  public void FixedUpdate()
  {
    this.TimeOfDay += (float) ((double) Time.deltaTime / 3600.0 * ((double) this.DaySpeed * (double) this.DaySpeedModifier.Evaluate(this.TimeOfDay)));
    if ((double) this.TimeOfDay > 24.0)
      this.TimeOfDay -= 24f;
    if ((double) this.TimeOfDay < 0.0)
      this.TimeOfDay += 24f;
    this.UpdateTimeBlock();
    this.UpdateBlockCompletion();
  }

  public string TimeOfDayAsClockString(bool Use24Hour)
  {
    this.TimeOfDayOutput = string.Empty;
    this.TimeOfDayOutput += !Use24Hour ? ((double) this.TimeOfDay <= 13.0 ? ((double) this.TimeOfDay >= 1.0 ? ((int) this.TimeOfDay).ToString() : "12") : ((int) this.TimeOfDay - 12).ToString()) : ((int) this.TimeOfDay).ToString();
    this.TimeOfDayOutput += ":";
    this.TimeOfDayOutput += ((int) (((double) this.TimeOfDay - (double) (int) this.TimeOfDay) * 60.0)).ToString("D2");
    if (!Use24Hour)
      this.TimeOfDayOutput += (double) this.TimeOfDay >= 12.0 ? " pm" : " am";
    return this.TimeOfDayOutput;
  }

  public string TimeOfDayAsClockString() => this.TimeOfDayAsClockString(false);

  private void UpdateBlockCompletion()
  {
    switch (this.CurrentTimeBlock)
    {
      case GameClock.TimeBlock.Dawn:
        this.BlockCompletion = (float) (((double) this.TimeOfDay - (double) this.HourOfSunriseStart) / ((double) this.HourOfSunriseEnd - (double) this.HourOfSunriseStart));
        break;
      case GameClock.TimeBlock.Morning:
        this.BlockCompletion = (float) (((double) this.TimeOfDay - (double) this.HourOfSunriseEnd) / ((double) this.HourOfDayPeak - (double) this.HourOfSunriseEnd));
        break;
      case GameClock.TimeBlock.Afternoon:
        this.BlockCompletion = (float) (((double) this.TimeOfDay - (double) this.HourOfDayPeak) / ((double) this.HourOfSunsetStart - (double) this.HourOfDayPeak));
        break;
      case GameClock.TimeBlock.Dusk:
        this.BlockCompletion = (float) (((double) this.TimeOfDay - (double) this.HourOfSunsetStart) / ((double) this.HourOfSunsetEnd - (double) this.HourOfSunsetStart));
        break;
      default:
        if ((double) this.TimeOfDay > 12.0)
        {
          this.BlockCompletion = (float) (((double) this.TimeOfDay - (double) this.HourOfSunsetEnd) / (24.0 - (double) this.HourOfSunsetEnd + (double) this.HourOfSunriseStart));
          break;
        }
        this.BlockCompletion = (float) (((double) this.TimeOfDay + (24.0 - (double) this.HourOfSunsetEnd)) / (24.0 - (double) this.HourOfSunsetEnd + (double) this.HourOfSunriseStart));
        break;
    }
  }

  private void UpdateTimeBlock()
  {
    if ((double) this.TimeOfDay < (double) this.HourOfSunriseStart || (double) this.TimeOfDay > (double) this.HourOfSunsetEnd)
      this.CurrentTimeBlock = GameClock.TimeBlock.Night;
    if ((double) this.TimeOfDay > (double) this.HourOfSunriseStart && (double) this.TimeOfDay < (double) this.HourOfSunriseEnd)
      this.CurrentTimeBlock = GameClock.TimeBlock.Dawn;
    if ((double) this.TimeOfDay > (double) this.HourOfSunriseEnd && (double) this.TimeOfDay < (double) this.HourOfDayPeak)
      this.CurrentTimeBlock = GameClock.TimeBlock.Morning;
    if ((double) this.TimeOfDay > (double) this.HourOfDayPeak && (double) this.TimeOfDay < (double) this.HourOfSunsetStart)
      this.CurrentTimeBlock = GameClock.TimeBlock.Afternoon;
    if ((double) this.TimeOfDay <= (double) this.HourOfSunsetStart || (double) this.TimeOfDay >= (double) this.HourOfSunsetEnd)
      return;
    this.CurrentTimeBlock = GameClock.TimeBlock.Dusk;
  }

  public void PauseAtTime(float time)
  {
    this.TimeOfDay = time;
    this.DaySpeed = 0.0f;
  }

  public void Unpause() => this.DaySpeed = this._StartDaySpeed;

  public ResetableOrder ResetOrder => ResetableOrder.Default;

  public void OnResetScene()
  {
    this.TimeOfDay = this.GameStartTime;
    this.DaySpeed = this._StartDaySpeed;
  }

  public void OnClearScene()
  {
    this.TimeOfDay = this.GameStartTime;
    this.DaySpeed = this._StartDaySpeed;
  }

  public void OnSwitch()
  {
    this.TimeOfDay = this.GameStartTime;
    this.DaySpeed = this._StartDaySpeed;
  }

  public string ModHandlerName => "Clock";

  public void GetModDefaults(IList<ModEntry> defaultModList)
  {
    defaultModList.Add(new ModEntry("DaySpeed", (object) this.DaySpeed));
  }

  public void ApplyMod(string key, object value)
  {
    string key1 = key;
    if (key1 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    if (GameClock.\u003C\u003Ef__switch\u0024map1F == null)
    {
      // ISSUE: reference to a compiler-generated field
      GameClock.\u003C\u003Ef__switch\u0024map1F = new Dictionary<string, int>(1)
      {
        {
          "DaySpeed",
          0
        }
      };
    }
    int num;
    // ISSUE: reference to a compiler-generated field
    if (!GameClock.\u003C\u003Ef__switch\u0024map1F.TryGetValue(key1, out num) || num != 0)
      return;
    this.DaySpeed = Mathf.Max((float) value, 0.0f);
  }

  public enum TimeBlock
  {
    Dawn,
    Morning,
    Afternoon,
    Dusk,
    Night,
  }
}
