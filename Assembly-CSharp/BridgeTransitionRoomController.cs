using System;

// Token: 0x0200084A RID: 2122
public class BridgeTransitionRoomController : BaseSpecialRoomController
{
	// Token: 0x06004192 RID: 16786 RVA: 0x00107C24 File Offset: 0x00105E24
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
