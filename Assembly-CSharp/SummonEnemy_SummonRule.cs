using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008B4 RID: 2228
[Serializable]
public class SummonEnemy_SummonRule : BaseSummonRule
{
	// Token: 0x17001840 RID: 6208
	// (get) Token: 0x060043F1 RID: 17393 RVA: 0x00006581 File Offset: 0x00004781
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SummonEnemies;
		}
	}

	// Token: 0x17001841 RID: 6209
	// (get) Token: 0x060043F2 RID: 17394 RVA: 0x00025785 File Offset: 0x00023985
	public override Color BoxColor
	{
		get
		{
			return Color.green;
		}
	}

	// Token: 0x17001842 RID: 6210
	// (get) Token: 0x060043F3 RID: 17395 RVA: 0x0002578C File Offset: 0x0002398C
	public override string RuleLabel
	{
		get
		{
			return "Summon Enemies";
		}
	}

	// Token: 0x060043F4 RID: 17396 RVA: 0x00025793 File Offset: 0x00023993
	public override void Initialize(SummonRuleController summonController)
	{
		base.Initialize(summonController);
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_possibleEnemiesList = new List<EnemyTypeAndRank>();
	}

	// Token: 0x060043F5 RID: 17397 RVA: 0x000257B8 File Offset: 0x000239B8
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

	// Token: 0x040034CC RID: 13516
	[SerializeField]
	private float m_summonValue;

	// Token: 0x040034CD RID: 13517
	[SerializeField]
	private float m_summonDelay;

	// Token: 0x040034CE RID: 13518
	[SerializeField]
	private bool m_randomizeEnemiesOnce;

	// Token: 0x040034CF RID: 13519
	[SerializeField]
	private bool m_spawnFast;

	// Token: 0x040034D0 RID: 13520
	[SerializeField]
	private bool m_summonAsCommander;

	// Token: 0x040034D1 RID: 13521
	private WaitRL_Yield m_waitYield;

	// Token: 0x040034D2 RID: 13522
	private List<EnemyTypeAndRank> m_possibleEnemiesList;
}
