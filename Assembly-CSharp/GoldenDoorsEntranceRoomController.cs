using System;
using UnityEngine;

// Token: 0x0200086C RID: 2156
public class GoldenDoorsEntranceRoomController : BossEntranceRoomController
{
	// Token: 0x06004268 RID: 17000 RVA: 0x0010A9A8 File Offset: 0x00108BA8
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (CutsceneManager.IsCutsceneActive && BossEntranceRoomController.RunDoorCrumbleAnimation)
		{
			BossEntranceRoomController.CutscenePlayed_DoNotDisableCrumble = true;
		}
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CastleBoss_Defeated) && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.BridgeBoss_Defeated) && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.ForestBoss_Defeated) && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.StudyBoss_Defeated) && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.TowerBoss_Defeated) && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveBoss_Defeated) && !CutsceneManager.IsCutsceneActive)
		{
			this.SetBossTunnelState(BossTunnelState.PartlyOpen, true);
			return;
		}
		this.SetBossTunnelState(BossTunnelState.Closed, true);
	}

	// Token: 0x06004269 RID: 17001 RVA: 0x0010AA4C File Offset: 0x00108C4C
	public override void SetBossTunnelState(BossTunnelState state, bool skipToIdleState)
	{
		if (state == BossTunnelState.PartlyOpen)
		{
			state = BossTunnelState.Open;
		}
		Animator animator = base.BossTunnelSpawner.Tunnel.Animator;
		animator.ResetTrigger("UnderConstruction");
		animator.ResetTrigger("UnderConstructionInstant");
		base.SetBossTunnelState(state, skipToIdleState);
	}

	// Token: 0x040033F6 RID: 13302
	private const bool UNLOCK_DOOR = true;
}
