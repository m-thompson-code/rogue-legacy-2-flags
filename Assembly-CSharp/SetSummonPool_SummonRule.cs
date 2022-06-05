using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000529 RID: 1321
[Serializable]
public class SetSummonPool_SummonRule : BaseSummonRule
{
	// Token: 0x170011FB RID: 4603
	// (get) Token: 0x060030AC RID: 12460 RVA: 0x000A5EA7 File Offset: 0x000A40A7
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SetSummonPool;
		}
	}

	// Token: 0x170011FC RID: 4604
	// (get) Token: 0x060030AD RID: 12461 RVA: 0x000A5EAE File Offset: 0x000A40AE
	public EnemyTypeAndRank[] EnemiesToSummonArray
	{
		get
		{
			return this.m_enemiesToSummonArray;
		}
	}

	// Token: 0x170011FD RID: 4605
	// (get) Token: 0x060030AE RID: 12462 RVA: 0x000A5EB6 File Offset: 0x000A40B6
	public bool PoolIsBiomeSpecific
	{
		get
		{
			return this.m_poolIsBiomeSpecific;
		}
	}

	// Token: 0x170011FE RID: 4606
	// (get) Token: 0x060030AF RID: 12463 RVA: 0x000A5EBE File Offset: 0x000A40BE
	public override Color BoxColor
	{
		get
		{
			return Color.cyan;
		}
	}

	// Token: 0x170011FF RID: 4607
	// (get) Token: 0x060030B0 RID: 12464 RVA: 0x000A5EC5 File Offset: 0x000A40C5
	public override string RuleLabel
	{
		get
		{
			return "Set Summon Pool";
		}
	}

	// Token: 0x060030B1 RID: 12465 RVA: 0x000A5ECC File Offset: 0x000A40CC
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

	// Token: 0x0400269C RID: 9884
	public static EnemyType[] SummonExceptionArray = new EnemyType[]
	{
		EnemyType.BouncySpike,
		EnemyType.Eggplant
	};

	// Token: 0x0400269D RID: 9885
	[SerializeField]
	private bool m_poolIsBiomeSpecific;

	// Token: 0x0400269E RID: 9886
	[SerializeField]
	private bool m_spawnFlyingOnly;

	// Token: 0x0400269F RID: 9887
	[SerializeField]
	private EnemyTypeAndRank[] m_enemiesToSummonArray = new EnemyTypeAndRank[0];
}
