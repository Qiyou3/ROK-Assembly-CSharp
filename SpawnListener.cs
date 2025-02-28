// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.SpawnListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Spawning;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class SpawnListener : MonoBehaviour
  {
    private DynamicSpawner mSpawner;

    public void OnEnable()
    {
      this.mSpawner = this.gameObject.GetComponent<DynamicSpawner>();
      if (!((Object) this.mSpawner == (Object) null))
        return;
      this.LogError<SpawnListener>("SpawnListener could not find the dynamic spawner.");
    }

    public void OnDisable()
    {
    }
  }
}
