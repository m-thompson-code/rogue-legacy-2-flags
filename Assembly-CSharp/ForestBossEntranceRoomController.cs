using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004FF RID: 1279
public class ForestBossEntranceRoomController : BossEntranceRoomController
{
	// Token: 0x06002FDC RID: 12252 RVA: 0x000A3D09 File Offset: 0x000A1F09
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_insightEventArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.None, false, 5f, null, null, null);
	}

	// Token: 0x06002FDD RID: 12253 RVA: 0x000A3D38 File Offset: 0x000A1F38
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		int num = 0;
		num += SaveManager.PlayerSaveData.GetRelic(RelicType.Lily1).Level;
		num += SaveManager.PlayerSaveData.GetRelic(RelicType.Lily2).Level;
		num += SaveManager.PlayerSaveData.GetRelic(RelicType.Lily3).Level;
		if (!base.IsRoomComplete)
		{
			if (SaveManager.PlayerSaveData.GetInsightState(InsightType.ForestBoss_DoorOpened) >= InsightState.ResolvedButNotViewed)
			{
				this.SetBossTunnelState(BossTunnelState.PartlyOpen, true);
				return;
			}
			if (num >= 2)
			{
				base.StartCoroutine(this.OpenDoorCoroutine());
				return;
			}
			this.SetBossTunnelState(BossTunnelState.Closed, true);
		}
	}

	// Token: 0x06002FDE RID: 12254 RVA: 0x000A3DD1 File Offset: 0x000A1FD1
	private IEnumerator OpenDoorCoroutine()
	{
		this.m_openDoorCutsceneRunning = true;
		GlobalTeleporterController.StayInCutsceneAfterTeleporting = true;
		RewiredMapController.SetIsInCutscene(true);
		RewiredMapController.SetCurrentMapEnabled(false);
		this.SetBossTunnelState(BossTunnelState.Closed, true);
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		PlayerController playerController = PlayerManager.GetPlayerController();
		Vector3 localPosition = playerController.transform.localPosition;
		this.m_cameraMovementObj.position = localPosition;
		base.Room.CinemachineCamera.SetFollowTarget(this.m_cameraMovementObj);
		yield return TweenManager.TweenTo(this.m_cameraMovementObj, 2f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"position.x",
			base.BossTunnelSpawner.transform.position.x,
			"position.y",
			base.BossTunnelSpawner.transform.position.y
		}).TweenCoroutine;
		this.m_waitYield.CreateNew(1.5f, false);
		yield return this.m_waitYield;
		this.SetBossTunnelState(BossTunnelState.PartlyOpen, false);
		this.m_waitYield.CreateNew(1.5f, false);
		yield return this.m_waitYield;
		yield return TweenManager.TweenTo(this.m_cameraMovementObj, 2f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"position.x",
			playerController.transform.localPosition.x,
			"position.y",
			playerController.transform.localPosition.y
		}).TweenCoroutine;
		base.Room.CinemachineCamera.SetFollowTarget(playerController.FollowTargetGO.transform);
		this.m_waitYield.CreateNew(1f, false);
		yield return this.m_waitYield;
		if (SaveManager.PlayerSaveData.GetInsightState(InsightType.ForestBoss_DoorOpened) < InsightState.ResolvedButNotViewed)
		{
			SaveManager.PlayerSaveData.SetInsightState(InsightType.ForestBoss_DoorOpened, InsightState.ResolvedButNotViewed, false);
			this.m_insightEventArgs.Initialize(InsightType.ForestBoss_DoorOpened, false, 5f, null, null, null);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_insightEventArgs);
		}
		RewiredMapController.SetIsInCutscene(false);
		RewiredMapController.SetCurrentMapEnabled(true);
		GlobalTeleporterController.StayInCutsceneAfterTeleporting = false;
		this.m_openDoorCutsceneRunning = false;
		yield break;
	}

	// Token: 0x06002FDF RID: 12255 RVA: 0x000A3DE0 File Offset: 0x000A1FE0
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_openDoorCutsceneRunning && !GameManager.IsApplicationClosing)
		{
			RewiredMapController.SetIsInCutscene(false);
			RewiredMapController.SetCurrentMapEnabled(true);
			GlobalTeleporterController.StayInCutsceneAfterTeleporting = false;
			this.m_openDoorCutsceneRunning = false;
		}
	}

	// Token: 0x0400262D RID: 9773
	[SerializeField]
	private Transform m_cameraMovementObj;

	// Token: 0x0400262E RID: 9774
	private InsightObjectiveCompleteHUDEventArgs m_insightEventArgs;

	// Token: 0x0400262F RID: 9775
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002630 RID: 9776
	private bool m_openDoorCutsceneRunning;
}
