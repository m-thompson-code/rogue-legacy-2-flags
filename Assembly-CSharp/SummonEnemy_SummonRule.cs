using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200052D RID: 1325
[Serializable]
public class SummonEnemy_SummonRule : BaseSummonRule
{
	// Token: 0x17001207 RID: 4615
	// (get) Token: 0x060030C6 RID: 12486 RVA: 0x000A6052 File Offset: 0x000A4252
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SummonEnemies;
		}
	}

	// Token: 0x17001208 RID: 4616
	// (get) Token: 0x060030C7 RID: 12487 RVA: 0x000A6056 File Offset: 0x000A4256
	public override Color BoxColor
	{
		get
		{
			return Color.green;
		}
	}

	// Token: 0x17001209 RID: 4617
	// (get) Token: 0x060030C8 RID: 12488 RVA: 0x000A605D File Offset: 0x000A425D
	public override string RuleLabel
	{
		get
		{
			return "Summon Enemies";
		}
	}

	// Token: 0x060030C9 RID: 12489 RVA: 0x000A6064 File Offset: 0x000A4264
	public override void Initialize(SummonRuleController summonController)
	{
		base.Initialize(summonController);
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_possibleEnemiesList = new List<EnemyTypeAndRank>();
	}

	// Token: 0x060030CA RID: 12490 RVA: 0x000A6089 File Offset: 0x000A4289
	public override IEnumerator RunSummonRule()
	{
		if (!base.SummonController.HasArenaStarted)
		{
			throw new Exception("Cannot start arena.  StartArena summon rule must be added before SummonEnemy can be called.");
		}
		SpawnPositionController spawnPosController = base.SummonController.gameObject.GetComponent<SpawnPositionController>();
		this.m_possibleEnemiesList.Clear();
		float remainingSummonValue = this.m_summonValue;
		int randomizeOnceEnemyIndex = -1;
		bool canEvolve = false;
		while (remainingSummonValue > 0f)
		{
			float num = float.MaxValue;
			foreach (EnemyTypeAndRank item in base.SummonController.SummonPool)
			{
				float summonValue = EnemyClassLibrary.GetEnemyData(item.Type, item.Rank).SummonValue;
				if (summonValue < num)
				{
					num = summonValue;
				}
				if (summonValue <= remainingSummonValue)
				{
					this.m_possibleEnemiesList.Add(item);
				}
			}
			if (this.m_possibleEnemiesList.Count == 0)
			{
				foreach (EnemyTypeAndRank item2 in base.SummonController.SummonPool)
				{
					if (EnemyClassLibrary.GetEnemyData(item2.Type, item2.Rank).SummonValue == num)
					{
						this.m_possibleEnemiesList.Add(item2);
					}
				}
			}
			int num2 = UnityEngine.Random.Range(0, this.m_possibleEnemiesList.Count);
			if (this.m_randomizeEnemiesOnce && randomizeOnceEnemyIndex != -1)
			{
				num2 = randomizeOnceEnemyIndex;
			}
			randomizeOnceEnemyIndex = num2;
			EnemyTypeAndRank randEnemy = this.m_possibleEnemiesList[num2];
			if (base.SummonController.SummonDifficultyOverride != EnemyRank.None)
			{
				randEnemy = new EnemyTypeAndRank(randEnemy.Type, base.SummonController.SummonDifficultyOverride);
			}
			num2 = UnityEngine.Random.Range(0, base.SummonController.AvailableSpawnPoints.Count);
			int randSpawnPointIndex = base.SummonController.AvailableSpawnPoints[num2];
			base.SummonController.AvailableSpawnPoints.RemoveAt(num2);
			if (base.SummonController.AvailableSpawnPoints.Count <= 0)
			{
				base.SummonController.AvailableSpawnPoints.AddRange(base.SummonController.SpawnPoints);
			}
			remainingSummonValue -= EnemyClassLibrary.GetEnemyData(randEnemy.Type, randEnemy.Rank).SummonValue;
			if (base.SummonController.PoolIsBiomeSpecific && canEvolve && base.SummonController.SummonDifficultyOverride == EnemyRank.None && canEvolve && randEnemy.Rank < EnemyRank.Expert)
			{
				float burdenStatGain = BurdenManager.GetBurdenStatGain(BurdenType.EnemyEvolve);
				if (UnityEngine.Random.Range(0f, 1f) < burdenStatGain)
				{
					randEnemy = new EnemyTypeAndRank(randEnemy.Type, randEnemy.Rank + 1);
				}
			}
			while (!PlayerManager.IsInstantiated || PlayerManager.GetCurrentPlayerRoom() == null)
			{
				yield return null;
			}
			Vector2 spawnPosOffset = spawnPosController.GetSpawnPosition(randSpawnPointIndex);
			float speedMod = this.m_spawnFast ? 3f : 1f;
			EnemyController enemyController = EnemyManager.SummonEnemy(PlayerManager.GetCurrentPlayerRoom(), randEnemy.Type, randEnemy.Rank, spawnPosOffset, true, true, speedMod, 1f);
			enemyController.IsCommander = this.m_summonAsCommander;
			enemyController.SetLevel(base.SummonController.SummonLevelOverride);
			if (remainingSummonValue > 0f && this.m_summonDelay > 0f)
			{
				this.m_waitYield.CreateNew(this.m_summonDelay, false);
				yield return this.m_waitYield;
			}
		}
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x040026A5 RID: 9893
	[SerializeField]
	private float m_summonValue;

	// Token: 0x040026A6 RID: 9894
	[SerializeField]
	private float m_summonDelay;

	// Token: 0x040026A7 RID: 9895
	[SerializeField]
	private bool m_randomizeEnemiesOnce;

	// Token: 0x040026A8 RID: 9896
	[SerializeField]
	private bool m_spawnFast;

	// Token: 0x040026A9 RID: 9897
	[SerializeField]
	private bool m_summonAsCommander;

	// Token: 0x040026AA RID: 9898
	private WaitRL_Yield m_waitYield;

	// Token: 0x040026AB RID: 9899
	private List<EnemyTypeAndRank> m_possibleEnemiesList;
}
