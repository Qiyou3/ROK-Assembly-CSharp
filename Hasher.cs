// Decompiled with JetBrains decompiler
// Type: Hasher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Security.Cryptography;
using System.Text;

#nullable disable
public class Hasher
{
  public static string EncryptString(string key, string data)
  {
    byte[] hash = new HMACSHA256(Encoding.ASCII.GetBytes(key)).ComputeHash(Encoding.ASCII.GetBytes(data));
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < hash.Length; ++index)
      stringBuilder.Append(hash[index].ToString("x2"));
    return stringBuilder.ToString();
  }
}
