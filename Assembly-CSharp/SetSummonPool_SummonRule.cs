using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020008AB RID: 2219
[Serializable]
public class SetSummonPool_SummonRule : BaseSummonRule
{
	// Token: 0x1700182C RID: 6188
	// (get) Token: 0x060043BC RID: 17340 RVA: 0x000255E2 File Offset: 0x000237E2
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SetSummonPool;
		}
	}

	// Token: 0x1700182D RID: 6189
	// (get) Token: 0x060043BD RID: 17341 RVA: 0x000255E9 File Offset: 0x000237E9
	public EnemyTypeAndRank[] EnemiesToSummonArray
	{
		get
		{
			return this.m_enemiesToSummonArray;
		}
	}

	// Token: 0x1700182E RID: 6190
	// (get) Token: 0x060043BE RID: 17342 RVA: 0x000255F1 File Offset: 0x000237F1
	public bool PoolIsBiomeSpecific
	{
		get
		{
			return this.m_poolIsBiomeSpecific;
		}
	}

	// Token: 0x1700182F RID: 6191
	// (get) Token: 0x060043BF RID: 17343 RVA: 0x0002552A File Offset: 0x0002372A
	public override Color BoxColor
	{
		get
		{
			return Color.cyan;
		}
	}

	// Token: 0x17001830 RID: 6192
	// (get) Token: 0x060043C0 RID: 17344 RVA: 0x000255F9 File Offset: 0x000237F9
	public override string RuleLabel
	{
		get
		{
			return "Set Summon Pool";
		}
	}

	// Token: 0x060043C1 RID: 17345 RVA: 0x00025600 File Offset: 0x00023800
	public override IEnumerator RunSummonRule()
	{
		base.SummonController.SummonPool.Clear();
		while (!PlayerManager.IsInstantiated || PlayerManager.GetCurrentPlayerRoom() == null)
		{
			yield return null;
		}
		base.SummonController.PoolIsBiomeSpecific = this.m_poolIsBiomeSpecific;
		if (this.m_poolIsBiomeSpecific)
		{
			List<EnemyTypeAndRank> allEnemiesInBiome = EnemyUtility.GetAllEnemiesInBiome(PlayerManager.GetCurrentPlayerRoom().BiomeType, this.m_spawnFlyingOnly);
			for (int i = 0; i < allEnemiesInBiome.Count; i++)
			{
				if (SetSummonPool_SummonRule.SummonExceptionArray.Contains(allEnemiesInBiome[i].Type))
				{
					allEnemiesInBiome.RemoveAt(i);
					i--;
				}
			}
			base.SummonController.SummonPool.AddRange(allEnemiesInBiome);
		}
		else
		{
			base.SummonController.SummonPool.AddRange(this.m_enemiesToSummonArray);
		}
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x040034AF RID: 13487
	public static EnemyType[] SummonExceptionArray = new EnemyType[]
	{
		EnemyType.BouncySpike,
		EnemyType.Eggplant
	};

	// Token: 0x040034B0 RID: 13488
	[SerializeField]
	private bool m_poolIsBiomeSpecific;

	// Token: 0x040034B1 RID: 13489
	[SerializeField]
	private bool m_spawnFlyingOnly;

	// Token: 0x040034B2 RID: 13490
	[SerializeField]
	private EnemyTypeAndRank[] m_enemiesToSummonArray = new EnemyTypeAndRank[0];
}
