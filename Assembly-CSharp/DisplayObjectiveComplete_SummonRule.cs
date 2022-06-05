using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000893 RID: 2195
[Serializable]
public class DisplayObjectiveComplete_SummonRule : BaseSummonRule
{
	// Token: 0x170017F7 RID: 6135
	// (get) Token: 0x0600433F RID: 17215 RVA: 0x000252E9 File Offset: 0x000234E9
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.DisplayObjectiveComplete;
		}
	}

	// Token: 0x170017F8 RID: 6136
	// (get) Token: 0x06004340 RID: 17216 RVA: 0x000252F0 File Offset: 0x000234F0
	public override string RuleLabel
	{
		get
		{
			return "Display Objective Complete";
		}
	}

	// Token: 0x06004341 RID: 17217 RVA: 0x000252F7 File Offset: 0x000234F7
	public override IEnumerator RunSummonRule()
	{
		ObjectiveCompleteHUDEventArgs objectiveCompleteHUDEventArgs = null;
		ObjectiveCompleteHUDType hudType = this.m_hudType;
		if (hudType != ObjectiveCompleteHUDType.Boss)
		{
			if (hudType == ObjectiveCompleteHUDType.Insight)
			{
				this.m_insightCompleteEventArgs.Initialize(this.m_insightType, this.m_insightDiscovered, 5f, null, null, null);
				objectiveCompleteHUDEventArgs = this.m_insightCompleteEventArgs;
			}
		}
		else
		{
			if (!string.IsNullOrEmpty(this.m_bossLocIDOverride))
			{
				string @string = LocalizationManager.GetString(this.m_bossLocIDOverride, false, false);
				this.m_bossCompleteEventArgs.Initialize(EnemyType.None, EnemyRank.None, 5f, @string, null, null);
			}
			objectiveCompleteHUDEventArgs = this.m_bossCompleteEventArgs;
		}
		if (objectiveCompleteHUDEventArgs != null)
		{
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, null, objectiveCompleteHUDEventArgs);
		}
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x0400346E RID: 13422
	[SerializeField]
	private ObjectiveCompleteHUDType m_hudType;

	// Token: 0x0400346F RID: 13423
	[SerializeField]
	private InsightType m_insightType;

	// Token: 0x04003470 RID: 13424
	[SerializeField]
	private bool m_insightDiscovered;

	// Token: 0x04003471 RID: 13425
	[SerializeField]
	private string m_bossLocIDOverride;

	// Token: 0x04003472 RID: 13426
	private InsightObjectiveCompleteHUDEventArgs m_insightCompleteEventArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.None, false, 5f, null, null, null);

	// Token: 0x04003473 RID: 13427
	private BossObjectiveCompleteHUDEventArgs m_bossCompleteEventArgs = new BossObjectiveCompleteHUDEventArgs(EnemyType.None, EnemyRank.None, 5f, null, null, null);
}
