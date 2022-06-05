using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200051D RID: 1309
[Serializable]
public class DisplayObjectiveComplete_SummonRule : BaseSummonRule
{
	// Token: 0x170011DE RID: 4574
	// (get) Token: 0x06003077 RID: 12407 RVA: 0x000A5C6E File Offset: 0x000A3E6E
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.DisplayObjectiveComplete;
		}
	}

	// Token: 0x170011DF RID: 4575
	// (get) Token: 0x06003078 RID: 12408 RVA: 0x000A5C75 File Offset: 0x000A3E75
	public override string RuleLabel
	{
		get
		{
			return "Display Objective Complete";
		}
	}

	// Token: 0x06003079 RID: 12409 RVA: 0x000A5C7C File Offset: 0x000A3E7C
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

	// Token: 0x04002680 RID: 9856
	[SerializeField]
	private ObjectiveCompleteHUDType m_hudType;

	// Token: 0x04002681 RID: 9857
	[SerializeField]
	private InsightType m_insightType;

	// Token: 0x04002682 RID: 9858
	[SerializeField]
	private bool m_insightDiscovered;

	// Token: 0x04002683 RID: 9859
	[SerializeField]
	private string m_bossLocIDOverride;

	// Token: 0x04002684 RID: 9860
	private InsightObjectiveCompleteHUDEventArgs m_insightCompleteEventArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.None, false, 5f, null, null, null);

	// Token: 0x04002685 RID: 9861
	private BossObjectiveCompleteHUDEventArgs m_bossCompleteEventArgs = new BossObjectiveCompleteHUDEventArgs(EnemyType.None, EnemyRank.None, 5f, null, null, null);
}
