// Decompiled with JetBrains decompiler
// Type: ArmorHidesHair
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ArmorHidesHair : MonoBehaviour
{
  public bool HideHair = true;
  public bool HideBeard = true;
  [Tooltip("If the character is using one of the specificed Hair Styles, it will be replaced with the 'HairToReplaceWith' while wearing this item")]
  public int[] ReplaceableHairStyles;
  public int HairToReplaceWith;
}
