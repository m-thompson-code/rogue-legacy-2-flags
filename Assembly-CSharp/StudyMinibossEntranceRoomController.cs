using System;
using UnityEngine;

// Token: 0x02000511 RID: 1297
public class StudyMinibossEntranceRoomController : BaseSpecialRoomController
{
	// Token: 0x0600302A RID: 12330 RVA: 0x000A4C90 File Offset: 0x000A2E90
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
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
				this.m_bossTunnel.Tunnel.Animator.SetTrigger("Disable");
				this.m_bossTunnel.Tunnel.SetIsLocked(true);
				this.RoomCompleted();
			}
		}
		else
		{
			this.m_bossTunnel.Tunnel.Animator.Play("DisabledIdle");
			this.m_bossTunnel.Tunnel.SetIsLocked(true);
		}
		if (SaveManager.PlayerSaveData.GetInsightState(InsightType.StudyBoss_DoorOpened) < InsightState.ResolvedButNotViewed && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.StudyMiniboss_SpearKnight_Defeated) && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.StudyMiniboss_SwordKnight_Defeated))
		{
			SaveManager.PlayerSaveData.SetInsightState(InsightType.StudyBoss_DoorOpened, InsightState.ResolvedButNotViewed, false);
			this.m_insightArgs.Initialize(InsightType.StudyBoss_DoorOpened, false, 5f, null, null, null);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_insightArgs);
		}
	}

	// Token: 0x0600302B RID: 12331 RVA: 0x000A4DC2 File Offset: 0x000A2FC2
	private void OnDisable()
	{
		BossEntranceRoomController.RunDoorCrumbleAnimation = false;
	}

	// Token: 0x04002650 RID: 9808
	[SerializeField]
	protected PlayerSaveFlag m_bossBeatenFlag;

	// Token: 0x04002651 RID: 9809
	[SerializeField]
	private TunnelSpawnController m_bossTunnel;

	// Token: 0x04002652 RID: 9810
	private InsightObjectiveCompleteHUDEventArgs m_insightArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.None, false, 5f, null, null, null);
}
