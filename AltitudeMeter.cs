// Decompiled with JetBrains decompiler
// Type: AltitudeMeter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AltitudeMeter : MonoBehaviour
{
  public UILabel label;

  public void Update()
  {
    string empty = string.Empty;
    float num1 = this.transform.position.y;
    string str;
    if ((int) num1 != 0)
    {
      if ((double) num1 < 0.0)
      {
        str = empty + "Descent:";
        num1 = -num1;
      }
      else
        str = empty + "Ascent:";
      int num2 = (int) num1 / 1000000;
      float num3 = num1 - (float) (num2 * 1000000);
      int num4 = (int) num3 / 1000;
      int num5 = (int) (num3 - (float) (num4 * 1000));
      if (num2 > 0)
      {
        if (num2 == 1)
          str += " 1 Megameter";
        else
          str = str + " " + (object) num2 + " Megameters";
      }
      if (num4 > 0)
      {
        if (num4 == 1)
          str += " 1 Kilometer";
        else
          str = str + " " + (object) num4 + " Kilometers";
      }
      if (num5 > 0)
      {
        if (num5 == 1)
          str += " 1 Meter";
        else
          str = str + " " + (object) num5 + " Meters";
      }
    }
    else
      str = "At Ground Level";
    this.label.text = str;
  }
}
