// Decompiled with JetBrains decompiler
// Type: CodeBasedAssertionExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityTest;

#nullable disable
[IntegrationTest.SucceedWithAssertions]
[IntegrationTest.DynamicTest("ExampleIntegrationTests")]
public class CodeBasedAssertionExample : MonoBehaviour
{
  public float FloatField = 3f;
  public GameObject goReference;

  public void Awake()
  {
    IAssertionComponentConfigurator configurator;
    FloatComparer floatComparer = AssertionComponent.Create<FloatComparer>(out configurator, CheckMethod.Start | CheckMethod.Update, this.gameObject, "CodeBasedAssertionExample.FloatField", (object) 3f);
    configurator.UpdateCheckRepeatFrequency = 5;
    floatComparer.floatingPointError = 0.1;
    floatComparer.compareTypes = FloatComparer.CompareTypes.Equal;
    AssertionComponent.Create<ValueDoesNotChange>(CheckMethod.Start | CheckMethod.Update, this.gameObject, "CodeBasedAssertionExample.FloatField");
    this.transform.position = new Vector3(0.0f, 3f, 0.0f);
    AssertionComponent.Create<FloatComparer>(CheckMethod.Update, this.gameObject, "CodeBasedAssertionExample.FloatField", this.gameObject, "transform.position.y");
    this.goReference = this.gameObject;
    AssertionComponent.Create<GeneralComparer>(CheckMethod.Update, this.gameObject, "CodeBasedAssertionExample.goReference", (object) null).compareType = GeneralComparer.CompareType.ANotEqualsB;
  }
}
