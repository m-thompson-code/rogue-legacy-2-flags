using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008A9 RID: 2217
[Serializable]
public class SetSummonPoolLevelMod_SummonRule : BaseSummonRule
{
	// Token: 0x17001828 RID: 6184
	// (get) Token: 0x060043B2 RID: 17330 RVA: 0x000255AE File Offset: 0x000237AE
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SetSummonPoolLevelMod;
		}
	}

	// Token: 0x17001829 RID: 6185
	// (get) Token: 0x060043B3 RID: 17331 RVA: 0x000255B5 File Offset: 0x000237B5
	public override string RuleLabel
	{
		get
		{
			return "Set Summon Pool Level Mod";
		}
	}

	// Token: 0x060043B4 RID: 17332 RVA: 0x000255BC File Offset: 0x000237BC
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

	// Token: 0x040034AA RID: 13482
	[SerializeField]
	private bool m_setLevelToRoom;

	// Token: 0x040034AB RID: 13483
	[SerializeField]
	private int m_levelMod;
}
