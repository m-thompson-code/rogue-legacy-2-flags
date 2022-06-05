using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000528 RID: 1320
[Serializable]
public class SetSummonPoolLevelMod_SummonRule : BaseSummonRule
{
	// Token: 0x170011F9 RID: 4601
	// (get) Token: 0x060030A8 RID: 12456 RVA: 0x000A5E82 File Offset: 0x000A4082
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SetSummonPoolLevelMod;
		}
	}

	// Token: 0x170011FA RID: 4602
	// (get) Token: 0x060030A9 RID: 12457 RVA: 0x000A5E89 File Offset: 0x000A4089
	public override string RuleLabel
	{
		get
		{
			return "Set Summon Pool Level Mod";
		}
	}

	// Token: 0x060030AA RID: 12458 RVA: 0x000A5E90 File Offset: 0x000A4090
	public override IEnumerator RunSummonRule()
	{
		if (this.m_setLevelToRoom)
		{
			while (!PlayerManager.IsInstantiated || PlayerManager.GetCurrentPlayerRoom() == null)
			{
				yield return null;
			}
			base.SummonController.SummonLevelOverride = PlayerManager.GetCurrentPlayerRoom().Level;
		}
		else
		{
			base.SummonController.SummonLevelOverride = this.m_levelMod;
		}
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x0400269A RID: 9882
	[SerializeField]
	private bool m_setLevelToRoom;

	// Token: 0x0400269B RID: 9883
	[SerializeField]
	private int m_levelMod;
}
