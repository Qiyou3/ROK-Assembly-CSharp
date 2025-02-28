// Decompiled with JetBrains decompiler
// Type: AstarProfiler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

#nullable disable
public class AstarProfiler
{
  private static Dictionary<string, AstarProfiler.ProfilePoint> profiles = new Dictionary<string, AstarProfiler.ProfilePoint>();
  private static DateTime startTime = DateTime.UtcNow;
  public static AstarProfiler.ProfilePoint[] fastProfiles;
  public static string[] fastProfileNames;

  private AstarProfiler()
  {
  }

  [Conditional("ProfileAstar")]
  public static void InitializeFastProfile(string[] profileNames)
  {
    AstarProfiler.fastProfileNames = profileNames;
    AstarProfiler.fastProfiles = new AstarProfiler.ProfilePoint[profileNames.Length];
  }

  [Conditional("ProfileAstar")]
  public static void StartFastProfile(int tag)
  {
    AstarProfiler.fastProfiles[tag].lastRecorded = DateTime.UtcNow;
  }

  [Conditional("ProfileAstar")]
  public static void EndFastProfile(int tag)
  {
    DateTime utcNow = DateTime.UtcNow;
    AstarProfiler.ProfilePoint fastProfile = AstarProfiler.fastProfiles[tag];
    fastProfile.totalTime += utcNow - fastProfile.lastRecorded;
    ++fastProfile.totalCalls;
    AstarProfiler.fastProfiles[tag] = fastProfile;
  }

  [Conditional("UNITY_PRO_PROFILER")]
  public static void EndProfile()
  {
  }

  [Conditional("ProfileAstar")]
  public static void StartProfile(string tag)
  {
    AstarProfiler.ProfilePoint profilePoint;
    AstarProfiler.profiles.TryGetValue(tag, out profilePoint);
    profilePoint.lastRecorded = DateTime.UtcNow;
    AstarProfiler.profiles[tag] = profilePoint;
  }

  [Conditional("ProfileAstar")]
  public static void EndProfile(string tag)
  {
    if (!AstarProfiler.profiles.ContainsKey(tag))
    {
      Logger.ErrorFormat("Can only end profiling for a tag which has already been started (tag was {0})", (object) tag);
    }
    else
    {
      DateTime utcNow = DateTime.UtcNow;
      AstarProfiler.ProfilePoint profile = AstarProfiler.profiles[tag];
      profile.totalTime += utcNow - profile.lastRecorded;
      ++profile.totalCalls;
      AstarProfiler.profiles[tag] = profile;
    }
  }

  [Conditional("ProfileAstar")]
  public static void Reset()
  {
    AstarProfiler.profiles.Clear();
    AstarProfiler.startTime = DateTime.UtcNow;
    if (AstarProfiler.fastProfiles == null)
      return;
    for (int index = 0; index < AstarProfiler.fastProfiles.Length; ++index)
      AstarProfiler.fastProfiles[index] = new AstarProfiler.ProfilePoint();
  }

  [Conditional("ProfileAstar")]
  public static void PrintFastResults()
  {
    TimeSpan timeSpan = DateTime.UtcNow - AstarProfiler.startTime;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("============================\n\t\t\t\tProfile results:\n============================\n");
    stringBuilder.Append("Name\t\t|\tTotal Time\t|\tTotal Calls\t|\tAvg/Call\t");
    for (int index = 0; index < AstarProfiler.fastProfiles.Length; ++index)
    {
      string fastProfileName = AstarProfiler.fastProfileNames[index];
      AstarProfiler.ProfilePoint fastProfile = AstarProfiler.fastProfiles[index];
      double totalMilliseconds = fastProfile.totalTime.TotalMilliseconds;
      int totalCalls = fastProfile.totalCalls;
      if (totalCalls >= 1)
      {
        stringBuilder.Append("\n").Append(fastProfileName.PadLeft(10)).Append("|   ");
        stringBuilder.Append(totalMilliseconds.ToString("0.0").PadLeft(10)).Append("|   ");
        stringBuilder.Append(totalCalls.ToString().PadLeft(10)).Append("|   ");
        stringBuilder.Append((totalMilliseconds / (double) totalCalls).ToString("0.000").PadLeft(10));
      }
    }
    stringBuilder.Append("\n\n============================\n\t\tTotal runtime: ");
    stringBuilder.Append(timeSpan.TotalSeconds.ToString("F3"));
    stringBuilder.Append(" seconds\n============================");
    Logger.Info(stringBuilder.ToString());
  }

  [Conditional("ProfileAstar")]
  public static void PrintResults()
  {
    TimeSpan timeSpan = DateTime.UtcNow - AstarProfiler.startTime;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("============================\n\t\t\t\tProfile results:\n============================\n");
    int num = 5;
    foreach (KeyValuePair<string, AstarProfiler.ProfilePoint> profile in AstarProfiler.profiles)
      num = Math.Max(profile.Key.Length, num);
    stringBuilder.Append(" Name ".PadRight(num)).Append("|").Append(" Total Time\t".PadRight(20)).Append("|").Append(" Total Calls ".PadRight(20)).Append("|").Append(" Avg/Call ".PadRight(20));
    foreach (KeyValuePair<string, AstarProfiler.ProfilePoint> profile in AstarProfiler.profiles)
    {
      double totalMilliseconds = profile.Value.totalTime.TotalMilliseconds;
      int totalCalls = profile.Value.totalCalls;
      if (totalCalls >= 1)
      {
        string key = profile.Key;
        stringBuilder.Append("\n").Append(key.PadRight(num)).Append("| ");
        stringBuilder.Append(totalMilliseconds.ToString("0.0").PadRight(20)).Append("| ");
        stringBuilder.Append(totalCalls.ToString().PadRight(20)).Append("| ");
        stringBuilder.Append((totalMilliseconds / (double) totalCalls).ToString("0.000").PadRight(20));
      }
    }
    stringBuilder.Append("\n\n============================\n\t\tTotal runtime: ");
    stringBuilder.Append(timeSpan.TotalSeconds.ToString("F3"));
    stringBuilder.Append(" seconds\n============================");
    Logger.Info(stringBuilder.ToString());
  }

  public struct ProfilePoint
  {
    public DateTime lastRecorded;
    public TimeSpan totalTime;
    public int totalCalls;
  }
}
