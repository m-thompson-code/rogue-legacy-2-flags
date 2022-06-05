using System;

// Token: 0x0200087C RID: 2172
public class RebelRoomController : BaseSpecialRoomController
{
	// Token: 0x060042B7 RID: 17079 RVA: 0x0010B5C0 File Offset: 0x001097C0
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		if (SaveManager.PlayerSaveData.GetInsightState(InsightType.Ending_RebelsHidout) < InsightState.ResolvedButNotViewed)
		{
			SaveManager.PlayerSaveData.SetInsightState(InsightType.Ending_RebelsHidout, InsightState.ResolvedButNotViewed, false);
			InsightObjectiveCompleteHUDEventArgs eventArgs2 = new InsightObjectiveCompleteHUDEventArgs(InsightType.Ending_RebelsHidout, false, 5f, null, null, null);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, null, eventArgs2);
		}
	}
}
