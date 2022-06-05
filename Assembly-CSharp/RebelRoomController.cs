using System;

// Token: 0x0200050D RID: 1293
public class RebelRoomController : BaseSpecialRoomController
{
	// Token: 0x06003019 RID: 12313 RVA: 0x000A4988 File Offset: 0x000A2B88
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
