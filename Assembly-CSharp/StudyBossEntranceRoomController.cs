using System;

// Token: 0x0200087E RID: 2174
public class StudyBossEntranceRoomController : BossEntranceRoomController
{
	// Token: 0x060042BC RID: 17084 RVA: 0x0010B618 File Offset: 0x00109818
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
