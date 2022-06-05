using System;

// Token: 0x0200087D RID: 2173
public class SlowTimerRoomController : BaseSpecialRoomController
{
	// Token: 0x060042B9 RID: 17081 RVA: 0x00024EA3 File Offset: 0x000230A3
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		GlobalTimerHUDController.SlowTime = true;
	}

	// Token: 0x060042BA RID: 17082 RVA: 0x00024EAB File Offset: 0x000230AB
	protected override void OnPlayerExitRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		GlobalTimerHUDController.SlowTime = false;
	}
}
