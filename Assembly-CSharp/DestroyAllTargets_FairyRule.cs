using System;

// Token: 0x020006F7 RID: 1783
public class DestroyAllTargets_FairyRule : KillAllEnemies_FairyRule
{
	// Token: 0x17001475 RID: 5237
	// (get) Token: 0x06003671 RID: 13937 RVA: 0x0001DF28 File Offset: 0x0001C128
	public override string Description
	{
		get
		{
			return string.Format(LocalizationManager.GetString("LOC_ID_FAIRY_RULE_TARGETS_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), Array.Empty<object>());
		}
	}

	// Token: 0x17001476 RID: 5238
	// (get) Token: 0x06003672 RID: 13938 RVA: 0x00006CB3 File Offset: 0x00004EB3
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.DestroyAllTargets;
		}
	}
}
