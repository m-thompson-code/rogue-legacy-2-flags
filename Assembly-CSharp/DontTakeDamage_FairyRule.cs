using System;

// Token: 0x02000432 RID: 1074
public class DontTakeDamage_FairyRule : DontGetHit_FairyRule
{
	// Token: 0x17000F99 RID: 3993
	// (get) Token: 0x06002774 RID: 10100 RVA: 0x0008329C File Offset: 0x0008149C
	public override string Description
	{
		get
		{
			return "LOC_ID_FAIRY_RULE_LOSE_NO_HEALTH_1";
		}
	}

	// Token: 0x17000F9A RID: 3994
	// (get) Token: 0x06002775 RID: 10101 RVA: 0x000832A3 File Offset: 0x000814A3
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.DontTakeDamage;
		}
	}

	// Token: 0x17000F9B RID: 3995
	// (get) Token: 0x06002776 RID: 10102 RVA: 0x000832A7 File Offset: 0x000814A7
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06002777 RID: 10103 RVA: 0x000832AA File Offset: 0x000814AA
	protected override void OnPlayerTakeDamage(object sender, EventArgs eventArgs)
	{
		if ((eventArgs as CharacterHitEventArgs).DamageTaken > 0f)
		{
			base.SetIsFailed();
		}
	}
}
