using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000526 RID: 1318
[Serializable]
public class SetSpawnPoints_SummonRule : BaseSummonRule
{
	// Token: 0x170011F3 RID: 4595
	// (get) Token: 0x0600309E RID: 12446 RVA: 0x000A5E1D File Offset: 0x000A401D
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SetSpawnPoints;
		}
	}

	// Token: 0x170011F4 RID: 4596
	// (get) Token: 0x0600309F RID: 12447 RVA: 0x000A5E24 File Offset: 0x000A4024
	public override Color BoxColor
	{
		get
		{
			return Color.cyan;
		}
	}

	// Token: 0x170011F5 RID: 4597
	// (get) Token: 0x060030A0 RID: 12448 RVA: 0x000A5E2B File Offset: 0x000A402B
	public override string RuleLabel
	{
		get
		{
			return "Set Spawn Points";
		}
	}

	// Token: 0x060030A1 RID: 12449 RVA: 0x000A5E32 File Offset: 0x000A4032
	public override IEnumerator RunSummonRule()
	{
		base.SummonController.SpawnPoints.Clear();
		base.SummonController.SpawnPoints.AddRange(this.m_spawnPointArray);
		base.SummonController.AvailableSpawnPoints.Clear();
		base.SummonController.AvailableSpawnPoints.AddRange(base.SummonController.SpawnPoints);
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x04002698 RID: 9880
	[SerializeField]
	private int[] m_spawnPointArray = new int[0];
}
