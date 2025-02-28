// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.CreateRandomBoxes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class CreateRandomBoxes : MonoBehaviour
  {
    public Bounds scaleRange;
    public Bounds bounds;
    public int numberOfCubes;

    public void Awake()
    {
      for (int index = 0; index < this.numberOfCubes; ++index)
      {
        Vector3 randomPointInBounds = this.scaleRange.GetRandomPointInBounds();
        Vector3 vector3 = this.bounds.GetRandomPointInBounds() + this.transform.position;
        GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
        primitive.transform.position = vector3;
        primitive.transform.localScale = randomPointInBounds;
      }
    }
  }
}
