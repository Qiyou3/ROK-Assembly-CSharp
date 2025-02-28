// Decompiled with JetBrains decompiler
// Type: UnityTest.ComparerBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace UnityTest
{
  public abstract class ComparerBase : ActionBase
  {
    public ComparerBase.CompareToType compareToType;
    public GameObject other;
    protected object m_ObjOtherVal;
    public string otherPropertyPath = string.Empty;
    private MemberResolver m_MemberResolverB;

    protected abstract bool Compare(object a, object b);

    protected override bool Compare(object objValue)
    {
      if (this.compareToType == ComparerBase.CompareToType.CompareToConstantValue)
        this.m_ObjOtherVal = this.ConstValue;
      else if (this.compareToType == ComparerBase.CompareToType.CompareToNull)
        this.m_ObjOtherVal = (object) null;
      else if ((UnityEngine.Object) this.other == (UnityEngine.Object) null)
      {
        this.m_ObjOtherVal = (object) null;
      }
      else
      {
        if (this.m_MemberResolverB == null)
          this.m_MemberResolverB = new MemberResolver(this.other, this.otherPropertyPath);
        this.m_ObjOtherVal = this.m_MemberResolverB.GetValue(this.UseCache);
      }
      return this.Compare(objValue, this.m_ObjOtherVal);
    }

    public virtual System.Type[] GetAccepatbleTypesForB() => (System.Type[]) null;

    public virtual object ConstValue { get; set; }

    public virtual object GetDefaultConstValue() => throw new NotImplementedException();

    public override string GetFailureMessage()
    {
      string str = this.GetType().Name + " assertion failed.\n" + this.go.name + "." + this.thisPropertyPath + " " + (object) this.compareToType;
      switch (this.compareToType)
      {
        case ComparerBase.CompareToType.CompareToObject:
          str = str + " (" + (object) this.other + ")." + this.otherPropertyPath + " failed.";
          break;
        case ComparerBase.CompareToType.CompareToConstantValue:
          str = str + " " + this.ConstValue + " failed.";
          break;
        case ComparerBase.CompareToType.CompareToNull:
          str += " failed.";
          break;
      }
      return str + " Expected: " + this.m_ObjOtherVal + " Actual: " + this.m_ObjVal;
    }

    public enum CompareToType
    {
      CompareToObject,
      CompareToConstantValue,
      CompareToNull,
    }
  }
}
