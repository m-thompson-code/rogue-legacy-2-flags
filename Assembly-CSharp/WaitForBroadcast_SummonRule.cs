using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000532 RID: 1330
[Serializable]
public class WaitForBroadcast_SummonRule : BaseSummonRule
{
	// Token: 0x17001214 RID: 4628
	// (get) Token: 0x060030F2 RID: 12530 RVA: 0x000A666F File Offset: 0x000A486F
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.WaitForBroadcast;
		}
	}

	// Token: 0x17001215 RID: 4629
	// (get) Token: 0x060030F3 RID: 12531 RVA: 0x000A6676 File Offset: 0x000A4876
	public override string RuleLabel
	{
		get
		{
			return "Wait For Broadcast";
		}
	}

	// Token: 0x060030F4 RID: 12532 RVA: 0x000A667D File Offset: 0x000A487D
	public override void Initialize(SummonRuleController summonController)
	{
		base.Initialize(summonController);
		this.m_onSummonRuleBroadcast = new Action<MonoBehaviour, EventArgs>(this.OnSummonRuleBroadcast);
	}

	// Token: 0x060030F5 RID: 12533 RVA: 0x000A6698 File Offset: 0x000A4898
	public override IEnumerator RunSummonRule()
	{
		this.m_continueRule = false;
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.SummonRuleBroadcast, this.m_onSummonRuleBroadcast);
		while (!this.m_continueRule)
		{
			yield return null;
		}
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.SummonRuleBroadcast, this.m_onSummonRuleBroadcast);
		yield break;
	}

	// Token: 0x060030F6 RID: 12534 RVA: 0x000A66A7 File Offset: 0x000A48A7
	private void OnSummonRuleBroadcast(object sender, EventArgs args)
	{
		this.m_continueRule = true;
	}

	// Token: 0x040026C1 RID: 9921
	private bool m_continueRule;

	// Token: 0x040026C2 RID: 9922
	private Action<MonoBehaviour, EventArgs> m_onSummonRuleBroadcast;
}
