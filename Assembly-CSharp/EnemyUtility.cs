using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CBE RID: 3262
public static class EnemyUtility
{
	// Token: 0x06005D4C RID: 23884 RVA: 0x0015ADB4 File Offset: 0x00158FB4
	private static void InitialiseEnemiesInBiomeTable()
	{
		EnemyUtility.m_enemiesInBiomeTable = new Dictionary<BiomeType, List<EnemyTypeAndRank>>();
		foreach (BiomeType biomeType in BiomeType_RL.TypeArray)
		{
			if (EnemyUtility.IsIncludedBiome(biomeType))
			{
				List<EnemyTypeAndRank> list = new List<EnemyTypeAndRank>();
				foreach (EnemyType enemyType in EnemyTypes_RL.TypeArray)
				{
					if (enemyType != EnemyType.None && enemyType != EnemyType.Any)
					{
						foreach (EnemyRank enemyRank in EnemyTypes_RL.RankArray)
						{
							if (enemyRank != EnemyRank.None && enemyRank != EnemyRank.Any)
							{
								EnemyData enemyData = EnemyClassLibrary.GetEnemyData(enemyType, enemyRank);
								if (enemyData && enemyData.GetSpawnInBiome(biomeType))
								{
									list.Add(new EnemyTypeAndRank(enemyType, enemyRank));
								}
							}
						}
					}
				}
				EnemyUtility.m_enemiesInBiomeTable.Add(biomeType, list);
			}
		}
		if (EnemyUtility.DISPLAY_IN_CONSOLE_ON_INITIALISE)
		{
			string text = "Enemies in Biome Table:\n";
			foreach (KeyValuePair<BiomeType, List<EnemyTypeAndRank>> keyValuePair in EnemyUtility.m_enemiesInBiomeTable)
			{
				text = text + keyValuePair.Key.ToString() + ":\n";
				foreach (EnemyTypeAndRank enemyTypeAndRank in keyValuePair.Value)
				{
					text += string.Format("{0}, {1}\n", enemyTypeAndRank.Type, enemyTypeAndRank.Rank);
				}
			}
			Debug.LogFormat(text, Array.Empty<object>());
		}
	}

	// Token: 0x06005D4D RID: 23885 RVA: 0x0015AF7C File Offset: 0x0015917C
	public static bool IsFlyingEnemy(EnemyType enemyType, EnemyRank enemyRank)
	{
		EnemyData enemyData = EnemyClassLibrary.GetEnemyData(enemyType, enemyRank);
		return enemyData && enemyData.IsFlying;
	}

	// Token: 0x06005D4E RID: 23886 RVA: 0x0003357B File Offset: 0x0003177B
	private static bool IsIncludedBiome(BiomeType biome)
	{
		return biome != BiomeType.Any && biome != BiomeType.None && biome != BiomeType.Spawn && biome != BiomeType.Special;
	}

	// Token: 0x06005D4F RID: 23887 RVA: 0x0015AFA4 File Offset: 0x001591A4
	public static Bounds GetBounds(GameObject gameObject)
	{
		EnemyUtility.m_boundsHelper_STATIC.Clear();
		Transform transform = gameObject.transform.FindDeep("Visuals");
		Bounds result = new Bounds(gameObject.transform.position, Vector3.zero);
		if (transform)
		{
			transform.GetComponentsInChildren<Renderer>(true, EnemyUtility.m_boundsHelper_STATIC);
			using (List<Renderer>.Enumerator enumerator = EnemyUtility.m_boundsHelper_STATIC.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Renderer renderer = enumerator.Current;
					if (!(renderer is ParticleSystemRenderer) && (!(renderer.bounds.center == Vector3.zero) || !(renderer.bounds.size == Vector3.zero)))
					{
						result.Encapsulate(renderer.bounds);
					}
				}
				return result;
			}
		}
		Debug.LogFormat("<color=purple>| {0} | Couldn't find child called \"Visuals\" in GameObject ({1})</color>", new object[]
		{
			"EnemyUtility",
			gameObject.name
		});
		return result;
	}

	// Token: 0x06005D50 RID: 23888 RVA: 0x0015B0A8 File Offset: 0x001592A8
	public static float GetEnemyScale(EnemyController enemyController)
	{
		EnemyData enemyData = EnemyClassLibrary.GetEnemyData(enemyController.EnemyType, enemyController.EnemyRank);
		if (enemyData)
		{
			return enemyData.Scale;
		}
		return 0f;
	}

	// Token: 0x06005D51 RID: 23889 RVA: 0x0015B0DC File Offset: 0x001592DC
	public static List<EnemyTypeAndRank> GetAllEnemiesInBiome(BiomeType biome, bool onlyFlyingEnemies = false)
	{
		if (EnemyUtility.m_enemiesInBiomeTable == null)
		{
			EnemyUtility.InitialiseEnemiesInBiomeTable();
		}
		if (biome == BiomeType.TowerExterior)
		{
			biome = BiomeType.Tower;
		}
		if (EnemyUtility.m_enemiesInBiomeTable.ContainsKey(biome))
		{
			List<EnemyTypeAndRank> collection = EnemyUtility.m_enemiesInBiomeTable[biome];
			EnemyUtility.m_enemiesInBiomeHelper.Clear();
			EnemyUtility.m_enemiesInBiomeHelper.AddRange(collection);
			if (biome == BiomeType.Castle && BurdenManager.IsBurdenActive(BurdenType.CastleBiomeUp))
			{
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.SwordKnight, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.SpearKnight, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.ElementalFire, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.SwordKnight, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.SpearKnight, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.ElementalFire, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.AxeKnight, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.PaintingEnemy, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.FlyingHunter, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.Sniper, EnemyRank.Basic));
			}
			if (biome == BiomeType.Stone && BurdenManager.IsBurdenActive(BurdenType.BridgeBiomeUp))
			{
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.ElementalBounce, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.Skeleton, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.BlobFish, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.Eyeball, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.ElementalBounce, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.Skeleton, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.BlobFish, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.Eyeball, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.FlyingSkull, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.Fireball, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.Zombie, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.Slug, EnemyRank.Advanced));
			}
			if (biome == BiomeType.Forest && BurdenManager.IsBurdenActive(BurdenType.ForestBiomeUp))
			{
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.BouncySpike, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.Wolf, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.ElementalIce, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.Starburst, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.Wisp, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.Zombie, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.FlyingBurst, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.BouncySpike, EnemyRank.Expert));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.Wolf, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.ElementalIce, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.Starburst, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.Wisp, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.Zombie, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.FlyingBurst, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.Plant, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.TopShotHazard, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.Volcano, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.AxeKnight, EnemyRank.Basic));
			}
			if (biome == BiomeType.Study && BurdenManager.IsBurdenActive(BurdenType.StudyBiomeUp))
			{
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.MimicChest, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.PaintingEnemy, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.Slug, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.Sniper, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.MimicChest, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.PaintingEnemy, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.Slug, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.Sniper, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.ElementalDash, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.ElementalIce, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.ElementalBounce, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.ElementalCurse, EnemyRank.Advanced));
			}
			if (biome == BiomeType.Tower && BurdenManager.IsBurdenActive(BurdenType.TowerBiomeUp))
			{
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.FlyingHunter, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.FlyingSkull, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.Fireball, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.ElementalCurse, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.FlyingHunter, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.FlyingSkull, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.Fireball, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.ElementalCurse, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.SkeletonArcher, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.Wisp, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.Plant, EnemyRank.Advanced));
			}
			if (biome == BiomeType.Cave && BurdenManager.IsBurdenActive(BurdenType.CaveBiomeUp))
			{
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.AxeKnight, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.Plant, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.TopShotHazard, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Remove(new EnemyTypeAndRank(EnemyType.ElementalDash, EnemyRank.Basic));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.AxeKnight, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.Plant, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.TopShotHazard, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.ElementalDash, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.FlyingSkull, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.MimicChest, EnemyRank.Advanced));
				EnemyUtility.m_enemiesInBiomeHelper.Add(new EnemyTypeAndRank(EnemyType.Wolf, EnemyRank.Advanced));
			}
			if (onlyFlyingEnemies)
			{
				for (int i = 0; i < EnemyUtility.m_enemiesInBiomeHelper.Count; i++)
				{
					if (!EnemyUtility.IsFlyingEnemy(EnemyUtility.m_enemiesInBiomeHelper[i].Type, EnemyUtility.m_enemiesInBiomeHelper[i].Rank))
					{
						EnemyUtility.m_enemiesInBiomeHelper.RemoveAt(i);
						i--;
					}
				}
			}
			return EnemyUtility.m_enemiesInBiomeHelper;
		}
		Debug.LogFormat("<color=red>(EnemyUtility) No Entry in Table for Biome ({0})</color>", new object[]
		{
			biome
		});
		return null;
	}

	// Token: 0x04004CB6 RID: 19638
	private static bool DISPLAY_IN_CONSOLE_ON_INITIALISE = false;

	// Token: 0x04004CB7 RID: 19639
	private static Dictionary<BiomeType, List<EnemyTypeAndRank>> m_enemiesInBiomeTable = null;

	// Token: 0x04004CB8 RID: 19640
	private static List<Renderer> m_boundsHelper_STATIC = new List<Renderer>();

	// Token: 0x04004CB9 RID: 19641
	private static List<EnemyTypeAndRank> m_enemiesInBiomeHelper = new List<EnemyTypeAndRank>();
}
