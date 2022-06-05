using System;
using UnityEngine;

// Token: 0x02000502 RID: 1282
public class GoldenDoorsEntranceRoomController : BossEntranceRoomController
{
	// Token: 0x06002FE8 RID: 12264 RVA: 0x000A3EE4 File Offset: 0x000A20E4
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

	// Token: 0x06002FE9 RID: 12265 RVA: 0x000A3F88 File Offset: 0x000A2188
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

	// Token: 0x04002632 RID: 9778
	private const bool UNLOCK_DOOR = true;
}
