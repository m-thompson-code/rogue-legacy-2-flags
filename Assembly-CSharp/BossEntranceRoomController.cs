using System;
using FMODUnity;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000841 RID: 2113
public class BossEntranceRoomController : BaseSpecialRoomController
{
	// Token: 0x17001778 RID: 6008
	// (get) Token: 0x06004135 RID: 16693 RVA: 0x00024097 File Offset: 0x00022297
	public IRelayLink<bool> DoorPartlyOpenedRelay
	{
		get
		{
			return this.m_doorPartlyOpenedRelay.link;
		}
	}

	// Token: 0x17001779 RID: 6009
	// (get) Token: 0x06004136 RID: 16694 RVA: 0x000240A4 File Offset: 0x000222A4
	public IRelayLink DoorOpenedRelay
	{
		get
		{
			return this.m_doorOpenedRelay.link;
		}
	}

	// Token: 0x1700177A RID: 6010
	// (get) Token: 0x06004137 RID: 16695 RVA: 0x000240B1 File Offset: 0x000222B1
	public IRelayLink<bool> DoorDestroyedRelay
	{
		get
		{
			return this.m_doorDestroyedRelay.link;
		}
	}

	// Token: 0x1700177B RID: 6011
	// (get) Token: 0x06004138 RID: 16696 RVA: 0x000240BE File Offset: 0x000222BE
	public BossTunnelState BossTunnelState
	{
		get
		{
			if (this.BossTunnelSpawner && this.BossTunnelSpawner.Tunnel)
			{
				return (this.BossTunnelSpawner.Tunnel as BossTunnel).TunnelState;
			}
			return BossTunnelState.Closed;
		}
	}

	// Token: 0x1700177C RID: 6012
	// (get) Token: 0x06004139 RID: 16697 RVA: 0x000240F6 File Offset: 0x000222F6
	public TunnelSpawnController BossTunnelSpawner
	{
		get
		{
			if (BurdenManager.IsBurdenActive(this.m_bossUpBurden) && this.m_bossUpTunnel)
			{
				return this.m_bossUpTunnel;
			}
			return this.m_bossTunnel;
		}
	}

	// Token: 0x0600413A RID: 16698 RVA: 0x0002411F File Offset: 0x0002231F
	protected override void Awake()
	{
		base.Awake();
		this.m_displayNextMemory = new Action(this.DisplayNextMemory);
	}

	// Token: 0x0600413B RID: 16699 RVA: 0x00105F60 File Offset: 0x00104160
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		if (CutsceneManager.IsCutsceneActive && BossEntranceRoomController.RunDoorCrumbleAnimation)
		{
			BossEntranceRoomController.CutscenePlayed_DoNotDisableCrumble = true;
		}
		if (this.m_bossUpTunnel)
		{
			if (BurdenManager.IsBurdenActive(this.m_bossUpBurden))
			{
				this.m_bossTunnel.Tunnel.SetIsLocked(true);
				this.m_bossTunnel.Tunnel.SetIsVisible(false);
				this.m_bossTunnel.Tunnel.Interactable.SetIsInteractableActive(false);
				this.m_bossUpTunnel.Tunnel.SetIsLocked(false);
				this.m_bossUpTunnel.Tunnel.SetIsVisible(true);
				this.m_bossUpTunnel.Tunnel.Interactable.SetIsInteractableActive(true);
			}
			else
			{
				this.m_bossTunnel.Tunnel.SetIsLocked(false);
				this.m_bossTunnel.Tunnel.SetIsVisible(true);
				this.m_bossTunnel.Tunnel.Interactable.SetIsInteractableActive(true);
				this.m_bossUpTunnel.Tunnel.SetIsLocked(true);
				this.m_bossUpTunnel.Tunnel.SetIsVisible(false);
				this.m_bossUpTunnel.Tunnel.Interactable.SetIsInteractableActive(false);
			}
		}
		foreach (PropSpawnController propSpawnController in this.m_memoryProps)
		{
			propSpawnController.PropInstance.Pivot.gameObject.SetActive(false);
			propSpawnController.PropInstance.GetComponent<StudioEventEmitter>().Stop();
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.CachedHealthOverride = 0f;
		playerController.CachedManaOverride = 0f;
		if (SaveManager.PlayerSaveData.GetFlag(this.m_bossBeatenFlag) && !BossEntranceRoomController.RunDoorCrumbleAnimation)
		{
			base.ForceRoomCompleted();
		}
		if (!base.IsRoomComplete)
		{
			if (SaveManager.PlayerSaveData.GetFlag(this.m_bossBeatenFlag))
			{
				this.RoomCompleted();
				this.SetMemoriesEnabled();
			}
			else
			{
				this.SetBossTunnelState(BossTunnelState.PartlyOpen, true);
			}
		}
		else
		{
			this.SetBossTunnelState(BossTunnelState.Destroyed, true);
			this.SetMemoriesEnabled();
		}
		if (SaveManager.PlayerSaveData.GetInsightState(InsightType.FinalDoorEntranceObjective) < InsightState.ResolvedButNotViewed && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CastleBoss_Defeated) && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.BridgeBoss_Defeated) && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.ForestBoss_Defeated) && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.StudyBoss_Defeated) && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.TowerBoss_Defeated) && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveBoss_Defeated))
		{
			SaveManager.PlayerSaveData.SetInsightState(InsightType.FinalDoorEntranceObjective, InsightState.ResolvedButNotViewed, false);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, new InsightObjectiveCompleteHUDEventArgs(InsightType.FinalDoorEntranceObjective, false, 5f, null, null, null));
		}
	}

	// Token: 0x0600413C RID: 16700 RVA: 0x001061E8 File Offset: 0x001043E8
	private void SetMemoriesEnabled()
	{
		if (BurdenManager.IsBurdenActive(this.m_bossUpBurden) && !this.m_memoryProps.IsNativeNull() && this.m_memoryProps.Length != 0)
		{
			for (int i = 0; i <= this.m_memoryIndex; i++)
			{
				this.SetMemoryActive(i);
			}
		}
	}

	// Token: 0x0600413D RID: 16701 RVA: 0x00106230 File Offset: 0x00104430
	private void SetMemoryActive(int index)
	{
		Prop propInstance = this.m_memoryProps[index].PropInstance;
		propInstance.Pivot.gameObject.SetActive(true);
		propInstance.GetComponent<StudioEventEmitter>().Play();
		if (index == this.m_memoryIndex)
		{
			propInstance.GetComponent<JournalSpecialPropController>().FinishedReadingRelay.AddListener(this.m_displayNextMemory, false);
		}
	}

	// Token: 0x0600413E RID: 16702 RVA: 0x00106288 File Offset: 0x00104488
	private void DisplayNextMemory()
	{
		this.m_memoryProps[this.m_memoryIndex].PropInstance.GetComponent<JournalSpecialPropController>().FinishedReadingRelay.RemoveListener(this.m_displayNextMemory);
		if (this.m_memoryIndex < this.m_memoryProps.Length - 1)
		{
			this.m_memoryIndex++;
			this.SetMemoryActive(this.m_memoryIndex);
		}
	}

	// Token: 0x0600413F RID: 16703 RVA: 0x001062EC File Offset: 0x001044EC
	public override void RoomCompleted()
	{
		base.RoomCompleted();
		BossEntranceRoomController.RunDoorCrumbleAnimation = false;
		this.SetBossTunnelState(BossTunnelState.Destroyed, false);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayPlayerHUD, null, null);
		if (this.BossTunnelSpawner && this.BossTunnelSpawner.Tunnel)
		{
			if (!this.m_proximityEventController)
			{
				this.m_proximityEventController = this.BossTunnelSpawner.Tunnel.GameObject.GetComponent<ProximityEventController>();
			}
			if (this.m_proximityEventController)
			{
				this.m_proximityEventController.enabled = false;
			}
		}
	}

	// Token: 0x06004140 RID: 16704 RVA: 0x00106378 File Offset: 0x00104578
	public virtual void SetBossTunnelState(BossTunnelState state, bool skipToIdleState)
	{
		Animator animator = this.BossTunnelSpawner.Tunnel.Animator;
		(this.BossTunnelSpawner.Tunnel as BossTunnel).TunnelState = state;
		animator.SetBool("Closed", false);
		animator.SetBool("PartlyOpen", false);
		animator.SetBool("Open", false);
		animator.SetBool("Destroyed", false);
		string text = null;
		switch (state)
		{
		case BossTunnelState.Closed:
			text = "Closed";
			this.BossTunnelSpawner.Tunnel.SetIsLocked(true);
			this.BossTunnelSpawner.Tunnel.Interactable.SetIsInteractableActive(true);
			break;
		case BossTunnelState.PartlyOpen:
			this.m_doorPartlyOpenedRelay.Dispatch(skipToIdleState);
			text = "PartlyOpen";
			this.BossTunnelSpawner.Tunnel.SetIsLocked(false);
			break;
		case BossTunnelState.Open:
			this.m_doorOpenedRelay.Dispatch();
			text = "Open";
			this.BossTunnelSpawner.Tunnel.SetIsLocked(false);
			break;
		case BossTunnelState.Destroyed:
			this.m_doorDestroyedRelay.Dispatch(skipToIdleState);
			text = "Destroyed";
			this.BossTunnelSpawner.Tunnel.SetIsLocked(true);
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			animator.SetBool(text, true);
			if (skipToIdleState)
			{
				animator.Update(0f);
				animator.Update(0f);
				animator.Play(text + "Idle");
				return;
			}
			RumbleManager.StartRumble(true, true, 0.5f, 0.5f, true);
		}
	}

	// Token: 0x06004141 RID: 16705 RVA: 0x00024139 File Offset: 0x00022339
	protected virtual void OnDisable()
	{
		if (!BossEntranceRoomController.CutscenePlayed_DoNotDisableCrumble)
		{
			BossEntranceRoomController.RunDoorCrumbleAnimation = false;
		}
		BossEntranceRoomController.CutscenePlayed_DoNotDisableCrumble = false;
	}

	// Token: 0x04003303 RID: 13059
	public static bool RunDoorCrumbleAnimation;

	// Token: 0x04003304 RID: 13060
	protected static bool CutscenePlayed_DoNotDisableCrumble;

	// Token: 0x04003305 RID: 13061
	[SerializeField]
	protected TunnelSpawnController m_bossTunnel;

	// Token: 0x04003306 RID: 13062
	[SerializeField]
	[FormerlySerializedAs("m_firstTimeLockedFlagCheck")]
	protected PlayerSaveFlag m_bossBeatenFlag;

	// Token: 0x04003307 RID: 13063
	[SerializeField]
	protected TunnelSpawnController m_bossUpTunnel;

	// Token: 0x04003308 RID: 13064
	[SerializeField]
	private BurdenType m_bossUpBurden;

	// Token: 0x04003309 RID: 13065
	[SerializeField]
	private PropSpawnController[] m_memoryProps;

	// Token: 0x0400330A RID: 13066
	protected ProximityEventController m_proximityEventController;

	// Token: 0x0400330B RID: 13067
	protected Relay<bool> m_doorPartlyOpenedRelay = new Relay<bool>();

	// Token: 0x0400330C RID: 13068
	private Action m_displayNextMemory;

	// Token: 0x0400330D RID: 13069
	protected Relay m_doorOpenedRelay = new Relay();

	// Token: 0x0400330E RID: 13070
	protected Relay<bool> m_doorDestroyedRelay = new Relay<bool>();

	// Token: 0x0400330F RID: 13071
	private int m_memoryIndex;
}
