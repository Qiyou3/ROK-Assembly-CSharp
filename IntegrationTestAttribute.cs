// Decompiled with JetBrains decompiler
// Type: IntegrationTestAttribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.IO;

#nullable disable
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class IntegrationTestAttribute : Attribute
{
  private readonly string m_Path;

  public IntegrationTestAttribute(string path)
  {
    if (path.EndsWith(".unity"))
      path = path.Substring(0, path.Length - ".unity".Length);
    this.m_Path = path;
  }

  public bool IncludeOnScene(string scenePath)
  {
    return scenePath == this.m_Path || Path.GetFileNameWithoutExtension(scenePath) == this.m_Path;
  }
}
