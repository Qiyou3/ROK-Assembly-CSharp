// Decompiled with JetBrains decompiler
// Type: AlternativePath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using Pathfinding;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("Pathfinding/Modifiers/Alternative Path")]
[Serializable]
public class AlternativePath : MonoModifier
{
  public int penalty = 1000;
  public int randomStep = 10;
  private Pathfinding.Node[] prevNodes;
  private int prevSeed;
  private int prevPenalty;
  private bool waitingForApply;
  private object lockObject = new object();
  private System.Random rnd = new System.Random();
  private System.Random seedGenerator = new System.Random();
  private Pathfinding.Node[] toBeApplied;

  public override ModifierData input => ModifierData.Original;

  public override ModifierData output => ModifierData.All;

  public override void Apply(Path p, ModifierData source)
  {
    lock (this.lockObject)
    {
      this.toBeApplied = p.path.ToArray();
      if (this.waitingForApply)
        return;
      this.waitingForApply = true;
      AstarPath.OnPathPreSearch += new OnPathDelegate(this.ApplyNow);
    }
  }

  private void ApplyNow(Path somePath)
  {
    lock (this.lockObject)
    {
      this.waitingForApply = false;
      AstarPath.OnPathPreSearch -= new OnPathDelegate(this.ApplyNow);
      this.rnd = new System.Random(this.prevSeed);
      if (this.prevNodes != null)
      {
        bool flag = false;
        for (int index = this.rnd.Next(this.randomStep); index < this.prevNodes.Length; index += this.rnd.Next(1, this.randomStep))
        {
          if ((long) this.prevNodes[index].penalty < (long) this.prevPenalty)
            flag = true;
          this.prevNodes[index].penalty = (uint) Mathf.Max((float) ((long) this.prevNodes[index].penalty - (long) this.prevPenalty));
        }
        if (flag)
          this.LogWarning<AlternativePath>("Penalty for some nodes has been reset while this modifier was active. Penalties might not be correctly set.");
      }
      int Seed = this.seedGenerator.Next();
      this.rnd = new System.Random(Seed);
      if (this.toBeApplied != null)
      {
        for (int index = this.rnd.Next(this.randomStep); index < this.toBeApplied.Length; index += this.rnd.Next(1, this.randomStep))
          this.toBeApplied[index].penalty = (uint) ((ulong) this.toBeApplied[index].penalty + (ulong) this.penalty);
      }
      this.prevPenalty = this.penalty;
      this.prevSeed = Seed;
      this.prevNodes = this.toBeApplied;
    }
  }
}
