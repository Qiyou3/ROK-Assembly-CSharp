// Decompiled with JetBrains decompiler
// Type: DestroyIfAlreadyExists
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DestroyIfAlreadyExists : MonoBehaviour
{
  private static readonly List<string> AlreadyExistsLookup = new List<string>();
  private bool m_allowedToExist;

  public bool WillDestroy
  {
    get
    {
      return !this.m_allowedToExist && DestroyIfAlreadyExists.AlreadyExistsLookup.Contains(this.gameObject.GetFullName());
    }
  }

  public void Awake()
  {
    if (this.WillDestroy)
    {
      Object.DestroyImmediate((Object) this.gameObject);
    }
    else
    {
      this.m_allowedToExist = true;
      DestroyIfAlreadyExists.AlreadyExistsLookup.Add(this.gameObject.GetFullName());
    }
  }
}
