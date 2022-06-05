using System;
using UnityEngine;

// Token: 0x02000882 RID: 2178
public class StudyMinibossEntranceRoomController : BaseSpecialRoomController
{
	// Token: 0x060042D4 RID: 17108 RVA: 0x0010BBF8 File Offset: 0x00109DF8
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

	// Token: 0x060042D5 RID: 17109 RVA: 0x00024604 File Offset: 0x00022804
	private void OnDisable()
	{
		BossEntranceRoomController.RunDoorCrumbleAnimation = false;
	}

	// Token: 0x0400342D RID: 13357
	[SerializeField]
	protected PlayerSaveFlag m_bossBeatenFlag;

	// Token: 0x0400342E RID: 13358
	[SerializeField]
	private TunnelSpawnController m_bossTunnel;

	// Token: 0x0400342F RID: 13359
	private InsightObjectiveCompleteHUDEventArgs m_insightArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.None, false, 5f, null, null, null);
}
