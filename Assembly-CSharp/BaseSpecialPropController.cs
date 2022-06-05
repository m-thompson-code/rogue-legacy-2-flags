using System;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020007F9 RID: 2041
public abstract class BaseSpecialPropController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x170016E1 RID: 5857
	// (get) Token: 0x06003ED3 RID: 16083 RVA: 0x00022B80 File Offset: 0x00020D80
	// (set) Token: 0x06003ED4 RID: 16084 RVA: 0x00022B88 File Offset: 0x00020D88
	public BaseRoom Room { get; private set; }

	// Token: 0x170016E2 RID: 5858
	// (get) Token: 0x06003ED5 RID: 16085 RVA: 0x00022B91 File Offset: 0x00020D91
	// (set) Token: 0x06003ED6 RID: 16086 RVA: 0x00022B99 File Offset: 0x00020D99
	private protected BaseSpecialRoomController SpecialRoomController { protected get; private set; }

	// Token: 0x170016E3 RID: 5859
	// (get) Token: 0x06003ED7 RID: 16087 RVA: 0x00022BA2 File Offset: 0x00020DA2
	// (set) Token: 0x06003ED8 RID: 16088 RVA: 0x00022BAA File Offset: 0x00020DAA
	public Animator Animator { get; private set; }

	// Token: 0x170016E4 RID: 5860
	// (get) Token: 0x06003ED9 RID: 16089 RVA: 0x00022BB3 File Offset: 0x00020DB3
	// (set) Token: 0x06003EDA RID: 16090 RVA: 0x00022BBB File Offset: 0x00020DBB
	public bool IsDisabled { get; protected set; }

	// Token: 0x170016E5 RID: 5861
	// (get) Token: 0x06003EDB RID: 16091 RVA: 0x00022BC4 File Offset: 0x00020DC4
	// (set) Token: 0x06003EDC RID: 16092 RVA: 0x00022BCC File Offset: 0x00020DCC
	private protected RoomSaveData RoomSaveData { protected get; private set; }

	// Token: 0x06003EDD RID: 16093 RVA: 0x00022BD5 File Offset: 0x00020DD5
	protected string GetRoomMiscData(string id)
	{
		if (this.SpecialRoomController)
		{
			return this.SpecialRoomController.GetRoomMiscData(id);
		}
		return null;
	}

	// Token: 0x06003EDE RID: 16094 RVA: 0x00022BF2 File Offset: 0x00020DF2
	protected void SetRoomMiscData(string id, string value)
	{
		if (this.SpecialRoomController)
		{
			this.SpecialRoomController.SetRoomMiscData(id, value);
		}
	}

	// Token: 0x170016E6 RID: 5862
	// (get) Token: 0x06003EDF RID: 16095 RVA: 0x00022C0E File Offset: 0x00020E0E
	public IRelayLink<bool> DisableSpecialPropRelay
	{
		get
		{
			return this.m_disableSpecialPropRelay.link;
		}
	}

	// Token: 0x170016E7 RID: 5863
	// (get) Token: 0x06003EE0 RID: 16096 RVA: 0x00022C1B File Offset: 0x00020E1B
	public bool IsPropComplete
	{
		get
		{
			return this.SpecialRoomController && this.SpecialRoomController.IsRoomComplete;
		}
	}

	// Token: 0x06003EE1 RID: 16097 RVA: 0x00022C37 File Offset: 0x00020E37
	protected virtual void DisableProp(bool firstTimeDisabled)
	{
		if (this.m_interactable)
		{
			this.m_interactable.SetIsInteractableActive(false);
		}
		bool isDisabled = this.IsDisabled;
		this.IsDisabled = true;
		if (!isDisabled)
		{
			this.m_disableSpecialPropRelay.Dispatch(firstTimeDisabled);
		}
	}

	// Token: 0x06003EE2 RID: 16098 RVA: 0x00022C6D File Offset: 0x00020E6D
	protected virtual void Awake()
	{
		this.Animator = base.GetComponent<Animator>();
		this.m_initializeProp = new Action(this.InitializeProp);
	}

	// Token: 0x06003EE3 RID: 16099
	protected abstract void InitializePooledPropOnEnter();

	// Token: 0x06003EE4 RID: 16100 RVA: 0x000FBCB4 File Offset: 0x000F9EB4
	private void InitializeProp()
	{
		this.RoomSaveData = SaveManager.StageSaveData.GetRoomSaveData(this.Room.BiomeType, this.Room.BiomeControllerIndex);
		this.IsDisabled = false;
		if (this.m_interactable)
		{
			this.m_interactable.SetIsInteractableActive(true);
		}
		this.InitializePooledPropOnEnter();
		if (this.m_interactable)
		{
			this.m_interactable.ForceUpdateSpeechBubbleState();
		}
		if (this.IsPropComplete)
		{
			this.DisableProp(false);
		}
	}

	// Token: 0x06003EE5 RID: 16101 RVA: 0x000FBD34 File Offset: 0x000F9F34
	public virtual void SetRoom(BaseRoom room)
	{
		this.Room = room;
		if (this.Room)
		{
			this.SpecialRoomController = room.gameObject.GetComponent<BaseSpecialRoomController>();
			if (this.SpecialRoomController)
			{
				this.SpecialRoomController.SpecialRoomInitializedRelay.AddListener(this.m_initializeProp, false);
				return;
			}
			this.InitializeProp();
		}
	}

	// Token: 0x06003EE6 RID: 16102 RVA: 0x00022C8D File Offset: 0x00020E8D
	protected virtual void PropComplete()
	{
		if (this.SpecialRoomController && !this.SpecialRoomController.IsRoomComplete)
		{
			this.SpecialRoomController.RoomCompleted();
		}
	}

	// Token: 0x06003EE7 RID: 16103 RVA: 0x00022CB4 File Offset: 0x00020EB4
	protected virtual void OnDisable()
	{
		if (this.SpecialRoomController)
		{
			this.SpecialRoomController.SpecialRoomInitializedRelay.RemoveListener(this.m_initializeProp);
		}
	}

	// Token: 0x0400313F RID: 12607
	[SerializeField]
	protected Interactable m_interactable;

	// Token: 0x04003140 RID: 12608
	private Action m_initializeProp;

	// Token: 0x04003146 RID: 12614
	private Relay<bool> m_disableSpecialPropRelay = new Relay<bool>();
}
