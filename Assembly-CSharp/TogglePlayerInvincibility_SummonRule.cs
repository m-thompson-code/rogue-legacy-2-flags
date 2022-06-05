using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000531 RID: 1329
public class TogglePlayerInvincibility_SummonRule : BaseSummonRule
{
	// Token: 0x17001212 RID: 4626
	// (get) Token: 0x060030EE RID: 12526 RVA: 0x000A664A File Offset: 0x000A484A
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.TogglePlayerInvincibility;
		}
	}

	// Token: 0x17001213 RID: 4627
	// (get) Token: 0x060030EF RID: 12527 RVA: 0x000A6651 File Offset: 0x000A4851
	public override string RuleLabel
	{
		get
		{
			return "Toggle Player Invincibility";
		}
	}

	// Token: 0x060030F0 RID: 12528 RVA: 0x000A6658 File Offset: 0x000A4858
	public override IEnumerator RunSummonRule()
	{
		PlayerManager.GetPlayerController().TakesNoDamage = this.m_setPlayerInvincible;
		base.SummonController.PlayerInvincibilityActive = true;
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x040026C0 RID: 9920
	[SerializeField]
	private bool m_setPlayerInvincible;
}
