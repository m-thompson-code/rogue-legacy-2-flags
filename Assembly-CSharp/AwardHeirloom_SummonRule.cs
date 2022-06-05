using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200051A RID: 1306
[Serializable]
public class AwardHeirloom_SummonRule : BaseSummonRule
{
	// Token: 0x170011D3 RID: 4563
	// (get) Token: 0x0600305E RID: 12382 RVA: 0x000A5BB0 File Offset: 0x000A3DB0
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.AwardHeirloom;
		}
	}

	// Token: 0x170011D4 RID: 4564
	// (get) Token: 0x0600305F RID: 12383 RVA: 0x000A5BB7 File Offset: 0x000A3DB7
	public override string RuleLabel
	{
		get
		{
			return "Award Heirloom";
		}
	}

	// Token: 0x06003060 RID: 12384 RVA: 0x000A5BBE File Offset: 0x000A3DBE
	public override IEnumerator RunSummonRule()
	{
		SaveManager.PlayerSaveData.SetHeirloomLevel(this.m_heirloomType, 1, false, true);
		PlayerManager.GetPlayerController().InitializeAbilities();
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x0400267A RID: 9850
	[SerializeField]
	private HeirloomType m_heirloomType;
}
