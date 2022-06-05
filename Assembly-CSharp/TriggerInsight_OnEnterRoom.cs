using System;
using UnityEngine;

// Token: 0x020005EB RID: 1515
public class TriggerInsight_OnEnterRoom : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17001277 RID: 4727
	// (get) Token: 0x06002E90 RID: 11920 RVA: 0x00019688 File Offset: 0x00017888
	// (set) Token: 0x06002E91 RID: 11921 RVA: 0x00019690 File Offset: 0x00017890
	public BaseRoom Room { get; private set; }

	// Token: 0x06002E92 RID: 11922 RVA: 0x00019699 File Offset: 0x00017899
	private void Awake()
	{
		this.m_insightEventArgs = new InsightObjectiveCompleteHUDEventArgs(this.m_insightToTrigger, !this.m_isResolved, 5f, null, null, null);
		this.m_onPlayerEnterRoom = new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x06002E93 RID: 11923 RVA: 0x000C8134 File Offset: 0x000C6334
	public void SetRoom(BaseRoom room)
	{
		this.Room = (room as Room);
		if (this.Room != null)
		{
			this.Room.PlayerEnterRelay.AddListener(this.m_onPlayerEnterRoom, false);
			return;
		}
		Debug.LogFormat("<color=red>| {0} | Room Field is null. Did you forget to connect the Room Component to the Room Field in the Inspector?</color>", new object[]
		{
			this
		});
	}

	// Token: 0x06002E94 RID: 11924 RVA: 0x000C8188 File Offset: 0x000C6388
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (!this.m_isResolved)
		{
			if (SaveManager.PlayerSaveData.GetInsightState(this.m_insightToTrigger) < InsightState.DiscoveredButNotViewed)
			{
				SaveManager.PlayerSaveData.SetInsightState(this.m_insightToTrigger, InsightState.DiscoveredButNotViewed, false);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_insightEventArgs);
				return;
			}
		}
		else if (SaveManager.PlayerSaveData.GetInsightState(this.m_insightToTrigger) < InsightState.ResolvedButNotViewed)
		{
			SaveManager.PlayerSaveData.SetInsightState(this.m_insightToTrigger, InsightState.ResolvedButNotViewed, false);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_insightEventArgs);
		}
	}

	// Token: 0x06002E95 RID: 11925 RVA: 0x000196CF File Offset: 0x000178CF
	private void OnDestroy()
	{
		if (this.Room != null)
		{
			this.Room.PlayerEnterRelay.RemoveListener(this.m_onPlayerEnterRoom);
		}
	}

	// Token: 0x04002620 RID: 9760
	[SerializeField]
	private InsightType m_insightToTrigger;

	// Token: 0x04002621 RID: 9761
	[SerializeField]
	private bool m_isResolved;

	// Token: 0x04002622 RID: 9762
	private InsightObjectiveCompleteHUDEventArgs m_insightEventArgs;

	// Token: 0x04002623 RID: 9763
	private Action<object, RoomViaDoorEventArgs> m_onPlayerEnterRoom;
}
