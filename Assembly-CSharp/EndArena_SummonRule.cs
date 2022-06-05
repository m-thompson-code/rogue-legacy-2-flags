using System;
using System.Collections;

// Token: 0x0200051E RID: 1310
[Serializable]
public class EndArena_SummonRule : BaseSummonRule
{
	// Token: 0x170011E0 RID: 4576
	// (get) Token: 0x0600307B RID: 12411 RVA: 0x000A5CBD File Offset: 0x000A3EBD
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.EndArena;
		}
	}

	// Token: 0x170011E1 RID: 4577
	// (get) Token: 0x0600307C RID: 12412 RVA: 0x000A5CC4 File Offset: 0x000A3EC4
	public override string RuleLabel
	{
		get
		{
			return "End Arena";
		}
	}

	// Token: 0x0600307D RID: 12413 RVA: 0x000A5CCB File Offset: 0x000A3ECB
	public override IEnumerator RunSummonRule()
	{
		EnemyManager.DisableAllSummonedEnemies();
		base.IsRuleComplete = true;
		base.SummonController.StopArena(true);
		yield break;
	}
}
