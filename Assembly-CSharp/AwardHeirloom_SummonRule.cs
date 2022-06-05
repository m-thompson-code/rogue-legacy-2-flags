using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200088E RID: 2190
[Serializable]
public class AwardHeirloom_SummonRule : BaseSummonRule
{
	// Token: 0x170017E8 RID: 6120
	// (get) Token: 0x0600431A RID: 17178 RVA: 0x00025210 File Offset: 0x00023410
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.AwardHeirloom;
		}
	}

	// Token: 0x170017E9 RID: 6121
	// (get) Token: 0x0600431B RID: 17179 RVA: 0x00025217 File Offset: 0x00023417
	public override string RuleLabel
	{
		get
		{
			return "Award Heirloom";
		}
	}

	// Token: 0x0600431C RID: 17180 RVA: 0x0002521E File Offset: 0x0002341E
	public override IEnumerator RunSummonRule()
	{
		SaveManager.PlayerSaveData.SetHeirloomLevel(this.m_heirloomType, 1, false, true);
		PlayerManager.GetPlayerController().InitializeAbilities();
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x04003462 RID: 13410
	[SerializeField]
	private HeirloomType m_heirloomType;
}
