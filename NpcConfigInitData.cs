// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.NpcConfigInitData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Networking;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class NpcConfigInitData : BaseInitData
  {
    [Designer]
    public float maxHealth;
    [Designer]
    public float moveSpeed;
    [Designer]
    public float attackForce;
    [Designer]
    public Color color;
    [Designer]
    public bool autoTargetOnSpawn;

    public override void Serialize(IStream bitStream)
    {
      bitStream.Write<float>(this.maxHealth);
      bitStream.Write<float>(this.moveSpeed);
      bitStream.Write<float>(this.attackForce);
      bitStream.Write<Color>(this.color);
      bitStream.Write<bool>(this.autoTargetOnSpawn);
    }

    public override void Deserialize(IStream bitStream)
    {
      this.maxHealth = bitStream.Read<float>();
      this.moveSpeed = bitStream.Read<float>();
      this.attackForce = bitStream.Read<float>();
      this.color = bitStream.Read<Color>();
      this.autoTargetOnSpawn = bitStream.Read<bool>();
    }
  }
}
