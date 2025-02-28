// Decompiled with JetBrains decompiler
// Type: AnimatorShapshotUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class AnimatorShapshotUtil
{
  public static AnimatorSnapshot GetSnapshot(this Animator animator)
  {
    return (Object) animator == (Object) null ? (AnimatorSnapshot) null : new AnimatorSnapshot(animator);
  }

  public static void SetSnapshot(this Animator animator, AnimatorSnapshot snapshot)
  {
    if ((Object) animator == (Object) null || snapshot == null)
      return;
    snapshot.Apply(animator);
  }
}
