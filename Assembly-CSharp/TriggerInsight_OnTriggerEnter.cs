using System;
using UnityEngine;

// Token: 0x0200036D RID: 877
public class TriggerInsight_OnTriggerEnter : MonoBehaviour
{
	// Token: 0x060020CE RID: 8398 RVA: 0x000671D0 File Offset: 0x000653D0
	private void Awake()
	{
		this.m_insightEventArgs = new InsightObjectiveCompleteHUDEventArgs(this.m_insightToTrigger, !this.m_isResolved, 5f, null, null, null);
		BoxCollider2D component = base.GetComponent<BoxCollider2D>();
		if (component == null)
		{
			throw new Exception("BoxCollider2D not found on TriggerInsight_OnTriggerEnter");
		}
		base.gameObject.layer = 13;
		component.isTrigger = true;
	}

	// Token: 0x060020CF RID: 8399 RVA: 0x00067230 File Offset: 0x00065430
	private void OnTriggerEnter2D(Collider2D otherCollider)
	{
		if (!otherCollider.CompareTag("Player"))
		{
			return;
		}
		if (!this.m_isResolved)
		{
			if (SaveManager.PlayerSaveData.GetInsightState(this.m_insightToTrigger) < InsightState.DiscoveredButNotViewed)
			{
				SaveManager.PlayerSaveData.SetInsightState(this.m_insightToTrigger, InsightState.DiscoveredButNotViewed, false);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_insightEventArgs);
			}
			base.gameObject.SetActive(false);
			return;
		}
		if (SaveManager.PlayerSaveData.GetInsightState(this.m_insightToTrigger) < InsightState.ResolvedButNotViewed)
		{
			SaveManager.PlayerSaveData.SetInsightState(this.m_insightToTrigger, InsightState.ResolvedButNotViewed, false);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_insightEventArgs);
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x04001C72 RID: 7282
	[SerializeField]
	private InsightType m_insightToTrigger;

	// Token: 0x04001C73 RID: 7283
	[SerializeField]
	private bool m_isResolved;

	// Token: 0x04001C74 RID: 7284
	private InsightObjectiveCompleteHUDEventArgs m_insightEventArgs;
}
