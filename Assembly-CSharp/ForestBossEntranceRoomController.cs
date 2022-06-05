using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000866 RID: 2150
public class ForestBossEntranceRoomController : BossEntranceRoomController
{
	// Token: 0x0600424A RID: 16970 RVA: 0x00024B3B File Offset: 0x00022D3B
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_insightEventArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.None, false, 5f, null, null, null);
	}

	// Token: 0x0600424B RID: 16971 RVA: 0x0010A4CC File Offset: 0x001086CC
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

	// Token: 0x0600424C RID: 16972 RVA: 0x00024B69 File Offset: 0x00022D69
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

	// Token: 0x0600424D RID: 16973 RVA: 0x00024B78 File Offset: 0x00022D78
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

	// Token: 0x040033E7 RID: 13287
	[SerializeField]
	private Transform m_cameraMovementObj;

	// Token: 0x040033E8 RID: 13288
	private InsightObjectiveCompleteHUDEventArgs m_insightEventArgs;

	// Token: 0x040033E9 RID: 13289
	private WaitRL_Yield m_waitYield;

	// Token: 0x040033EA RID: 13290
	private bool m_openDoorCutsceneRunning;
}
