using System;

// Token: 0x0200050F RID: 1295
public class StudyBossEntranceRoomController : BossEntranceRoomController
{
	// Token: 0x0600301E RID: 12318 RVA: 0x000A4A00 File Offset: 0x000A2C00
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		if (!base.IsRoomComplete)
		{
			bool flag = false;
			if ((SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.StudyMiniboss_SpearKnight_Defeated) && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.StudyMiniboss_SwordKnight_Defeated)) || flag)
			{
				this.SetBossTunnelState(BossTunnelState.PartlyOpen, true);
				return;
			}
			this.SetBossTunnelState(BossTunnelState.Closed, true);
		}
	}
}
