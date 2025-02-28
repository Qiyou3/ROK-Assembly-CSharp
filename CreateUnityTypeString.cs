// Decompiled with JetBrains decompiler
// Type: CreateUnityTypeString
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class CreateUnityTypeString : MonoBehaviour
{
  public string str;

  [ContextMenu("Create String")]
  private void CreateString()
  {
    this.str = new string(((IEnumerable<char>) this.str.ToCharArray()).Select<char, char>((Func<char, char>) (c => char.IsWhiteSpace(c) ? ' ' : c)).ToArray<char>());
    string[] strArray = this.str.Split(' ');
    this.str = string.Empty;
    foreach (string str in strArray)
    {
      if (str.Length >= 1)
      {
        CreateUnityTypeString createUnityTypeString = this;
        createUnityTypeString.str = createUnityTypeString.str + "case \"" + str + "\": return typeof(" + str + ");\n";
      }
    }
    this.LogInfo<CreateUnityTypeString>(this.str);
  }
}
