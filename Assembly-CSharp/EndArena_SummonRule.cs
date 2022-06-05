using System;
using System.Collections;

// Token: 0x02000895 RID: 2197
[Serializable]
public class EndArena_SummonRule : BaseSummonRule
{
	// Token: 0x170017FB RID: 6139
	// (get) Token: 0x06004349 RID: 17225 RVA: 0x00017F3C File Offset: 0x0001613C
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.EndArena;
		}
	}

	// Token: 0x170017FC RID: 6140
	// (get) Token: 0x0600434A RID: 17226 RVA: 0x0002534F File Offset: 0x0002354F
	public override string RuleLabel
	{
		get
		{
			return "End Arena";
		}
	}

	// Token: 0x0600434B RID: 17227 RVA: 0x00025356 File Offset: 0x00023556
	public override IEnumerator RunSummonRule()
	{
		EnemyManager.DisableAllSummonedEnemies();
		base.IsRuleComplete = true;
		base.SummonController.StopArena(true);
		yield break;
	}
}
