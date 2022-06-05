using System;
using FMODUnity;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020004EC RID: 1260
public class BossEntranceRoomController : BaseSpecialRoomController
{
	// Token: 0x1700119F RID: 4511
	// (get) Token: 0x06002F33 RID: 12083 RVA: 0x000A114A File Offset: 0x0009F34A
	public IRelayLink<bool> DoorPartlyOpenedRelay
	{
		get
		{
			return this.m_doorPartlyOpenedRelay.link;
		}
	}

	// Token: 0x170011A0 RID: 4512
	// (get) Token: 0x06002F34 RID: 12084 RVA: 0x000A1157 File Offset: 0x0009F357
	public IRelayLink DoorOpenedRelay
	{
		get
		{
			return this.m_doorOpenedRelay.link;
		}
	}

	// Token: 0x170011A1 RID: 4513
	// (get) Token: 0x06002F35 RID: 12085 RVA: 0x000A1164 File Offset: 0x0009F364
	public IRelayLink<bool> DoorDestroyedRelay
	{
		get
		{
			return this.m_doorDestroyedRelay.link;
		}
	}

	// Token: 0x170011A2 RID: 4514
	// (get) Token: 0x06002F36 RID: 12086 RVA: 0x000A1171 File Offset: 0x0009F371
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

	// Token: 0x170011A3 RID: 4515
	// (get) Token: 0x06002F37 RID: 12087 RVA: 0x000A11A9 File Offset: 0x0009F3A9
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

	// Token: 0x06002F38 RID: 12088 RVA: 0x000A11D2 File Offset: 0x0009F3D2
	protected override void Awake()
	{
		base.Awake();
		this.m_displayNextMemory = new Action(this.DisplayNextMemory);
	}

	// Token: 0x06002F39 RID: 12089 RVA: 0x000A11EC File Offset: 0x0009F3EC
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

	// Token: 0x06002F3A RID: 12090 RVA: 0x000A1474 File Offset: 0x0009F674
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

	// Token: 0x06002F3B RID: 12091 RVA: 0x000A14BC File Offset: 0x0009F6BC
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

	// Token: 0x06002F3C RID: 12092 RVA: 0x000A1514 File Offset: 0x0009F714
	private void DisplayNextMemory()
	{
		this.m_memoryProps[this.m_memoryIndex].PropInstance.GetComponent<JournalSpecialPropController>().FinishedReadingRelay.RemoveListener(this.m_displayNextMemory);
		if (this.m_memoryIndex < this.m_memoryProps.Length - 1)
		{
			this.m_memoryIndex++;
			this.SetMemoryActive(this.m_memoryIndex);
		}
	}

	// Token: 0x06002F3D RID: 12093 RVA: 0x000A1578 File Offset: 0x0009F778
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

	// Token: 0x06002F3E RID: 12094 RVA: 0x000A1604 File Offset: 0x0009F804
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

	// Token: 0x06002F3F RID: 12095 RVA: 0x000A176D File Offset: 0x0009F96D
	protected virtual void OnDisable()
	{
		if (!BossEntranceRoomController.CutscenePlayed_DoNotDisableCrumble)
		{
			BossEntranceRoomController.RunDoorCrumbleAnimation = false;
		}
		BossEntranceRoomController.CutscenePlayed_DoNotDisableCrumble = false;
	}

	// Token: 0x04002599 RID: 9625
	public static bool RunDoorCrumbleAnimation;

	// Token: 0x0400259A RID: 9626
	protected static bool CutscenePlayed_DoNotDisableCrumble;

	// Token: 0x0400259B RID: 9627
	[SerializeField]
	protected TunnelSpawnController m_bossTunnel;

	// Token: 0x0400259C RID: 9628
	[SerializeField]
	[FormerlySerializedAs("m_firstTimeLockedFlagCheck")]
	protected PlayerSaveFlag m_bossBeatenFlag;

	// Token: 0x0400259D RID: 9629
	[SerializeField]
	protected TunnelSpawnController m_bossUpTunnel;

	// Token: 0x0400259E RID: 9630
	[SerializeField]
	private BurdenType m_bossUpBurden;

	// Token: 0x0400259F RID: 9631
	[SerializeField]
	private PropSpawnController[] m_memoryProps;

	// Token: 0x040025A0 RID: 9632
	protected ProximityEventController m_proximityEventController;

	// Token: 0x040025A1 RID: 9633
	protected Relay<bool> m_doorPartlyOpenedRelay = new Relay<bool>();

	// Token: 0x040025A2 RID: 9634
	private Action m_displayNextMemory;

	// Token: 0x040025A3 RID: 9635
	protected Relay m_doorOpenedRelay = new Relay();

	// Token: 0x040025A4 RID: 9636
	protected Relay<bool> m_doorDestroyedRelay = new Relay<bool>();

	// Token: 0x040025A5 RID: 9637
	private int m_memoryIndex;
}
