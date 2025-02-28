// Decompiled with JetBrains decompiler
// Type: IDReference
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class IDReference
{
  public string itsID = string.Empty;
  public bool itsEmpty = true;
  public bool itsCanBeDeleted;

  public string GetID() => this.itsID;

  public void SetID(string theID)
  {
    this.itsID = theID;
    this.itsEmpty = false;
  }

  public bool GetHasValue() => !this.itsEmpty;

  public void SetEmpty() => this.itsEmpty = true;

  public override string ToString() => this.GetID();

  public bool GetCanBeDeleted() => this.itsCanBeDeleted;

  public void SetCanBeDeleted(bool theCanBeDeleted) => this.itsCanBeDeleted = theCanBeDeleted;
}
