using System;

// Token: 0x020004F0 RID: 1264
public class BridgeTransitionRoomController : BaseSpecialRoomController
{
	// Token: 0x06002F71 RID: 12145 RVA: 0x000A2308 File Offset: 0x000A0508
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		if (SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockBouncableDownstrike) > 0)
		{
			InsightType insightFromHeirloomType = InsightType_RL.GetInsightFromHeirloomType(HeirloomType.UnlockBouncableDownstrike);
			if (SaveManager.PlayerSaveData.GetInsightState(insightFromHeirloomType) < InsightState.ResolvedButNotViewed)
			{
				InsightObjectiveCompleteHUDEventArgs eventArgs2 = new InsightObjectiveCompleteHUDEventArgs(insightFromHeirloomType, false, 5f, null, null, null);
				SaveManager.PlayerSaveData.SetInsightState(insightFromHeirloomType, InsightState.ResolvedButNotViewed, false);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, eventArgs2);
			}
		}
	}
}
