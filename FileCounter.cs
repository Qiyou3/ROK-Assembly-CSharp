// Decompiled with JetBrains decompiler
// Type: FileCounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.IO;
using UnityEngine;

#nullable disable
public class FileCounter : MonoBehaviour
{
  public string FolderLocationFromDataPath;
  public string SearchPattern;
  private string[] _folders;

  public string FolderLocation => Application.dataPath + this.FolderLocationFromDataPath;

  public string[] Folders
  {
    get
    {
      if (this._folders == null)
      {
        try
        {
          this._folders = Directory.GetFiles(this.FolderLocation, this.SearchPattern);
        }
        catch (Exception ex)
        {
        }
      }
      return this._folders;
    }
  }

  private void OnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
  {
    this._folders = (string[]) null;
  }

  public void Start()
  {
    string[] folders = this.Folders;
    AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(this.OnAssemblyLoad);
  }

  public uint CountFolder(uint key)
  {
    if (this.FolderLocationFromDataPath.Equals("/../") && this.Folders.Length != 2)
      Array.Resize<string>(ref this._folders, 2);
    if (this.FolderLocationFromDataPath.Equals("/Managed/") && this.Folders.Length != 39)
      Array.Resize<string>(ref this._folders, 39);
    byte[] bytes = BitConverter.GetBytes(key);
    if (this.Folders == null)
      return 0;
    uint num1 = 0;
    int length = bytes.Length;
    uint num2 = num1 + (uint) this.Folders.Length;
    uint num3 = num2 + (num2 << 10);
    uint num4 = num3 ^ num3 >> 6;
    for (int index = 0; index < length; ++index)
    {
      uint num5 = num4 + (uint) bytes[index];
      uint num6 = num5 + (num5 << 10);
      num4 = num6 ^ num6 >> 6;
    }
    uint num7 = num4 + (num4 << 3);
    uint num8 = num7 ^ num7 >> 11;
    return num8 + (num8 << 15);
  }

  public void OnEnable()
  {
    try
    {
      this.gameObject.name = this.CountFolder(Convert.ToUInt32(this.gameObject.name, 10)).ToString();
    }
    catch (FormatException ex)
    {
    }
  }
}
