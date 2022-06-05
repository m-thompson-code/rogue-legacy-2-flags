using System;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020004CE RID: 1230
public abstract class BaseSpecialPropController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17001154 RID: 4436
	// (get) Token: 0x06002DB5 RID: 11701 RVA: 0x0009A43E File Offset: 0x0009863E
	// (set) Token: 0x06002DB6 RID: 11702 RVA: 0x0009A446 File Offset: 0x00098646
	public BaseRoom Room { get; private set; }

	// Token: 0x17001155 RID: 4437
	// (get) Token: 0x06002DB7 RID: 11703 RVA: 0x0009A44F File Offset: 0x0009864F
	// (set) Token: 0x06002DB8 RID: 11704 RVA: 0x0009A457 File Offset: 0x00098657
	private protected BaseSpecialRoomController SpecialRoomController { protected get; private set; }

	// Token: 0x17001156 RID: 4438
	// (get) Token: 0x06002DB9 RID: 11705 RVA: 0x0009A460 File Offset: 0x00098660
	// (set) Token: 0x06002DBA RID: 11706 RVA: 0x0009A468 File Offset: 0x00098668
	public Animator Animator { get; private set; }

	// Token: 0x17001157 RID: 4439
	// (get) Token: 0x06002DBB RID: 11707 RVA: 0x0009A471 File Offset: 0x00098671
	// (set) Token: 0x06002DBC RID: 11708 RVA: 0x0009A479 File Offset: 0x00098679
	public bool IsDisabled { get; protected set; }

	// Token: 0x17001158 RID: 4440
	// (get) Token: 0x06002DBD RID: 11709 RVA: 0x0009A482 File Offset: 0x00098682
	// (set) Token: 0x06002DBE RID: 11710 RVA: 0x0009A48A File Offset: 0x0009868A
	private protected RoomSaveData RoomSaveData { protected get; private set; }

	// Token: 0x06002DBF RID: 11711 RVA: 0x0009A493 File Offset: 0x00098693
	protected string GetRoomMiscData(string id)
	{
		if (this.SpecialRoomController)
		{
			return this.SpecialRoomController.GetRoomMiscData(id);
		}
		return null;
	}

	// Token: 0x06002DC0 RID: 11712 RVA: 0x0009A4B0 File Offset: 0x000986B0
	protected void SetRoomMiscData(string id, string value)
	{
		if (this.SpecialRoomController)
		{
			this.SpecialRoomController.SetRoomMiscData(id, value);
		}
	}

	// Token: 0x17001159 RID: 4441
	// (get) Token: 0x06002DC1 RID: 11713 RVA: 0x0009A4CC File Offset: 0x000986CC
	public IRelayLink<bool> DisableSpecialPropRelay
	{
		get
		{
			return this.m_disableSpecialPropRelay.link;
		}
	}

	// Token: 0x1700115A RID: 4442
	// (get) Token: 0x06002DC2 RID: 11714 RVA: 0x0009A4D9 File Offset: 0x000986D9
	public bool IsPropComplete
	{
		get
		{
			return this.SpecialRoomController && this.SpecialRoomController.IsRoomComplete;
		}
	}

	// Token: 0x06002DC3 RID: 11715 RVA: 0x0009A4F5 File Offset: 0x000986F5
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

	// Token: 0x06002DC4 RID: 11716 RVA: 0x0009A52B File Offset: 0x0009872B
	protected virtual void Awake()
	{
		this.Animator = base.GetComponent<Animator>();
		this.m_initializeProp = new Action(this.InitializeProp);
	}

	// Token: 0x06002DC5 RID: 11717
	protected abstract void InitializePooledPropOnEnter();

	// Token: 0x06002DC6 RID: 11718 RVA: 0x0009A54C File Offset: 0x0009874C
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

	// Token: 0x06002DC7 RID: 11719 RVA: 0x0009A5CC File Offset: 0x000987CC
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

	// Token: 0x06002DC8 RID: 11720 RVA: 0x0009A62A File Offset: 0x0009882A
	protected virtual void PropComplete()
	{
		if (this.SpecialRoomController && !this.SpecialRoomController.IsRoomComplete)
		{
			this.SpecialRoomController.RoomCompleted();
		}
	}

	// Token: 0x06002DC9 RID: 11721 RVA: 0x0009A651 File Offset: 0x00098851
	protected virtual void OnDisable()
	{
		if (this.SpecialRoomController)
		{
			this.SpecialRoomController.SpecialRoomInitializedRelay.RemoveListener(this.m_initializeProp);
		}
	}

	// Token: 0x04002497 RID: 9367
	[SerializeField]
	protected Interactable m_interactable;

	// Token: 0x04002498 RID: 9368
	private Action m_initializeProp;

	// Token: 0x0400249E RID: 9374
	private Relay<bool> m_disableSpecialPropRelay = new Relay<bool>();
}
