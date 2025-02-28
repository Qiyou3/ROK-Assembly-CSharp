// Decompiled with JetBrains decompiler
// Type: DestroyExtraTransforms
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DestroyExtraTransforms : MonoBehaviour
{
  [ContextMenu("Fuck you")]
  private void FuckingShit()
  {
    Transform[] components = this.gameObject.GetComponents<Transform>();
    if (components.Length <= 1)
      return;
    int num = 1;
    while (num < components.Length)
      ++num;
  }
}
