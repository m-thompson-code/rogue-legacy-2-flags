using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008BF RID: 2239
[Serializable]
public class WaitForBroadcast_SummonRule : BaseSummonRule
{
	// Token: 0x17001855 RID: 6229
	// (get) Token: 0x0600443A RID: 17466 RVA: 0x000259F4 File Offset: 0x00023BF4
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.WaitForBroadcast;
		}
	}

	// Token: 0x17001856 RID: 6230
	// (get) Token: 0x0600443B RID: 17467 RVA: 0x000259FB File Offset: 0x00023BFB
	public override string RuleLabel
	{
		get
		{
			return "Wait For Broadcast";
		}
	}

	// Token: 0x0600443C RID: 17468 RVA: 0x00025A02 File Offset: 0x00023C02
	public override void Initialize(SummonRuleController summonController)
	{
		base.Initialize(summonController);
		this.m_onSummonRuleBroadcast = new Action<MonoBehaviour, EventArgs>(this.OnSummonRuleBroadcast);
	}

	// Token: 0x0600443D RID: 17469 RVA: 0x00025A1D File Offset: 0x00023C1D
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

	// Token: 0x0600443E RID: 17470 RVA: 0x00025A2C File Offset: 0x00023C2C
	private void OnSummonRuleBroadcast(object sender, EventArgs args)
	{
		this.m_continueRule = true;
	}

	// Token: 0x04003500 RID: 13568
	private bool m_continueRule;

	// Token: 0x04003501 RID: 13569
	private Action<MonoBehaviour, EventArgs> m_onSummonRuleBroadcast;
}
