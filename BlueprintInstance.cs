// Decompiled with JetBrains decompiler
// Type: BlueprintInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Common.Attributes;
using CodeHatch.Engine;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Core.Consoles;
using CodeHatch.Engine.Core.Interaction;
using CodeHatch.Engine.Core.Interaction.Attributes;
using CodeHatch.Engine.Core.Interaction.Behaviours;
using CodeHatch.Engine.Core.Interaction.Players;
using CodeHatch.Engine.Modding.Abstract;
using CodeHatch.Engine.Modules.SocialSystem.Objects;
using CodeHatch.Engine.Networking;
using CodeHatch.Engine.Serialization;
using CodeHatch.Inventory.Blueprints;
using CodeHatch.ItemContainer;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Entities.Objects.Gadgets;
using CodeHatch.Thrones.Scripts.Modding;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BlueprintInstance : uLinkEntityBehaviour, IInteractable, IModInstance, ISerializable
{
  private const string MESSAGE_NO_SPACE = "Inventory is full! You do not have enough space to collect this item.";
  private static readonly Console.Message CONSOLE_MESSAGE_NO_SPACE = new Console.Message("Inventory is full! You do not have enough space to collect this item.", ConsoleSettings.DebugColor);
  [SerializeField]
  private InteractionState _InteractionDisabled;
  [SerializeField]
  private long _creationTimeStamp;
  [NoEdit]
  public bool IsFlagCollected;
  [SerializeField]
  private InvItemBlueprint _blueprint;
  private InvGameItemStack _containingStack;
  private Dictionary<string, IModable> _modablesByKey;

  public event SimpleEvent OnCollection;

  public long CreationTimeStamp
  {
    get => this._creationTimeStamp;
    set => this._creationTimeStamp = value;
  }

  public ISecurable ObjectSecurity => this.Entity.TryGet<ISecurable>();

  public InteractionState InteractionDisabled
  {
    get
    {
      return this.ObjectSecurity != null && !this.ObjectSecurity.IsLocalUseable() ? InteractionState.Locked : this._InteractionDisabled;
    }
    set => this._InteractionDisabled = value;
  }

  public bool IsolateInteraction => false;

  public string Identifier => this.Blueprint.Name + (object) this.Entity.Position;

  public void Awake()
  {
    if (this.CreationTimeStamp == 0L)
      this.CreationTimeStamp = SafeTime.ServerUpTicks;
    InteractableBehaviour.allInteractables.Add((IInteractable) this);
    ModManager.Instance.RegisterInstance((IModInstance) this);
  }

  public void OnDisabled() => this.IsFlagCollected = false;

  public void OnDespawned() => this.IsFlagCollected = false;

  public void OnDestroy()
  {
    ModManager.Instance.UnregisterInstance((IModInstance) this);
    InteractableBehaviour.allInteractables.Remove((IInteractable) this);
  }

  public void Serialize(IStream stream)
  {
    stream.WriteInt64(this.CreationTimeStamp);
    if (!this.HasEntity)
    {
      this.LogError<BlueprintInstance>("Unable to serialize {0} because it is not an entity.", (object) this.name);
      stream.WriteInt32(0);
    }
    else
    {
      ISerializable[] array = this.Entity.TryGetArray<ISerializable>();
      if (array == null)
      {
        stream.WriteInt32(0);
      }
      else
      {
        stream.WriteInt32(array.Length);
        for (int index = 0; index < array.Length; ++index)
        {
          ISerializable serializable = array[index];
          IStream stream1 = (IStream) null;
          try
          {
            stream1 = StreamPool.Instance.Get();
            stream1.WriteString(serializable.Identifier);
            if (serializable is BlueprintInstance)
            {
              stream.WriteStream(stream1);
            }
            else
            {
              serializable.Serialize(stream1);
              stream.WriteStream(stream1);
            }
          }
          catch (Exception ex)
          {
            this.LogError<BlueprintInstance>("An exception occured while serializing {0}", serializable == null ? (object) this.Blueprint.Name : (object) serializable.GetType().ToString());
            this.LogException<BlueprintInstance>(ex);
          }
          finally
          {
            if (stream1 != null)
              StreamPool.Instance.Release(stream1);
          }
        }
      }
    }
  }

  public void Deserialize(IStream stream)
  {
    this.CreationTimeStamp = stream.ReadInt64();
    int num = stream.ReadInt32();
    if (this.HasEntity)
    {
      ISerializable[] array = this.Entity.TryGetArray<ISerializable>();
      Dictionary<string, Queue<ISerializable>> dictionary = new Dictionary<string, Queue<ISerializable>>((IEqualityComparer<string>) new BlueprintInstance.StringEqualityComparer());
      for (int index = 0; index < array.Length; ++index)
      {
        ISerializable serializable = array[index];
        switch (serializable)
        {
          case null:
          case BlueprintInstance _:
            continue;
          default:
            string identifier = serializable.Identifier;
            Queue<ISerializable> serializableQueue;
            if (!dictionary.TryGetValue(identifier, out serializableQueue))
            {
              serializableQueue = new Queue<ISerializable>();
              dictionary[identifier] = serializableQueue;
            }
            serializableQueue.Enqueue(serializable);
            continue;
        }
      }
      for (int index = 0; index < num; ++index)
      {
        IStream stream1 = stream.ReadStream();
        string key = stream1.ReadString();
        ISerializable serializable = (ISerializable) null;
        if (dictionary.ContainsKey(key))
        {
          Queue<ISerializable> serializableQueue = dictionary[key];
          serializable = serializableQueue.Dequeue();
          if (serializableQueue.Count == 0)
            dictionary.Remove(key);
        }
        serializable?.Deserialize(stream1);
      }
    }
    else
      this.LogError<BlueprintInstance>("Unable to deserialize {0} because it is not an entity.", (object) this.GetType().Name);
  }

  [InteractableTutorial("Occupied", InteractionState.Occupied, "occupied_icon")]
  [Interact]
  [InteractableTutorial("(Hold)\nCollect Item", InteractionState.Enabled, "")]
  [Player(CodeHatch.Engine.Core.Interaction.Players.Key.PickUp, Gesture.Hold, false)]
  public virtual void Collect()
  {
    if (!ItemCollection.CanAutoMergeAdd(SingletonMonoBehaviour<InvEquipment>.Instance.Items, new InvGameItemStack(this.Blueprint, 1, (BlueprintInstance) null)))
      Console.AddMessage(BlueprintInstance.CONSOLE_MESSAGE_NO_SPACE);
    else
      EventManager.CallEvent((BaseEvent) new GadgetCollectEvent(Entity.LocalPlayer, this.Entity));
  }

  public void InvokeOnCollection()
  {
    SimpleEvent onCollection = this.OnCollection;
    if (onCollection == null)
      return;
    onCollection();
  }

  public InvItemBlueprint Blueprint
  {
    get
    {
      if ((UnityEngine.Object) this._blueprint == (UnityEngine.Object) null && Application.isPlaying)
        this.LogError<BlueprintInstance>("{0} has no blueprint set.", (object) this.name);
      return this._blueprint;
    }
    set => this._blueprint = value;
  }

  public InvGameItemStack ContainingStack
  {
    get
    {
      if (this._containingStack == null)
        this._containingStack = SingletonMonoBehaviour<InvEquipment>.Instance.GetInstanceStack(this);
      return this._containingStack;
    }
  }

  public virtual InvGameItemStack ConvertToStack()
  {
    if ((UnityEngine.Object) this.Blueprint == (UnityEngine.Object) null)
    {
      this.LogError<BlueprintInstance>("Blueprint == null");
      this.LogInfo<BlueprintInstance>("{0} is marked for destruction.", (object) this.gameObject.GetFullName());
      if ((UnityEngine.Object) this.networkView != (UnityEngine.Object) null)
        SingletonMonoBehaviour<InvEquipment>.Instance.MarkDestroyGameItemInstance(this);
      return (InvGameItemStack) null;
    }
    InvGameItemStack stack = new InvGameItemStack(this.Blueprint, 1, (BlueprintInstance) null);
    this.LogInfo<BlueprintInstance>("{0} is marked for destruction.", (object) this.gameObject.GetFullName());
    if ((UnityEngine.Object) this.networkView != (UnityEngine.Object) null)
      SingletonMonoBehaviour<InvEquipment>.Instance.MarkDestroyGameItemInstance(this);
    return stack;
  }

  public void SetupAsEquippable()
  {
    this.InteractionDisabled = InteractionState.Disabled;
    if (!this.gameObject.activeSelf)
      return;
    this.gameObject.SetActive(false);
  }

  public void ChildToInventory()
  {
    if (!this.HasEntity || !((UnityEngine.Object) SingletonMonoBehaviour<InvEquipment>.Instance != (UnityEngine.Object) null))
      return;
    this.transform.parent = SingletonMonoBehaviour<InvEquipment>.Instance.transform;
  }

  public void ChildToRoot()
  {
    if (!this.HasEntity || !((UnityEngine.Object) this.Entity.transform.parent == (UnityEngine.Object) SingletonMonoBehaviour<InvEquipment>.Instance.transform))
      return;
    this.Entity.transform.parent = this.transform.root.parent;
  }

  public string ModName => "Blueprints";

  public void ApplyMod(string key, object value)
  {
    this.GetAndCacheModables();
    IModable modable;
    if (!this._modablesByKey.TryGetValue(key, out modable))
      return;
    string privateModKey = BlueprintModHandler.GetPrivateModKey(this._blueprint, key);
    this.CallApplyMod(modable, privateModKey, value);
  }

  private void CallApplyMod(IModable modable, string privateKey, object value)
  {
    try
    {
      modable.ApplyMod(privateKey, value);
    }
    catch (Exception ex)
    {
      this.LogException<BlueprintInstance>(ex);
    }
  }

  private void GetAndCacheModables()
  {
    if (this._modablesByKey != null)
      return;
    this._modablesByKey = new Dictionary<string, IModable>();
    IModable[] array = this.Entity.GetArray<IModable>();
    List<ModEntry> defaultModEntries = new List<ModEntry>();
    for (int index = 0; index < array.Length; ++index)
    {
      IModable modable = array[index];
      if (modable != this)
      {
        int count = defaultModEntries.Count;
        this.CallGetModDefaults(modable, defaultModEntries);
        for (; count < defaultModEntries.Count; ++count)
        {
          ModEntry modEntry = defaultModEntries[count];
          string publicKey = BlueprintModHandler.GetPublicModKey(this._blueprint, modEntry.Key);
          modEntry.Key = publicKey;
          this.LogDebug<BlueprintInstance>("{0} instance cached the mod key {1} for {2}.", (Func<object[]>) (() => new object[3]
          {
            (object) this.name,
            (object) publicKey,
            (object) this.ModName
          }));
          this._modablesByKey.Add(publicKey, modable);
        }
      }
    }
  }

  private void CallGetModDefaults(IModable modable, List<ModEntry> defaultModEntries)
  {
    try
    {
      modable.GetModDefaults((IList<ModEntry>) defaultModEntries);
    }
    catch (Exception ex)
    {
      this.LogException<BlueprintInstance>(ex);
    }
  }

  public class StringEqualityComparer : IEqualityComparer<string>
  {
    public bool Equals(string x, string y) => x.Equals(y);

    public int GetHashCode(string obj) => obj.GetHashCode();
  }
}
