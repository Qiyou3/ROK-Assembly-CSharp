// Decompiled with JetBrains decompiler
// Type: Console
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Chatting;
using CodeHatch.Engine.Core.Consoles;
using CodeHatch.Engine.Core.Input;
using CodeHatch.Engine.Networking;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Entities.Players;
using CodeHatch.Networking.Events.Gaming;
using CodeHatch.Networking.Events.Players;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

#nullable disable
public class Console : MonoBehaviour
{
  public static Console main;
  public bool ManagesInput = true;
  public ConsoleLabel InputLabel;
  public ConsoleLabel OutputField;
  public string MessageScrollUp = "[808080](Scroll Up)[-]\n";
  public string MessageScrollDown = "[808080](Scroll Down)[-]";
  public string DebugInput = string.Empty;
  public static readonly List<Console.Message> Messages = new List<Console.Message>();
  public static Console.EntryField InputField;
  private static float _scrollAccum;
  public static int ScrollIndex;
  public static bool AutoScroll = true;
  public static string CurrentInput;
  public static int InputHistoryIndex = -1;
  public static List<string> InputHistory = new List<string>();

  public static void AddMessage(Console.Message message)
  {
    message.ResetLifespanTimer();
    Console.Messages.Remove(message);
    Console.Messages.Add(message);
    Console.SendEvent(message.message);
    if (Console.Messages.Count < ConsoleSettings.OutputHistoryLength)
      return;
    Console.Messages.RemoveAt(0);
  }

  public static void AddMessage(string text)
  {
    Console.AddMessageFinal(text, ConsoleSettings.MessageColor);
  }

  public static void AddMessage(string text, Player owner)
  {
    Console.AddMessageFinal(text, ConsoleSettings.MessageColor, owner);
  }

  public static void AddDebug(string text)
  {
    Console.AddMessageFinal(text, ConsoleSettings.DebugColor);
  }

  public static void AddDebug(string text, Player owner)
  {
    Console.AddMessageFinal(text, ConsoleSettings.DebugColor, owner);
  }

  public static void AddWarning(string text)
  {
    Console.AddMessageFinal(text, ConsoleSettings.WarningColor);
  }

  public static void AddWarning(string text, Player owner)
  {
    Console.AddMessageFinal(text, ConsoleSettings.WarningColor, owner);
  }

  public static void AddError(string text)
  {
    Console.AddMessageFinal(text, ConsoleSettings.ErrorColor);
  }

  public static void AddError(string text, Player owner)
  {
    Console.AddMessageFinal(text, ConsoleSettings.ErrorColor, owner);
  }

  private static void AddMessageFinal(string text, Color colour)
  {
    Console.AddMessageFinal(text, colour, (Player) null);
  }

  private static void AddMessageFinal(string text, Color colour, Player owner)
  {
    if (text.IsNullEmptyOrWhite())
      return;
    string[] strArray = text.Split('\n');
    for (int index = 0; index < strArray.Length; ++index)
    {
      new Console.Message(strArray[index], colour).Owner = owner;
      Logger.Info<Console>(strArray[index]);
      if (Console.Messages.Count >= ConsoleSettings.OutputHistoryLength)
        Console.Messages.RemoveAt(0);
    }
  }

  private static void SendEvent(string text)
  {
    text = Console.CropMessage(text);
    if (string.IsNullOrEmpty(text))
      return;
    EventManager.CallEvent((BaseEvent) new ConsoleAddMessageEvent(text));
  }

  public static void Submit(string message)
  {
    if (string.IsNullOrEmpty(message) || message == "/")
      return;
    if (message.StartsWith("/"))
    {
      EventManager.CallEvent((BaseEvent) new PlayerCommandEvent(Player.Local.Id, message));
    }
    else
    {
      message = Console.CropMessage(message);
      string chatChannel = Player.Local.GetChatChannel();
      switch (chatChannel)
      {
        case "guild":
          if (Player.Local.GetGuild() == null)
          {
            EventManager.CallEvent((BaseEvent) new PlayerChatEvent(Player.Local.Id, message));
            Player.Local.SetChatChannel("yell");
            break;
          }
          Player.Local.SendGuildMessage(message);
          break;
        case "local":
          Player.Local.SendLocalMessage(message);
          break;
        default:
          if (chatChannel.StartsWith("whisper"))
          {
            ulong result;
            if (ulong.TryParse(chatChannel.Split('_')[1], out result))
            {
              Player.Local.SendWhisper(result, message);
              break;
            }
            Player.Local.SetChatChannel("yell");
            break;
          }
          EventManager.CallEvent((BaseEvent) new PlayerChatEvent(Player.Local.Id, message));
          break;
      }
    }
  }

  private static string CropMessage(string message)
  {
    if (string.IsNullOrEmpty(message))
      return string.Empty;
    string str = message.Trim();
    if (str.Length > ConsoleSettings.InputMessageLength)
      str = str.Substring(0, ConsoleSettings.InputMessageLength);
    return str;
  }

  public static void Cancel()
  {
    if (!((Object) Console.main != (Object) null))
      return;
    Console.main.InputEnd();
  }

  public static string CurrentOutput
  {
    get
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < Console.Messages.Count; ++index)
      {
        Console.Message message = Console.Messages[index];
        stringBuilder.AppendLine(NGUIText.StripSymbols(message.message));
      }
      return stringBuilder.ToString();
    }
  }

  public static bool IsShowing
  {
    get
    {
      if (!((Object) Console.main != (Object) null))
        return false;
      return Console.InputField.IsSubmitting || !ConsoleSettings.InputAutoHide;
    }
  }

  public static bool IsSubmitting
  {
    get => (Object) Console.main != (Object) null && Console.InputField.IsSubmitting;
  }

  public static bool HasInput
  {
    get
    {
      return (Object) Console.main != (Object) null && (Object) Console.main.InputLabel != (Object) null && !string.IsNullOrEmpty(Console.main.InputLabel.Text);
    }
  }

  public static bool HasOutput
  {
    get
    {
      return (Object) Console.main != (Object) null && (Object) Console.main.OutputField != (Object) null && !string.IsNullOrEmpty(Console.main.OutputField.Text);
    }
  }

  [ContextMenu("Submit Debug Input")]
  private void SubmitDebugInputContext()
  {
    if (this.DebugInput.IsNullEmptyOrWhite())
      return;
    Console.Submit(this.DebugInput);
    this.DebugInput = string.Empty;
  }

  public void Awake()
  {
    if ((Object) Console.main == (Object) null)
    {
      Console.main = this;
      if ((Object) this.OutputField == (Object) null || (Object) this.InputLabel == (Object) null)
      {
        IEnumerable<ConsoleLabel> implementorsInChildren = this.gameObject.GetImplementorsInChildren<ConsoleLabel>();
        if (implementorsInChildren != null)
        {
          foreach (ConsoleLabel consoleLabel in implementorsInChildren)
          {
            if ((Object) this.OutputField == (Object) null && (Object) consoleLabel != (Object) this.InputLabel)
              this.OutputField = consoleLabel;
            if ((Object) this.InputLabel == (Object) null && (Object) consoleLabel != (Object) this.OutputField)
              this.InputLabel = consoleLabel;
          }
        }
        else
        {
          this.LogError<Console>("Console chat and/or input labels are not assigned.");
          this.enabled = false;
          return;
        }
      }
      Console.InputField.Text = string.Empty;
      Console.InputField.IsSubmitting = !ConsoleSettings.InputAutoHide;
      EventManager.Subscribe<GameEndEvent>(new EventSubscriber<GameEndEvent>(this.OnGameEnd));
    }
    else
    {
      this.LogError<Console>("There should only be one notifications menu. Notifications.main != null");
      Object.DestroyImmediate((Object) this);
    }
  }

  public void OnEnable()
  {
    if (!((Object) Console.main == (Object) this))
      return;
    InputLayersManager.EnableLayer(ConsoleSettings.InputLayer);
  }

  public void OnDisable()
  {
    if (!((Object) Console.main == (Object) this))
      return;
    InputLayersManager.DisableLayer(ConsoleSettings.InputLayer);
  }

  public void OnDestroy()
  {
    if ((Object) Console.main == (Object) this)
    {
      if (Console.IsSubmitting)
        this.InputEnd();
      Console.main = (Console) null;
    }
    EventManager.Unsubscribe<GameEndEvent>(new EventSubscriber<GameEndEvent>(this.OnGameEnd));
  }

  private void OnGameEnd(GameEndEvent e)
  {
    for (int index = 0; index < Console.Messages.Count; ++index)
    {
      Console.Message message = Console.Messages[index];
      message.color = new Color(0.65f, 0.65f, 0.65f, 0.4f);
      message.shortMessage = string.Empty;
      message.message = ColorUtil.StripHexColor(message.message);
    }
  }

  public void Update()
  {
    if (!Console.InputField.IsSubmitting && ConsoleSettings.InputAutoHide)
      return;
    DisableCursor.EnableCursorOneFrame();
  }

  public void LateUpdate()
  {
    this.UpdateInput();
    this.UpdateScroll();
    if (!UnityEngine.Input.GetMouseButtonUp(1))
      ;
    if (Console.InputField.IsSubmitting || !ConsoleSettings.InputAutoHide)
      this.ShowScrollArea();
    else
      this.UpdateOutput();
    if (!this.ManagesInput)
      return;
    this.InputLabel.Text = this.InputLabel.Truncate(Console.InputField.Text, ConsoleSettings.InputClippingLength);
  }

  private void UpdateScroll()
  {
    if (!Console.IsShowing)
      return;
    Console._scrollAccum -= UnityEngine.Input.GetAxis("Mouse ScrollWheel") * 50f;
    int num = (double) Console._scrollAccum >= 0.0 ? Mathf.FloorToInt(Console._scrollAccum) : Mathf.CeilToInt(Console._scrollAccum);
    if (num > 0)
    {
      Console.ScrollIndex += num;
      Console._scrollAccum -= (float) num;
    }
    else
    {
      if (num >= 0)
        return;
      Console.ScrollIndex += num;
      Console._scrollAccum -= (float) num;
      Console.AutoScroll = false;
    }
  }

  private void ShowScrollArea()
  {
    int num1 = Mathf.Min(Console.Messages.Count, ConsoleSettings.OutputLinesShownTyping);
    int a = Console.Messages.Count - num1;
    if (Console.AutoScroll)
    {
      Console.ScrollIndex = a;
    }
    else
    {
      Console.ScrollIndex = Mathf.Min(a, Mathf.Max(0, Console.ScrollIndex));
      if (Console.ScrollIndex == a)
        Console.AutoScroll = true;
    }
    int num2 = Console.ScrollIndex;
    int num3 = num2 + num1;
    if (num3 > Console.Messages.Count)
    {
      this.LogError<Console>("end >= messages.Count");
      num3 = Console.Messages.Count;
    }
    if (num2 < 0)
    {
      this.LogError<Console>("start < 0");
      num2 = 0;
    }
    StringBuilder stringBuilder = new StringBuilder();
    if (num2 != 0)
      stringBuilder.Append(this.MessageScrollUp);
    for (int index = num2; index < num3; ++index)
    {
      Console.Messages[index].lifespanRemaining -= Time.deltaTime;
      if (index == num3 - 1 && num3 != Console.Messages.Count)
      {
        stringBuilder.Append(this.MessageScrollDown);
      }
      else
      {
        string str = Console.Messages[index].color.GetHexCodeRGBA("[", "]") + "{0}[-]";
        if (this.OutputField.Colour == Console.Messages[index].color)
          stringBuilder.Append(Console.Messages[index].message);
        else
          stringBuilder.AppendFormat("[{0}]{1}[-]", (object) Console.Messages[index].color.GetHexCodeRGB(string.Empty, string.Empty), (object) Console.Messages[index].message);
        if (index < num3 - 1)
          stringBuilder.Append("\n");
      }
    }
    this.OutputField.Text = stringBuilder.ToString();
  }

  private void UpdateOutput()
  {
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = Mathf.Max(0, Console.Messages.Count - ConsoleSettings.OutputLinesShown); index < Console.Messages.Count; ++index)
    {
      Console.Messages[index].lifespanRemaining -= Time.deltaTime;
      if ((double) Console.Messages[index].lifespanRemaining < 0.0)
      {
        Console.Messages[index].lifespanRemaining = 0.0f;
        if (Console.Messages[index].shortMessage.Length > 0)
          Console.Messages[index].shortMessage = Regex.Replace(Console.Messages[index].shortMessage, "(?:\\[(?:(?:[A-Fa-f0-9]{8}|[A-Fa-f0-9]{6}|[A-Fa-f0-9]{2}|sub|sup|b|u|i|s|c)|\\/(?:sup|sub|b|u|i|c|s|)|\\-)\\]|.)$", string.Empty, RegexOptions.Multiline);
      }
      if (Console.Messages[index].shortMessage.Length > 0)
      {
        string hexCodeRgba = Console.Messages[index].color.GetHexCodeRGBA("[", "]");
        stringBuilder.AppendFormat("{0}{1}[-]", (object) hexCodeRgba, (object) Console.CloseColours(Console.Messages[index].shortMessage));
        if (index < Console.Messages.Count - 1)
          stringBuilder.Append("\n");
      }
    }
    this.OutputField.Text = stringBuilder.ToString();
  }

  public void UpdateInput()
  {
    if (!InputLayersManager.HasLayer(InputLayers.Chat))
      return;
    if (Console.InputField.IsSubmitting || !ConsoleSettings.InputAutoHide)
    {
      if (ConsoleSettings.ButtonEscape.GetDown())
        Console.Cancel();
      else if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
      {
        ++Console.InputHistoryIndex;
        if (Console.InputHistoryIndex >= Console.InputHistory.Count)
          --Console.InputHistoryIndex;
        if (Console.InputHistoryIndex >= 0)
          Console.InputField.Text = Console.InputHistory[Console.InputHistoryIndex];
        else
          Console.InputField.Text = Console.CurrentInput;
      }
      else if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
      {
        --Console.InputHistoryIndex;
        if (Console.InputHistoryIndex < -1)
          Console.InputHistoryIndex = -1;
        if (Console.InputHistoryIndex >= 0)
          Console.InputField.Text = Console.InputHistory[Console.InputHistoryIndex];
        else
          Console.InputField.Text = Console.CurrentInput;
      }
      else
      {
        if (this.ManagesInput)
        {
          foreach (char ch in UnityEngine.Input.inputString)
          {
            if (ch == '\b')
            {
              if (Console.InputField.Text.Length > 0)
                Console.InputField.Text = Console.InputField.Text.Substring(0, Console.InputField.Text.Length - 1);
            }
            else if (Console.InputField.Text.Length < ConsoleSettings.InputMessageLength)
            {
              switch (ch)
              {
                case '\n':
                case '\r':
                  continue;
                case '/':
                  if (Console.InputField.Text.Length <= 0 || Console.InputField.Text[0] != '/')
                    break;
                  continue;
              }
              // ISSUE: explicit reference operation
              (^ref Console.InputField).Text += (string) (object) ch;
              Console.InputField.Text = Console.InputField.Text.Replace("\\n", "/n");
            }
          }
        }
        Console.CurrentInput = Console.InputField.Text;
        if (!ConsoleSettings.ButtonReturn.GetDown() && !ConsoleSettings.ButtonEnter.GetDown())
          return;
        Console.Submit(Console.InputField.Text);
        if (Console.InputField.Text != string.Empty && (Console.InputHistory.Count == 0 || Console.InputHistory[0] != Console.InputField.Text))
          Console.InputHistory.Insert(0, Console.InputField.Text);
        Console.InputHistoryIndex = -1;
        this.InputEnd();
      }
    }
    else if (ConsoleSettings.ButtonReturn.GetDown() || ConsoleSettings.ButtonEnter.GetDown())
    {
      this.InputBegin(string.Empty);
    }
    else
    {
      if (!ConsoleSettings.ButtonSlash.GetDown())
        return;
      this.InputBegin("/");
    }
  }

  private void InputBegin(string text)
  {
    if (InputLayersManager.ExclusiveMode)
      return;
    this.InputLabel.Selected = true;
    if (this.ManagesInput)
      Console.InputField.Text = text;
    else
      this.InputLabel.Text = text;
    Console.InputField.IsSubmitting = true;
    InputLayersManager.EnableExclusiveLayer(InputLayers.Chat);
  }

  private void InputEnd()
  {
    InputLayersManager.DisableExclusiveLayer(InputLayers.Chat);
    if (ConsoleSettings.InputAutoHide)
      Console.InputField.IsSubmitting = false;
    if (this.ManagesInput)
      Console.InputField.Text = string.Empty;
    else
      this.InputLabel.Text = string.Empty;
    Console.CurrentInput = string.Empty;
    this.InputLabel.Selected = false;
  }

  public static string CloseColours(string message)
  {
    string str = message;
    int num1 = -1;
    int num2 = 0;
    int num3 = 0;
    for (int index1 = 0; index1 < message.Length; ++index1)
    {
      switch (message[index1])
      {
        case '[':
          num1 = index1;
          break;
        case ']':
          if (num1 >= 0)
          {
            string input = message.Substring(num1 + 1, index1 - num1 - 1);
            if (Regex.IsMatch(input, "\\/(?:sup|sub|b|u|i|c|s|)|\\-"))
              ++num3;
            else if (Regex.IsMatch(input, "(?:[A-Fa-f0-9]{8}|[A-Fa-f0-9]{6}|[A-Fa-f0-9]{2}|sub|sup|b|u|i|s|c)"))
            {
              bool flag = true;
              for (int index2 = 0; index2 < input.Length; ++index2)
              {
                if (!input[index2].IsHex())
                {
                  flag = false;
                  break;
                }
              }
              if (flag)
                ++num2;
            }
          }
          num1 = -1;
          break;
      }
    }
    for (; num2 > num3; ++num3)
      str += "[-]";
    return str;
  }

  public class Message
  {
    public const float initialLifespan = 8f;
    public const float resetLifespan = 4f;
    public float lifespanRemaining;
    public string message;
    public string shortMessage;
    public Color color;
    public Player Owner;

    public Message(string message, Color color)
    {
      this.message = message;
      this.shortMessage = message;
      this.lifespanRemaining = 8f;
      this.color = color;
    }

    public void ResetLifespanTimer()
    {
      this.shortMessage = this.message;
      this.lifespanRemaining = 4f;
    }

    public override string ToString() => this.message;
  }

  public struct EntryField
  {
    private bool m_isSubmitting;
    private string _text;

    public bool IsSubmitting
    {
      get => this.m_isSubmitting;
      set
      {
        if (this.m_isSubmitting && !value)
          this.Text = string.Empty;
        this.m_isSubmitting = value;
      }
    }

    public string Text
    {
      get
      {
        return (Object) Console.main == (Object) null || Console.main.ManagesInput ? this._text : Console.main.InputLabel.Text;
      }
      set
      {
        if ((Object) Console.main == (Object) null || Console.main.ManagesInput)
          this._text = value;
        else
          Console.main.InputLabel.Text = value;
      }
    }
  }
}
