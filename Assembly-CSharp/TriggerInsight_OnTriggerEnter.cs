using System;
using UnityEngine;

// Token: 0x020005EC RID: 1516
public class TriggerInsight_OnTriggerEnter : MonoBehaviour
{
	// Token: 0x06002E97 RID: 11927 RVA: 0x000C8204 File Offset: 0x000C6404
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

	// Token: 0x06002E98 RID: 11928 RVA: 0x000C8264 File Offset: 0x000C6464
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

	// Token: 0x04002625 RID: 9765
	[SerializeField]
	private InsightType m_insightToTrigger;

	// Token: 0x04002626 RID: 9766
	[SerializeField]
	private bool m_isResolved;

	// Token: 0x04002627 RID: 9767
	private InsightObjectiveCompleteHUDEventArgs m_insightEventArgs;
}
