using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000871 RID: 2161
public class HeirloomRoomController : BaseSpecialRoomController
{
	// Token: 0x170017CC RID: 6092
	// (get) Token: 0x06004286 RID: 17030 RVA: 0x00024CDE File Offset: 0x00022EDE
	public HeirloomType HeirloomType
	{
		get
		{
			return this.m_heirloomType;
		}
	}

	// Token: 0x170017CD RID: 6093
	// (get) Token: 0x06004287 RID: 17031 RVA: 0x00024CE6 File Offset: 0x00022EE6
	public TunnelSpawnController HeirloomTunnelSpawnController
	{
		get
		{
			return this.m_heirloomTunnelSpawnController;
		}
	}

	// Token: 0x170017CE RID: 6094
	// (get) Token: 0x06004288 RID: 17032 RVA: 0x00024CEE File Offset: 0x00022EEE
	public bool FaceRightAfterTeleport
	{
		get
		{
			return this.m_faceRightAfterTeleport;
		}
	}

	// Token: 0x170017CF RID: 6095
	// (get) Token: 0x06004289 RID: 17033 RVA: 0x00024CF6 File Offset: 0x00022EF6
	// (set) Token: 0x0600428A RID: 17034 RVA: 0x00024CFE File Offset: 0x00022EFE
	public byte HeirloomDialogueIndex { get; set; }

	// Token: 0x170017D0 RID: 6096
	// (get) Token: 0x0600428B RID: 17035 RVA: 0x00024D07 File Offset: 0x00022F07
	// (set) Token: 0x0600428C RID: 17036 RVA: 0x00024D0F File Offset: 0x00022F0F
	public bool HeirloomLockedRepeatDialogue { get; set; }

	// Token: 0x170017D1 RID: 6097
	// (get) Token: 0x0600428D RID: 17037 RVA: 0x00024D18 File Offset: 0x00022F18
	public bool IsFinalRoom
	{
		get
		{
			return this.m_isFinalRoom;
		}
	}

	// Token: 0x0600428E RID: 17038 RVA: 0x00024D20 File Offset: 0x00022F20
	protected override void Awake()
	{
		base.Awake();
		this.m_insightEventArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.None, false, 5f, null, null, null);
	}

	// Token: 0x0600428F RID: 17039 RVA: 0x0010AE94 File Offset: 0x00109094
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		if (MainMenuWindowController.splitStep <= 25 && this.HeirloomType == HeirloomType.UnlockEarthShift)
		{
			MainMenuWindowController.splitStep = 26;
		}
		else if (MainMenuWindowController.splitStep <= 13 && this.HeirloomType == HeirloomType.UnlockVoidDash)
		{
			MainMenuWindowController.splitStep = 14;
		}
		else if (MainMenuWindowController.splitStep <= 9 && this.HeirloomType == HeirloomType.UnlockDoubleJump)
		{
			MainMenuWindowController.splitStep = 10;
		}
		else if (MainMenuWindowController.splitStep <= 5 && this.HeirloomType == HeirloomType.UnlockBouncableDownstrike)
		{
			MainMenuWindowController.splitStep = 6;
		}
		else if (MainMenuWindowController.splitStep == 234234234 && this.HeirloomType == HeirloomType.UnlockAirDash)
		{
			MainMenuWindowController.splitStep = 2;
		}
		if (!this.IsFinalRoom)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			playerController.CachedHealthOverride = 0f;
			playerController.CachedManaOverride = 0f;
		}
		if (!this.IsFinalRoom)
		{
			SaveManager.PlayerSaveData.TemporaryHeirloomList.Clear();
		}
		if (!base.IsRoomComplete && SaveManager.PlayerSaveData.GetHeirloomLevel(this.HeirloomType) > 0 && !this.IsFinalRoom)
		{
			this.RoomCompleted();
		}
		if (base.IsRoomComplete)
		{
			foreach (ISpawnController spawnController in base.Room.SpawnControllerManager.SpawnControllers)
			{
				if (spawnController.ShouldSpawn)
				{
					PropSpawnController propSpawnController = spawnController as PropSpawnController;
					if (!(propSpawnController == null) && !(propSpawnController.PropInstance == null) && propSpawnController.PropInstance.name.StartsWith("Heirloom"))
					{
						propSpawnController.PropInstance.gameObject.GetComponentsInChildren<ParticleSystem>(false, this.m_nonAllocParticleSystemList);
						foreach (ParticleSystem particleSystem in this.m_nonAllocParticleSystemList)
						{
							particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
						}
					}
				}
			}
		}
	}

	// Token: 0x06004290 RID: 17040 RVA: 0x0010B068 File Offset: 0x00109268
	public override void RoomCompleted()
	{
		base.RoomCompleted();
		this.RunInsightResolved();
		Debug.Log("You've been awarded the " + this.HeirloomType.ToString() + " heirloom!");
	}

	// Token: 0x06004291 RID: 17041 RVA: 0x0010B0AC File Offset: 0x001092AC
	private void RunInsightResolved()
	{
		InsightType insightFromHeirloomType = InsightType_RL.GetInsightFromHeirloomType(this.HeirloomType);
		if (insightFromHeirloomType != InsightType.None)
		{
			if (SaveManager.PlayerSaveData.GetInsightState(insightFromHeirloomType) < InsightState.ResolvedButNotViewed)
			{
				this.m_insightEventArgs.Initialize(insightFromHeirloomType, false, 5f, null, null, null);
				SaveManager.PlayerSaveData.SetInsightState(insightFromHeirloomType, InsightState.ResolvedButNotViewed, false);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_insightEventArgs);
				return;
			}
		}
		else
		{
			Debug.Log("<color=yellow>Could not run 'Insight Resolved' from Heirloom type: " + this.HeirloomType.ToString() + "</color>");
		}
	}

	// Token: 0x04003404 RID: 13316
	[SerializeField]
	private HeirloomType m_heirloomType;

	// Token: 0x04003405 RID: 13317
	[SerializeField]
	private bool m_isFinalRoom;

	// Token: 0x04003406 RID: 13318
	[SerializeField]
	private TunnelSpawnController m_heirloomTunnelSpawnController;

	// Token: 0x04003407 RID: 13319
	[SerializeField]
	private bool m_faceRightAfterTeleport;

	// Token: 0x04003408 RID: 13320
	private InsightObjectiveCompleteHUDEventArgs m_insightEventArgs;

	// Token: 0x04003409 RID: 13321
	private List<ParticleSystem> m_nonAllocParticleSystemList = new List<ParticleSystem>();
}
