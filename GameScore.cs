// Decompiled with JetBrains decompiler
// Type: GameScore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GameScore : MonoBehaviour
{
  private static GameScore s_Instance;
  public string playerLayerName = "Player";
  public string enemyLayerName = "Enemies";
  private int m_Deaths;
  private readonly Dictionary<string, int> m_Kills = new Dictionary<string, int>();
  private float m_StartTime;

  private static GameScore Instance
  {
    get
    {
      if ((Object) GameScore.s_Instance == (Object) null)
        GameScore.s_Instance = (GameScore) Object.FindObjectOfType(typeof (GameScore));
      return GameScore.s_Instance;
    }
  }

  public void OnApplicationQuit() => GameScore.s_Instance = (GameScore) null;

  public static int Deaths
  {
    get => (Object) GameScore.Instance == (Object) null ? 0 : GameScore.Instance.m_Deaths;
  }

  public static ICollection<string> KillTypes
  {
    get
    {
      return (Object) GameScore.Instance == (Object) null ? (ICollection<string>) new string[0] : (ICollection<string>) GameScore.Instance.m_Kills.Keys;
    }
  }

  public static int GetKills(string type)
  {
    return (Object) GameScore.Instance == (Object) null || !GameScore.Instance.m_Kills.ContainsKey(type) ? 0 : GameScore.Instance.m_Kills[type];
  }

  public static float GameTime
  {
    get
    {
      return (Object) GameScore.Instance == (Object) null ? 0.0f : Time.time - GameScore.Instance.m_StartTime;
    }
  }

  public static void RegisterDeath(GameObject deadObject)
  {
    if ((Object) GameScore.Instance == (Object) null)
    {
      Debug.Log((object) "Game score not loaded");
    }
    else
    {
      int layer1 = LayerMask.NameToLayer(GameScore.Instance.playerLayerName);
      int layer2 = LayerMask.NameToLayer(GameScore.Instance.enemyLayerName);
      if (deadObject.layer == layer1)
      {
        ++GameScore.Instance.m_Deaths;
      }
      else
      {
        if (deadObject.layer != layer2)
          return;
        GameScore.Instance.m_Kills[deadObject.name] = !GameScore.Instance.m_Kills.ContainsKey(deadObject.name) ? 1 : GameScore.Instance.m_Kills[deadObject.name] + 1;
      }
    }
  }

  public void OnLevelWasLoaded(int level)
  {
    if ((double) this.m_StartTime != 0.0)
      return;
    this.m_StartTime = Time.time;
  }
}
