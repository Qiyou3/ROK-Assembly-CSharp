// Decompiled with JetBrains decompiler
// Type: BaseContactSensor`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public abstract class BaseContactSensor<ContactType> where ContactType : BaseContact, new()
{
  public List<ContactType> contactsRead = new List<ContactType>();
  public List<ContactType> contactsWrite = new List<ContactType>();
  public bool contactPointFiltering = true;
  public float contactSmoothingTime = 0.5f;
  public float contactSustainTime = 0.5f;
  public float tractionThreshold = 0.01f;

  public virtual bool ContactClustering(ContactType current, RaycastHit raycastHit)
  {
    return !((Object) current.collider != (Object) raycastHit.collider);
  }

  public virtual void ContactBlending(ContactType contactType, RaycastHit raycastHit)
  {
    Vector3 point = raycastHit.point;
    Vector3 normal = raycastHit.normal;
    contactType.lastPosition = point;
    float num = 1f - Mathf.Pow(0.5f, Time.deltaTime / this.contactSmoothingTime);
    // ISSUE: variable of a boxed type
    __Boxed<ContactType> local1 = (object) contactType;
    local1.position = local1.position + (point * 1f - contactType.position) * num;
    // ISSUE: variable of a boxed type
    __Boxed<ContactType> local2 = (object) contactType;
    local2.normal = local2.normal + (normal * 1f - contactType.normal) * num;
    ((object) contactType).traction += (1f - contactType.traction) * num;
  }

  public virtual void ContactInit(ContactType contactType, RaycastHit raycastHit)
  {
    contactType.collider = raycastHit.collider;
    Vector3 point = raycastHit.point;
    Vector3 normal = raycastHit.normal;
    contactType.lastPosition = point;
    contactType.position = point;
    contactType.normal = normal;
    contactType.velocity = Vector3.zero;
    contactType.traction = 1f;
  }

  public virtual void ContactDecay(ContactType contactType)
  {
    float num = Mathf.Pow(0.5f, Time.deltaTime / this.contactSustainTime);
    // ISSUE: variable of a boxed type
    __Boxed<ContactType> local1 = (object) contactType;
    local1.position = local1.position * num;
    // ISSUE: variable of a boxed type
    __Boxed<ContactType> local2 = (object) contactType;
    local2.normal = local2.normal * num;
    // ISSUE: variable of a boxed type
    __Boxed<ContactType> local3 = (object) contactType;
    local3.velocity = local3.velocity * num;
    ((object) contactType).traction *= num;
  }

  public void RegisterContact(RaycastHit raycastHit)
  {
    if ((Object) raycastHit.collider == (Object) null)
    {
      this.LogError<BaseContactSensor<ContactType>>("collider == null");
    }
    else
    {
      ContactType contactType1 = (ContactType) null;
      if (this.contactsRead.Count > 0 && this.contactPointFiltering)
      {
        foreach (ContactType current in this.contactsRead)
        {
          if (this.ContactClustering(current, raycastHit))
            contactType1 = current;
        }
      }
      if ((object) contactType1 != null)
      {
        this.ContactBlending(contactType1, raycastHit);
        contactType1.inContact = true;
      }
      else
      {
        ContactType contactType2 = new ContactType();
        this.ContactInit(contactType2, raycastHit);
        this.contactsRead.Add(contactType2);
        contactType2.inContact = true;
      }
    }
  }

  public void UpdateContacts()
  {
    if (this.contactPointFiltering)
    {
      if (this.contactsRead.Count <= 0)
        return;
      foreach (ContactType contactType in this.contactsRead)
      {
        if (this.contactPointFiltering || contactType.inContact)
        {
          if (!contactType.inContact)
            this.ContactDecay(contactType);
          else
            contactType.inContact = false;
          if ((double) contactType.traction >= (double) this.tractionThreshold)
            this.contactsWrite.Add(contactType);
        }
      }
      List<ContactType> contactsRead = this.contactsRead;
      this.contactsRead.Clear();
      this.contactsRead = this.contactsWrite;
      this.contactsWrite = contactsRead;
    }
    else
      this.contactsRead.Clear();
  }
}
