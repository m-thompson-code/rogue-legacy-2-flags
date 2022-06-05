using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008A5 RID: 2213
[Serializable]
public class SetSpawnPoints_SummonRule : BaseSummonRule
{
	// Token: 0x1700181E RID: 6174
	// (get) Token: 0x0600439C RID: 17308 RVA: 0x00017BA1 File Offset: 0x00015DA1
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SetSpawnPoints;
		}
	}

	// Token: 0x1700181F RID: 6175
	// (get) Token: 0x0600439D RID: 17309 RVA: 0x0002552A File Offset: 0x0002372A
	public override Color BoxColor
	{
		get
		{
			return Color.cyan;
		}
	}

	// Token: 0x17001820 RID: 6176
	// (get) Token: 0x0600439E RID: 17310 RVA: 0x00025531 File Offset: 0x00023731
	public override string RuleLabel
	{
		get
		{
			return "Set Spawn Points";
		}
	}

	// Token: 0x0600439F RID: 17311 RVA: 0x00025538 File Offset: 0x00023738
	public override IEnumerator RunSummonRule()
	{
		base.SummonController.SpawnPoints.Clear();
		base.SummonController.SpawnPoints.AddRange(this.m_spawnPointArray);
		base.SummonController.AvailableSpawnPoints.Clear();
		base.SummonController.AvailableSpawnPoints.AddRange(base.SummonController.SpawnPoints);
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x040034A2 RID: 13474
	[SerializeField]
	private int[] m_spawnPointArray = new int[0];
}
