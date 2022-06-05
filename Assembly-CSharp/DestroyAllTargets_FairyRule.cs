using System;

// Token: 0x02000430 RID: 1072
public class DestroyAllTargets_FairyRule : KillAllEnemies_FairyRule
{
	// Token: 0x17000F94 RID: 3988
	// (get) Token: 0x06002768 RID: 10088 RVA: 0x000831F5 File Offset: 0x000813F5
	public override string Description
	{
		get
		{
			return string.Format(LocalizationManager.GetString("LOC_ID_FAIRY_RULE_TARGETS_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), Array.Empty<object>());
		}
	}

	// Token: 0x17000F95 RID: 3989
	// (get) Token: 0x06002769 RID: 10089 RVA: 0x0008321B File Offset: 0x0008141B
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.DestroyAllTargets;
		}
	}
}
