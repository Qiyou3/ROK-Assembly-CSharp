// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.ModdableTargeting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Modding.Abstract;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class ModdableTargeting : MonoBehaviour, IModable
  {
    private const string _targetAquisitionVisionRange = "TargetAquisition.Vision.Range";
    private const string _targetAquisitionVisionAngle = "TargetAquisition.Vision.Angle";
    private const string _targetAquisitionHearingRange = "TargetAquisition.Hearing.Range";
    private const string _targetLossRange = "TargetLoss.Range";
    private const string _targetLossDuration = "TargetLoss.Duration";
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    [JetBrains.Annotations.CanBeNull]
    public SetAIServerFromBundle SetBundle;
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    [JetBrains.Annotations.CanBeNull]
    public ConeVisibilityTargetFilter Vision;
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    [JetBrains.Annotations.CanBeNull]
    public DistanceTargetFilter Hearing;
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    [JetBrains.Annotations.CanBeNull]
    public TimedDistanceTargetFilter TargetLossFilter;

    [JetBrains.Annotations.CanBeNull]
    private AIValueBundle Bundle
    {
      get
      {
        return (Object) this.SetBundle != (Object) null ? this.SetBundle.bundle : (AIValueBundle) null;
      }
    }

    public string ModHandlerName => "AI";

    public void GetModDefaults(IList<ModEntry> defaultModList)
    {
      if ((Object) this.Bundle != (Object) null)
        defaultModList.Add(new ModEntry("TargetAquisition.Vision.Range", (object) this.Bundle.noticePlayerDistance));
      else if ((Object) this.Vision != (Object) null)
        defaultModList.Add(new ModEntry("TargetAquisition.Vision.Range", (object) this.Vision.maxDistance));
      if ((Object) this.Vision != (Object) null)
        defaultModList.Add(new ModEntry("TargetAquisition.Vision.Angle", (object) this.Vision.maxAngle));
      if ((Object) this.Hearing != (Object) null)
        defaultModList.Add(new ModEntry("TargetAquisition.Hearing.Range", (object) this.Hearing.maxDistance));
      if ((Object) this.Bundle != (Object) null)
        defaultModList.Add(new ModEntry("TargetLoss.Range", (object) this.Bundle.forgetAboutPlayerDistance));
      else if ((Object) this.TargetLossFilter != (Object) null)
        defaultModList.Add(new ModEntry("TargetLoss.Range", (object) this.TargetLossFilter.maxDistance));
      if (!((Object) this.TargetLossFilter != (Object) null))
        return;
      defaultModList.Add(new ModEntry("TargetLoss.Duration", (object) this.TargetLossFilter.timeout));
    }

    public void ApplyMod(string key, object value)
    {
      if (value == null || !(value is float))
        return;
      string key1 = key;
      if (key1 == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (ModdableTargeting.\u003C\u003Ef__switch\u0024map17 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ModdableTargeting.\u003C\u003Ef__switch\u0024map17 = new Dictionary<string, int>(5)
        {
          {
            "TargetAquisition.Vision.Range",
            0
          },
          {
            "TargetAquisition.Vision.Angle",
            1
          },
          {
            "TargetAquisition.Hearing.Range",
            2
          },
          {
            "TargetLoss.Range",
            3
          },
          {
            "TargetLoss.Duration",
            4
          }
        };
      }
      int num1;
      // ISSUE: reference to a compiler-generated field
      if (!ModdableTargeting.\u003C\u003Ef__switch\u0024map17.TryGetValue(key1, out num1))
        return;
      switch (num1)
      {
        case 0:
          float num2 = Mathf.Max((float) value, 0.0f);
          if ((Object) this.Bundle != (Object) null)
            this.Bundle.noticePlayerDistance = num2;
          if (!((Object) this.Vision != (Object) null))
            break;
          this.Vision.maxDistance = num2;
          break;
        case 1:
          float num3 = Mathf.Clamp((float) value, 0.0f, 180f);
          if (!((Object) this.Vision != (Object) null))
            break;
          this.Vision.maxAngle = num3;
          break;
        case 2:
          float num4 = Mathf.Max((float) value, 0.0f);
          if (!((Object) this.Hearing != (Object) null))
            break;
          this.Hearing.maxDistance = num4;
          break;
        case 3:
          float num5 = Mathf.Max((float) value, 0.0f);
          if ((Object) this.Bundle != (Object) null)
            this.Bundle.forgetAboutPlayerDistance = num5;
          if (!((Object) this.TargetLossFilter != (Object) null))
            break;
          this.TargetLossFilter.maxDistance = num5;
          break;
        case 4:
          float num6 = Mathf.Max((float) value, 0.0f);
          if (!((Object) this.TargetLossFilter != (Object) null))
            break;
          this.TargetLossFilter.timeout = num6;
          break;
      }
    }
  }
}
