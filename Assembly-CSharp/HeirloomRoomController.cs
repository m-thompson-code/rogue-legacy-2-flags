using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000504 RID: 1284
public class HeirloomRoomController : BaseSpecialRoomController
{
	// Token: 0x170011C5 RID: 4549
	// (get) Token: 0x06002FF4 RID: 12276 RVA: 0x000A41A5 File Offset: 0x000A23A5
	public HeirloomType HeirloomType
	{
		get
		{
			return this.m_heirloomType;
		}
	}

	// Token: 0x170011C6 RID: 4550
	// (get) Token: 0x06002FF5 RID: 12277 RVA: 0x000A41AD File Offset: 0x000A23AD
	public TunnelSpawnController HeirloomTunnelSpawnController
	{
		get
		{
			return this.m_heirloomTunnelSpawnController;
		}
	}

	// Token: 0x170011C7 RID: 4551
	// (get) Token: 0x06002FF6 RID: 12278 RVA: 0x000A41B5 File Offset: 0x000A23B5
	public bool FaceRightAfterTeleport
	{
		get
		{
			return this.m_faceRightAfterTeleport;
		}
	}

	// Token: 0x170011C8 RID: 4552
	// (get) Token: 0x06002FF7 RID: 12279 RVA: 0x000A41BD File Offset: 0x000A23BD
	// (set) Token: 0x06002FF8 RID: 12280 RVA: 0x000A41C5 File Offset: 0x000A23C5
	public byte HeirloomDialogueIndex { get; set; }

	// Token: 0x170011C9 RID: 4553
	// (get) Token: 0x06002FF9 RID: 12281 RVA: 0x000A41CE File Offset: 0x000A23CE
	// (set) Token: 0x06002FFA RID: 12282 RVA: 0x000A41D6 File Offset: 0x000A23D6
	public bool HeirloomLockedRepeatDialogue { get; set; }

	// Token: 0x170011CA RID: 4554
	// (get) Token: 0x06002FFB RID: 12283 RVA: 0x000A41DF File Offset: 0x000A23DF
	public bool IsFinalRoom
	{
		get
		{
			return this.m_isFinalRoom;
		}
	}

	// Token: 0x06002FFC RID: 12284 RVA: 0x000A41E7 File Offset: 0x000A23E7
	protected override void Awake()
	{
		base.Awake();
		this.m_insightEventArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.None, false, 5f, null, null, null);
	}

	// Token: 0x06002FFD RID: 12285 RVA: 0x000A4204 File Offset: 0x000A2404
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
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

	// Token: 0x06002FFE RID: 12286 RVA: 0x000A434C File Offset: 0x000A254C
	public override void RoomCompleted()
	{
		base.RoomCompleted();
		this.RunInsightResolved();
		Debug.Log("You've been awarded the " + this.HeirloomType.ToString() + " heirloom!");
	}

	// Token: 0x06002FFF RID: 12287 RVA: 0x000A4390 File Offset: 0x000A2590
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

	// Token: 0x04002637 RID: 9783
	[SerializeField]
	private HeirloomType m_heirloomType;

	// Token: 0x04002638 RID: 9784
	[SerializeField]
	private bool m_isFinalRoom;

	// Token: 0x04002639 RID: 9785
	[SerializeField]
	private TunnelSpawnController m_heirloomTunnelSpawnController;

	// Token: 0x0400263A RID: 9786
	[SerializeField]
	private bool m_faceRightAfterTeleport;

	// Token: 0x0400263B RID: 9787
	private InsightObjectiveCompleteHUDEventArgs m_insightEventArgs;

	// Token: 0x0400263C RID: 9788
	private List<ParticleSystem> m_nonAllocParticleSystemList = new List<ParticleSystem>();
}
