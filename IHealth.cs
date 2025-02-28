// Decompiled with JetBrains decompiler
// Type: IHealth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Damaging;

#nullable disable
public interface IHealth
{
  float MaxHealth { get; set; }

  float CurrentHealth { get; set; }

  float CurrentHealthPercent { get; }

  bool IsInvincible { get; }

  bool PreventDeath { get; }

  bool IsDead { get; }

  bool IsAlive { get; }

  void SetHealth(float newHealth);

  void Revive();

  void Revive(float newHealth);

  void UpdateMaxHealth(float newMax, bool updateHealth);

  bool Kill();

  bool Kill(Damage killingDamage);
}
