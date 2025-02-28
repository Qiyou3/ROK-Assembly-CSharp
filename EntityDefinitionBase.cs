// Decompiled with JetBrains decompiler
// Type: EntityDefinitionBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Cache;
using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using System;
using UnityEngine;

#nullable disable
public abstract class EntityDefinitionBase : EntityBehaviour
{
  private bool _issuesReported;
  private string _issues;
  private DebugMessage.MessageType _issueSeverity;
  private bool _isValidated;
  private bool _validating;

  public void Start() => this.EnsureValid();

  public void EnsureValid(bool isValid)
  {
    if (this._isValidated && isValid)
      return;
    this.InternalValidate();
    this._isValidated = true;
  }

  public void EnsureValid(object obj)
  {
    if (this._isValidated && !ReferenceUtil.IsObjectNull(obj))
      return;
    this.InternalValidate();
    this._isValidated = true;
  }

  public void EnsureValid()
  {
    if (this._isValidated)
      return;
    this.InternalValidate();
    this._isValidated = true;
  }

  [ContextMenu("Validate")]
  private void InternalValidate()
  {
    try
    {
      this._validating = !this._validating ? true : throw new Exception("Recursion detected. This should not happen. Breaking.");
      this.Validate();
    }
    catch (Exception ex)
    {
      this.LogException<EntityDefinitionBase>(ex);
    }
    this._validating = false;
    this.ReportIssuesIfAny();
  }

  protected abstract void Validate();

  private void ReportIssuesIfAny()
  {
    if (!this._issuesReported)
      return;
    if (string.IsNullOrEmpty(this._issues) && this._issueSeverity != DebugMessage.MessageType.Normal)
      this._issues = this._issueSeverity.ToString();
    if (!string.IsNullOrEmpty(this._issues))
    {
      string str = string.Format("There were some issues with the attached {0}:\n", (object) this.GetType().Name);
      switch (this._issueSeverity)
      {
        case DebugMessage.MessageType.Normal:
          this.LogInfo<EntityDefinitionBase>(str + this._issues, (object) this);
          break;
        case DebugMessage.MessageType.Warning:
          this.LogWarning<EntityDefinitionBase>(str + this._issues, (object) this);
          break;
        case DebugMessage.MessageType.Error:
          this.LogError<EntityDefinitionBase>(str + this._issues, (object) this);
          break;
      }
    }
    this._issuesReported = false;
    this._issues = (string) null;
    this._issueSeverity = DebugMessage.MessageType.Normal;
  }

  protected void RecordIssue(string issue, DebugMessage.MessageType issueType)
  {
    this._issuesReported = true;
    switch (issueType)
    {
      case DebugMessage.MessageType.Normal:
        this._issues += string.Format("{0}\n", (object) issue);
        break;
      case DebugMessage.MessageType.Warning:
        this._issues += string.Format("<color=yellow>{0}</color>\n", (object) issue);
        break;
      case DebugMessage.MessageType.Error:
        this._issues += string.Format("<color=red>{0}</color>\n", (object) issue);
        break;
    }
    if (!this.FirstMoreSevereThanSecond(issueType, this._issueSeverity))
      return;
    this._issueSeverity = issueType;
  }

  private bool FirstMoreSevereThanSecond(
    DebugMessage.MessageType first,
    DebugMessage.MessageType second)
  {
    return first > second;
  }
}
