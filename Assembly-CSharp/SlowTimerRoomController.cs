using System;

// Token: 0x0200050E RID: 1294
public class SlowTimerRoomController : BaseSpecialRoomController
{
	// Token: 0x0600301B RID: 12315 RVA: 0x000A49E5 File Offset: 0x000A2BE5
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		GlobalTimerHUDController.SlowTime = true;
	}

	// Token: 0x0600301C RID: 12316 RVA: 0x000A49ED File Offset: 0x000A2BED
	protected override void OnPlayerExitRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		GlobalTimerHUDController.SlowTime = false;
	}
}
