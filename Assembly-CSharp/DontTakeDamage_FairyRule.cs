using System;

// Token: 0x020006F9 RID: 1785
public class DontTakeDamage_FairyRule : DontGetHit_FairyRule
{
	// Token: 0x1700147A RID: 5242
	// (get) Token: 0x0600367D RID: 13949 RVA: 0x0001DFC4 File Offset: 0x0001C1C4
	public override string Description
	{
		get
		{
			return "LOC_ID_FAIRY_RULE_LOSE_NO_HEALTH_1";
		}
	}

	// Token: 0x1700147B RID: 5243
	// (get) Token: 0x0600367E RID: 13950 RVA: 0x0001DFCB File Offset: 0x0001C1CB
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.DontTakeDamage;
		}
	}

	// Token: 0x1700147C RID: 5244
	// (get) Token: 0x0600367F RID: 13951 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003680 RID: 13952 RVA: 0x0001DFCF File Offset: 0x0001C1CF
	protected override void OnPlayerTakeDamage(object sender, EventArgs eventArgs)
	{
		if ((eventArgs as CharacterHitEventArgs).DamageTaken > 0f)
		{
			base.SetIsFailed();
		}
	}
}
