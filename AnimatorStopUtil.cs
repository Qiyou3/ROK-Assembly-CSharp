// Decompiled with JetBrains decompiler
// Type: AnimatorStopUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class AnimatorStopUtil
{
  public static readonly string NullStateName = "null";
  private static readonly Dictionary<RuntimeAnimatorController, int[]> _nullStateHashLookupTable = new Dictionary<RuntimeAnimatorController, int[]>();

  public static void Stop(this Animator animator, int layer)
  {
    animator.Play(animator.GetStopStateHash(layer), layer);
  }

  private static int GetStopStateHash(this Animator animator, int layer)
  {
    if ((Object) animator == (Object) null)
    {
      Logger.Error("animator == null");
      return 0;
    }
    if ((Object) animator.runtimeAnimatorController == (Object) null)
    {
      Logger.Error("animator.runtimeAnimatorController == null");
      return 0;
    }
    int[] hashArray = AnimatorStopUtil.GetHashArray(animator);
    if (hashArray == null)
    {
      Logger.ErrorFormat("Could not generate array for animator {0}.", (object) animator);
      return 0;
    }
    if (((IList<int>) hashArray).IsWithinRange<int>(layer))
      return hashArray[layer];
    Logger.ErrorFormat("Layer {0} does not exist for animator {1}.", (object) layer, (object) animator);
    return 0;
  }

  private static int[] GetHashArray(Animator animator)
  {
    if ((Object) animator == (Object) null)
    {
      Logger.Error("animator == null");
      return (int[]) null;
    }
    if ((Object) animator.runtimeAnimatorController == (Object) null)
    {
      Logger.Error("animator.runtimeAnimatorController == null");
      return (int[]) null;
    }
    int[] hashArray;
    if (!AnimatorStopUtil._nullStateHashLookupTable.TryGetValue(animator.runtimeAnimatorController, out hashArray))
    {
      hashArray = new int[animator.layerCount];
      for (int layerIndex = 0; layerIndex < animator.layerCount; ++layerIndex)
      {
        string name = string.Format("{0}.{1}", (object) animator.GetLayerName(layerIndex), (object) AnimatorStopUtil.NullStateName);
        hashArray[layerIndex] = Animator.StringToHash(name);
      }
      AnimatorStopUtil._nullStateHashLookupTable.Add(animator.runtimeAnimatorController, hashArray);
    }
    return hashArray;
  }
}
