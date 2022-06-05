using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008BD RID: 2237
public class TogglePlayerInvincibility_SummonRule : BaseSummonRule
{
	// Token: 0x17001851 RID: 6225
	// (get) Token: 0x06004430 RID: 17456 RVA: 0x00019204 File Offset: 0x00017404
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.TogglePlayerInvincibility;
		}
	}

	// Token: 0x17001852 RID: 6226
	// (get) Token: 0x06004431 RID: 17457 RVA: 0x000259C7 File Offset: 0x00023BC7
	public override string RuleLabel
	{
		get
		{
			return "Toggle Player Invincibility";
		}
	}

	// Token: 0x06004432 RID: 17458 RVA: 0x000259CE File Offset: 0x00023BCE
	public override IEnumerator RunSummonRule()
	{
		PlayerManager.GetPlayerController().TakesNoDamage = this.m_setPlayerInvincible;
		base.SummonController.PlayerInvincibilityActive = true;
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x040034FC RID: 13564
	[SerializeField]
	private bool m_setPlayerInvincible;
}
