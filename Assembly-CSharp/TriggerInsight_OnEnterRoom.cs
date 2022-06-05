using System;
using UnityEngine;

// Token: 0x0200036C RID: 876
public class TriggerInsight_OnEnterRoom : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17000DFE RID: 3582
	// (get) Token: 0x060020C7 RID: 8391 RVA: 0x00067087 File Offset: 0x00065287
	// (set) Token: 0x060020C8 RID: 8392 RVA: 0x0006708F File Offset: 0x0006528F
	public BaseRoom Room { get; private set; }

	// Token: 0x060020C9 RID: 8393 RVA: 0x00067098 File Offset: 0x00065298
	private void Awake()
	{
		this.m_insightEventArgs = new InsightObjectiveCompleteHUDEventArgs(this.m_insightToTrigger, !this.m_isResolved, 5f, null, null, null);
		this.m_onPlayerEnterRoom = new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x060020CA RID: 8394 RVA: 0x000670D0 File Offset: 0x000652D0
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

	// Token: 0x060020CB RID: 8395 RVA: 0x00067124 File Offset: 0x00065324
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

	// Token: 0x060020CC RID: 8396 RVA: 0x000671A0 File Offset: 0x000653A0
	private void OnDestroy()
	{
		if (this.Room != null)
		{
			this.Room.PlayerEnterRelay.RemoveListener(this.m_onPlayerEnterRoom);
		}
	}

	// Token: 0x04001C6D RID: 7277
	[SerializeField]
	private InsightType m_insightToTrigger;

	// Token: 0x04001C6E RID: 7278
	[SerializeField]
	private bool m_isResolved;

	// Token: 0x04001C6F RID: 7279
	private InsightObjectiveCompleteHUDEventArgs m_insightEventArgs;

	// Token: 0x04001C70 RID: 7280
	private Action<object, RoomViaDoorEventArgs> m_onPlayerEnterRoom;
}
